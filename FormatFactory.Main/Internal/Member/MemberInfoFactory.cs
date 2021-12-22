using System;
using System.Data;
using System.Reflection;

namespace Irvin.FormatFactory.Internal.Member
{
    internal static class MemberInfoFactory
    {
        public static IMemberInfo Get(MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo)
            {
                return new PropertyInfoWrapper(memberInfo as PropertyInfo);
            }

            if (memberInfo is System.Reflection.FieldInfo)
            {
                return new FieldInfoWrapper(memberInfo as System.Reflection.FieldInfo);
            }

            throw new NotSupportedException();
        }

        public static IMemberInfo Get(DataColumn memberInfo)
        {
            return new DataColumnWrapper(memberInfo);
        }
    }
}