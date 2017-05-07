using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;


namespace getWebconfig
{
    /// <summary>
    /// Summary description for getWebconfig
    /// </summary>
    public static class selectString


    {
        public static string SqlConnect = ConfigurationManager.AppSettings["CRMDATA"].ToString();

    }

   
}

