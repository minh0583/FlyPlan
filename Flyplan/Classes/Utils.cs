using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyPlan.Api.Classes
{
    public static class Utils
    {
        private static Random random = new Random();

        internal static string GenerateReservationCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
