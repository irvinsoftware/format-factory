namespace Irvin.FormatFactory
{
	public enum EscapeKind
	{
		SingleQuote,
		DoubleQuote,
		Repeat,
		Remove,

		/// <summary>
		/// The delimiter is transformed into another character altogether (as defined by FormatOptions.TransformEscapeCharacter)
		/// </summary>
		Transform,

		Backslash,
		ForwardSlash
	}
}