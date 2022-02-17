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

		internal void ThrowIfInvalidDelimiterUsage()
		{
			if (!Options.AllowDelimitersAsEscapedContent)
			{
				string messageFooter =
					RowIndex >= 0
						? $"in element #{RowIndex + 1}"
						: "in the header";
				string descriptor =
					!string.IsNullOrWhiteSpace(MemberName)
						? $"field '{MemberName}'"
						: "sub-elements";

				string message =
					$"The {DelimiterName} delimiter ('{Delimiter}') " +
					$"was found in the content for {descriptor} {messageFooter}.";

				throw new InvalidDataException(message);
			}
		}
	}
}