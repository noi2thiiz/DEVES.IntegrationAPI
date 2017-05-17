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
    public class PolisyClientService : BaseEwiServiceProxy
    {
        #region Singleton
        private static PolisyClientService _instance;

        private PolisyClientService()
        {
            this.serviceName = "COMPInquiryClientNoByCleansingID";
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


        #endregion
        /// <summary>
        /// สำหรับหา Polisy Client จาก CleansingId โดยที่หากพบมากกว่า 1 รายการจะ return รายการที่มี clientNumber มากสุด
        /// </summary>
        /// <param name="cleansingClientId"></param>
        /// <param name="clientType"></param>
        /// <returns></returns>
        public COMPInquiryClientMasterClientModel FindByCleansingId(string cleansingClientId, string clientType = "P")
        {
            var input = new COMPInquiryClientNoByCleansingIdInputModel
            {
                cleansingId = cleansingClientId,
                topRecord = "5"

            };
            Console.WriteLine(input.ToJson());


            string endpoint = AppConfig.Instance.Get("EWI_ENDPOINT_COMPInquiryClientNoByCleansingID");
            Console.WriteLine(endpoint);
            var result = SendRequest(input, endpoint);
            Console.WriteLine("====result====");
            Console.WriteLine(result.Content);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("====InternalErrorException====");
                Console.WriteLine(result.Message);
                throw new InternalErrorException(result.Message);
            }
            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<COMPInquiryClientNoByCleansingIdOutputModel>(result.Content);
            var clientListCollection = contentObj?.content?.itemListCollection;
            //เลือกรายการที่มีค่า clientNumber มากที่สุด
            var selecetedResult = clientListCollection?.OrderByDescending(r => r.itemList.clntnum).First();
           
            var model = new COMPInquiryClientMasterClientModel
            {
                clientNumber = selecetedResult?.itemList?.clntnum,
                cleansingId = selecetedResult?.itemList?.cleansingId,
                clientType = selecetedResult?.itemList?.clientType
            };
            return model;


        }


    }
}