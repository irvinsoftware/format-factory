using System;

namespace Irvin.FormatFactory.Internal.Member
{
    internal class FieldInfoWrapper : IMemberInfo
    {
        private readonly System.Reflection.FieldInfo _fieldInfo;

        public FieldInfoWrapper(System.Reflection.FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public bool Equals(IMemberInfo other)
        {
            FieldInfoWrapper otherField = other as FieldInfoWrapper;

            if (otherField == null)
            {
                return false;
            }

            return _fieldInfo.Equals(otherField._fieldInfo);
        }

        public string Name => _fieldInfo.Name;
        public Type MemberType => _fieldInfo.FieldType;
        public IMemberContainer Container => new TypeInfo(_fieldInfo.ReflectedType);

        public bool SetValue(object target, object value)
        {
            _fieldInfo.SetValue(target, value);
            return true;
        }

        public object GetValue(object source)
        {
            return _fieldInfo.GetValue(source);
        }
    }
}