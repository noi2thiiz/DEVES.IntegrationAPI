namespace DEVES.IntegrationAPI.WebApi.Core.FieldErrorsParser
{
    public class InvalidParameterError:ErrorBase
    {
        public InvalidParameterError(string fieldName,string message)
        {
            this.Init(fieldName,"InvalidParameter",message);
        }

    }
}