using Irvin.FormatFactory;

namespace Company.Entities.Inherited
{
	public class FunctionalGroupTrailer : EdiBase
	{
		public FunctionalGroupTrailer() : base("GE") { }

		[Field(Order = 2)]
		public int TransactionSetCount { get; set; }

		[Field(Order = 3)]
		public string ControlNumber { get; set; }
	}
}