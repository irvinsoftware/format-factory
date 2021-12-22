namespace Irvin.FormatFactory.Internal
{
    internal static class InterfaceExtensions
    {
        public static bool HasOwnRecordDelimiter(this IRecordOptions recordOptions)
        {
            return !string.IsNullOrEmpty(recordOptions.RecordDelimiter) && 
                   recordOptions.RecordDelimiter != FormatOptions.DefaultRecordDelimiter;
        }

        public static bool IsLengthValidForChar(this IFieldSettings fieldSettings)
        {
            return fieldSettings.MaximumLength <= 1 || fieldSettings.MaximumLength == short.MaxValue;
        }
    }
}