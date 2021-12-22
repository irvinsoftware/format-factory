using Irvin.FormatFactory;

namespace Company.Entities.Malformed
{
	[Record]
	public class ExampleB
	{
		[Field]
		[ChildElement]
		public string Name { get; set; }
	}
}