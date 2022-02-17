namespace Irvin.FormatFactory.Internal
{
    internal interface IFieldSettings
    {
        string Name { get; set; }
        uint Order { get; set; }
        string Format { get; set; }
        bool IsFixedWidth { get; set; }
        int MinimumLength { get; set; }
        int MaximumLength { get; set; }
        bool AllowTruncation { get; set; }
        FieldAlignment Alignment { get; set; }
        char PaddingCharacter { get; set; }
        string Delimiter { get; set; }
        bool Optional { get; set; }
        string AlwaysQuoteWith { get; set; }
    }
}