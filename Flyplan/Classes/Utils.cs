using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FlyPlan.Api.Classes
{
    public static class Utils
    {
        private static Random random = new Random();
        private static List<Type> allowedTypes = new List<Type> { typeof(string), typeof(DateTime?) };

        private static string DATE_TIME_FORMAT = "ddd MMM dd yyyy HH:mm:ss";

        public static string GenerateReservationCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal static DateTime ToDateTime(this string value)
        {
            try
            {
                var dateTime = DateTime.ParseExact(value.Replace(" GMT+0000", String.Empty), DATE_TIME_FORMAT, CultureInfo.CurrentCulture);

                return dateTime.Date;
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
        }

        internal static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }

        internal static List<PropertyInfo> GetProperties(this object obj)
        {
            return obj.GetType()
                .GetProperties()
                .Where(p => allowedTypes.Contains(p.PropertyType) &&
                            string.IsNullOrWhiteSpace(p.GetValue(obj)?.ToString())).ToList();
        }
    }
}
