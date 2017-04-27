using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.Core.TechnicalService.ExtensionMethods
{
    public static class DateTimeHelper
    {

        public static string ToEwiDateString(this DateTime obj)

        {
            return "2017-04-20 20:03:20";
           // var usaCulture = new CultureInfo("en-US");
            //return obj.ToString("yyyy-MM-dd HH:mm:ss", usaCulture);


        }
    }
}
