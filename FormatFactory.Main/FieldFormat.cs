namespace Irvin.FormatFactory
{
	/// <summary>
	/// Contains commonly-used string formats
	/// </summary>
	public static class FieldFormat
	{
        /// <summary>
        /// Examples: 0915, 1733
        /// </summary>
		public const string HourMinuteFixedLength24HourClock = "hhmm";

        /// <summary>
        /// Examples: 04291983
        /// </summary>
		public const string AmericanStyleLongYearUndelimitedDate = "MMddyyyy";

        /// <summary>
        /// Examples: $1,390.23, $400.00
        /// </summary>
        /// <remarks>always displays as dollars</remarks>
		public const string Money = "$#,#.##";

        /// <summary>
        /// Always includes the decimal
        /// </summary>
		public const string FewestDigits = "#.#";

        /// <summary>
        /// Examples: 170429
        /// </summary>
		public const string ShortYearFirstUndelimitedDate = "yyMMdd";

        /// <summary>
        /// Examples: 04/29/1983
        /// </summary>
		public const string ShortDateAmericanStyle = "MM/dd/yyyy";

        /// <summary>
        /// This format is for the integer value of the enum member.
        /// </summary>
	    public const string EnumValue = "D";

        /// <summary>
        /// This format is for the text name of the enum member.
        /// </summary>
	    public const string EnumName = "G";

        /// <summary>
        /// Examples: 04/29/1983 17:25:33
        /// </summary>
	    public const string DateTime24Hour = ShortDateAmericanStyle + " HH:mm:ss";
	}
}