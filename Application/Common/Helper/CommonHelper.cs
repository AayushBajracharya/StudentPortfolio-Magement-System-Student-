using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Common.Helper
{
    public static class CommonHelper
    {
        /// <summary>
        /// This method generate random number
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static string GenerateRandomNumber(int number)
        {
            string randomNumber = Guid.NewGuid().ToString("N").Substring(0, number);
            return randomNumber;
        }

        /// <summary>
        /// This method validate the email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string @email)
        {
            if (string.IsNullOrWhiteSpace(@email))
                return false;
            else
                return new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.IgnoreCase).IsMatch(email);
        }
    }
}
