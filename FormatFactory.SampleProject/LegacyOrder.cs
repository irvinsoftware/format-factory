using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(FieldDelimiter = "|")]
	public class LegacyOrder
	{
		[Field]
		public string OrderNumber { get; set; }

		[Field]
		public string ShippingName { get; set; }

		[Field(Format=FieldFormat.ShortDateAmericanStyle)]
		public DateTime Ordered { get; set; }

		[Field]
		public decimal OrderTotal { get; set; }

		public bool IsActive { get; set; }

		public void Cancel()
		{
		}
	}
}