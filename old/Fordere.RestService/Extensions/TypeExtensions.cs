using System;
using System.Globalization;

using ServiceStack;

namespace Fordere.RestService.Extensions
{
    public static class TypeExtensions
    {
        public static object ChangeType(this string str, Type target)
        {
            if (string.IsNullOrEmpty(str))
            {
                return target.GetDefaultValue();
            }

            if (target.IsEnum)
            {
                return Enum.Parse(target, str);
            }

            if (target == typeof(DateTime) || target == typeof(DateTime?))
            {
                // expect iso8601 datestring if target is a DateTime property
                
                // Bug in servicestack json parser, remove trailing "
                str = str.Replace("\"", "");

                return DateTime.Parse(str, null, DateTimeStyles.RoundtripKind);
            }

            if (target.IsGenericType && target.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(target);

                if (underlyingType == typeof(TimeSpan))
                {
                    return TimeSpan.ParseExact(str, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
                }

                if (underlyingType.IsEnum)
                {
                    return Enum.Parse(underlyingType, str);
                }

                return Convert.ChangeType(str, underlyingType, CultureInfo.InvariantCulture);
            }

            if (target == typeof(byte[]))
            {
                return Convert.FromBase64String(str);
            }

            if (target == typeof(TimeSpan))
            {
                return TimeSpan.ParseExact(str, "hh\\:mm\\:ss", CultureInfo.InvariantCulture);
            }

            return Convert.ChangeType(str, target, CultureInfo.InvariantCulture);
        }
    }
}