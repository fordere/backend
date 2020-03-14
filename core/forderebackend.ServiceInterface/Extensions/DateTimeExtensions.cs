using System;
using System.Globalization;

namespace forderebackend.ServiceInterface.Extensions
{
    public static class DateTimeExtensions
    {
        public const string FordereDateTimeFormat = "dd.MM.yyyy-HH.mm";

        public static string ToFordereFormat(this DateTime date)
        {
            return date.ToString(FordereDateTimeFormat);
        }

        public static DateTime FromFordereFormat(this string date)
        {
            return DateTime.ParseExact(date, FordereDateTimeFormat, CultureInfo.CurrentCulture);
        }
    }
}