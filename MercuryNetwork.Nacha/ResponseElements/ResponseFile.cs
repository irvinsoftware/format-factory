using System.Collections.Generic;
using Irvin.FormatFactory;

namespace Company.Nacha.ResponseElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class ResponseFile
	{
		[ChildElement(Order = 1, IndentCharacters = "", IsRequired = true)]
		public FileHeaderRecord FileHeaderRecord { get; set; }

		[ChildElement(Order = 2, IndentCharacters = "", IsRequired = true)]
		public List<Batch> Batches { get; set; }

		[ChildElement(Order = 3, IndentCharacters = "", IsRequired = true)]
		public FileControl FileControl { get; set; }
	}
}