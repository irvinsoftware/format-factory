using System;

namespace Irvin.FormatFactory
{
	/// <summary>
	/// Used to decorate a member that is a sub-record
	/// </summary>
	/// <remarks>
	/// A sub-record cannot be marked as a field or a child element
	/// </remarks>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class SubRecordAttribute : Attribute
	{
		/// <summary>
		/// Determines the sequence of the sub-record in relation to the other fields, sub-records, and children.
		/// </summary>
		/// <remarks>
		/// This setting is not required, but the output order of a sub-record where Order is not specified is not guaranteed.
		/// No field, sub-record, or child may have the same order as another field, sub-record, or child.
		/// </remarks>
		public uint Order { get; set; }
	}
}