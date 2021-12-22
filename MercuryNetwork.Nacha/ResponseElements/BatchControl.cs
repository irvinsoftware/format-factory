using Irvin.FormatFactory;
using Company.Nacha.NachaElements.Enum;

namespace Company.Nacha.ResponseElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class BatchControl
	{
		[FixedWidthField(Order = 1, MaximumLength = 1, PaddingCharacter = '0', Alignment = FieldAlignment.Right, Format = "D")]
		public RecordTypeCode RecordTypeCode => RecordTypeCode.BatchControlRecord;

		[FixedWidthField(Order = 2, MaximumLength = 3, PaddingCharacter = '0', Alignment = FieldAlignment.Right, Format = "D")]
		public ServiceClassCode ServiceClassCode;

		[FixedWidthField(Order = 3, MaximumLength = 6, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int EntryCount;

		[FixedWidthField(Order = 4, MaximumLength = 10, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public long EntryHash;

		[FixedWidthField(Order = 5, MaximumLength = 12, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public decimal TotalDebitAmmount;

		[FixedWidthField(Order = 6, MaximumLength = 12, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public decimal TotalCreditAmmount;

		[FixedWidthField(Order = 7, MaximumLength = 10, PaddingCharacter = '1', Alignment = FieldAlignment.Right)]
		public string CompanyId;

		[FixedWidthField(Order = 8, MaximumLength = 19, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string MessageAuthenticationCode =>string.Empty;

		[FixedWidthField(Order = 9, MaximumLength = 6, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string Blanks =>string.Empty;

		[FixedWidthField(Order = 10, MaximumLength = 8, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public long OriginatingDfiId;

		[FixedWidthField(Order = 11, MaximumLength = 7, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int BatchNumber;
	}
}
