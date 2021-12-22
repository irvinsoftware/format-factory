using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
	internal class SubRecordInfo : IOrderedMemberInfo
	{
		public uint Order { get; set; }
		public IMemberInfo MemberInfo { get; set; }
	    public bool IsRequired => false;
        public string HeaderName => null;
        public bool IsInternalToRecord => true;
        public string IndentCharacters => null;

        public bool HasSameFieldSettingsAs(FormatOptions options)
        {
            return false;
        }
    }
}