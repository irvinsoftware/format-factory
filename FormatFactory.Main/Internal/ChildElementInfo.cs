using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
	internal class ChildElementInfo : IOrderedMemberInfo
	{
		public IMemberInfo MemberInfo { get; set; }
		public uint Order { get; internal set; }
		public string IndentCharacters { get; set; }
	    public bool IsRequired { get; internal set; }
        public string HeaderName => null;
        public bool IsInternalToRecord => false;

        public bool HasSameFieldSettingsAs(FormatOptions options)
        {
            return false;
        }
    }
}