using Irvin.FormatFactory;

namespace Company.Entities.Malformed
{
	[Record(RecordDelimiter = "|", FieldDelimiter = ",")]
	public class BadD
	{
		[Field(MinimumLength = 4, MaximumLength = 3)]
		public string Gorzo { get; set; } 
	}
}