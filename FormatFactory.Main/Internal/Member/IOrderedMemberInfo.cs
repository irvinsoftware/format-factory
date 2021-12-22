namespace Irvin.FormatFactory.Internal.Member
{
	internal interface IOrderedMemberInfo
	{
		IMemberInfo MemberInfo { get; }
		uint Order { get; }
	    bool IsRequired { get; }
        string HeaderName { get; }
        bool IsInternalToRecord { get; }
        string IndentCharacters { get; }
        bool HasSameFieldSettingsAs(FormatOptions options);
    }
}