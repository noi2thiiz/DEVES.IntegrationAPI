using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public static class CommonConstant
    {
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

    }
}