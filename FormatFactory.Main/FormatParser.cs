using Irvin.FormatFactory.Internal;
using Irvin.Parser;

namespace Irvin.FormatFactory
{
    internal class FormatParser : Parser.Parser
    {
        protected readonly FormatOptions _readOptions;

        public FormatParser(FormatOptions readOptions)
        {
            _readOptions = readOptions;
        }

        protected override ParserSettings GetSettings()
        {
            return FillParserSettingsFromRecordOptions(_readOptions);
        }

        protected ParserSettings FillParserSettingsFromRecordOptions(IRecordOptions options)
        {
            ParserSettings settings = new ParserSettings();
            FillParserSettingsFromRecordOptions(settings, options);
            return settings;
        }

        protected static void FillParserSettingsFromRecordOptions(ParserSettings settings, IRecordOptions options)
        {
            settings.AddDelimiter(options.RecordDelimiter);
            settings.AddDelimiter(options.FieldDelimiter);

            if (options.EscapeKind == EscapeKind.Backslash)
            {
                settings.AddEscapeCharacter("\\");
            }

            if (options.EscapeKind == EscapeKind.ForwardSlash)
            {
                settings.AddEscapeCharacter("/");
            }

            if (options.EscapeKind == EscapeKind.DoubleQuote)
            {
                settings.Subgroups.Add(new SubgroupSettings
                {
                    StartSymbol = "\"",
                    EndSymbol = "\""
                });
            }

            if (options.EscapeKind == EscapeKind.SingleQuote)
            {
                settings.Subgroups.Add(new SubgroupSettings
                {
                    StartSymbol = "'",
                    EndSymbol = "'"
                });
            }
        }
    }
}