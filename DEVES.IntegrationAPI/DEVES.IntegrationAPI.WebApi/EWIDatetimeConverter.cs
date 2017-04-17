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
        string DateTimeCustomFormat;
        public EWIDatetimeConverter()
        {
            DateTimeCustomFormat = "yyyy-MM-dd HH:mm:ss";
        }

        public EWIDatetimeConverter(string customFormat)
        {
            DateTimeCustomFormat = customFormat;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            CultureInfo enUS = new CultureInfo("en-US");
            writer.WriteValue(((DateTime)value).ToString(DateTimeCustomFormat, enUS));
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