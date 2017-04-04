using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace DEVES.IntegrationAPI.WebApi
{
    public class EWIDatetimeConverter: DateTimeConverterBase
    {
        string DateTimeCustomFormat;

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
            return DateTime.Parse(reader.Value.ToString());
        }
    }
}