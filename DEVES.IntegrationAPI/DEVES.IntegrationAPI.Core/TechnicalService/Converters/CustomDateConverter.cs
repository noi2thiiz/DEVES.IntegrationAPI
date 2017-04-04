using Newtonsoft.Json.Converters;

namespace DEVES.IntegrationAPI.WebApi.Core
{
    /// <remarks>
    /// http://www.vickram.me/custom-datetime-model-binding-in-asp-net-web-api/
    /// https://github.com/domaindrivendev/Swashbuckle/issues/741
    /// Credit: http://stackoverflow.com/a/21915763/1309248
    /// </remarks>
    public class CustomDateConverter : IsoDateTimeConverter
    {

        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public CustomDateConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }


    }
}