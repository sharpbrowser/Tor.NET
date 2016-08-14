using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Tor.Converters
{
    /// <summary>
    /// A class providing the methods necessary to convert between a <see cref="HostAuth"/> object and <see cref="System.String"/> object.
    /// </summary>
    public sealed class HostAuthTypeConverter : TypeConverter
    {
        #region System.ComponentModel.TypeConverter

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type" /> that represents the type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType.Equals(typeof(string)))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        /// <exception cref="InvalidCastException">A string must contain a username, or a username and password, format</exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                return HostAuth.Null;

            if (value is string)
            {
                string actual = value as string;

                if (actual.Contains(":"))
                {
                    string[] parts = actual.Split(':');

                    if (parts.Length != 2)
                        throw new InvalidCastException("A string must contain a username, or a username and password, format");

                    return new HostAuth(parts[0], parts[1]);
                }

                return new HostAuth(actual, null);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType.Equals(typeof(string)))
            {
                HostAuth hostAuth = (HostAuth)value;

                if (hostAuth.IsNull)
                    return "";

                if (hostAuth.Password == null)
                    return hostAuth.Username;

                return string.Format("{0}:{1}", hostAuth.Username, hostAuth.Password);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}
