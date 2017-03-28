using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCrmInquiryPersonalClientMaster : BaseCommand
    {
        const string ewiEndpointKeyCLSInquiryPersonalClient = "EWI_ENDPOINT_CLSInquiryPersonalClient";

        public override BaseContentOutputModel Execute(object input)
        {
            CRMInquiryClientContentOutputModel crmInqContent = (CRMInquiryClientContentOutputModel)Model.DataModelFactory.GetModel(typeof(CRMInquiryClientContentOutputModel));
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = Guid.NewGuid().ToString();

            try
            {
                //+ Deserialize Input
                InquiryClientMasterInputModel contentModel = DeserializeJson<InquiryClientMasterInputModel>(input.ToString());
                CLSInquiryPersonalClientInputModel clsPersonalInput = new CLSInquiryPersonalClientInputModel();
                clsPersonalInput = (CLSInquiryPersonalClientInputModel)TransformerFactory.TransformModel(contentModel, clsPersonalInput);

                //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                string uid = "CRMClaim";
                CLSInquiryPersonalClientContentOutputModel retCLSInqPersClient = (CLSInquiryPersonalClientContentOutputModel)CallEWIService<EWIResCLSInquiryPersonalClient>
                                                                                        (ewiEndpointKeyCLSInquiryPersonalClient, clsPersonalInput, uid);

                //+ If Success then pour the data from Cleansing to contentOutputModel
                if (IsSearchFound(retCLSInqPersClient))
                {
                    crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCLSInqPersClient, crmInqContent);
                    string crmClientId = "";
                    try
                    {
                        List<string> lstCrmClientId = SearchCrmContactClientId(retCLSInqPersClient.data.First().cleansing_id);
                        if (lstCrmClientId.Count == 1)
                        {
                            crmClientId = lstCrmClientId.First();
                        }
                    }
                    finally
                    {
                        CRMInquiryClientOutputDataModel data = crmInqContent.data.First();
                        data.generalHeader.crmClientId = crmClientId;
                    }
                }
                crmInqContent.code = CONST_CODE_SUCCESS;
                crmInqContent.message = "SUCCESS";
            }
            catch (Exception e)
            {
                crmInqContent.code = CONST_CODE_FAILED;
                crmInqContent.message = e.Message;
                crmInqContent.description = e.StackTrace;
            }
            return crmInqContent;
        }

        internal bool IsSearchFound(CLSInquiryPersonalClientContentOutputModel content)
        {
            return ((content.success | IsOutputSuccess(content)) & (content.data.Count() > 0));
        }
    }
}