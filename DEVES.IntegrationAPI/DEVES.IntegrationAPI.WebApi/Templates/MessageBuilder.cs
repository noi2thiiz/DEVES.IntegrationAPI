namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public class MessageBuilder
    {
        private static MessageBuilder _instance;

        private MessageBuilder()
        {
        }

        public static MessageBuilder Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new MessageBuilder();

                return _instance;
            }
        }

        public string GetInvalidMasterMessage()
        {
            return "The value is not defined in master data";
        }

        public string GetInvalidMasterMessage(string masterName, string value)
        {
            return $"The {masterName.ToLower()} code '{value}' is not defined in master data";
        }
    }
}