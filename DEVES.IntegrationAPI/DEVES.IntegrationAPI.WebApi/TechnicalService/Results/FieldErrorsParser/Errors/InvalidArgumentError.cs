namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public class InvalidArgumentError:ErrorBase
    {
        public InvalidArgumentError(string fieldName,string message)
        {
            this.Init(fieldName,"InvalidArgument",message);
        }

    }
}