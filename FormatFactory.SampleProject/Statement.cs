using Irvin.FormatFactory;

namespace Company.Entities
{
    [Record(RecordDelimiter = "\r\n", FieldDelimiter = ",")]
    public class Statement
    {
        [Field]
        public string Name { get; set; }

        [Field]
        public string Description { get; set; }
    }

    [CsvRecord]
    public class Statement2
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}