using System;
using System.Web.Management;

namespace  DEVES.IntegrationAPI.WebApi.Core.DataAdepter
{
    public class DbResult
    {
        public string code { get; set; }
        public string message { get; set; }
        public int total { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public object data { get; set; }


    }
}