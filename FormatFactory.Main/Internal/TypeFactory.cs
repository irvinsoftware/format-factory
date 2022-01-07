using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
    internal static class TypeFactory
    {
        private static readonly ConcurrentDictionary<Type, IEnumerable<MemberInfo>> _typeMemberCache = new ConcurrentDictionary<Type, IEnumerable<MemberInfo>>();
        private static readonly ConcurrentDictionary<Type, IRecordOptions> _typeRecordCache = new ConcurrentDictionary<Type, IRecordOptions>();
        private static readonly ConcurrentDictionary<IMemberInfo, Type> _memberTypeCache = new ConcurrentDictionary<IMemberInfo, Type>();

        public static IEnumerable<MemberInfo> GetTypeMembers(Type elementType)
        {
            return _typeMemberCache.GetOrAdd(elementType, type =>
            {
                List<MemberInfo> typeMembers = new List<MemberInfo>();

                const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
                typeMembers.AddRange(type.GetFields(bindingFlags));
                typeMembers.AddRange(type.GetProperties(bindingFlags));

                return typeMembers.Where(member => member.CustomAttributes.All(x => x.AttributeType != typeof(CompilerGeneratedAttribute)));
            });
        }

        public static IRecordOptions GetRecordAttribute(Type elementType)
        {
            return _typeRecordCache.GetOrAdd(elementType, type =>
            {
                IRecordOptions recordAttribute =
                    type.GetCustomAttributes(inherit: true)
                        .FirstOrDefault(x => x is IRecordOptions)
                        as IRecordOptions;

                if (recordAttribute == null)
                {
                    throw new InvalidUsageException("Only types decorated as Records can be used.");
                }

                return recordAttribute;
            });
        }

        internal static Type GetMemberType(IMemberInfo memberInfo)
        {
            return _memberTypeCache.GetOrAdd(memberInfo, member => member.MemberType);
        }
    }
}