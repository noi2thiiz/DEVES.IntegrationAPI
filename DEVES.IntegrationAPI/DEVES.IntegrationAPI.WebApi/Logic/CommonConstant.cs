using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public static class CommonConstant
    {
        public const string ewiEndpointKeyCLSInquiryPersonalClient = "EWI_ENDPOINT_CLSInquiryPersonalClient";
        public const string ewiEndpointKeyCOMPInquiryClient = "EWI_ENDPOINT_COMPInquiryClientMaster";

        public const string sqlcmd_Get_RegClaimInfo = "sp_CustomApp_RegClaimInfo_Incident";
        public const string ewiEndpointKeyClaimRegistration = "EWI_ENDPOINT_ClaimRegistration";

    }
}