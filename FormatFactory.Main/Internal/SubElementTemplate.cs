using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Irvin.Extensions;
using Irvin.FormatFactory.Internal.Member;

namespace Irvin.FormatFactory.Internal
{
    internal class SubElementTemplate
    {
        public SubElementTemplate(MemberInfo typeMember)
        {
            TypeMember = typeMember;

            FieldAttribute = TypeMember.GetAttribute<FieldAttribute>(inherit: true);
            SubRecordAttribute = TypeMember.GetAttribute<SubRecordAttribute>(inherit: true);
            ChildElementAttribute = TypeMember.GetAttribute<ChildElementAttribute>(inherit: true);

            ThrowIfAttributesIncorrectlyCombined("field", FieldAttribute, "sub-record", SubRecordAttribute);
            ThrowIfAttributesIncorrectlyCombined("field", FieldAttribute, "child element", ChildElementAttribute);
            ThrowIfAttributesIncorrectlyCombined("sub-record", SubRecordAttribute, "child element", ChildElementAttribute);
            ThrowIfFieldAttributeSettingsNotValid();
        }

        public MemberInfo TypeMember { get; }
        public FieldAttribute FieldAttribute { get; }
        public SubRecordAttribute SubRecordAttribute { get; }
        public ChildElementAttribute ChildElementAttribute { get; }
        public bool IsEmpty => FieldAttribute == null && ChildElementAttribute == null && SubRecordAttribute == null;

        [SuppressMessage("ReSharper", "TooManyArguments")]
        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
        private void ThrowIfAttributesIncorrectlyCombined(string concept1, Attribute attribute1, string concept2, Attribute attribute2)
        {
            if (attribute1 != null && attribute2 != null)
            {
                throw new InvalidUsageException($"A member cannot be a {concept1} and a {concept2} (violator: '{TypeMember.Name}')");
            }
        }

        private void ThrowIfFieldAttributeSettingsNotValid()
        {
            if (FieldAttribute != null)
            {
                if (FieldAttribute.MinimumLength > FieldAttribute.MaximumLength)
                {
                    throw new InvalidUsageException("The maximum length of a field cannot be less than the minimum width.");
                }

                if (TypeFactory.GetMemberType(MemberInfoFactory.Get(TypeMember)) == typeof(char) && !FieldAttribute.IsLengthValidForChar())
                {
                    throw new InvalidUsageException($"The field '{TypeMember.Name}' has an invalid length (chars are only 1 character long)");
                }
            }
        }

        public IOrderedMemberInfo ChildElementInfo()
        {
            ChildElementInfo childElementInfo = new ChildElementInfo();
            childElementInfo.MemberInfo = MemberInfoFactory.Get(TypeMember);
            childElementInfo.Order = ChildElementAttribute.Order;
            childElementInfo.IndentCharacters = ChildElementAttribute.IndentCharacters;
            childElementInfo.IsRequired = ChildElementAttribute.IsRequired;
            return childElementInfo;
        }

        public IOrderedMemberInfo ToSubRecordInfo()
        {
            SubRecordInfo subRecordInfo = new SubRecordInfo();
            subRecordInfo.Order = SubRecordAttribute.Order;
            subRecordInfo.MemberInfo = MemberInfoFactory.Get(TypeMember);
            return subRecordInfo;
        }

        public IOrderedMemberInfo ToFieldInfo()
        {
            FieldInfo fieldInfo = new FieldInfo();
            fieldInfo.MemberInfo = MemberInfoFactory.Get(TypeMember);
            fieldInfo.Settings = FieldAttribute;
            return fieldInfo;
        }
    }
}