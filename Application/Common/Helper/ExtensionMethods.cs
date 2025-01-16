using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Application.Common.Helper
{
    public static class ExtensionMethods
    {
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
            return $"?{string.Join("&", properties.ToArray())}";
        }

        /// <summary>
        /// Uses the regex expression "[^0-9]" to strip out any non numeric characters.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StripNonNumericCharacters(this string value)
        {
            //garbage in. garbage out.
            if (string.IsNullOrEmpty(value))
            {
                return @value;
            }

            return Regex.Replace(value, "[^0-9]", string.Empty);
        }
        public static string HumanisedDate(this DateTime date)
        {
            string ordinal;

            switch (date.Day)
            {
                case 1:
                case 21:
                case 31:
                    ordinal = "st";
                    break;
                case 2:
                case 22:
                    ordinal = "nd";
                    break;
                case 3:
                case 23:
                    ordinal = "rd";
                    break;
                default:
                    ordinal = "th";
                    break;
            }

            return string.Format("{0:dddd, MMMM} {0:%d}{1}, {0:yyyy}", date, ordinal);
        }
        /// <summary>
        ///  A DateTime extension method that return a DateTime with the time set to "00:00:00:000". The first moment of
        ///     the day. Use "DateTime2" column type in sql to keep the precision.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime @date)
        {
            return new DateTime(@date.Year, @date.Month, @date.Day, 0, 0, 0, 0);
        }

        /// <summary>
        ///     A DateTime extension method that return a DateTime with the time set to "23:59:59:999". The last moment of
        ///     the day. Use "DateTime2" column type in sql to keep the precision.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>A DateTime of the day with the time set to "23:59:59:999".</returns>
        public static DateTime EndOfDay(this DateTime @date)
        {
            return new DateTime(@date.Year, @date.Month, @date.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
        }

        /// <summary>
        /// This Extention Validate the email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string @email)
        {
            return CommonHelper.IsValidEmail(@email);
        }
    }
}
