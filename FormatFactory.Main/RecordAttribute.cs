using System;
using Irvin.FormatFactory.Internal;

namespace Irvin.FormatFactory
{
	[AttributeUsage(AttributeTargets.Class)]
	public class RecordAttribute : Attribute, IRecordOptions
	{
        /// <summary> 
        /// </summary>
		public RecordAttribute()
		{
			RecordDelimiter = Environment.NewLine;
		    EscapeKind = FormatOptions.DefaultEscape;
		    TransformEscapeCharacter = default(char);
		}

		public string RecordDelimiter { get; set; }

		public string FieldDelimiter { get; set; }

	    /// <summary>
	    /// Output/expect the field delimiter to appear after the last field
	    /// </summary>
		public bool UseTrailingDelimiter { get; set; }
        
        public bool IncludeHeaders { get; set; }

        /// <summary>
        /// Strict Mode means properties &amp; fields not decorated will cause an error
        /// </summary>
        public bool UseStrictMode { get; set; }

        /// <summary>
        /// When specified, every field will be encapsulated with these character(s).
        /// </summary>
        public string QuoteEverythingWith { get; set; }

        public EscapeKind EscapeKind { get; }

	    /// <summary>
	    /// When <see cref="EscapeKind"/> is set to Transform, this is the character used.
	    /// </summary>
	    public char TransformEscapeCharacter { get; }
	}
}