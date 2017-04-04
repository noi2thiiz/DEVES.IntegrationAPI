namespace DEVES.IntegrationAPI.WebApi.Services.FieldErrorsParser
{
    public abstract class ErrorBase
    {

        private string name { get; set; }
        public string reason { get; set; }
        public string message { get; set; }

       protected void Init(string name, string reason, string message)
        {
            this.name = name;
            this.reason = reason;
            this.message = message;
        }

        public string getFieldName()
        {
            return name;
        }
    }
}