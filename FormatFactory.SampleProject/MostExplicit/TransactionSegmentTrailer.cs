using Irvin.FormatFactory;

namespace Company.Entities.MostExplicit
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class TransactionSegmentTrailer
	{
		[Field(Order = 1)]
		public string Prefix { get { return "SE"; } }

		[Field(Order = 2)]
		public int SegmentCount { get; set; }

		[Field(Order = 3, IsFixedWidth = true, PaddingCharacter = '0', Alignment = FieldAlignment.Right, MaximumLength = 4)]
		public int ControlNumber { get; set; }
	}
}