using Irvin.FormatFactory;

namespace Company.Entities.Malformed
{
	[Record(RecordDelimiter = "|", FieldDelimiter = ",")]
	public class BadE
	{
		[Field(IsFixedWidth = true, MaximumLength = 5)]
		public char Gorzo { get; set; }
	}
}