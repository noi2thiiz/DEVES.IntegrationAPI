using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCrmInquiryPersonalClientMaster : BaseCommand
    {

        public override BaseDataModel Execute(object input)
        {
            #region Prepare box for output 
            CRMInquiryClientContentOutputModel crmInqContent = (CRMInquiryClientContentOutputModel)Model.DataModelFactory.GetModel(typeof(CRMInquiryClientContentOutputModel));
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = Guid.NewGuid().ToString();
            #endregion Prepare box for output 

            try
            {
                #region Search Client from Cleansing

                //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
                CLSInquiryPersonalClientInputModel clsPersonalInput = new CLSInquiryPersonalClientInputModel();
                clsPersonalInput = (CLSInquiryPersonalClientInputModel)TransformerFactory.TransformModel(contentModel, clsPersonalInput);

                //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                CLSInquiryPersonalClientContentOutputModel retCLSInqPersClient = CallDevesServiceProxy<EWIResCLSInquiryPersonalClient, CLSInquiryPersonalClientContentOutputModel>
                                                                                        (CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient, clsPersonalInput);

                //++ If Found records in Cleansing(CLS) then pour the data from Cleansing to contentOutputModel
                if (IsSearchFound(retCLSInqPersClient))
                {
                    crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCLSInqPersClient, crmInqContent);
                    string crmClientId = "";
                    try
                    {
                        List<string> lstCrmClientId = SearchCrmContactClientId(retCLSInqPersClient.data.First().cleansing_id);
                        if (lstCrmClientId != null && lstCrmClientId.Count == 1)
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
                #endregion Search Client from Cleansing
                else //+ If not records found in Cleansing(CLS), then Search from Polisy400 
                #region Search client from Polisy400
                {
                    try
                    {
                        COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                        compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(contentModel, compInqClientInput);

                        //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                        EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                                                                                                (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);

                        //Found in Polisy400
                        if (retCOMPInqClient.clientListCollection != null) 
                        {
                            crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCOMPInqClient, crmInqContent);
                        }
                        //Not found in Polisy400
                        else
                        {

                        }

                    }
                    catch (Exception e)
                    {
                        throw;
                    }

                }
                #endregion Search client from Polisy400


                #region finishing output 

                crmInqContent.code = CONST_CODE_SUCCESS;
                crmInqContent.message = "SUCCESS";
                #endregion finishing output 
            }
            catch (Exception e)
            {
                #region put error in the output

                crmInqContent.code = CONST_CODE_FAILED;
                crmInqContent.message = e.Message;
                crmInqContent.description = e.StackTrace;
                #endregion put error in the output
            }
            return crmInqContent;
        }

        internal bool IsSearchFound(CLSInquiryPersonalClientContentOutputModel content)
        {
            return ((content.success | IsOutputSuccess(content)) & (content.data.Count() > 0));
        }
    }
}