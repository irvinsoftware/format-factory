using System;

namespace Irvin.FormatFactory.Internal
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class DefaultFieldSetupAttribute : Attribute, IFieldSettings
    {
        public DefaultFieldSetupAttribute()
        {
            SetDefaults(this);
        }

        public string Name { get; set; }
        public uint Order { get; set; }
        public string Format { get; set; }
        public bool IsFixedWidth { get; set; }
        public int MinimumLength { get; set; }
        public int MaximumLength { get; set; }
        public bool AllowTruncation { get; set; }
        public FieldAlignment Alignment { get; set; }
        public char PaddingCharacter { get; set; }
        public string Delimiter { get; set; }
        public bool Optional { get; set; }

        internal static void SetDefaults(IFieldSettings fieldSettings)
        {
            fieldSettings.Alignment = FieldAlignment.Left;
            fieldSettings.PaddingCharacter = ' ';
            fieldSettings.MaximumLength = short.MaxValue;
            fieldSettings.Optional = false;
            fieldSettings.AllowTruncation = true;
        }
    }
}