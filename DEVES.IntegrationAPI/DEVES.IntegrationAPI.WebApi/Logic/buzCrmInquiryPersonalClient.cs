using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;
using WebGrease.Css.Visitor;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCrmInquiryPersonalClientMaster : BuzCommand
    {

        

        public override BaseDataModel ExecuteInput(object input)
        {
            
            #region Prepare box for output 
            CRMInquiryClientContentOutputModel crmInqContent = (CRMInquiryClientContentOutputModel)Model.DataModelFactory.GetModel(typeof(CRMInquiryClientContentOutputModel));
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = TransactionId;
            #endregion Prepare box for output 

            

         

            #region Search Client from Cleansing
           
            //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
            InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;

            string var1 = contentModel.conditionDetail.cleansingId;
            string var2 = contentModel.conditionDetail.polisyClientId;
            string var3 = contentModel.conditionDetail.crmClientId;
            string var4 = contentModel.conditionDetail.clientName1;
            string var5 = contentModel.conditionDetail.clientName2;
            string var6 = contentModel.conditionDetail.clientFullname;
            string var7 = contentModel.conditionDetail.idCard;
            string var8 = contentModel.conditionDetail.corporateBranch;
            string var9 = contentModel.conditionDetail.emcsCode;
            string isOptionalEmpty = var1 + var2 + var3 + var4 + var5 + var6 + var7 + var8 + var9;
            if (string.IsNullOrEmpty(isOptionalEmpty.Trim()))
            {
                crmInqContent.code = CONST_CODE_FAILED;
                crmInqContent.message = "FAILED";
                crmInqContent.description = "Please fill at least 1 condition";
                crmInqContent.transactionId = TransactionId;
                crmInqContent.transactionDateTime = DateTime.Now;

                return crmInqContent;
            }

            CLSInquiryPersonalClientInputModel clsPersonalInput = new CLSInquiryPersonalClientInputModel();
            clsPersonalInput = (CLSInquiryPersonalClientInputModel)TransformerFactory.TransformModel(contentModel, clsPersonalInput);
            crmInqContent.AddDebugInfo("Start Search", clsPersonalInput);

            //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
            debugInfo.AddDebugInfo("Call CLS_InquiryCLSPersonalClient through ServiceProxy", clsPersonalInput);
            CLSInquiryPersonalClientContentOutputModel retCLSInqPersClient = CallDevesServiceProxy<EWIResCLSInquiryPersonalClient, CLSInquiryPersonalClientContentOutputModel>
                                                                                    (CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient, clsPersonalInput);

            //++ If Found records in Cleansing(CLS) then pour the data from Cleansing to contentOutputModel

            if (IsSearchFound(retCLSInqPersClient))
            {
                debugInfo.AddDebugInfo(" Found records in Cleansing(CLS) ", retCLSInqPersClient);
                // Console.WriteLine(retCLSInqPersClient.ToJson());
                crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCLSInqPersClient, crmInqContent);

                foreach(CRMInquiryClientOutputDataModel temp in crmInqContent.data) {
                    debugInfo.AddDebugInfo("loop foreach temp in crmInqContent.data", temp);
                    string crmPolisyClientId = "";
                    string crmClientId = "";
                    try
                    {
                        if (string.IsNullOrEmpty(temp.generalHeader.crmClientId))
                        {
                            List<string> lstCrmClientId = SpApiCustomerClient.Instance.SearchCrmContactClientId("P", temp?.generalHeader?.cleansingId);
                            //SearchCrmContactClientId(temp?.generalHeader?.cleansingId);
                            if (lstCrmClientId != null && lstCrmClientId.Count == 1)
                            {

                                temp.generalHeader.crmClientId = lstCrmClientId.First();
                            }
                        }
                       
                        
                        // ดึงค่าจาก Polisy มาเติมในกรณีที่ข้อมูลสร้างใหม่ CLS จะยังไม่มีเลข Polisy
                        Console.WriteLine("polisyClientId=" + temp.generalHeader.polisyClientId);
                        if (string.IsNullOrEmpty(temp.generalHeader.polisyClientId) || temp.generalHeader.polisyClientId.Equals("0"))
                        {
                            debugInfo.AddDebugInfo("Find Polisy for new client  "+ temp.generalHeader.polisyClientId,"");
                            

           
                        var lstPolisyClient = PolisyClientService.Instance.FindByCleansingId(temp.generalHeader.cleansingId, contentModel.conditionHeader.clientType.ToUpperIgnoreNull());
                        List<string> lstPolisyClientId = SearchContactPolisyId(temp.generalHeader.cleansingId);
                        if (lstPolisyClient?.cleansingId != null)
                        {

                                debugInfo.AddDebugInfo("found Polisy for new client =" + lstPolisyClient.clientNumber,"");
                                temp.generalHeader.polisyClientId = lstPolisyClient.clientNumber;
                            }
                            else
                            {
                                debugInfo.AddDebugInfo(" not found Polisy for new client =" ,"");
                            }
                            if (temp.generalHeader.polisyClientId=="0")
                            {
                                temp.generalHeader.polisyClientId = "";
                            }

                        }

                    }

                    catch (Exception e)
                    {
                       
                        debugInfo.AddDebugInfo("Error", "Error: "+e.Message+"--"+e.StackTrace);
                    }
                }
                /*
                string crmPolisyClientId = "";
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

                    List<string> lstPolisyClientId = SearchContactPolisyId(retCLSInqPersClient.data.First().cleansing_id);
                    if (lstPolisyClientId != null && lstPolisyClientId.Count == 1)
                    {
                        AddNode("A2 (lstPolisyClientId != null)");
                        crmPolisyClientId = lstPolisyClientId.First();
                    }
                }
                */
                /*
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
                    data.generalHeader.polisyClientId = crmPolisyClientId;
                    data.generalHeader.clientType = contentModel.conditionHeader.clientType;

                    data.generalHeader.roleCode = contentModel.conditionHeader.roleCode;
                    crmInqContent.data.Add(data);

                }*/
                debugInfo.AddDebugInfo("endregion Search Client from Cleansing", "");
            }
           
            #endregion Search Client from Cleansing
            else //+ If not records found in Cleansing(CLS), then Search from Polisy400 
            #region Search client from Polisy400
            {
                debugInfo.AddDebugInfo("region Search client from Polisy400", "");
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
                        debugInfo.AddDebugInfo("Found in Polisy400", retCOMPInqClient);
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
            
            foreach (CRMInquiryClientOutputDataModel temp in crmInqContent.data)
            {
                temp.generalHeader.clientType = "P";
                temp.generalHeader.roleCode = "G";
            }

                
            crmInqContent.code = CONST_CODE_SUCCESS;
            crmInqContent.message = "SUCCESS";
            #endregion finishing output 

            
         
            if (crmInqContent.data != null)
            {
                crmInqContent.data = crmInqContent.data.Where(row => row?.profileInfo?.name1.Trim() != "" || row?.profileInfo?.fullName.Trim() != "").ToList();
            }
            crmInqContent.AddDebugInfo("Output","");
            crmInqContent._debugInfo.AddRange(debugInfo._debugInfo);
            return crmInqContent;
        }

        internal bool IsSearchFound(CLSInquiryPersonalClientContentOutputModel content)
        {
            return ((content.success | IsOutputSuccess(content)) & (content.data.Any()));
        }
    }
}