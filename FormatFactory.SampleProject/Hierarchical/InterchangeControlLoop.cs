using System;
using Irvin.FormatFactory;

namespace Company.Entities.Hierarchical
{
	[Record]
	public class InterchangeControlLoop
	{
		[Field(Order = 1)]
		public string Prefix { get { return "ISA"; } }

		[Field(Order = 2, IsFixedWidth = true, MaximumLength = 2, Alignment = FieldAlignment.Left, PaddingCharacter = '0')]  //this is more complex than it needs to be, I'm testing the framework
		public int AuthorizationInformationQualifier { get { return 0; } }

		[Field(Order = 3, IsFixedWidth = true, MaximumLength = 10)]
		public string AuthorizationInformation { get { return string.Empty; } }

		[Field(Order = 4)]
		public string SecurityInformationQualifier { get { return "00"; } }

		[Field(Order = 5, IsFixedWidth = true, MaximumLength = 10)]
		public string SecurityInformation { get { return string.Empty; } }

		[Field(Order = 6)]
		public int SenderInterchangeQualifier { get { return 30; } }

		[Field(Order = 7, IsFixedWidth = true, MaximumLength = 15)]
		public int InterchangeSenderID { get; set; }

		[Field(Order = 8)]
		public string ReceiverInterchangeQualifier { get { return "ZZ"; } }

		[Field(Order = 9, IsFixedWidth = true, MaximumLength = 15)]
		public string InterchangeReceiverID { get { return "EYEMED"; } }

		[Field(Order = 10, Format = FieldFormat.ShortYearFirstUndelimitedDate)]
		public DateTime InterchangeDate { get; set; }

		[Field(Order = 11, Format = FieldFormat.HourMinuteFixedLength24HourClock)]
		public DateTime InterchangeTime { get; set; }

		[Field(Order = 12)]
		public char ControlStandardsIdentifier { get { return '^'; } }

		[Field(Order = 13)]
		public string InterchangeVersionNumber { get { return "00501"; } }

		[Field(Order = 14, IsFixedWidth = true, MaximumLength = 9, Alignment = FieldAlignment.Right, PaddingCharacter = '0')]
		public int InterchangeControlNumber { get; set; }

		[Field(Order = 15)]
		public int AcknowledgmentRequested { get { return 0; } }

		[Field(Order = 16)]
		public char UsageIndicator { get; set; }

		[Field(Order = 17)]
		public char ElementSeparator { get { return ':'; } }

		[ChildElement(Order = 18)]
		public FunctionalGroupHeader FunctionalGroup { get; set; }

		[ChildElement(Order = 19)]
		public InterchangeControlTrailer Trailer { get; set; }
	}
}