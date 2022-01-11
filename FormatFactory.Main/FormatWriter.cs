using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Irvin.Extensions;
using Irvin.FormatFactory.Internal;
using Irvin.FormatFactory.Internal.Member;
using FieldInfo = Irvin.FormatFactory.Internal.FieldInfo;

namespace Irvin.FormatFactory
{
	public class FormatWriter : IFormatWriter
	{
		static FormatWriter()
		{
			Instance = new FormatWriter();
		}

		public string WriteSingle<T>(T element, FormatOptions writerOptions = null)
		{
			return Write(new List<T> {element}, writerOptions);
		}

		public void WriteSingle<T>(StringBuilder container, T element, FormatOptions writerOptions = null)
		{
			container.Append(WriteSingle(element, writerOptions));
		}

		public void WriteSingle<T>(Stream stream, T element, FormatOptions writerOptions = null)
		{
			Write(stream, new List<T> {element}, writerOptions);
		}

		public void WriteSingle<T>(TextWriter writer, T element, FormatOptions writerOptions = null)
		{
			Write(writer, new List<T> {element}, writerOptions);
		}

		public void WriteSingle<T>(string filename, T element, FormatOptions writerOptions = null)
		{
			Write(filename, new List<T> {element}, writerOptions);
		}

		public string Write<T>(IEnumerable<T> elements, FormatOptions writerOptions = null)
		{
			StringBuilder builder = new StringBuilder();
			Write(builder, elements.ToList(), writerOptions);
			return builder.ToString();
		}

		public void Write<T>(StringBuilder container, IList<T> elements, FormatOptions writerOptions = null)
		{
			WriteForType(container, (IList) elements, typeof(T), writerOptions);
		}

        public string Write(DataTable dataTable, FormatOptions writerOptions)
        {
            StringBuilder container = new StringBuilder();
            Write(container, dataTable, writerOptions);
            return container.ToString();
        }

		public void Write(StringBuilder container, DataTable dataTable, FormatOptions writerOptions)
        {
            RecordSettings settings = SettingsFactory.GetSettingsForDataTable(dataTable, writerOptions);
            WriteToContainer(container, dataTable.Rows.Cast<DataRow>().ToList(), settings, writerOptions);
        }

        public void Write(Stream stream, DataTable dataTable, FormatOptions writerOptions)
        {
            TextWriter streamWriter = new StreamWriter(stream);
            Write(streamWriter, dataTable, writerOptions);
        }

        public void Write(TextWriter writer, DataTable dataTable, FormatOptions writerOptions)
        {
            string content = Write(dataTable, writerOptions);
			writer.Write(content);
			writer.Flush();
        }

        public void Write(string filename, DataTable dataTable, FormatOptions writerOptions)
        {
            using (Stream fileStream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            {
                Write(fileStream, dataTable, writerOptions);
            }
        }

        private void WriteForType(StringBuilder container, IList elements, Type elementType, FormatOptions writerOptions)
        {
            RecordSettings settings = SettingsFactory.GetRecordSettingsForWrite(elementType, writerOptions);
            WriteToContainer(container, elements, settings, writerOptions);
        }

        private void WriteToContainer(StringBuilder container, IList elements, RecordSettings settings, FormatOptions writerOptions)
        {
            WriteHeaders(container, settings);

            for (int i = 0; i < elements.Count; i++)
            {
                object element = elements[i];
                settings.RowIndex = i;
                string record = BuildRecord(element, settings);
                container.Append(record);

                WriteAllChildren(container, element, settings, writerOptions);
            }
        }

        private static void WriteHeaders(StringBuilder container, RecordSettings settings)
	    {
	        if (settings.Options.IncludeHeaders)
	        {
	            string[] orderedHeaders = settings.OrderedMembers
	                .Where(x => !string.IsNullOrWhiteSpace(x.HeaderName))
	                .Select(fieldInfo =>
	                {
		                EscapeSettings escapeSettings = new EscapeSettings(-1, fieldInfo.MemberInfo.Name, settings.Options);
		                escapeSettings.Delimiter = settings.Options.FieldDelimiter;
		                escapeSettings.DelimiterName = "header";
		                string headerName = GetEscapedFieldValue(fieldInfo.HeaderName, escapeSettings);
		                
		                return new
		                {
			                HeaderName = headerName,
			                FieldInfo = fieldInfo as FieldInfo
		                };
	                })
	                .Select(headerInfo => GetQuotedValue(headerInfo.HeaderName, settings, headerInfo.FieldInfo))
	                .ToArray();
	            string headers = string.Join(settings.Options.FieldDelimiter, orderedHeaders);
	            container.Append(headers);
	            container.Append(settings.Options.RecordDelimiter);
	        }
	    }

        private void WriteAllChildren(StringBuilder container, object element, RecordSettings settings, FormatOptions writerOptions)
        {
            IEnumerable<IOrderedMemberInfo> childMemberInfos = settings.OrderedMembers.Where(x => !x.IsInternalToRecord);
            foreach (IOrderedMemberInfo childElementInfo in childMemberInfos)
			{
				object value = childElementInfo.MemberInfo.GetValue(element);

				ArrayList childElements = new ArrayList();
				Type memberType = TypeFactory.GetMemberType(childElementInfo.MemberInfo);
				if (memberType.IsGenericList())
				{
					IList list = value as IList;
					if (list != null)
					{
						childElements.AddRange(list);
					}
				}
				else
				{
					if (value != null)
					{
						childElements.Add(value);
					}
				}

				if (childElements.Count > 0)
				{
					if (writerOptions == null)
					{
						writerOptions = new FormatOptions();
					}
					writerOptions.IndentCharacters = childElementInfo.IndentCharacters;
					WriteForType(container, childElements, childElements[0].GetType(), writerOptions);
				}
			}
        }

		public void Write<T>(Stream stream, IEnumerable<T> elements, FormatOptions writerOptions = null)
		{
			StreamWriter writer = new StreamWriter(stream);
			string content = Write(elements, writerOptions);
			writer.Write(content);
			writer.Flush();
		}

		public void Write<T>(TextWriter writer, IEnumerable<T> elements, FormatOptions writerOptions = null)
		{
			writer.Write(Write(elements, writerOptions));
		}

		public void Write<T>(string filename, IEnumerable<T> elements, FormatOptions writerOptions = null)
		{
			string content = Write(elements, writerOptions);
			File.WriteAllText(filename, content);
		}
		
        private string BuildRecord(object item, RecordSettings settings)
		{
			StringBuilder record = new StringBuilder(settings.Options.IndentCharacters);

			string finalDelimiter = string.Empty;
		    List<IOrderedMemberInfo> inlineElements = 
                settings.OrderedMembers
                    .Where(x => x.IsInternalToRecord)
                    .ToList();
		    foreach (IOrderedMemberInfo elementInfo in inlineElements)
			{
				object rawValue = elementInfo.MemberInfo.GetValue(item);

				if (elementInfo is FieldInfo)
				{
					FieldInfo fieldInfo = elementInfo as FieldInfo;
					if (rawValue != null || !fieldInfo.Settings.Optional)
					{
					    string settingsFormat = fieldInfo.Settings.Format;

                        Type memberType = TypeFactory.GetMemberType(elementInfo.MemberInfo);
                        if (string.IsNullOrEmpty(settingsFormat) && memberType == typeof(DateTime))
                        {
                            settingsFormat = FieldFormat.DateTime24Hour;
                        }

					    string formattedValue = GetFormattedValue(rawValue, settingsFormat);
                        string adjustedValue = GetAdjustedValue(settings, fieldInfo, formattedValue);
                        string quotedValue = GetQuotedValue(adjustedValue, settings, fieldInfo);
                        
					    record.Append(quotedValue);

						finalDelimiter = AppendFieldDelimiter(record, settings.Options.FieldDelimiter, fieldInfo.Settings.Delimiter);
					}
				}
				else
				{
					Type memberType = TypeFactory.GetMemberType(elementInfo.MemberInfo);
					FormatOptions options = new FormatOptions();
					options.RecordDelimiter = string.Empty;
					WriteForType(record, new ArrayList {rawValue}, memberType, options);
					finalDelimiter = AppendFieldDelimiter(record, settings.Options.FieldDelimiter, null);
				}
			}

			if (!settings.Options.UseTrailingDelimiter)
			{
				record.Remove(record.Length - finalDelimiter.Length, finalDelimiter.Length);
			}
		    if (inlineElements.Any())
		    {
		        record.Append(settings.Options.RecordDelimiter);
		    }

			return record.ToString();
		}

        private string GetAdjustedValue(RecordSettings settings, FieldInfo fieldInfo, string formattedValue)
        {
            string lengthAdjustedValue = AdjustValueWidth(fieldInfo, formattedValue, settings.RowIndex);

            EscapeSettings escapeSettings = new EscapeSettings(settings.RowIndex, fieldInfo.MemberInfo.Name, settings.Options);
            escapeSettings.Delimiter = settings.Options.RecordDelimiter;
            escapeSettings.DelimiterName = "record";
            string semiEscapedValue = GetEscapedFieldValue(lengthAdjustedValue, escapeSettings);

            escapeSettings.Delimiter = settings.Options.FieldDelimiter;
            escapeSettings.DelimiterName = "field";
            return GetEscapedFieldValue(semiEscapedValue, escapeSettings);
        }

        private static string GetEscapedFieldValue(string originalValue, EscapeSettings settings)
		{
			string fieldValue = originalValue;

		    if (!string.IsNullOrEmpty(settings.Delimiter))
			{
				if (originalValue.Contains(settings.Delimiter))
				{
				    ThrowIfInvalidDelimiterUsage(settings);

				    string character = GetEscapeCharacter(settings.Options);
				    switch (settings.Options.EscapeKind)
				    {
				        case EscapeKind.SingleQuote:
				        case EscapeKind.DoubleQuote:
				            fieldValue = $"{character}{fieldValue}{character}";
				            break;
                        case EscapeKind.Repeat:
				            fieldValue = ReplaceByPattern(fieldValue, settings.Delimiter, "{0}{0}");
                            break;
                        case EscapeKind.Remove:
				            fieldValue = fieldValue.Replace(settings.Delimiter, string.Empty);
                            break;
                        case EscapeKind.Backslash:
                        case EscapeKind.ForwardSlash:
                            fieldValue = ReplaceByPattern(fieldValue, settings.Delimiter, character + "{0}");
                            break;
				        case EscapeKind.Transform:
				            fieldValue = fieldValue.Replace(settings.Delimiter, character);
                            break;
                    }
				}
			}

			return fieldValue;
		}

        private static string GetEscapeCharacter(FormatOptions options)
        {
	        switch (options.EscapeKind)
	        {
		        case EscapeKind.SingleQuote:
			        return "'";
		        case EscapeKind.DoubleQuote:
			        return "\"";
		        case EscapeKind.Backslash:
			        return "\\";
		        case EscapeKind.ForwardSlash:
			        return "/";
		        case EscapeKind.Transform:
			        return options.TransformEscapeCharacter.ToString(CultureInfo.InvariantCulture);
		        default:
			        return null;
	        }
        }

        private static string ReplaceByPattern(string str, string oldValue, string transform)
	    {
	        return str.Replace(oldValue, string.Format(transform, oldValue));
	    }

	    private static void ThrowIfInvalidDelimiterUsage(EscapeSettings settings)
	    {
	        if (!settings.Options.AllowDelimitersAsEscapedContent)
            {
                string messageFooter =
                    settings.RowIndex >= 0
                        ? $"in element #{settings.RowIndex + 1}"
                        : "in the header";
                string descriptor =
	                !string.IsNullOrWhiteSpace(settings.MemberName)
		                ? $"field '{settings.MemberName}'"
		                : "sub-elements";

	            string message =
                    $"The {settings.DelimiterName} delimiter ('{settings.Delimiter}') " +
                    $"was found in the content for {descriptor} {messageFooter}.";

	            throw new InvalidDataException(message);
	        }
	    }

	    private string AdjustValueWidth(FieldInfo fieldInfo, string formattedValue, int rowIndex)
		{
			string lengthAdjustedValue;

			int minimumLength = fieldInfo.Settings.MinimumLength;
			int maximumLength = fieldInfo.Settings.MaximumLength;
			bool enforceFixedWidth = fieldInfo.Settings.IsFixedWidth && minimumLength == 0;

			string minimumFormattedValue = formattedValue;
			if (formattedValue.Length < minimumLength)
			{
				minimumFormattedValue = PadValue(formattedValue, minimumLength, fieldInfo.Settings.Alignment, fieldInfo.Settings.PaddingCharacter);
			}

			if (enforceFixedWidth)
            {
                if (minimumFormattedValue.Length > maximumLength)
				{
					throw new InvalidDataException(
                        $"The value of '{rowIndex}' for element {fieldInfo.MemberInfo.Name} is too long for the field.");
				}

                lengthAdjustedValue =
                    minimumFormattedValue.Length != maximumLength
                        ? PadValue(minimumFormattedValue, maximumLength, fieldInfo.Settings.Alignment, fieldInfo.Settings.PaddingCharacter)
                        : minimumFormattedValue;
            }
			else
			{
				if (minimumFormattedValue.Length > maximumLength)
				{
					if (!fieldInfo.Settings.AllowTruncation)
					{
						throw new InvalidDataException(
                            $"The value of '{fieldInfo.MemberInfo.Name}' cannot exceed {fieldInfo.Settings.MaximumLength} characters.");
					}
					lengthAdjustedValue = minimumFormattedValue.Substring(0, maximumLength);
				}
				else
				{
					lengthAdjustedValue = minimumFormattedValue;
				}
			}

			return lengthAdjustedValue;
		}

		private static string PadValue(string originalValue, int length, FieldAlignment alignment, char paddingCharacter)
		{
			switch (alignment)
			{
				case FieldAlignment.Left:
					return originalValue.PadRight(length, paddingCharacter);
				case FieldAlignment.Right:
					return originalValue.PadLeft(length, paddingCharacter);
				default:
					throw new NotSupportedException();
			}
		}
		
		private static string GetQuotedValue(string value, RecordSettings recordSettings, FieldInfo fieldInfo)
		{
			string quoteCharacter = recordSettings.Options.QuoteEverythingWith;
			string overrideCharacter = fieldInfo?.Settings.AlwaysQuoteWith;
			if (!string.IsNullOrWhiteSpace(overrideCharacter))
			{
				quoteCharacter = overrideCharacter;
			}

			if (string.IsNullOrWhiteSpace(quoteCharacter))
			{
				return value;
			}
			
			if (quoteCharacter == GetEscapeCharacter(recordSettings.Options) &&
			    value.StartsWith(quoteCharacter) && 
			    value.EndsWith(quoteCharacter))
			{
				return value;
			}
			
			EscapeSettings escapeSettings = new EscapeSettings(recordSettings.RowIndex, fieldInfo?.MemberInfo.Name, recordSettings.Options.Clone());
			escapeSettings.Delimiter = quoteCharacter;
			escapeSettings.DelimiterName = "quote";
			escapeSettings.Options.EscapeKind = EscapeKind.Repeat;
			value = GetEscapedFieldValue(value, escapeSettings);

			return quoteCharacter + value + quoteCharacter;
		}

		private string AppendFieldDelimiter(StringBuilder record, string mainDelimiter, string overrideDelimiter)
		{
			string finalDelimiter;
			if (!string.IsNullOrEmpty(overrideDelimiter))
			{
				record.Append(overrideDelimiter);
				finalDelimiter = overrideDelimiter;
			}
			else
			{
				record.Append(mainDelimiter);
				finalDelimiter = mainDelimiter;
			}

			return finalDelimiter ?? string.Empty;
		}

        [SuppressMessage("ReSharper", "FormatStringProblem")]
        private static string GetFormattedValue(object rawValue, string settingsFormat)
        {
            if (rawValue == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(settingsFormat))
            {
                if (rawValue is DateTime)
                {
                    DateTime dateTimeValue = (DateTime) rawValue;
                    return dateTimeValue.ToString(settingsFormat, CultureInfo.InvariantCulture.DateTimeFormat);
                }

                return string.Format("{0:" + settingsFormat + "}", rawValue);
            }

            return rawValue.ToString();
        }

        public static FormatWriter Default => (FormatWriter) Instance;
        public static IFormatWriter Instance { get; }
	}
}