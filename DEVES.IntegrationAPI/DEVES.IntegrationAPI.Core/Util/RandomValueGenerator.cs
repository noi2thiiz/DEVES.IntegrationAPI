using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.Util
{
    public class RandomValueGenerator
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumber(int length)
        {

            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static DateTime RandomDateTime()
        {
            return DateTime.UtcNow.AddDays(-(new Random().Next(90)));
        }

        public static string RandomOptions(string[] options)
        {

          
            int rInt = random.Next(0, options.Length-1);
            return options[rInt];
        }
        
    }

   
}
