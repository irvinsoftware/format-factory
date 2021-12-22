using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace TestProject
{
    public static class ObjectAssert
    {
        public static void ListsEqual<T>(IList<T> expected, IList<T> actual)
            where T : class
        {   
            AssertListsEqual(expected, actual);
        }

        private static void AssertListsEqual<T>(IList<T> expected, IList<T> actual)
            where T : class
        {
            AssertListsEqual((IList) expected, (IList) actual, typeof(T), "list");
        }

        private static void AssertListsEqual(IList expected, IList actual, Type itemType, string path = null)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            if (expected != null && actual == null)
            {
                throw new AssertionException(path + ": 'not null' != 'null'");
            }
            if (expected == null && actual != null)
            {
                throw new AssertionException(path + ": 'null' != 'not null'");
            }

            if (expected.Count != actual.Count)
            {
                throw new AssertionException($"{path}.Count: {expected.Count} != {actual.Count}");
            }

            for (int i = 0; i < expected.Count; i++)
            {
                AssertObjectsEquivalent(expected[i], actual[i], itemType, $"{path}[{i}]");
            }
        }

        public static void ObjectsEquivalent<T>(T expected, T actual) 
            where T : class
        {
            AssertObjectsEquivalent(expected, actual);
        }

        private static void AssertObjectsEquivalent<T>(T expected, T actual)
            where T : class
        {
            AssertObjectsEquivalent(expected, actual, typeof(T));
        }

        private static void AssertObjectsEquivalent(object expected, object actual, Type type, string path = null)
        {
            if (expected == null && actual == null)
            {
                return;
            }
            if (expected != null && actual == null)
            {
                throw new AssertionException(path + ": 'not null' != 'null'");
            }
            if (expected == null)
            {
                throw new AssertionException(path + ": 'null' != 'not null'");
            }
            if (expected.GetType() != actual.GetType())
            {
                throw new AssertionException(path + ": types not equivalent");
            }
            if (expected.GetType() != type || actual.GetType() != type)
            {
                throw new AssertionException(path + ": wrong types");
            }

            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                object valueFromX = property.GetValue(expected, null);
                object valueFromY = property.GetValue(actual, null);
                AssertValuesEqual(valueFromX, valueFromY, property.PropertyType, $"{path}.{property.Name}");
            }

            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                object valueFromX = field.GetValue(expected);
                object valueFromY = field.GetValue(actual);
                AssertValuesEqual(valueFromX, valueFromY, field.FieldType, $"{path}.{field.Name}");
            }

            Debug.Assert(properties.Length+fields.Length > 0);
        }

        private static void AssertValuesEqual(object expected, object actual, Type type, string path)
        {
            if (type == typeof(string))
            {
                if (!string.IsNullOrWhiteSpace((string)expected) ||
                    !string.IsNullOrWhiteSpace((string)actual))
                {
                    if (!string.IsNullOrWhiteSpace((string)expected) &&
                        string.IsNullOrWhiteSpace((string)actual))
                    {
                        throw new AssertionException(string.Format("{0}: '{1}' != '{2}'", path, expected, actual));
                    }
                    if (string.IsNullOrWhiteSpace((string)expected) &&
                        !string.IsNullOrWhiteSpace((string)actual))
                    {
                        throw new AssertionException(string.Format("{0}: '{1}' != '{2}'", path, expected, actual));
                    }
                    if (!expected.Equals(actual))
                    {
                        throw new AssertionException(string.Format("{0}: '{1}' != '{2}'", path, expected, actual));
                    }
                }
            }
            else if (type.IsValueType || type.BaseType == typeof(Enum))
            {
                DebugAssertTypeKnown(type);

                if (expected == null && actual != null)
                {
                    throw new AssertionException(string.Format("{0}: 'null' != 'not null'", path));
                }
                if (expected != null && actual == null)
                {
                    throw new AssertionException(string.Format("{0}: 'not null' != 'null'", path));
                }
                if (expected != null)
                {
                    if (!expected.Equals(actual))
                    {
                        throw new AssertionException(string.Format("{0}: '{1}' != '{2}'", path, expected, actual));
                    }
                }
            }
            else if (IsGenericListType(type))
            {
                Type itemType = type.GetGenericArguments().First();
                AssertListsEqual((IList) expected, (IList) actual, itemType, path);
            }
            else
            {
                AssertObjectsEquivalent(expected, actual, type, path);
            }
        }

        private static bool IsGenericListType(Type propertyType)
        {
            return typeof(IList).IsAssignableFrom(propertyType) && propertyType.IsGenericType;
        }

        private static void DebugAssertTypeKnown(Type type)
        {
            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = type.GetGenericArguments().First();
            }

            if (type.BaseType != typeof(Enum))
            {
                Debug.Assert(new[]
                {
                    typeof(int),
                    typeof(decimal),
                    typeof(byte),
                    typeof(bool),
                    typeof(DateTime),
                    typeof(char),
                    typeof(long)
                }.Contains(type), type.FullName);
            }
        }
    }
}