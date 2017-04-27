namespace DEVES.IntegrationAPI.WebApi.Core.FieldErrorsParser
{
    public class InvalidDateTimeError:ErrorBase
    {
        private string template = "The field {0} must be a string with datetime format 'YYYY-MM-dd HH:mm:ss'";
        public InvalidDateTimeError(string fieldName,string message)
        {
            this.Init(fieldName,"InvalidDateTime",string.Format(template,fieldName));
        }
    }
}