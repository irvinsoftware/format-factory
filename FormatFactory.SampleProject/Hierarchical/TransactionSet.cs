using Irvin.FormatFactory;

namespace Company.Entities.Hierarchical
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class TransactionSet
	{
		[Field]
		public string PlanType { get; set; }
	}
}