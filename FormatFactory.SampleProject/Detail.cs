using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(FieldDelimiter = ";", RecordDelimiter = "|")]
	public class Detail
	{
		[Field]
		public int Samwise { get { return 45; } }

		[Field]
		public bool Gamgee { get { return true; }}
	}
}