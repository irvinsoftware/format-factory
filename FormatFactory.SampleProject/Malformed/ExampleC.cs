using Irvin.FormatFactory;

namespace Company.Entities.Malformed
{
	[Record]
	public class ExampleC
	{
		[SubRecord]
		[ChildElement]
		public string Name { get; set; }
	}
}