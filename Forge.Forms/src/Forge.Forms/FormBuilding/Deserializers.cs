using System;
using System.Globalization;

namespace Forge.Forms.FormBuilding
{
    public static class Deserializers
    {
        #region Default

        public static object String(string expression)
        {
            return expression;
        }

        public static object DateTime(string expression)
        {
            return System.DateTime.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableDateTime(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return System.DateTime.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object Boolean(string expression)
        {
            return bool.Parse(expression);
        }

        public static object NullableBoolean(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return bool.Parse(expression);
        }

        public static object Char(string expression)
        {
            if (expression.Length == 2 && expression[1] == '\0')
            {
                return expression[0];
            }

            return char.Parse(expression);
        }

        public static object NullableChar(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (expression.Length == 2 && expression[1] == '\0')
            {
                return expression[0];
            }

            return char.Parse(expression);
        }

        public static object Byte(string expression)
        {
            return byte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableByte(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return byte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object SByte(string expression)
        {
            return sbyte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableSByte(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return sbyte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object Int16(string expression)
        {
            return short.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableInt16(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return short.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object Int32(string expression)
        {
            return int.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableInt32(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return int.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object Int64(string expression)
        {
            return long.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableInt64(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return long.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object UInt16(string expression)
        {
            return ushort.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableUInt16(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return ushort.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object UInt32(string expression)
        {
            return uint.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableUInt32(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return uint.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object UInt64(string expression)
        {
            return ulong.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableUInt64(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return ulong.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object Single(string expression)
        {
            return float.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableSingle(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return float.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object Double(string expression)
        {
            return double.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableDouble(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return double.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object Decimal(string expression)
        {
            return decimal.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object NullableDecimal(string expression)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return decimal.Parse(expression, CultureInfo.InvariantCulture);
        }

        #endregion

        #region CultureInvariant

        public static object CultureInvariantDateTime(string expression, CultureInfo culture)
        {
            return System.DateTime.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableDateTime(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return System.DateTime.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantBoolean(string expression, CultureInfo culture)
        {
            return bool.Parse(expression);
        }

        public static object CultureInvariantNullableBoolean(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return bool.Parse(expression);
        }

        public static object CultureInvariantChar(string expression, CultureInfo culture)
        {
            if (expression.Length == 2 && expression[1] == '\0')
            {
                return expression[0];
            }

            return char.Parse(expression);
        }

        public static object CultureInvariantNullableChar(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (expression.Length == 2 && expression[1] == '\0')
            {
                return expression[0];
            }

            return char.Parse(expression);
        }

        public static object CultureInvariantByte(string expression, CultureInfo culture)
        {
            return byte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableByte(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return byte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantSByte(string expression, CultureInfo culture)
        {
            return sbyte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableSByte(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return sbyte.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantInt16(string expression, CultureInfo culture)
        {
            return short.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableInt16(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return short.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantInt32(string expression, CultureInfo culture)
        {
            return int.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableInt32(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return int.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantInt64(string expression, CultureInfo culture)
        {
            return long.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableInt64(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return long.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantUInt16(string expression, CultureInfo culture)
        {
            return ushort.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableUInt16(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return ushort.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantUInt32(string expression, CultureInfo culture)
        {
            return uint.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableUInt32(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return uint.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantUInt64(string expression, CultureInfo culture)
        {
            return ulong.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableUInt64(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return ulong.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantSingle(string expression, CultureInfo culture)
        {
            return float.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableSingle(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return float.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantDouble(string expression, CultureInfo culture)
        {
            return double.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableDouble(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return double.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantDecimal(string expression, CultureInfo culture)
        {
            return decimal.Parse(expression, CultureInfo.InvariantCulture);
        }

        public static object CultureInvariantNullableDecimal(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return decimal.Parse(expression, CultureInfo.InvariantCulture);
        }

        #endregion

        #region NumberStyles

        public static object Byte(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return Byte(expression, culture);
            }

            return byte.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableByte(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return Byte(expression, culture);
            }

            return byte.Parse(expression, numberStyles.Value, culture);
        }

        public static object SByte(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return SByte(expression, culture);
            }

            return sbyte.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableSByte(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return SByte(expression, culture);
            }

            return sbyte.Parse(expression, numberStyles.Value, culture);
        }

        public static object Int16(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return Int16(expression, culture);
            }

            return short.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableInt16(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return Int16(expression, culture);
            }

            return short.Parse(expression, numberStyles.Value, culture);
        }

        public static object Int32(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return Int32(expression, culture);
            }

            return int.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableInt32(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return Int32(expression, culture);
            }

            return int.Parse(expression, numberStyles.Value, culture);
        }

        public static object Int64(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return Int64(expression, culture);
            }

            return long.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableInt64(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return Int64(expression, culture);
            }

            return long.Parse(expression, numberStyles.Value, culture);
        }

        public static object UInt16(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return UInt16(expression, culture);
            }

            return ushort.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableUInt16(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return UInt16(expression, culture);
            }

            return ushort.Parse(expression, numberStyles.Value, culture);
        }

        public static object UInt32(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return UInt32(expression, culture);
            }

            return uint.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableUInt32(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return UInt32(expression, culture);
            }

            return uint.Parse(expression, numberStyles.Value, culture);
        }

        public static object UInt64(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return UInt64(expression, culture);
            }

            return ulong.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableUInt64(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return UInt64(expression, culture);
            }

            return ulong.Parse(expression, numberStyles.Value, culture);
        }

        public static object Single(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return Single(expression, culture);
            }

            return float.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableSingle(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return Single(expression, culture);
            }

            return float.Parse(expression, numberStyles.Value, culture);
        }

        public static object Double(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (numberStyles == null)
            {
                return Double(expression, culture);
            }

            return double.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableDouble(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (numberStyles == null)
            {
                return Double(expression, culture);
            }

            return double.Parse(expression, numberStyles.Value, culture);
        }

        public static object Decimal(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            return decimal.Parse(expression, numberStyles.Value, culture);
        }

        public static object NullableDecimal(string expression, CultureInfo culture, NumberStyles? numberStyles)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return decimal.Parse(expression, numberStyles.Value, culture);
        }

        #endregion

        #region CultureSpecific

        public static object DateTime(string expression, CultureInfo culture)
        {
            return System.DateTime.Parse(expression, culture);
        }

        public static object NullableDateTime(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return System.DateTime.Parse(expression, culture);
        }

        public static object Boolean(string expression, CultureInfo culture)
        {
            return bool.Parse(expression);
        }

        public static object NullableBoolean(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return bool.Parse(expression);
        }

        public static object Char(string expression, CultureInfo culture)
        {
            if (expression.Length == 2 && expression[1] == '\0')
            {
                return expression[0];
            }

            return char.Parse(expression);
        }

        public static object NullableChar(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            if (expression.Length == 2 && expression[1] == '\0')
            {
                return expression[0];
            }

            return char.Parse(expression);
        }

        public static object Byte(string expression, CultureInfo culture)
        {
            return byte.Parse(expression, culture);
        }

        public static object NullableByte(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return byte.Parse(expression, culture);
        }

        public static object SByte(string expression, CultureInfo culture)
        {
            return sbyte.Parse(expression, culture);
        }

        public static object NullableSByte(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return sbyte.Parse(expression, culture);
        }

        public static object Int16(string expression, CultureInfo culture)
        {
            return short.Parse(expression, culture);
        }

        public static object NullableInt16(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return short.Parse(expression, culture);
        }

        public static object Int32(string expression, CultureInfo culture)
        {
            return int.Parse(expression, culture);
        }

        public static object NullableInt32(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return int.Parse(expression, culture);
        }

        public static object Int64(string expression, CultureInfo culture)
        {
            return long.Parse(expression, culture);
        }

        public static object NullableInt64(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return long.Parse(expression, culture);
        }

        public static object UInt16(string expression, CultureInfo culture)
        {
            return ushort.Parse(expression, culture);
        }

        public static object NullableUInt16(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return ushort.Parse(expression, culture);
        }

        public static object UInt32(string expression, CultureInfo culture)
        {
            return uint.Parse(expression, culture);
        }

        public static object NullableUInt32(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return uint.Parse(expression, culture);
        }

        public static object UInt64(string expression, CultureInfo culture)
        {
            return ulong.Parse(expression, culture);
        }

        public static object NullableUInt64(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return ulong.Parse(expression, culture);
        }

        public static object Single(string expression, CultureInfo culture)
        {
            return float.Parse(expression, culture);
        }

        public static object NullableSingle(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return float.Parse(expression, culture);
        }

        public static object Double(string expression, CultureInfo culture)
        {
            return double.Parse(expression, culture);
        }

        public static object NullableDouble(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return double.Parse(expression, culture);
        }

        public static object Decimal(string expression, CultureInfo culture)
        {
            return decimal.Parse(expression, culture);
        }

        public static object NullableDecimal(string expression, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return null;
            }

            return decimal.Parse(expression, culture);
        }

        #endregion

        #region Enum

        public static Func<string, object> Enum<TEnum>()
        {
            return Enum(typeof(TEnum), true);
        }

        public static Func<string, object> Enum(Type enumType)
        {
            return Enum(enumType, true);
        }

        public static Func<string, object> Enum(Type enumType, bool ignoreCase)
        {
            if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = Nullable.GetUnderlyingType(enumType);
                return expr => string.IsNullOrEmpty(expr)
                    ? null
                    : System.Enum.Parse(enumType, expr, ignoreCase);
            }

            return expr => System.Enum.Parse(enumType, expr, ignoreCase);
        }

        #endregion
    }
}
