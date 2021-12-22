using Irvin.FormatFactory;

namespace Company.Entities.Malformed
{
	[Record]
	public class ExampleA
	{
		[Field]
		[SubRecord]
		public string Name { get; set; }
	}
}