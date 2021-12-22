using System.Collections.Generic;
using Irvin.FormatFactory;

namespace Company.Entities.Hierarchical
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class FunctionalGroupHeader
	{
		[Field]
		public int Samuel { get; set; }

		[ChildElement]
		public List<TransactionSet> TransactionSets { get; set; }
	}
}