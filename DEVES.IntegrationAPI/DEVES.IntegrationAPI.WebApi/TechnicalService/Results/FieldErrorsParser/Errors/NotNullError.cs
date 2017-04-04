namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public class NotNullError:ErrorBase
    {
        public NotNullError(string fieldName,string message)
        {
            this.Init(fieldName,"NotNull",message);
        }

    }
}