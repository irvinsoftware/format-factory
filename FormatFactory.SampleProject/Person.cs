using System;
using Irvin.FormatFactory;

namespace Company.Entities
{
    public interface IPerson
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        uint Age { get; set; }
        string State { get; set; }
        DateTime EnteredDate { get; set; }
        float Amount { get; set; }
    }

    [Record(IncludeHeaders = true, FieldDelimiter = ",", RecordDelimiter = "\r\n")]
    public class Person : IPerson
    {
        [Field(Name = "Given Name")]
        public string FirstName { get; set; }

        [Field(Name = "Family Name")]
        public string LastName { get; set; }

        [Field(Name = "Age")]
        public uint Age { get; set; }

        [Field(Name = "State,Province", MinimumLength = 2, MaximumLength = 2)]
        public string State { get; set; }

        [Field(Name = "Created")]
        public DateTime EnteredDate { get; set; }

        [Field(Name = "Balance (USD)")]
        public float Amount { get; set; }
    }

    [CsvRecord]
    public class Person2 : IPerson
    {
        [Field(Name = "Given Name")]
        public string FirstName { get; set; }

        [Field(Name = "Family Name")]
        public string LastName { get; set; }

        [Field(Name = "Age")]
        public uint Age { get; set; }

        [Field(Name = "State,Province", MinimumLength = 2, MaximumLength = 2)]
        public string State { get; set; }

        [Field(Name = "Created")]
        public DateTime EnteredDate { get; set; }

        [Field(Name = "Balance (USD)")]
        public float Amount { get; set; }
    }
}