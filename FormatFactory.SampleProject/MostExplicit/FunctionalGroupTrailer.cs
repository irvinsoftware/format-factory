using Irvin.FormatFactory;

namespace Company.Entities.MostExplicit
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class FunctionalGroupTrailer
	{
		[Field(Order = 1)]
		public string Prefix { get { return "GE"; } }

		[Field(Order = 2)]
		public int TransactionSetCount { get; set; }

		[Field(Order = 3)]
		public string ControlNumber { get; set; }
	}
}