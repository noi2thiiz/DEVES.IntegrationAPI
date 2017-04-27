namespace DEVES.IntegrationAPI.WebApi.Core.FieldErrorsParser
{
    public class NotNullError:ErrorBase
    {
        public NotNullError(string fieldName,string message)
        {
            this.Init(fieldName,"NotNull",message);
        }

    }
}