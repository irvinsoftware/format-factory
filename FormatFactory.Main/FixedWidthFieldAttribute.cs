namespace Irvin.FormatFactory
{
	/// <summary>
	/// Used to decorate a member that is a fixed-width field
	/// </summary>
	/// <remarks>
	/// A field cannot be marked as a sub-record or a child element
	/// </remarks>
	public class FixedWidthFieldAttribute : FieldAttribute
	{
		/// <summary>
		/// Always returns true
		/// </summary>
		/// <exception cref="InvalidUsageException">
		/// thrown if value changed by client
		/// </exception>
		public override bool IsFixedWidth
		{
			get
			{
				return true;
			}
			set
			{
				throw new InvalidUsageException("Use FieldAttribute if this field is not fixed width.");
			}
		}

		/// <summary>
		/// Not applicable. Always returns the default
		/// </summary>
		/// <exception cref="InvalidUsageException">
		/// thrown if value changed by client
		/// </exception>
		public override int MinimumLength
		{
			get
			{
				return 0;
			}
			set
			{
				throw new InvalidUsageException("Use FieldAttribute if this field is not fixed width.");
			}
		}
	}
}