using System;
using System.Reflection;

namespace Irvin.FormatFactory.Internal.Member
{
    internal class PropertyInfoWrapper : IMemberInfo
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyInfoWrapper(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public bool Equals(IMemberInfo other)
        {
            PropertyInfoWrapper other2 = other as PropertyInfoWrapper;
            if (other2 == null)
            {
                return false;
            }

            return _propertyInfo.Equals(other2._propertyInfo);
        }

        public string Name => _propertyInfo.Name;
        public Type MemberType => _propertyInfo.PropertyType;
        public IMemberContainer Container => new TypeInfo(_propertyInfo.ReflectedType);

        public bool SetValue(object target, object value)
        {
            if (_propertyInfo.GetSetMethod(nonPublic: true) != null)
            {
                _propertyInfo.SetValue(target, value, null);
                return true;
            }

            return false;
        }

        public object GetValue(object source)
        {
            return _propertyInfo.GetValue(source, null);
        }
    }
}