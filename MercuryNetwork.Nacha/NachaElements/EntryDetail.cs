using Irvin.FormatFactory;
using Company.Nacha.NachaElements.Enum;

namespace Company.Nacha.NachaElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class EntryDetail
	{
		[FixedWidthField(Order = 1, MaximumLength = 1, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = "D")]
		public RecordTypeCode RecordTypeCode => RecordTypeCode.EntryDetailRecord;

		[FixedWidthField(Order = 2, MaximumLength = 2, PaddingCharacter = '0', Alignment = FieldAlignment.Right,Format = "D")]
		public TransactionCode TransactionCode;

		[FixedWidthField(Order = 3, MaximumLength = 8, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public string ReceivingDfiRoutingNumber;

		[FixedWidthField(Order = 4, MaximumLength = 1, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int CheckDigit => CalculateCheckDigit(); //generate the check digit based off of the 8 digit ReceivingDfiRoutingNumber

		[FixedWidthField(Order = 5, MaximumLength = 17, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string ReceivingDfiAccountNumber;

		[FixedWidthField(Order = 6, MaximumLength = 10, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public string Amount;

		[FixedWidthField(Order = 7, MaximumLength = 15, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string IdNumber;

		[FixedWidthField(Order = 8, MaximumLength = 22, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string Name;

		[FixedWidthField(Order = 9, MaximumLength = 2, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string DiscretionaryData;

		[FixedWidthField(Order = 10, MaximumLength = 1, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int AddendaRecordIndicator;

		[FixedWidthField(Order = 11, MaximumLength = 8, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public long TraceNumberA;
		[FixedWidthField(Order = 12, MaximumLength = 7, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int TraceNumberB;

		private int CalculateCheckDigit()
		{
			int sum = 0;
			char[] digits = ReceivingDfiRoutingNumber.ToUpper().ToCharArray();
			const string weights = "37137137";

			for (int i = 0; i < digits.Length; i++)
			{
				int val;
				int positionWeight;

				int.TryParse(weights[i].ToString(), out positionWeight);
				int.TryParse(digits[i].ToString(), out val);

				sum += val * positionWeight;
			}

			int check = (10 - (sum % 10)) % 10;

			return check;
		}
	}
}
