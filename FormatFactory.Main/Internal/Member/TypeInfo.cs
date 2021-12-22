using System;

namespace Irvin.FormatFactory.Internal.Member
{
    internal class TypeInfo : IMemberContainer
    {
        public TypeInfo(Type type)
        {
            Type = type;
        }

        private Type Type { get; }

        public string Name => Type.Name;

        public object CreateNew()
        {
            return Activator.CreateInstance(Type);
        }
    }
}