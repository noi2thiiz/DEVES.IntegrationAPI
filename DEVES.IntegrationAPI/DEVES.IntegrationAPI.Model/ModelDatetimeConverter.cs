using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;


namespace DEVES.IntegrationAPI.WebApi
{
    public class ModelDatetimeConverter: DateTimeConverterBase
    {
        private const string const_defaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        string DateTimeCustomFormat;
        public ModelDatetimeConverter()
        {
            DateTimeCustomFormat = const_defaultDateTimeFormat;
        }

        public ModelDatetimeConverter(string customFormat)
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
                return "";
            }
            else
            {
                return DateTime.Parse(reader.Value.ToString());
            }
        }
    }
}