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


        public const string sqlcmd_Get_RegClaimInfo = "sp_CustomApp_RegClaimInfo_Incident";

    }
}