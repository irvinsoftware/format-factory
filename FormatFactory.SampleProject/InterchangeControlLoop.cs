using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class InterchangeControlLoop
	{
		[Field]
		public string Prefix { get { return "ISA"; } }

		[FixedWidthField(MaximumLength = 2, PaddingCharacter = '0')]
		public int AuthorizationInformationQualifier { get { return 0; } }

		[FixedWidthField(MaximumLength = 10)]
		public string AuthorizationInformation { get { return string.Empty; }}

		[Field]
		public string SecurityInformationQualifier { get { return "00"; } }

		[FixedWidthField(MaximumLength = 10)]
		public string SecurityInformation { get { return string.Empty; } }

		[Field]
		public int SenderInterchangeQualifier { get { return 30; } }

		[FixedWidthField(MaximumLength = 15)]
		public int InterchangeSenderID { get; set; }

		[Field]
		public string ReceiverInterchangeQualifier { get { return "ZZ"; } }

		[FixedWidthField(MaximumLength = 15)]
		public string InterchangeReceiverID { get { return "EYEMED"; } }

		[Field(Format = FieldFormat.ShortYearFirstUndelimitedDate)]
		public DateTime InterchangeDate { get; set; }

		[Field(Format = FieldFormat.HourMinuteFixedLength24HourClock)]
		public DateTime InterchangeTime { get; set; }

		[Field]
		public char ControlStandardsIdentifier { get { return '^'; } }

		[Field]
		public string InterchangeVersionNumber { get { return "00501"; } }

		[FixedWidthField(MaximumLength = 9, Alignment = FieldAlignment.Right, PaddingCharacter = '0')]
		public int InterchangeControlNumber { get; set; }

		[Field]
		public int AcknowledgmentRequested { get { return 0; } }

		[Field]
		public char UsageIndicator { get; set; }

		[Field]
		public char ElementSeparator { get { return ':'; } }
	}
}