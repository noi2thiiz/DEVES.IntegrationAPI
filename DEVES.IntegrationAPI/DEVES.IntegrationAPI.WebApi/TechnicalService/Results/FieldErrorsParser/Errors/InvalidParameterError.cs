namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public class InvalidParameterError:ErrorBase
    {
        public InvalidParameterError(string fieldName,string message)
        {
            this.Init(fieldName,"InvalidParameter",message);
        }

    }
}