using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Templates;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCrmInquiryCorporateClientMaster : BaseCommand
    {
        //const string ewiEndpointKeyCLSInquiryCorporateClient = "EWI_ENDPOINT_CLSInquiryCorporateClient";

        public override BaseDataModel Execute(object input)
        {
            CRMInquiryClientContentOutputModel crmInqContent = (CRMInquiryClientContentOutputModel)Model.DataModelFactory.GetModel(typeof(CRMInquiryClientContentOutputModel));
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = Guid.NewGuid().ToString();
            bool bFoundIn_APAR_or_Master = false;
            try
            {
                //+ Deserialize Input
                InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;// DeserializeJson<InquiryClientMasterInputModel>(input.ToString());

                if (contentModel.conditionHeader.roleCode.ToUpper() == ENUM_CLIENT_ROLE.G.ToString())
                {
                    #region Call CLS_InquiryCLSCorporateClient through ServiceProxy
                    CLSInquiryCorporateClientInputModel clsCorpInput = new CLSInquiryCorporateClientInputModel();
                    clsCorpInput = (CLSInquiryCorporateClientInputModel)TransformerFactory.TransformModel(contentModel, clsCorpInput);

                    string uid = "CRMClaim";
                    CLSInquiryCorporateClientContentOutputModel retCLSInqCorpClient = (CLSInquiryCorporateClientContentOutputModel)CallDevesJsonProxy<EWIResCLSInquiryCorporateClient>
                                                                                            (CommonConstant.ewiEndpointKeyCLSInquiryCorporateClient, clsCorpInput, uid);

                    //+ If Success then pour the data from Cleansing to contentOutputModel


                    if (IsSearchFound(retCLSInqCorpClient))
                    {

                        crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCLSInqCorpClient, crmInqContent);
                        string crmClientId = "";
                        try
                        {
                            List<string> lstCrmClientId = SearchCrmAccountClientId(retCLSInqCorpClient.data.First().cleansing_id);
                            if (lstCrmClientId.Count == 1)
                            {
                                crmClientId = lstCrmClientId.First();
                            }
                            bFoundIn_APAR_or_Master = true;
                        }
                        finally
                        {
                            CRMInquiryClientOutputDataModel data = crmInqContent.data.First();
                            data.generalHeader.crmClientId = crmClientId;
                        }

                    }
                }
                #endregion 

                else
                {
                    #region IF inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH
                    // Search in MASTER: MOTOR_InquiryMasterASRH()
                    InquiryMasterASRHDataInputModel inqASRHIn = (InquiryMasterASRHDataInputModel)DataModelFactory.GetModel(typeof(InquiryMasterASRHDataInputModel));
                    inqASRHIn = (InquiryMasterASRHDataInputModel)TransformerFactory.TransformModel(contentModel, inqASRHIn);
                    InquiryMasterASRHContentModel inqASRHOut = CallDevesServiceProxy<InquiryMasterASRHOutputModel, InquiryMasterASRHContentModel>(CommonConstant.ewiEndpointKeyMOTORInquiryMasterASRH, inqASRHIn);

                    if (inqASRHOut != null && inqASRHOut.ASRHListCollection != null)
                    {
                        if (inqASRHOut.ASRHListCollection.Count > 0)
                        {
                            crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(inqASRHOut, crmInqContent);
                            bFoundIn_APAR_or_Master = true;
                        }
                    }
                    #endregion inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH
                }

                if (bFoundIn_APAR_or_Master)
                {
                    #region Search crmClientId by CleansingId
                    foreach ( CRMInquiryClientOutputDataModel data in crmInqContent.data)
                    {
                        List<string> crmData = SearchCrmContactClientId(contentModel.conditionDetail.cleansingId);
                        if (crmData != null && crmData.Count == 1)
                        {
                            data.generalHeader.crmClientId = crmData.First();
                        }
                    }
                    #endregion Search crmClientId by CleansingId
                }
                else
                {
                    #region Call COMP_Inquiry through ServiceProxy
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


                    #endregion Call COMP_Inquiry through ServiceProxy
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

        internal bool IsSearchFound(CLSInquiryCorporateClientContentOutputModel content)
        {
            return ((content.success | IsOutputSuccess(content)) & (content.data.Count() > 0));
        }
    }
}