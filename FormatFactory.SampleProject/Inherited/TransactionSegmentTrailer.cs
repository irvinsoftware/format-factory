using Irvin.FormatFactory;

namespace Company.Entities.Inherited
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class TransactionSegmentTrailer : EdiBase
	{
		public TransactionSegmentTrailer() : base("SE") { }

		[Field(Order = 2)]
		public int SegmentCount { get; set; }

		[Field(Order = 3, IsFixedWidth = true, PaddingCharacter = '0', Alignment = FieldAlignment.Right, MaximumLength = 4)]
		public int ControlNumber { get; set; }
	}
}