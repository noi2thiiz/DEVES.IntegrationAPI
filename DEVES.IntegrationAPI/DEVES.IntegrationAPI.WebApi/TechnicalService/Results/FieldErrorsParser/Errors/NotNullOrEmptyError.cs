namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public class NotNullOrEmptyError:ErrorBase
    {
        public NotNullOrEmptyError(string fieldName,string message)
        {
            this.Init(fieldName,"NotNullOrEmpty",$"The required field cannot be null or empty");
        }

    }
}