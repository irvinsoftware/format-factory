using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(RecordDelimiter = "|", FieldDelimiter = ",")]
	public class GoodD
	{
		[Field(IsFixedWidth = true, MinimumLength = 4, MaximumLength = 8, PaddingCharacter = 'z', Alignment = FieldAlignment.Left)]
		public string Gorzo { get; set; }

		[Field(IsFixedWidth = false, MinimumLength = 4, MaximumLength = 8, PaddingCharacter = 'z', Alignment = FieldAlignment.Left)]
		public string JackieChan { get; set; }

		[Field(IsFixedWidth = true, MinimumLength = 5)]
		public bool WWF { get; set; }  
	}
}