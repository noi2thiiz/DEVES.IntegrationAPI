using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Core.DataAdepter;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.IdentityModel.Protocols.WSIdentity;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class CleansingClientService: BaseEwiServiceProxy
    {
        #region Singleton
        private static CleansingClientService _instance;

        private CleansingClientService()
        {
           
        }

        public static CleansingClientService Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new CleansingClientService();

                return _instance;
            }
        }

        internal object InquiryPersonalClient(CRMRequestCleansingIdDataInputModel input)
        {
            throw new NotImplementedException();
        }
        public class RemoveByCleansingIdInputModel :BaseDataModel
        {
            public string cleansing_id { get; set; }
        }

        #endregion
        public BaseEWIResponseModel RemoveByCleansingId(string cleansingClientId, string clientType = "P")
        {
            
            var input = new RemoveByCleansingIdInputModel
            {
                cleansing_id = cleansingClientId
            };
            Console.WriteLine(input.ToJson());

            string endpointKey ;
            if (clientType == "P")
            {
                endpointKey = "EWI_ENDPOINT_CLSDeleteCLSPersonalClient";
                this.serviceName = "CLSDeleteCLSPersonalClient";
            }
            else
            {
                endpointKey = "EWI_ENDPOINT_CLSDeleteCLSCorporateClient";
                this.serviceName = "CLSDeleteCLSCorporateClient";
            }
            string endpoint =  AppConfig.Instance.Get(endpointKey);


            var result = SendRequest(input, endpoint);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<BaseEWIResponseModel>(result.Content);
            return contentObj;
        }

        public EWIResCLSInquiryPersonalClient InquiryPersonalClient(CLSInquiryPersonalClientInputModel input)
        {
           // var endpointKey = ;
            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient);
            var result = SendRequest(input, endpoint);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<EWIResCLSInquiryPersonalClient>(result.Content);
            return contentObj;
        }

        public CLSCreatePersonalClientOutputModel CreatePersonalClient(CLSCreatePersonalClientInputModel input)
        {
           
            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSCreatePersonalClient);
            var result = SendRequest(input, endpoint);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLSCreatePersonalClientOutputModel>(result.Content);
            return contentObj;
        }

        public string GetCleansingId(CRMRequestCleansingIdDataInputModel input)
        {
           
            //หาไม่เจอ เลยต้องสร้างใหม่
            var clsPersonalInput = new CLSCreatePersonalClientInputModel
            {
                roleCode = "G",
                personalName = input?.firstname ?? "",
                personalSurname = input?.lastname ?? "",
                telephone1 = input?.telephone1 ?? "",
                telephone2 = input?.telephone2 ?? "",
                mobilePhone = input?.mobilePhone1??"",
                emailAddress = input?.emailaddress1??"",
                fax = input?.fax,
                idCitizen = input?.citizenId
                
             };

            if(!string.IsNullOrEmpty(input?.gendercode))
            {
                switch (input.gendercode)
                {
                    case "100000001":
                        clsPersonalInput.sex = "M"; break;
                    case "100000002":
                        clsPersonalInput.sex = "F"; break;
                    default:
                        clsPersonalInput.sex = "U"; break;
                }
                
            }

            if (!string.IsNullOrEmpty(input?.personalCode))
            {
                clsPersonalInput.salutation = input?.personalCode?.ToUpper()??"0001";
            }
            

            var createResult = CreatePersonalClient(clsPersonalInput);
            if (createResult.success)
            {
                return createResult?.content?.data?.cleansingId??"";
            }

            return "";
        }
        
    }
}