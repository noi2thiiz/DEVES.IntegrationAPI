namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public static class AppConst
    {
        public const string CODE_SUCCESS = "200";
        public const string MESSAGE_SUCCESS = "SUCCESS";
        public const string CODE_FAILED = "500";
        public const string MESSAGE_INTERNAL_ERROR = "An error has occurred";
        public const string DEFAULT_UID = "uid";
        public const string CODE_INVALID_INPUT = "400";
        public const string MESSAGE_INVALID_INPUT = "Invalid input(s)";
        public const string DESC_INVALID_INPUT = "Some of your input is invalid. Please recheck again";

        //Create Payee
        public const string DEFAULT_CORPORATE_RECPTYPE = "53";//นิติบุคคล
        public const string DEFAULT_PERSONAL_RECPTYPE = "03";//บุคคล
        public const string DEFAULT_CORPORATE_SAP_VENDOR_COMPANY  = "2020";
    }
}