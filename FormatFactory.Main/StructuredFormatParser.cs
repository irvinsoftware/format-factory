using System;
using System.Linq;
using Irvin.Extensions;
using Irvin.FormatFactory.Internal;
using Irvin.FormatFactory.Internal.Member;
using Irvin.Parser;

namespace Irvin.FormatFactory
{
    internal class StructuredFormatParser : FormatParser
    {
        private readonly RecordSettings _recordSettings;

        public StructuredFormatParser(RecordSettings recordSettings, FormatOptions readOptions)
            : base(readOptions)
        {
            _recordSettings = recordSettings;
        }
        
        protected override ParserSettings GetSettings()
        {
            ParserSettings settings = FillParserSettingsFromRecordOptions(_recordSettings.Options);

            foreach (IOrderedMemberInfo recordMemberInfo in _recordSettings.OrderedMembers)
            {
                FieldInfo fieldInfo = recordMemberInfo as FieldInfo;
                if (fieldInfo != null)
                {
                    settings.AddDelimiter(fieldInfo.Settings.Delimiter);
                }
                else
                {
                    Type recordType = TypeFactory.GetMemberType(recordMemberInfo.MemberInfo);

                    if (!recordMemberInfo.IsInternalToRecord)
                    {
                        settings.AddDelimiter(recordMemberInfo.IndentCharacters);

                        if (recordType.IsGenericList())
                        {
                            Type listElementType = recordType.GetGenericArguments().First();
                            recordType = listElementType;
                        }
                    }

                    RecordSettings subRecordSettings = SettingsFactory.GetRecordSettingsForRead(recordType, _readOptions);
                    FillParserSettingsFromRecordOptions(settings, subRecordSettings.Options);
                }
            }

            return settings;
        }
    }
}