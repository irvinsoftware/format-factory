using Irvin.FormatFactory;

namespace Company.Entities
{
    [Record]
    public class FixItFelix
    {
        [FixedWidthField(MaximumLength = 10, PaddingCharacter = '0', Alignment=FieldAlignment.Right)]
        public int ID { get; set; }

        [FixedWidthField(MaximumLength = 30)]
        public string Name { get; set; }

        [FixedWidthField(MaximumLength = 10)]
        public decimal Amount { get; set; }
    }
}