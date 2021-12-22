using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
    [Record(IncludeHeaders = true, FieldDelimiter = ",")]
    public class Order2
    {
        [Field]
        public string OrderNumber { get; set; }

        [Field(Name = "ShipName")]
        public string ShippingName { get; set; }

        [Field(Format = FieldFormat.ShortDateAmericanStyle)]
        public DateTime Ordered { get; set; }

        [Field(Name = "Total")]
        public decimal OrderTotal { get; set; }

        public bool IsActive { get; set; }

        public void Cancel()
        {
        }
    }
}