using Irvin.FormatFactory;

namespace Company.Entities.Inherited
{
	[Record(FieldDelimiter = "*", RecordDelimiter = "~\r\n")]
	public class EdiBase
	{
		private readonly string _prefix;

		protected EdiBase(string prefix)
		{
			_prefix = prefix;
		}

		[Field(Order = 1)]
		public string Prefix { get { return _prefix; } }
	}
}