using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Converters;

namespace DEVES.IntegrationAPI.WebApi
{
    public class EWIDatetimeConverter: IsoDateTimeConverter
    {
        public EWIDatetimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}