namespace DEVES.IntegrationAPI.WebApi.Core.FieldErrorsParser
{
    public class InvalidEnumError:ErrorBase
    {
        public InvalidEnumError(string fieldName,string message)
        {
            this.Init(fieldName,"InvalidEnumValue",message);
        }
    }
}