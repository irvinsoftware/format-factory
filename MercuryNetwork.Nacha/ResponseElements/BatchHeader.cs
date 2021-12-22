using System;
using Irvin.FormatFactory;
using Company.Nacha.NachaElements.Enum;

namespace Company.Nacha.ResponseElements
{
	[Record(FieldDelimiter = "",RecordDelimiter = "",UseTrailingDelimiter = false) ]
	public class BatchHeader
	{
		[FixedWidthField(Order = 1, MaximumLength = 1, PaddingCharacter = '0', Alignment = FieldAlignment.Right, Format = "D")]
		public RecordTypeCode RecordTypeCode => RecordTypeCode.BatchHeaderRecord;

		[FixedWidthField(Order = 2, MaximumLength = 3, PaddingCharacter = '0', Alignment = FieldAlignment.Right, Format = "D")]
		public ServiceClassCode ServiceClassCode;

		[FixedWidthField(Order = 3, MaximumLength = 16, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string CompanyName;

		[FixedWidthField(Order = 4, MaximumLength = 20, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string DiscretionaryData;

		[FixedWidthField(Order = 5, MaximumLength = 10, PaddingCharacter = '1', Alignment = FieldAlignment.Right)]
		public string CompanyId;

		[FixedWidthField(Order = 6, MaximumLength = 3, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public StandardEntryClass StandardEntryClassCode;

		[FixedWidthField(Order = 7, MaximumLength = 10, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string CompanyEntryDescription;

		[FixedWidthField(Order = 8, MaximumLength = 6, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = FieldFormat.ShortYearFirstUndelimitedDate)]
		public DateTime CompanyDescriptiveDate;

		[FixedWidthField(Order = 9, MaximumLength = 6, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = FieldFormat.ShortYearFirstUndelimitedDate)]
		public DateTime EffectiveEntryDate;

		[FixedWidthField(Order = 10, MaximumLength = 3, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string SettlementDate;  // reserve for ACH operator should always be empty

		[FixedWidthField(Order = 11, MaximumLength = 1, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string OriginatorStatusCode;

		[FixedWidthField(Order = 12, MaximumLength = 8, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string OriginatingFinancialInstitutionId;

		[FixedWidthField(Order = 13, MaximumLength = 7,PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int BatchNumber;
	}
}
