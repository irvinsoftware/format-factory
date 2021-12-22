namespace Irvin.FormatFactory.Internal
{
	internal sealed class EscapeSettings
	{
		public EscapeSettings(int rowIndex, string memberName, FormatOptions options)
		{
			RowIndex = rowIndex;
			MemberName = memberName;
			Options = options;
		}

		public int RowIndex { get; }
		public string Delimiter { get; set; }
		public string DelimiterName { get; set; }
		public string MemberName { get; }
		public FormatOptions Options { get; }
	}
}