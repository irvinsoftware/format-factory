using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Irvin.Extensions;
using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
    internal static class SettingsFactory
    {
        internal static RecordSettings GetRecordSettingsForRead(Type elementType, FormatOptions formatOptions)
        {
            IRecordOptions recordAttribute = TypeFactory.GetRecordAttribute(elementType);
            RecordSettings settings = BuildRecordSettings(elementType, recordAttribute, formatOptions);
            settings.Options.IncludeHeaders = recordAttribute.IncludeHeaders;

            ThrowIfSetupCannotBeParsed(settings.OrderedMembers);

            return settings;
        }

        private static void ThrowIfSetupCannotBeParsed(IEnumerable<IOrderedMemberInfo> orderedFields)
        {
            bool isUnparseable =
                orderedFields
                    .Where(memberDetails => !memberDetails.IsInternalToRecord)
                    .Select(childMemberDetails => childMemberDetails.MemberInfo)
                    .Where(propertyInfo => propertyInfo.MemberType.IsGenericList())
                    .Select(childMemberDetail => new
                    {
                        ChildMemberName = childMemberDetail.Name,
                        ElementType = childMemberDetail.MemberType.GetGenericArguments().First()
                    })
                    .GroupBy(x => x.ElementType)
                    .Any(x => x.Count() > 1);

            if (isUnparseable)
            {
                throw new InvalidUsageException("Two or more child lists of the same type is not parseable.");
            }
        }

        internal static RecordSettings GetRecordSettingsForWrite(Type elementType, FormatOptions formatOptions)
        {
            IRecordOptions recordAttribute = TypeFactory.GetRecordAttribute(elementType);
            return BuildRecordSettings(elementType, recordAttribute, formatOptions);
        }

        private static RecordSettings BuildRecordSettings(Type elementType, IRecordOptions recordAttribute, FormatOptions formatOptions)
        {
            FormatOptions options = GetFormatOptions(recordAttribute, formatOptions);
            List<IOrderedMemberInfo> unorderedMemberList = GetAllMemberInfos(elementType, options);
            IOrderedMemberInfo[] orderedFields = GetOrderedMemberList(unorderedMemberList);

            return new RecordSettings(orderedFields, options)
            {
                RecordType = elementType
            };
        }

        private static FormatOptions GetFormatOptions(IRecordOptions recordAttribute, FormatOptions overrideOptions)
        {
            FormatOptions options = new FormatOptions();

            if (overrideOptions != null)
            {
                options = overrideOptions.Clone();
            }
            if (recordAttribute.HasOwnRecordDelimiter())
            {
                options.RecordDelimiter = recordAttribute.RecordDelimiter;
            }
            if (!string.IsNullOrEmpty(recordAttribute.FieldDelimiter))
            {
                options.FieldDelimiter = recordAttribute.FieldDelimiter;
            }
            if (recordAttribute.UseTrailingDelimiter)
            {
                options.UseTrailingDelimiter = true;
            }
            if (recordAttribute.UseStrictMode)
            {
                options.UseStrictMode = true;
            }
            if (recordAttribute.IncludeHeaders)
            {
                options.IncludeHeaders = true;
            }
            if (recordAttribute.EscapeKind != FormatOptions.DefaultEscape)
            {
                options.EscapeKind = recordAttribute.EscapeKind;
            }
            if (recordAttribute.TransformEscapeCharacter != default(char))
            {
                options.TransformEscapeCharacter = recordAttribute.TransformEscapeCharacter;
            }
            if (!string.IsNullOrWhiteSpace(recordAttribute.QuoteEverythingWith))
            {
                options.QuoteEverythingWith = recordAttribute.QuoteEverythingWith;
            }

            options.DefaultFieldSetup = recordAttribute.GetType().GetAttribute<DefaultFieldSetupAttribute>();

            return options;
        }

        private static readonly ConcurrentDictionary<MemberInfo, SubElementTemplate> _memberTemplateCache = new ConcurrentDictionary<MemberInfo, SubElementTemplate>();

        private static List<IOrderedMemberInfo> GetAllMemberInfos(Type elementType, FormatOptions options)
        {
            List<IOrderedMemberInfo> memberInfos = new List<IOrderedMemberInfo>();

            int decorations = 0;
            IEnumerable<MemberInfo> typeMembers = TypeFactory.GetTypeMembers(elementType);
            foreach (MemberInfo typeMember in typeMembers)
            {
                SubElementTemplate template = _memberTemplateCache.GetOrAdd(typeMember, memberInfo => new SubElementTemplate(memberInfo));

                if (template.IsEmpty && options.UseStrictMode)
                {
                    throw new InvalidUsageException("In strict mode, all members must be decorated.");
                }

                if (template.FieldAttribute != null)
                {
                    decorations++;
                    memberInfos.Add(template.ToFieldInfo());
                }
                else if (template.ChildElementAttribute != null)
                {
                    decorations++;
                    memberInfos.Add(template.ChildElementInfo());
                }
                else if (template.SubRecordAttribute != null)
                {
                    decorations++;
                    memberInfos.Add(template.ToSubRecordInfo());
                }
                else if (options.DefaultFieldSetup != null && decorations == 0)
                {
                    FieldInfo fieldInfo = new FieldInfo();
                    fieldInfo.MemberInfo = MemberInfoFactory.Get(template.TypeMember);
                    fieldInfo.Settings = options.DefaultFieldSetup;
                    memberInfos.Add(fieldInfo);
                }
            }

            if (decorations > 0)
            {
                memberInfos.RemoveAll(memberInfo => memberInfo.HasSameFieldSettingsAs(options));
            }

            return memberInfos;
        }

        private static IOrderedMemberInfo[] GetOrderedMemberList(IReadOnlyCollection<IOrderedMemberInfo> recordFields)
        {
            IOrderedMemberInfo[] orderedFields = new IOrderedMemberInfo[recordFields.Count];

            Queue<IOrderedMemberInfo> unorderedFields = new Queue<IOrderedMemberInfo>();
            foreach (IOrderedMemberInfo fieldInfo in recordFields)
            {
                uint order = fieldInfo.Order;
                if (order == 0)
                {
                    unorderedFields.Enqueue(fieldInfo);
                }
                else
                {
                    if (order > orderedFields.Length)
                    {
                        throw new InvalidOperationException(
                            $"There are fields orders that exceed the field count (e.g., field '{fieldInfo.MemberInfo.Name}' has order #{order})");
                    }
                    if (orderedFields[order - 1] != null)
                    {
                        throw new InvalidOperationException(
                            $"Fields '{orderedFields[order - 1].MemberInfo.Name}' and '{fieldInfo.MemberInfo.Name}' have the same order.");
                    }
                    orderedFields[order - 1] = fieldInfo;
                }
            }

            int lastElement = -1;
            for (int i = 0; i < orderedFields.Length; i++)
            {
                if (orderedFields[i] != null)
                {
                    lastElement = i;
                }
            }
            for (int i = 0; i <= lastElement; i++)
            {
                if (orderedFields[i] == null)
                {
                    throw new InvalidOperationException($"There are gaps in the order (field #{i + 1} is missing)");
                }
            }
            for (int i = lastElement + 1; i < orderedFields.Length; i++)
            {
                orderedFields[i] = unorderedFields.Dequeue();
            }

            return orderedFields;
        }

        public static RecordSettings GetSettingsForDataTable(DataTable dataTable, FormatOptions formatOptions)
        {
            IList<IOrderedMemberInfo> members = new List<IOrderedMemberInfo>();
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                DataColumn dataColumn = dataTable.Columns[i];
                FieldInfo fieldInfo = new FieldInfo();
                fieldInfo.Settings = new FieldAttribute((uint) (i + 1))
                {
                    Name = dataColumn.ColumnName
                };
                fieldInfo.MemberInfo = MemberInfoFactory.Get(dataColumn);
                members.Add(fieldInfo);
            }

            return new RecordSettings(members, formatOptions) {RecordType = typeof(DataRow)};
        }

        public static StringComparison GetCompareOption(FormatOptions formatOptions)
        {
            return formatOptions?.CompareOption ?? FormatOptions.DefaultCompareOption;
        }
    }
}