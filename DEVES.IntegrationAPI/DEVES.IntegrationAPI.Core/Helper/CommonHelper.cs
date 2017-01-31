using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DEVES.IntegrationAPI.Core.Helper
{
    public static class CommonHelper
    {
        public static string GetIpAddress()
        {
            string ipaddress;
            ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ipaddress))
                ipaddress += HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            return ipaddress;
        }
    }
}
