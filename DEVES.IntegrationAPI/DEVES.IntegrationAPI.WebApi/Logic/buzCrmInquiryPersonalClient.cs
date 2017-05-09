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
using WebGrease.Css.Visitor;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCrmInquiryPersonalClientMaster : BuzCommand
    {
        protected string nodePart = "nodePart:";
        protected void AddNode(string node)
        {
            nodePart += node+",==>";
        }

        public override BaseDataModel ExecuteInput(object input)
        {
            AddNode("START");
            #region Prepare box for output 
            CRMInquiryClientContentOutputModel crmInqContent = (CRMInquiryClientContentOutputModel)Model.DataModelFactory.GetModel(typeof(CRMInquiryClientContentOutputModel));
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = TransactionId;
            #endregion Prepare box for output 

            AddNode("35");

            #region Search client from CRM



            #endregion

            #region Search Client from Cleansing
            AddNode("start Search Client from Cleansing ");
                //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;
                CLSInquiryPersonalClientInputModel clsPersonalInput = new CLSInquiryPersonalClientInputModel();
                clsPersonalInput = (CLSInquiryPersonalClientInputModel)TransformerFactory.TransformModel(contentModel, clsPersonalInput);

                //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                CLSInquiryPersonalClientContentOutputModel retCLSInqPersClient = CallDevesServiceProxy<EWIResCLSInquiryPersonalClient, CLSInquiryPersonalClientContentOutputModel>
                                                                                        (CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient, clsPersonalInput);

                //++ If Found records in Cleansing(CLS) then pour the data from Cleansing to contentOutputModel

                if (IsSearchFound(retCLSInqPersClient))
                {   AddNode("A (SearchFound)");

                    AddNode("If Found records in Cleansing(CLS) ");
                   // Console.WriteLine(retCLSInqPersClient.ToJson());
                    crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCLSInqPersClient, crmInqContent);
                   
                string crmClientId = "";
                    try
                    {
                        AddNode("try SearchCrmContactClientId");
                        List<string> lstCrmClientId = SearchCrmContactClientId(retCLSInqPersClient.data.First().cleansing_id);
                        if (lstCrmClientId != null && lstCrmClientId.Count == 1)
                        {
                            AddNode("A2 (lstCrmClientId != null)");
                            crmClientId = lstCrmClientId.First();
                        }
                    }
                    finally
                    {
                        CRMInquiryClientOutputDataModel 
                        data = crmInqContent?.data?.FirstOrDefault();

                    if (crmInqContent?.data != null || crmInqContent?.data?.Count > 0)
                        {
                         
                            data = new CRMInquiryClientOutputDataModel
                            {
                                addressInfo = new CRMInquiryClientAddressInfoModel(),
                                asrhHeader = new CRMInquiryClientAsrhHeaderModel(),
                                contactInfo = new CRMInquiryClientContactInfoModel(),
                                generalHeader = new CRMInquiryClientGeneralHeaderModel(),
                                profileInfo = new CRMInquiryClientProfileInfoModel()
                            };
                        }
                      
                       
                    data.generalHeader.crmClientId = crmClientId;
                      
                    data.generalHeader.clientType = contentModel.conditionHeader.clientType;
                      
                    data.generalHeader.roleCode = contentModel.conditionHeader.roleCode;
                        crmInqContent.data.Add(data);

                    }
                }
                #endregion Search Client from Cleansing
                else //+ If not records found in Cleansing(CLS), then Search from Polisy400 
                #region Search client from Polisy400
                { AddNode("B (Search client from Polisy400)");

                    AddNode("Search client from Polisy400");
                    try
                    {
                        COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                        compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(contentModel, compInqClientInput);

                        //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                        EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                                                                                                (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);

                        //Found in Polisy400
                        if (retCOMPInqClient.clientListCollection != null) 
                        {    AddNode("B2 (ound in Polisy400)");
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

                AddNode("END (finishing output )");
                crmInqContent.code = CONST_CODE_SUCCESS;
                crmInqContent.message = "SUCCESS";
                #endregion finishing output 
            
            AddNode("STOP (RETURN OUTPUT )");
            Console.WriteLine(nodePart);
            return crmInqContent;
        }

        internal bool IsSearchFound(CLSInquiryPersonalClientContentOutputModel content)
        {
            return ((content.success | IsOutputSuccess(content)) & (content.data.Any()));
        }
    }
}