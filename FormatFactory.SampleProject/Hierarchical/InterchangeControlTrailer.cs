using Irvin.FormatFactory;

namespace Company.Entities.Hierarchical
{
	[Record]
	public class InterchangeControlTrailer
	{
		[Field(Order = 1)]
		public string Prefix { get { return "IEA"; } }

		[Field(Order = 2)]
		public int FunctionalGroupCount { get; set; }

		[Field(Order = 3, IsFixedWidth = true, PaddingCharacter = '0', Alignment = FieldAlignment.Right, MaximumLength = 9)]
		public int ControlNumber { get; set; }
	}
}