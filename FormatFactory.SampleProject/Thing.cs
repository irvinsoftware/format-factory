using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(FieldDelimiter = ";")]
	public class Thing
	{
		[Field(MaximumLength = 10, AllowTruncation = false)]
		public string Goober { get; set; }

        [Field]
        public StringComparison CompareKind { get; set; }

        [Field(Format = FieldFormat.EnumValue)]
        public StringComparison C2 { get; set; }

        [Field(Format = FieldFormat.EnumName)]
        public StringComparison C3 { get; set; }
    }
}