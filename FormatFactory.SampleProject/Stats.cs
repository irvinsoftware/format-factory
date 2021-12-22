using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(FieldDelimiter = ",")]
	public class Stats
	{
		[Field(Order = 1)]
		public string Name { get; set; }

		[Field(Order = 2)]
		public int FigureA;

		[Field(Order = 3)]
		public int FigureB;

		[Field(Order = 4)]
		public int FigureC { get { return 0; } }

		[Field(Order = 5)]
		public int FigureD;

		[Field(PaddingCharacter = '0', Alignment = FieldAlignment.Right, MinimumLength = 4)]
		public int Id { get; set; }
	}
}