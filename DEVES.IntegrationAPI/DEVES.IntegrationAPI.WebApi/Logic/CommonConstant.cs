using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public static class CommonConstant
    {
        public static string CRMConnectionStr = ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString;

        public const string ewiEndpointKeyAPARInquiryPayeeList = "EWI_ENDPOINT_APARInquiryPayeeList";
        public const string ewiEndpointKeyCLSInquiryPersonalClient = "EWI_ENDPOINT_CLSInquiryPersonalClient";
        public const string ewiEndpointKeyCOMPInquiryClient = "EWI_ENDPOINT_COMPInquiryClientMaster";
        public const string ewiEndpointKeyLOCUSClaimRegistration = "EWI_ENDPOINT_ClaimRegistration";
        public const string ewiEndpointKeySAPInquiryVendor = "EWI_ENDPOINT_SAPInquiryVendor";
        public const string ewiEndpointKeyMOTORInquiryMasterASRH = "EWI_ENDPOINT_MOTORInquiryMasterASRH";

        public const string ewiEndpointKeyCLSInquiryCorporateClient = "EWI_ENDPOINT_CLSInquiryCorporateClient";
        public const string ewiEndpointKeyCLSCreatePersonalClient = "EWI_ENDPOINT_CLSCreatePersonalClient";
        public const string ewiEndpointKeyCLSCreateCorporateClient = "EWI_ENDPOINT_CLSCreateCorporateClient";
        public const string ewiEndpointKeySAPCreateVendor = "EWI_ENDPOINT_SAPCreateVendor";
        public const string ewiEndpointKeyCLIENTCreatePersonalClient = "EWI_ENDPOINT_CLIENTCreatePersonalClient";
        public const string ewiEndpointKeyCLIENTCreateCorporateClient = "EWI_ENDPOINT_CLIENTCreateCorporateClient";
        public const string ewiEndpointKeyCLIENTUpdateCorporateClient = "EWI_ENDPOINT_CLIENTUpdateCorporateClient";

        public const string sqlcmd_Get_RegClaimInfo = "sp_CustomApp_RegClaimInfo_Incident";


        public const string CONST_SYSTEM_POLISY400 = "POLISY400";
        public const string CONST_SYSTEM_CRM = "CRM";
        public const string CONST_SYSTEM_CLS = "CLS";
        public const string CONST_SYSTEM_SAP = "SAP";
        public const string CONST_SYSTEM_MASTER_ASRH = "MASTER_ASRH";
        public const string CONST_SYSTEM_APAR= "APAR";




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
        public const string DEFAULT_CORPORATE_SAP_VENDOR_COMPANY = "2020";

        public const string QA_SERVER_NAME = "8121-CRM-QA";

        public const string CONST_DEFAULT_UID = "";


        public static DateTime GetDevesAPINullDate()
        {
            return new DateTime(1900, 01, 01);
        }
    }
}