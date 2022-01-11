using System;
using System.Globalization;
using System.Linq;
using Irvin.FormatFactory.Internal;

namespace Irvin.FormatFactory
{
    /// <summary>
    /// Specify options for the recordset as a whole. 
    /// This values will be overridden by the record and field settings specified in the decorated attributes.
    /// </summary>
	public class FormatOptions : IRecordOptions
	{
	    internal const StringComparison DefaultCompareOption = StringComparison.CurrentCultureIgnoreCase;

        /// <summary> 
        /// </summary>
	    public FormatOptions()
		{
			RecordDelimiter = DefaultRecordDelimiter;
			AllowDelimitersAsEscapedContent = true;
			EscapeKind = DefaultEscape;
            CompareOption = DefaultCompareOption;
		}

	    internal static string DefaultRecordDelimiter
	    {
	        get { return Environment.NewLine; }
	    }

	    internal static EscapeKind DefaultEscape
	    {
	        get { return EscapeKind.DoubleQuote; }
	    }

	    internal StringComparison CompareOption { get; }

        /// <summary>
        /// The characters to use to indent each record
        /// </summary>
		public string IndentCharacters { get; set; }

		public string RecordDelimiter { get; set; }

		public string FieldDelimiter { get; set; }

        /// <summary>
        /// Output/expect the field delimiter to appear after the last field
        /// </summary>
		public bool UseTrailingDelimiter { get; set; }

		/// <summary>
		/// Strict Mode means properties &amp; fields not decorated will cause an error
		/// </summary>
		public bool UseStrictMode { get; set; }

		/// <summary>
		/// When specified, every field value will be encapsulated with these character(s).
		/// </summary>
		public string QuoteEverythingWith { get; set; }

		public bool AllowDelimitersAsEscapedContent { get; set; }

		public EscapeKind EscapeKind { get; set; }

        /// <summary>
        /// When <see cref="EscapeKind"/> is set to Transform, this is the character used.
        /// </summary>
		public char TransformEscapeCharacter { get; set; }

	    public bool IncludeHeaders { get; set; }

	    public FormatOptions Clone()
		{
			return new FormatOptions
				{
					RecordDelimiter = RecordDelimiter,
					FieldDelimiter = FieldDelimiter,
					UseTrailingDelimiter = UseTrailingDelimiter,
					UseStrictMode = UseStrictMode,
					AllowDelimitersAsEscapedContent = AllowDelimitersAsEscapedContent,
					EscapeKind = EscapeKind,
					TransformEscapeCharacter = TransformEscapeCharacter,
					IndentCharacters = IndentCharacters,
                    IncludeHeaders = IncludeHeaders,
                    QuoteEverythingWith = QuoteEverythingWith
				};
		}

	    internal IFieldSettings DefaultFieldSetup { get; set; }

	    public static FormatOptions DefaultCsvSettings
        {
            get
            {
                return new FormatOptions
                {
                    RecordDelimiter = Environment.NewLine.Last().ToString(),
					FieldDelimiter = ",",
					UseTrailingDelimiter = false,
					IncludeHeaders = true,
					UseStrictMode = false,
					EscapeKind = EscapeKind.DoubleQuote,
					TransformEscapeCharacter = default(char)
				};
            }
        }

	    public string EscapeCharacter
	    {
		    get
		    {
			    switch (EscapeKind)
			    {
				    case EscapeKind.SingleQuote:
					    return "'";
				    case EscapeKind.DoubleQuote:
					    return "\"";
				    case EscapeKind.Backslash:
					    return "\\";
				    case EscapeKind.ForwardSlash:
					    return "/";
				    case EscapeKind.Transform:
					    return TransformEscapeCharacter.ToString(CultureInfo.InvariantCulture);
				    case EscapeKind.Remove:
					    return string.Empty;
				    case EscapeKind.Repeat:
				    default:
					    return null;
			    }
		    }
	    }
	}
}