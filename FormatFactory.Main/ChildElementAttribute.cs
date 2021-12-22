using System;

namespace Irvin.FormatFactory
{
	/// <summary>
	/// Used to decorate a member that represents nested/accompanying elements
	/// </summary>
	/// <remarks>
	/// A child element cannot be marked as a field or sub-record
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property)]
	public class ChildElementAttribute : Attribute
	{
        /// <summary> 
        /// </summary>
		public ChildElementAttribute()
		{
		}

        /// <summary> 
        /// </summary>
        /// <param name="indentCharacters"><see cref="IndentCharacters"/></param>
		public ChildElementAttribute(string indentCharacters)
		{
			IndentCharacters = indentCharacters;
		}

        /// <summary>
        /// The characters (if any) to use to indent each child element.
        /// </summary>
		public string IndentCharacters { get; set; }

		/// <summary>
		/// Determines the sequence of the child element in relation to the other fields, sub-records, and children.
		/// </summary>
		/// <remarks>
		/// This setting is not required, but the output order of a child where Order is not specified is not guaranteed.
		/// No field, sub-record, or child may have the same order as another field, sub-record, or child.
		/// </remarks>
		public uint Order { get; set; }

	    /// <summary>
	    /// If True, and this element is null, the section is not written and can be skipped when reading.
	    /// If False, this element will not be written when it is null, and it can be skipped during reading.
	    /// </summary>
	    public bool IsRequired { get; set; }
	}
}