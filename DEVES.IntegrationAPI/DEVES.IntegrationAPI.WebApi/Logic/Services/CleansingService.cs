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
using DEVES.IntegrationAPI.WebApi.DataAccessService;
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
            if (false == contentObj.success)
            {
                throw new Exception($"CLS Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }

           
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
            if (false == contentObj.success)
            {
                throw new Exception($"CLS Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }
            if (false == (contentObj.content.code == AppConst.CODE_CLS_DUPLICATE || contentObj.content.code == AppConst.CODE_SUCCESS))
            {
                throw new Exception($"CLS Error {contentObj.content.code}: {contentObj.content.message}");
            }
            return contentObj;
        }

        public CLSCreatePersonalClientOutputModel CreatePersonalClient(CLSCreatePersonalClientInputModel input)
        {
           
            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSCreatePersonalClient);
            Console.WriteLine("98:"+endpoint);
            var result = SendRequest(input, endpoint);
            Console.WriteLine("result"+ result.Content);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                
                throw new InternalErrorException(result.Message);
            }
            Console.WriteLine("1000000005");
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLSCreatePersonalClientOutputModel>(result.Content);
            if (false == contentObj.success)
            {
                throw new Exception($"CLS Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }
            if (false == (contentObj.content.code == AppConst.CODE_CLS_DUPLICATE || contentObj.content.code == AppConst.CODE_SUCCESS))
            {
                throw new Exception($"CLS Error {contentObj.content.code}: {contentObj.content.message}");
            }
            return contentObj;
        }

        public CLSCreateCorporateClientOutputModel CreateCorporateClient(CLSCreateCorporateClientInputModel input)
        {

            string endpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyCLSCreateCorporateClient);
            var result = SendRequest(input, endpoint);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<CLSCreateCorporateClientOutputModel>(result.Content);
            if (false == contentObj.success)
            {
                throw new Exception($"CLS Error {contentObj.responseCode}: {contentObj.responseMessage}");
            }
            if (false == (contentObj.content.code == AppConst.CODE_CLS_DUPLICATE || contentObj.content.code == AppConst.CODE_SUCCESS))
            {
                throw new Exception($"CLS Error {contentObj.content.code}: {contentObj.content.message}");
            }
            return contentObj;
        }
        /// <summary>
        /// สร้าง client  Personal เพื่อเอา CleansingId หรือ ถ้าซ้ำก็เอา CleansingId ที่ CLS return กลับมา
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetPersonalCleansingId(CLSCreatePersonalClientInputModel input)
        {
           
            var createResult = CreatePersonalClient(input);
            Console.WriteLine("createResult.success"+ createResult.success);

            

            if (createResult.success)
            {
                if (createResult.content.code == AppConst.CODE_CLS_DUPLICATE || createResult.content.code == AppConst.CODE_SUCCESS)
                {
                    string cleansingId = createResult.content.data.cleansingId;
                    Console.WriteLine("cleansingId=" + cleansingId);
                    return cleansingId;
                }
  
            }
           

            return "";
        }
        /// <summary>
        /// สร้าง client corporate เพื่อเอา CleansingId หรือ ถ้าซ้ำก็เอา CleansingId ที่ CLS return กลับมา
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetCorporateCleansingId(CLSCreateCorporateClientInputModel input)
        {

            var createResult = CreateCorporateClient(input);
            if (createResult.success)
            {
                if (createResult.content.code == AppConst.CODE_CLS_DUPLICATE || createResult.content.code == AppConst.CODE_SUCCESS)
                {
                    string cleansingId = createResult.content.data.cleansingId;
                    Console.WriteLine("cleansingId=" + cleansingId);
                    return cleansingId;
                }
               
            }
           

            return "";
        }

    }
}