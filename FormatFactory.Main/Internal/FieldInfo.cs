using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
	internal sealed class FieldInfo : IOrderedMemberInfo
	{
		public IMemberInfo MemberInfo { get; set; }
		public IFieldSettings Settings { get; set; }
		public uint Order => Settings.Order;

        public string HeaderName
        {
            get
            {
                return !string.IsNullOrEmpty(Settings.Name)
                    ? Settings.Name
                    : MemberInfo.Name;
            }
        }

        public bool IsInternalToRecord => true;
        public string IndentCharacters => null;
        public bool IsRequired => !Settings.Optional;

        public bool HasSameFieldSettingsAs(FormatOptions options)
        {
            return ReferenceEquals(Settings, options.DefaultFieldSetup);
        }
    }
}