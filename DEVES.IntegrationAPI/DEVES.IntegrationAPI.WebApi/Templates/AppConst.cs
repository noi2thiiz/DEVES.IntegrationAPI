using System;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public static class AppConst
    {
        public const string CODE_SUCCESS = "200";
        public const string CODE_CLS_DUPLICATE = "CLS-1109";
        public const string CODE_CLS_NOTFOUND = "CLS-1106";
        public const string MESSAGE_SUCCESS = "Success";
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

        public const string QA_SERVER_NAME = "8121-CRM-QA";

        public const string CONST_DEFAULT_UID = "";

        public const string PRO1_SERVER_NAME = "CRM-APP01";
        public const string PRO2_SERVER_NAME = "CRM-APP02";
        public const string LOCAL_MACHINE_NAAME = "DESKTOP-Q30CAGJ";

        public static string COMM_BACK_DAY = AppConfig.Instance.Get("SEARCH_POLISY_BACKDAY")??"5";

        public static bool IS_FORCE_TO_DB_READER = false;

        public static bool IS_SERVER=
        
             Environment.MachineName == QA_SERVER_NAME || Environment.MachineName == PRO1_SERVER_NAME ||
                   Environment.MachineName == PRO2_SERVER_NAME;

        public static bool IS_PRODUCTION =

            Environment.MachineName == PRO1_SERVER_NAME ||
            Environment.MachineName == PRO2_SERVER_NAME;

        // สองค่านี้จะเอาไปรวมกัน เช่น 1 minutes 2 seconds
        public static int HTTP_CLIENT_TIMEOUT_IN_MINUTES = 5;
        public static int HTTP_CLIENT_TIMEOUT_IN_SECONDS = 0; 
    }

    public class TestAppConst
    {
        public string GetCommBackDayKey()
        {
            return AppConst.COMM_BACK_DAY;

        }
    }
}