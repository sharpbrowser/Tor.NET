using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;

namespace Tor.Helpers
{
    /// <summary>
    /// A class containing methods to assist with reflection-based processing.
    /// </summary>
    //[DebuggerStepThrough]
    internal static class ReflectionHelper
    {
        /// <summary>
        /// Converts a value into the destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>A <see cref="System.Object"/> matching the destination type.</returns>
        public static object Convert(object value, Type destinationType)
        {
            if (value == null)
                return destinationType.IsPrimitive || destinationType.IsValueType ? Activator.CreateInstance(destinationType) : null;

            if (destinationType.IsEnum)
                return GetEnumerator<DescriptionAttribute>(destinationType, attribute => attribute.Description.Equals(value.ToString(), StringComparison.CurrentCultureIgnoreCase));

            if (value is bool && destinationType == typeof(string))
                return ((bool)value) ? "1" : "0";

            if (destinationType == typeof(bool) && ("0".Equals(value) || "1".Equals(value)))
                return "1".Equals(value);

            if (destinationType.IsValueType)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(destinationType);

                if (value == null)
                    return Activator.CreateInstance(destinationType);

                if (converter != null && converter.CanConvertFrom(value.GetType()))
                    return converter.ConvertFrom(value);
            }

            return System.Convert.ChangeType(value, destinationType);
        }

        /// <summary>
        /// Gets an attribute from the value of an enumerator.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumerator.</typeparam>
        /// <typeparam name="TAttribute">The attribute to retrieve.</typeparam>
        /// <param name="value">The value of the enumerator.</param>
        /// <returns>A <typeparamref name="TAttribute"/> object instance; otherwise, <c>null</c>.</returns>
        public static TAttribute GetAttribute<TEnum, TAttribute>(TEnum value) where TAttribute : Attribute
        {
            MemberInfo member = typeof(TEnum).GetMember(System.Convert.ToString(value), BindingFlags.Public | BindingFlags.Static).FirstOrDefault();

            if (member == null)
                return null;

            return Attribute.GetCustomAttribute(member, typeof(TAttribute)) as TAttribute;
        }

        /// <summary>
        /// Gets the description value of an enumerator.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumerator.</typeparam>
        /// <param name="value">The value of the enumerator.</param>
        /// <returns>A <see cref="System.String"/> containing the description value; otherwise, <c>null</c>.</returns>
        public static string GetDescription<TEnum>(TEnum value)
        {
            DescriptionAttribute attribute = GetAttribute<TEnum, DescriptionAttribute>(value);

            if (attribute == null)
                return null;

            return attribute.Description;
        }

        /// <summary>
        /// Gets an enumerator value by performing a specified selector against the matched attribute of the enumerator values.
        /// </summary>
        /// <typeparam name="TAttribute">The attribute to retrieve.</typeparam>
        /// <param name="enumType">The type of the enumerator.</param>
        /// <param name="selector">The selector to execute against the attribute.</param>
        /// <returns>A <typeparamref name="TEnum"/> value matching the selector pattern.</returns>
        public static object GetEnumerator<TAttribute>(Type enumType, Func<TAttribute, bool> selector) where TAttribute : Attribute
        {
            MemberInfo[] members = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (MemberInfo member in members)
            {
                TAttribute attribute = Attribute.GetCustomAttribute(member, typeof(TAttribute)) as TAttribute;

                if (attribute == null)
                    continue;

                if (selector(attribute))
                    return Enum.Parse(enumType, member.Name);
            }

            return Activator.CreateInstance(enumType);
        }

        /// <summary>
        /// Gets an enumerator value by performing a specified selector against the matched attribute of the enumerator values.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumerator.</typeparam>
        /// <typeparam name="TAttribute">The attribute to retrieve.</typeparam>
        /// <param name="selector">The selector to execute against the attribute.</param>
        /// <returns>A <typeparamref name="TEnum"/> value matching the selector pattern.</returns>
        public static TEnum GetEnumerator<TEnum, TAttribute>(Func<TAttribute, bool> selector) where TAttribute : Attribute
        {
            MemberInfo[] members = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (MemberInfo member in members)
            {
                TAttribute attribute = Attribute.GetCustomAttribute(member, typeof(TAttribute)) as TAttribute;

                if (attribute == null)
                    continue;

                if (selector(attribute))
                    return (TEnum)Enum.Parse(typeof(TEnum), member.Name);
            }

            return default(TEnum);
        }

        /// <summary>
        /// Gets a collection of attributes from all enumerator values.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumerator.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <returns>A <see cref="List{TAttribute}"/> containing the resulting attribute list.</returns>
        public static List<TAttribute> GetEnumeratorAttributes<TEnum, TAttribute>() where TAttribute : Attribute
        {
            List<TAttribute> values = new List<TAttribute>();
            MemberInfo[] members = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (MemberInfo member in members)
            {
                TAttribute attribute = Attribute.GetCustomAttribute(member, typeof(TAttribute)) as TAttribute;

                if (attribute != null && !values.Contains(attribute))
                    values.Add(attribute);
            }

            return values;
        }

        /// <summary>
        /// Gets a collection of attribute values from all enumerator values of a specified type.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumerator.</typeparam>
        /// <typeparam name="TAttribute">The type of attribute to retrieve.</typeparam>
        /// <typeparam name="TValue">The type of value to retrieve from the attribute.</typeparam>
        /// <param name="selector">The selector to execute against the attribute.</param>
        /// <returns>A <see cref="List{TValue}"/> containing the resulting attribute value list.</returns>
        public static List<TValue> GetEnumeratorAttributes<TEnum, TAttribute, TValue>(Func<TAttribute, TValue> selector) where TAttribute : Attribute
        {
            List<TAttribute> attributes = GetEnumeratorAttributes<TEnum, TAttribute>();
            List<TValue> values = new List<TValue>();

            foreach (TAttribute attribute in attributes)
            {
                if (attribute == null)
                    continue;

                values.Add(selector(attribute));
            }

            return values;
        }
    }
}
