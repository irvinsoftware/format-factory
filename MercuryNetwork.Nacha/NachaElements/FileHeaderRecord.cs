using System;
using Irvin.FormatFactory;
using Company.Nacha.NachaElements.Enum;

namespace Company.Nacha.NachaElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class FileHeaderRecord
	{
		private string _fileIdModifier;

		[FixedWidthField(Order = 1, MaximumLength = 1, PaddingCharacter = ' ', Alignment = FieldAlignment.Left, Format = "D")]
		public RecordTypeCode RecordTypeCode => RecordTypeCode.FileHeaderRecord;

		[FixedWidthField(Order = 2, MaximumLength = 2, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string PriorityCode;

		[FixedWidthField(Order = 3, MaximumLength = 10, PaddingCharacter = ' ', Alignment = FieldAlignment.Right)]
		public string ImmediateDestination;

		[FixedWidthField(Order = 4, MaximumLength = 10, PaddingCharacter = ' ', Alignment = FieldAlignment.Right)]
		public long ImmediateOrigin;

		[FixedWidthField(Order = 5, MaximumLength = 10, Format = "yyMMddHHmm")]
		public DateTime FileCreationDateAndTime;

		[FixedWidthField(Order = 6, MaximumLength = 1, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string FileIdModifier //must be uppercase
		{
			get { return _fileIdModifier.ToUpper(); }
			set { _fileIdModifier = value; }
		}

		[FixedWidthField(Order = 7, MaximumLength = 3, PaddingCharacter = '0', Alignment = FieldAlignment.Right)]
		public int RecordSize;

		[FixedWidthField(Order = 8, MaximumLength = 2, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public int BlockingFactor;

		[FixedWidthField(Order = 9, MaximumLength = 1, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string FormatCode;

		[FixedWidthField(Order = 10, MaximumLength = 23, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string ImmediateDestinationName;

		[FixedWidthField(Order = 11, MaximumLength = 23, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string ImmediateOrginName;

		[FixedWidthField(Order = 12, MaximumLength = 8, PaddingCharacter = ' ', Alignment = FieldAlignment.Left)]
		public string ReferenceCode;
	}
}
