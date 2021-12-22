using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(RecordDelimiter = "|", FieldDelimiter = ",")]
	public class GoodE
	{
		[Field(IsFixedWidth = true, MinimumLength = 4, MaximumLength = 8, PaddingCharacter = 'z', Alignment = FieldAlignment.Left)]
		public string Gorzo { get; set; }

		[Field(IsFixedWidth = false, MinimumLength = 4, MaximumLength = 8, PaddingCharacter = 'z', Alignment = FieldAlignment.Left)]
		internal string JackieChan { get { return "la";  } }

		[Field(IsFixedWidth = true, MinimumLength = 5)]
		private bool WWF { get { return true; } }
	}
}