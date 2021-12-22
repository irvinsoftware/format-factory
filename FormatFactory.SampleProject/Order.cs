using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
	[Record(FieldDelimiter = ",")]
	public class Order
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

    [Record(FieldDelimiter = ",")]
    public class OrderQ
    {
        public bool IsActive { get; set; }

        public void Cancel()
        {
        }

        [Field]
        public string OrderNumber { get; set; }

        [Field]
        public string ShippingName { get; set; }

        [Field(Format = FieldFormat.ShortDateAmericanStyle)]
        public DateTime Ordered { get; set; }

        [Field]
        public decimal OrderTotal { get; set; }
    }

    [CsvRecord]
    public class OrderB
    {
        [Field]
        public string OrderNumber { get; set; }

        [Field]
        public string ShippingName { get; set; }

        [Field(Format = FieldFormat.ShortDateAmericanStyle)]
        public DateTime Ordered { get; set; }

        [Field]
        public decimal OrderTotal { get; set; }

        public bool IsActive { get; set; }

        public void Cancel()
        {
        }
    }

    [CsvRecord]
    public class OrderC
    {
        public bool IsActive { get; set; }

        [Field]
        public string OrderNumber { get; set; }

        [Field]
        public string ShippingName { get; set; }

        [Field(Format = FieldFormat.ShortDateAmericanStyle)]
        public DateTime Ordered { get; set; }

        [Field]
        public decimal OrderTotal { get; set; }
    }
}