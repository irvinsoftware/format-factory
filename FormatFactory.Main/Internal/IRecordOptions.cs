namespace Irvin.FormatFactory.Internal
{
    internal interface IRecordOptions
	{
		string RecordDelimiter { get; }
		string FieldDelimiter { get; }
		bool UseTrailingDelimiter { get; }
        bool IncludeHeaders { get; }
        EscapeKind EscapeKind { get; }
        char TransformEscapeCharacter { get; }
        bool UseStrictMode { get; }
	}
}