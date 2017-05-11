using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class PolisyClientService: BaseProxyService
    {
        
        public COMPInquiryClientMasterClientModel FindByCleansingId(string cleansingClientId,string clientType="P")
        {
            var input = new COMPInquiryClientMasterInputModel
            {
                cleansingId = cleansingClientId,
                cltType = clientType
            };
            Console.WriteLine(input.ToJson());

            
            const string endpoint = "https://crmappdev.deves.co.th/proxy/xml.ashx?http://192.168.3.194/ServiceProxy/ClaimMotor/jsonservice/COMP_InquiryClientMaster";
       
            var result = SendRequest(input, endpoint);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<COMPInquiryClientMasterOutputModel>(result.Content);
            return contentObj?.content?.clientListCollection?[0].clientList;
        }



        private static PolisyClientService _instance;

        private PolisyClientService()
        {
        }

        public static PolisyClientService Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new PolisyClientService();

                return _instance;
            }
        }
    }
}