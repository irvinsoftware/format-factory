using Irvin.FormatFactory;

namespace Company.Entities.Inherited
{
	public class SalesTaxSegment : EdiBase
	{
		public SalesTaxSegment() : base("AMT") { }

		[Field(Order = 2)]
		public char AmountQualifier { get { return 'T'; } }

		[Field(Order = 3, Format = FieldFormat.FewestDigits)]
		public decimal MonetaryAmount { get; set; }
	}
}