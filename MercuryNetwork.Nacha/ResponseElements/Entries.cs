using Irvin.FormatFactory;

namespace Company.Nacha.ResponseElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class Entries
	{
		[ChildElement(Order = 1, IndentCharacters = "", IsRequired = true)]
		public EntryDetail EntryDetail { get; set; }

		[ChildElement(Order = 2, IndentCharacters = "", IsRequired = true)]
		public Addenda Addenda { get; set; }
	}
}