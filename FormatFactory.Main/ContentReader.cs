using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Irvin.Extensions;
using Irvin.FormatFactory.Internal;
using Irvin.FormatFactory.Internal.Member;
using Irvin.Parser;

namespace Irvin.FormatFactory
{
    internal class ContentReader
    {
        private readonly FormatOptions _readOptions;
        private readonly StringComparison _compareOption;

        public ContentReader(FormatOptions readOptions)
        {
            _readOptions = readOptions;
            _compareOption = SettingsFactory.GetCompareOption(readOptions);
        }

        public List<T> Parse<T>(string content)
        {
            RecordSettings recordSettings = SettingsFactory.GetRecordSettingsForRead(typeof(T), _readOptions);
            StructuredFormatParser parser = new StructuredFormatParser(recordSettings, _readOptions);
            TokenCollection tokens = parser.Parse(content, _compareOption);

            HandleHeaders(tokens, recordSettings);

            return ParseRecords<T>(tokens, recordSettings);
        }

        //Hmm. We don't really handle a mix of field headers and sub-records or children. Would that ever come up?
        private void HandleHeaders(TokenCollection tokens, RecordSettings recordSettings)
        {
            if (recordSettings.Options.IncludeHeaders)
            {
                tokens.MoveNext();
                List<IOrderedMemberInfo> orderedMembers = recordSettings.OrderedMembers.ToList();

                List<IOrderedMemberInfo> newOrderedFields = GetMemberOrderFromHeaderRow(tokens, recordSettings);
                ThrowIfHeaderRowInvalid(orderedMembers, newOrderedFields);

                List<IOrderedMemberInfo> newOrder = new List<IOrderedMemberInfo>();
                newOrder.AddRange(newOrderedFields);
                newOrder.AddRange(orderedMembers.FindAll(memberInfo => !(memberInfo is FieldInfo)));
                recordSettings.OrderedMembers = newOrder;
            }
        }

        private List<IOrderedMemberInfo> GetMemberOrderFromHeaderRow(TokenCollection tokens, RecordSettings recordSettings)
        {
            bool rowRead = false;
            List<IOrderedMemberInfo> fields = new List<IOrderedMemberInfo>();

            while (!rowRead)
            {
                string columnName = GetRawValue(tokens, recordSettings);
                IOrderedMemberInfo matchingField = recordSettings.OrderedMembers.FirstOrDefault(x => MemberIsColumn(x, columnName));
                if (matchingField == null)
                {
                    throw new InvalidDataException($"The header '{columnName}' was not recognized.");
                }
                fields.Add(matchingField);

                if (AtRecordDelimiter(tokens, recordSettings.Options))
                {
                    tokens.MoveNext();
                    rowRead = true;
                }
                else
                {
                    if (tokens.HasNext())
                    {
                        tokens.MoveNext();
                    }
                    else
                    {
                        rowRead = true;
                    }
                }
            }

            return fields;
        }

        private bool MemberIsColumn(IOrderedMemberInfo memberInfo, string columnName)
        {
            return memberInfo?.HeaderName != null && memberInfo.HeaderName.Equals(columnName, _compareOption);
        }

        private static void ThrowIfHeaderRowInvalid(IList<IOrderedMemberInfo> orderedMembers, IList<IOrderedMemberInfo> newOrderedFields)
        {
            for (int i = 0; i < newOrderedFields.Count; i++)
            {
                IOrderedMemberInfo dataFieldInfo = newOrderedFields[i];
                if (i < orderedMembers.Count)
                {
                    string dataFieldName = dataFieldInfo.HeaderName ?? string.Empty;
                    Internal.FieldInfo reflectedFieldInfo = orderedMembers[i] as Internal.FieldInfo;
                    string reflectedFieldName = reflectedFieldInfo?.HeaderName ?? string.Empty;
                    if (!dataFieldName.Equals(reflectedFieldName))
                    {
                        if (reflectedFieldInfo != null && reflectedFieldInfo.Order > 0)
                        {
                            throw new InvalidDataException("The columns are not in the correct order.");
                        }
                    }
                }
                else
                {
                    throw new InvalidDataException("There were more columns specified than exist.");
                }
            }
        }

        public List<T> ParseRecords<T>(TokenCollection tokens, RecordSettings recordSettings)
        {
            IList list = new ArrayList();

            while (tokens.HasNext())
            {
                object currentItem = PopulateObject(tokens, recordSettings);
                list.Add(currentItem);
            }

            return list.Cast<T>().ToList();
        }

        private object PopulateObject(TokenCollection tokens, RecordSettings recordSettings)
        {
            object currentItem = null;
            
            int i = 0;
            int fixedWidthOffset = 0;
            bool finishedWithRecord = false;
            while (!finishedWithRecord && i < recordSettings.OrderedMembers.Count)
            {
                IOrderedMemberInfo recordMemberInfo = recordSettings.OrderedMembers[i];

                if (currentItem == null)
                {
                    currentItem = recordMemberInfo.MemberInfo.Container.CreateNew();
                }

                object memberValue;
                FormatOptions formatOptions = recordSettings.Options;

                if (recordMemberInfo is FieldInfo)
                {
                    FieldInfo fieldSettings = recordMemberInfo as FieldInfo;
                    
                    string rawValue = GetRawValue(tokens, recordSettings, fieldSettings, fixedWidthOffset);
                    try
                    {
                        memberValue = ParseFieldValue(rawValue, fieldSettings);
                    }
                    catch (InvalidDataException)
                    {
                        BubbleDataBindFailure = i != 0;
                        throw;
                    }

                    if (!NextMemberIsChildElement(recordSettings, i))
                    {
                        finishedWithRecord = AtRecordDelimiter(tokens, formatOptions);
                    }
                    if (IsTrueFixedWith(recordSettings, fieldSettings))
                    {
                        fixedWidthOffset += fieldSettings.Settings.MaximumLength;
                        if (i == recordSettings.OrderedMembers.Count - 1)
                        {
                            finishedWithRecord = true;
                            if (tokens.HasNext())
                            {
                                tokens.MoveNext();
                            }
                            if (AtRecordDelimiter(tokens, formatOptions) && tokens.HasNext())
                            {
                                tokens.MoveNext();
                            }
                        }
                    }
                    else if (tokens.HasNext())
                    {
                        tokens.MoveNext();
                        fixedWidthOffset = 0;
                    }
                }
                else
                {
                    memberValue = PopulateSubElement(tokens, recordMemberInfo, formatOptions);

                    if (memberValue == null)
                    {
                        if (recordMemberInfo.IsRequired)
                        {
                            throw new InvalidDataException($"{recordMemberInfo.MemberInfo.Name} is required.");
                        }
                    }

                    if (i == recordSettings.OrderedMembers.Count - 1)
                    {
                        finishedWithRecord = true;
                    }
                }

                SetValue(currentItem, recordMemberInfo.MemberInfo, memberValue);

                i++;
            }

            if (i < recordSettings.OrderedMembers.Count)
            {
                ThrowIfRequiredItemsNotFilled(recordSettings, i);
            }
            else if (!finishedWithRecord && tokens.HasNext())
            {
                throw new InvalidDataException($"The data does not match the shape of {recordSettings.RecordType}.");
            }

            return currentItem;
        }

        private static bool IsTrueFixedWith(RecordSettings recordSettings, Internal.FieldInfo fieldSettings)
        {
            return  string.IsNullOrEmpty(recordSettings.Options.FieldDelimiter) && 
                    string.IsNullOrEmpty(fieldSettings.Settings.Delimiter) && 
                    fieldSettings.Settings.IsFixedWidth;
        }

        private string GetRawValue(TokenCollection tokens, RecordSettings recordSettings, Internal.FieldInfo fieldSettings = null, int fixedWidthOffset = 0)
        {
            string rawValue = GetRawFieldValue(tokens, recordSettings, fieldSettings, fixedWidthOffset);
            rawValue = rawValue.Trim('\r');
            return RemoveEscapeCharacters(rawValue, recordSettings.Options);
        }

        private string GetRawFieldValue(TokenCollection tokens, RecordSettings recordSettings, Internal.FieldInfo fieldSettings, int fixedWidthOffset)
        {
            FormatOptions formatOptions = recordSettings.Options;

            if (IsTrueFixedWith(recordSettings, fieldSettings))
            {
                tokens.SetCheckpoint();
            }
            string rawValue = GetNextChunk(tokens, recordSettings, fieldSettings);
            if (fixedWidthOffset > 0)
            {
                rawValue = rawValue.Substring(fixedWidthOffset);
            }
            if (IsTrueFixedWith(recordSettings, fieldSettings))
            {
                rawValue = rawValue.Substring(0, fieldSettings.Settings.MaximumLength);
                tokens.Rewind();
            }

            if (formatOptions.EscapeKind == EscapeKind.Repeat)
            {
                string currentTokenValue = tokens.Current.Content;
                if (currentTokenValue.Equals(formatOptions.FieldDelimiter, _compareOption))
                {
                    string nextTokenValue = tokens.PeekNext().Content;
                    if (nextTokenValue.Equals(formatOptions.FieldDelimiter, _compareOption))
                    {
                        tokens
                            .MoveNext()
                            .MoveNext();

                        string nextChunk = GetNextChunk(tokens, recordSettings, fieldSettings);
                        rawValue = string.Format("{0}{1}{2}", rawValue, formatOptions.FieldDelimiter, nextChunk);
                    }
                }
            }

            return rawValue;
        }

        private string GetNextChunk(TokenCollection tokens, RecordSettings recordSettings, Internal.FieldInfo fieldSettings)
        {
            ReadOnlyCollection<Token> fieldTokens = tokens.MoveUntil(token => IsDelimiter(token, recordSettings, fieldSettings));
            return Token.Join(fieldTokens);
        }

        private static string RemoveEscapeCharacters(string rawValue, IRecordOptions formatOptions)
        {
            switch (formatOptions.EscapeKind)
            {
                case EscapeKind.Transform:
                {
                    string transformCharacter = formatOptions.TransformEscapeCharacter.ToString();
                    return rawValue
                        .Replace(transformCharacter, formatOptions.FieldDelimiter)
                        .Replace(transformCharacter, formatOptions.RecordDelimiter);
                }
                case EscapeKind.DoubleQuote:
                    return rawValue.Trim('"');
                case EscapeKind.SingleQuote:
                    return rawValue.Trim('\'');
                default:
                    return rawValue;
            }
        }

        private bool AtRecordDelimiter(TokenCollection tokens, IRecordOptions recordOptions)
        {
            string current = tokens.Current.Content;
            bool isUnambiguousRecordEnd = current.Equals(recordOptions.RecordDelimiter, _compareOption);
            bool isAlternateNewlineRecordEnd = recordOptions.RecordDelimiter == Environment.NewLine &&
                                               current.EndsWith(Environment.NewLine.Last().ToString());
            return isUnambiguousRecordEnd || isAlternateNewlineRecordEnd;
        }

        private bool IsDelimiter(Token token, RecordSettings recordSettings, Internal.FieldInfo fieldSettings)
        {
            bool isSpecificFieldDelimiter = 
                fieldSettings != null 
                && 
                token.Content.Equals(fieldSettings.Settings.Delimiter, _compareOption);

            return token.Content.Equals(recordSettings.Options.FieldDelimiter, _compareOption) ||
                   token.Content.Equals(recordSettings.Options.RecordDelimiter, _compareOption) ||
                   isSpecificFieldDelimiter;
        }

        private object ParseFieldValue(string rawValue, Internal.FieldInfo fieldInfo)
        {
            Type targetType = TypeFactory.GetMemberType(fieldInfo.MemberInfo);
            if (targetType == typeof(string))
            {
                return SanitizeStringValue(rawValue, fieldInfo);
            }

            return ParseNumericalValue(rawValue, fieldInfo);
        }

        private string SanitizeStringValue(string rawValue, Internal.FieldInfo fieldInfo)
        {
            IFieldSettings fieldSettings = fieldInfo.Settings;

            if (rawValue != null && fieldSettings != null)
            {
                char paddingCharacter = fieldSettings.PaddingCharacter;
                if (fieldSettings.Alignment == FieldAlignment.Left)
                {
                    rawValue = rawValue.Trim(paddingCharacter);
                }
                else if (fieldSettings.Alignment == FieldAlignment.Right)
                {
                    while (rawValue.Length > 0 && rawValue[0] == paddingCharacter)
                    {
                        rawValue = rawValue.Substring(1);
                    }
                }

                if (rawValue.Length > fieldSettings.MaximumLength)
                {
                    string message = string.Format("The value of '{0}' cannot exceed {1} characters.",
                                                    fieldInfo.MemberInfo.Name, fieldSettings.MaximumLength);
                    throw new InvalidDataException(message);
                }
            }

            return rawValue;
        }

        private object ParseNumericalValue(string rawValue, FieldInfo fieldInfo)
        {
            Type targetType = TypeFactory.GetMemberType(fieldInfo.MemberInfo);

            try
            {
                if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                {
                    object defaultValue = targetType == typeof(DateTime) ? (object) DateTime.MinValue : null;
                    return ParseDateTime(rawValue, fieldInfo.Settings, defaultValue);
                }

                if (targetType == typeof(decimal) || targetType == typeof(float))
                {
                    rawValue = rawValue.Replace("$", string.Empty).Replace(",", string.Empty);
                }

                if (targetType == typeof(object))
                {
                    return rawValue;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                return converter.ConvertFromString(rawValue);
            }
            catch (Exception conversionException)
            {
                string message =$"The value '{rawValue}' cannot be converted to a {targetType.Name} (field '{fieldInfo.MemberInfo.Name}').";
                throw new InvalidDataException(message, conversionException);
            }
        }

        private static object ParseDateTime(string rawValue, IFieldSettings fieldSettings, object defaultValue)
        {
            rawValue = (rawValue ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(rawValue))
            {
                return defaultValue;
            }

            if (fieldSettings != null && !string.IsNullOrEmpty(fieldSettings.Format))
            {
                return DateTime.ParseExact(rawValue, fieldSettings.Format, CultureInfo.InvariantCulture.DateTimeFormat);
            }

            return DateTime.Parse(rawValue);
        }

        private static bool NextMemberIsChildElement(RecordSettings recordSettings, int i)
        {
            return (i+1 < recordSettings.OrderedMembers.Count) && !recordSettings.OrderedMembers[i + 1].IsInternalToRecord;
        }

        private object PopulateSubElement(TokenCollection tokens, IOrderedMemberInfo recordMemberInfo, FormatOptions formatOptions)
        {
            object memberValue = null;

            Type listType = null;
            Type memberType = TypeFactory.GetMemberType(recordMemberInfo.MemberInfo);
            if (!recordMemberInfo.IsInternalToRecord)
            {
                SkipIdentTokens(tokens, recordMemberInfo.IndentCharacters);

                if (memberType.IsGenericList())
                {
                    listType = memberType;
                    memberType = memberType.GetGenericArguments().First();
                }
            }

            RecordSettings subRecordSettings = SettingsFactory.GetRecordSettingsForRead(memberType, _readOptions);
            if (recordMemberInfo is SubRecordInfo)
            {
                subRecordSettings.Options.RecordDelimiter = formatOptions.FieldDelimiter;
            }

            try
            {
                if (listType == null)
                {
                    tokens.SetCheckpoint();
                    memberValue = PopulateObject(tokens, subRecordSettings);    
                }
                else
                {
                    memberValue = Activator.CreateInstance(listType);
                    IList list = (IList)memberValue;
                    while (tokens.HasNext())
                    {
                        tokens.SetCheckpoint();
                        object value = PopulateObject(tokens, subRecordSettings);
                        list.Add(value);
                    }
                }
            }
            catch (InvalidDataException ex)
            {
                tokens.Rewind();
                Debug.Print(ex.Message);
                if (BubbleDataBindFailure)
                {
                    throw;
                }
            }

            return memberValue;
        }

        //TODO: this is making an assumption that if the first field of a sub-element fails to bind that we need to rewind
        //but that if a n > 1 field of a sub-element fails to find it is a data error. There may be scenarios where this is not the case
        private bool BubbleDataBindFailure { get; set; }

        private void SkipIdentTokens(TokenCollection tokens, string indentCharacters)
        {
            if (!string.IsNullOrEmpty(indentCharacters))
            {
                while (tokens.Current.Content.Equals(indentCharacters, _compareOption))
                {
                    tokens.MoveNext();
                }
            }
        }

        private static void SetValue(object currentItem, IMemberInfo memberInfo, object memberValue)
        {
            if (!memberInfo.SetValue(currentItem, memberValue))
            {
                object expectedValue = memberInfo.GetValue(currentItem);
                if (expectedValue != null && !expectedValue.Equals(memberValue))
                {
                    string message =
                        $"The value '{memberValue}' cannot be assigned to {memberInfo.Container.Name}.{memberInfo.Name} (value is always '{expectedValue}')";
                    throw new InvalidDataException(message);
                }
            }
        }

        private static void ThrowIfRequiredItemsNotFilled(RecordSettings recordSettings, int currentIndex)
        {
            IOrderedMemberInfo missingMember =
                recordSettings.OrderedMembers
                    .ToList()
                    .GetRange(currentIndex, recordSettings.OrderedMembers.Count - currentIndex)
                    .FirstOrDefault(x =>
                    {
                        Internal.FieldInfo missedField = x as FieldInfo;
                        return missedField != null && !missedField.Settings.Optional;
                    });

            if (missingMember != null)
            {
                throw new InvalidDataException($"The field '{missingMember.MemberInfo.Name}' must be supplied.");
            }
        }
    }
}