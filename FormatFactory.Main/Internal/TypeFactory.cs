using System;
using System.Collections.Concurrent;
using System.Linq;
using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
    internal static class TypeFactory
    {
        private static readonly ConcurrentDictionary<Type, IRecordOptions> _typeRecordCache = new ConcurrentDictionary<Type, IRecordOptions>();
        private static readonly ConcurrentDictionary<IMemberInfo, Type> _memberTypeCache = new ConcurrentDictionary<IMemberInfo, Type>();

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

        internal static int TotalMembersCached => _memberTypeCache.Count;
    }
}