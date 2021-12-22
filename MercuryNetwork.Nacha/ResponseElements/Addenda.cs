using System;
using Irvin.FormatFactory;
using Company.Nacha.NachaElements.Enum;

namespace Company.Nacha.ResponseElements
{
    [Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
    public class Addenda
	{

		[FixedWidthField(Order = 1, MaximumLength = 1, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = "D")]
		public RecordTypeCode RecordType => RecordTypeCode.EntryDetailAddendaRecord;

		[FixedWidthField(Order = 2, MaximumLength = 2, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = "D")]
		public AddendaTypeCode AddendaType;

		[FixedWidthField(Order = 3, MaximumLength = 3, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = "D")]
		public ReturnReasonCode ReturnReason;

		[FixedWidthField(Order = 4, MaximumLength = 8, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public long OriginalTraceNumberA;

		[FixedWidthField(Order = 5, MaximumLength = 7, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int OriginalTraceNumberB;

		[FixedWidthField(Order = 6, MaximumLength = 6, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = FieldFormat.ShortYearFirstUndelimitedDate)]
		public DateTime DateOfDeath;

		[FixedWidthField(Order = 7, MaximumLength = 8, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public string OriginalDfiRoutingNumber;

		[FixedWidthField(Order = 8, MaximumLength =44, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string AddendaInfo;

		[FixedWidthField(Order = 9, MaximumLength = 15, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int CurrentTraceNumber;
	}
}
