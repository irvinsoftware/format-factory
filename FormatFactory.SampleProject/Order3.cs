using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
    [Record(IncludeHeaders = true, FieldDelimiter = ",")]
    public class Order3
    {
        [Field(Order = 1)]
        public string OrderNumber { get; set; }

        [Field(Name = "ShipName", Order = 2)]
        public string ShippingName { get; set; }

        [Field(Order = 3, Format = FieldFormat.ShortDateAmericanStyle)]
        public DateTime Ordered { get; set; }

        [Field(Order = 4, Name = "Total")]
        public decimal OrderTotal { get; set; }

        public bool IsActive { get; set; }

        public void Cancel()
        {
        }
    }
}