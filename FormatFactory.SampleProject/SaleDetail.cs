using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
    [Record(FieldDelimiter = ",", IncludeHeaders = false, RecordDelimiter = "\r\n", UseStrictMode = false, UseTrailingDelimiter = false)]
    public class SaleDetail
    {
        [Field(Order = 1)]
        public DateTime WeekStart { get; set; }   
         
        [Field(Order = 2)]
        public string ChannelName { get; set; }

        [Field(Order = 3)]
        public string RawSKU { get; set; }

        [Field(Order = 4)]
        public ulong? UPC { get; set; }

        [Field(Order = 6)]
        public uint ConvertedSKUNumber { get; set; }
        
        [Field(Order = 5, AlwaysQuoteWith = "\"")]
        public string RawDescription { get; set; }

        public ProductInfo Product { get; set; }

        [Field(Order = 7)]
        public uint ActualSKUNumber => (Product?.SKUNumber).GetValueOrDefault();

        [Field(Order = 8)]
        public decimal SoldAmount { get; set; }

        [Field(Order = 9)]
        public int SoldQuantity { get; set; }

        [Field(Order = 10)]
        public int? OnHandQuantity { get; set; }

        [Field(Order = 11)]
        public decimal? OnHandValue { get; set; }

        [Field(Order = 12)]
        public bool InformationalOnly { get; set; }
    }
    
    public class ProductInfo
    {
        public string LongSKU { get; set; }
        public string CompanyUPC { get; set; }
        public uint SKUNumber { get; set; }
        public ulong ShortUPC { get; set; }
    }
}