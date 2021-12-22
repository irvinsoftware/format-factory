using System;

namespace Irvin.FormatFactory.Internal.Member
{
    internal interface IMemberInfo : IEquatable<IMemberInfo>
    {
        string Name { get; }
        Type MemberType { get; }
        IMemberContainer Container { get; }
        object GetValue(object source);
        bool SetValue(object target, object value);
    }
}