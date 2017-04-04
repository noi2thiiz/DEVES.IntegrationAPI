namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public class InvalidStringLengthError:ErrorBase
    {
        public InvalidStringLengthError(string fieldName,string message)
        {
            this.Init(fieldName,"InvalidStringLength",message);
        }

    }
}