using System;
using Irvin.FormatFactory.Internal;

namespace Irvin.FormatFactory
{
    /// <summary> 
    /// </summary>
    [DefaultFieldSetup]
    [AttributeUsage(AttributeTargets.Class)]
    public class CsvRecordAttribute : Attribute, IRecordOptions
    {
        /// <summary>
        /// <seealso cref="RecordAttribute.RecordDelimiter"/>. Returns <seealso cref="Environment.NewLine"/>.
        /// </summary>
        public string RecordDelimiter
        {
            get { return FormatOptions.DefaultCsvSettings.RecordDelimiter; }
        }

        /// <summary>
        /// <seealso cref="RecordAttribute.FieldDelimiter"/>. Returns comma.
        /// </summary>
        public string FieldDelimiter
        {
            get { return FormatOptions.DefaultCsvSettings.FieldDelimiter; }
        }

        /// <summary>
        /// <seealso cref="RecordAttribute.UseTrailingDelimiter"/>. Returns false.
        /// </summary>
        public bool UseTrailingDelimiter
        {
            get { return FormatOptions.DefaultCsvSettings.UseTrailingDelimiter; }
        }

        /// <summary>
        /// <seealso cref="RecordAttribute.IncludeHeaders"/>. Returns true.
        /// </summary>
        public bool IncludeHeaders
        {
            get { return FormatOptions.DefaultCsvSettings.IncludeHeaders; }
        }

        /// <summary>
        /// <seealso cref="RecordAttribute.UseStrictMode"/>. Return false.
        /// </summary>
        public bool UseStrictMode
        {
            get { return FormatOptions.DefaultCsvSettings.UseStrictMode; }
        }

        /// <summary>
        /// <seealso cref="RecordAttribute.EscapeKind"/>. Returns double-quote.
        /// </summary>
        public EscapeKind EscapeKind
        {
            get { return FormatOptions.DefaultCsvSettings.EscapeKind; }
        }

        /// <summary>
        /// <seealso cref="RecordAttribute.TransformEscapeCharacter"/>
        /// </summary>
        public char TransformEscapeCharacter
        {
            get { return FormatOptions.DefaultCsvSettings.TransformEscapeCharacter; }
        }
    }
}