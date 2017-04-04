namespace DEVES.IntegrationAPI.WebApi.Core.FieldErrorsParser
{
    public class InvalidStringLengthError:ErrorBase
    {
        public InvalidStringLengthError(string fieldName,string message)
        {
            this.Init(fieldName,"InvalidStringLength",message);
        }

    }
}