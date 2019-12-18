using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlyPlan.Api.Classes
{
    public static class Utils
    {
        private static Random random = new Random();

        private static string DATE_TIME_FORMAT = "ddd MMM dd yyyy HH:mm:ss 'GMT'K";

        internal static string GenerateReservationCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal static DateTime? ConvertToDateTime(string value)
        {
            try
            {
                var dateTime = DateTime.ParseExact(value, DATE_TIME_FORMAT, CultureInfo.InvariantCulture);

                return dateTime;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
