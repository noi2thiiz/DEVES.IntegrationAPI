using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using DEVES.IntegrationAPI.WebApi.Logic;

namespace DEVES.IntegrationAPI.WebApi
{
    public class EWIDatetimeConverter: DateTimeConverterBase
    {
        private const string const_defaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        string DateTimeCustomFormat;
        public EWIDatetimeConverter()
        {
            DateTimeCustomFormat = const_defaultDateTimeFormat;
        }

        public EWIDatetimeConverter(string customFormat)
        {
            DateTimeCustomFormat = customFormat;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            if (value == null)
            {
                writer.WriteValue("");
            }
            if(DateTimeCustomFormat== const_defaultDateTimeFormat && value == null)
            {
                writer.WriteValue("");
            }
            //else 
            //if ((string)value == "1900-01-01 00:00:00")
           // {
            //   writer.WriteValue("");
            //}
            else
            {
                writer.WriteValue(((DateTime)value).ToString(DateTimeCustomFormat, enUS));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string stringDate = reader.Value.ToString();
            if (string.IsNullOrEmpty(stringDate))
            {
                return CommonConstant.GetDevesAPINullDate();
            }
            else
            {
                return DateTime.Parse(reader.Value.ToString());
            }
        }
    }
}