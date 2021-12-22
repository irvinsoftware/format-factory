using Irvin.FormatFactory;

namespace Company.Entities.MostExplicit
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class SalesTaxSegment
	{
		[Field(Order = 1)]
		public string Prefix { get { return "AMT"; } }

		[Field(Order = 2)]
		public char AmountQualifier { get { return 'T'; } }

		[Field(Order = 3, Format = FieldFormat.FewestDigits)]
		public decimal MonetaryAmount { get; set; }
	}
}