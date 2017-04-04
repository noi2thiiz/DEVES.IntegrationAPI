namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public class UnknownError:ErrorBase
    {



        public UnknownError(string fieldName,string message)
        {
            this.Init(fieldName,"UnknownError",message);
        }
    }
}