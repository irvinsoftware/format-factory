using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(FieldDelimiter = ",", RecordDelimiter = "|")]
	public class Master
	{
		[Field]
		public string FirstName { get; set; }

		[Field]
		public string LastName { get; set; }

		[Field]
		public string Description { get; set; }

		[Field(Format = FieldFormat.Money)]
		public decimal Money { get; set; }

		[ChildElement("~~~")]
		public Detail Detail { get; set; }
	}
}