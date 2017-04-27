namespace DEVES.IntegrationAPI.WebApi.Core.FieldErrorsParser
{
    public class NotNullOrEmptyError:ErrorBase
    {
        public NotNullOrEmptyError(string fieldName,string message)
        {
            this.Init(fieldName,"NotNullOrEmpty",$"The required field cannot be null or empty");
        }

    }
}