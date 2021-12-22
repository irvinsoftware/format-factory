using System;
using Irvin.FormatFactory.Internal;

namespace Irvin.FormatFactory
{
	/// <summary>
	/// Used to decorate a member that is a field
	/// </summary>
	/// <remarks>
	/// A field cannot be marked as a sub-record or a child element
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class FieldAttribute : Attribute, IFieldSettings
	{
        /// <summary> 
        /// </summary>
		public FieldAttribute()
		{
		    DefaultFieldSetupAttribute.SetDefaults(this);
		}

        /// <summary> 
        /// </summary>
        /// <param name="order"><seealso cref="Order"/></param>
	    public FieldAttribute(uint order) 
            : this()
		{
			Order = order;
		}

        /// <summary> 
        /// </summary>
        /// <param name="format"><seealso cref="Format"/></param>
		public FieldAttribute(string format) 
            : this()
		{
			Format = format;
		}

        /// <summary> 
        /// </summary>
        /// <param name="order"><seealso cref="Order"/></param>
        /// <param name="format"><seealso cref="Format"/></param>
		public FieldAttribute(uint order, string format) 
            : this()
		{
			Order = order;
			Format = format;
		}

        /// <summary>
        /// The column name/header
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Determines the sequence of the field in relation to the other fields, sub-records, and children.
        /// </summary>
        /// <remarks>
        /// This setting is not required, but the output order of a field where Order is not specified is not guaranteed.
        /// No field, sub-record, or child may have the same order as another field, sub-record, or child.
        /// </remarks>
        public uint Order { get; set; }

        /// <summary>
        /// The string format that will be applied to the field on write and/or expected when the field is read.
        /// </summary>
		public string Format { get; set; }

		/// <summary>
		/// Specifies whether the field must have always have the same width (specified by MaximumLength)
		/// </summary>
		/// <remarks>
		/// This setting is ignored/not honored if MinimumLength is specified
		/// </remarks>
		public virtual bool IsFixedWidth { get; set; }

        /// <summary>
        /// Specifies a minimum width for the field. Use of this value renders <see cref="IsFixedWidth"/> unnecessary.
        /// </summary>
        public virtual int MinimumLength { get; set; }

        /// <summary>
        /// The maximum length of the field allowed, or the exact length of the field if it is fixed-width.
        /// </summary>
		public int MaximumLength { get; set; }

        /// <summary>
        /// If True, fields longer than <see cref="MaximumLength"/> are truncated on write.
        /// If False, fields longer than <see cref="MaximumLength"/> cause a write error.
        /// </summary>
        public bool AllowTruncation { get; set; }
        
        /// <summary>
        /// If the field will be padded to satisfy length constraints, this property controls which side. <seealso cref="PaddingCharacter"/>
        /// </summary>
		public FieldAlignment Alignment { get; set; }

        /// <summary>
        /// If the field value is smaller than the length constraints, this character will be used to pad the difference.
        /// Which side of the value gets padded is controlled by <see cref="Alignment"/>
        /// </summary>
		public char PaddingCharacter { get; set; }

        /// <summary>
        /// The character(s) used to separate one field from the next.
        /// </summary>
		public string Delimiter { get; set; }

        /// <summary>
        /// If True, and the value is blank for the record, this field is not written and can be skipped when reading.
        /// If False, a blank value is expected to be read and written for records that do have have content for the field.
        /// </summary>
		public bool Optional { get; set; }
	}
}