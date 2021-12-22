using Irvin.FormatFactory;
using Company.Nacha.NachaElements.Enum;

namespace Company.Nacha.NachaElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class FileControl
	{
		[FixedWidthField(Order = 1, MaximumLength = 1, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = "D")]
		public RecordTypeCode RecordTypeCode => RecordTypeCode.FileControlRecord;

		[FixedWidthField(Order = 2, MaximumLength = 6, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int BatchCount;

		[FixedWidthField(Order = 3, MaximumLength = 6, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int BlockCount;

		[FixedWidthField(Order = 4, MaximumLength = 8, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int EntryCount;

		[FixedWidthField(Order = 5, MaximumLength = 10, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public long EntryHash;

		[FixedWidthField(Order = 6, MaximumLength = 12, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public string TotalDebitAmmount;

		[FixedWidthField(Order = 7, MaximumLength = 12, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public string TotalCreditAmmount;

		[FixedWidthField(Order = 8, MaximumLength = 39, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string Reserved => string.Empty;


	}
}
