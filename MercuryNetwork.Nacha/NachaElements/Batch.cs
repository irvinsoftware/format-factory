using System.Collections.Generic;
using Irvin.FormatFactory;

namespace Company.Nacha.NachaElements
{
	[Record(FieldDelimiter = "", RecordDelimiter = "", UseTrailingDelimiter = false)]
	public class Batch
	{
		[ChildElement("", IsRequired = true)]
		public BatchHeader BatchHeader { get; set; }

		[ChildElement("", IsRequired = true)]
		public List<EntryDetail> EntryDetails { get; set; }

		[ChildElement("", IsRequired = true)]
		public BatchControl BatchControl { get; set; }
	}
}