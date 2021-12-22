using System.Collections.Generic;
using Irvin.FormatFactory;

namespace Company.Nacha.ResponseElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class Batch
	{
		[ChildElement(Order = 1, IndentCharacters = "", IsRequired = true)]
		public BatchHeader BatchHeader { get; set; }

		[ChildElement(Order = 2, IndentCharacters = "", IsRequired = true)]
		public List<Entries> EntryDetails { get; set; }

		[ChildElement(Order = 3, IndentCharacters = "", IsRequired = true)]
		public BatchControl BatchControl { get; set; }
	}
}