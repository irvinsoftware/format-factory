using Irvin.FormatFactory;

namespace Company.Nacha.NachaElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	class FillRecord
	{
		[FixedWidthField(Order = 1, MaximumLength = 94, PaddingCharacter = '9')]
		public string FillField;
	}
}
