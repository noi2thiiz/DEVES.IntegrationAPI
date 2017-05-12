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

            

            #region Search client from CRM



            #endregion

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
               
                // Console.WriteLine(retCLSInqPersClient.ToJson());
                crmInqContent = (CRMInquiryClientContentOutputModel)TransformerFactory.TransformModel(retCLSInqPersClient, crmInqContent);

                foreach(CRMInquiryClientOutputDataModel temp in crmInqContent.data) {

                    string crmPolisyClientId = "";
                    string crmClientId = "";
                    try
                    {
                        List<string> lstCrmClientId = SearchCrmContactClientId(temp.generalHeader.cleansingId);
                        if (lstCrmClientId != null && lstCrmClientId.Count == 1)
                        {
                            
                            temp.generalHeader.crmClientId = lstCrmClientId.First();
                        }
                        // ดึงค่าจาก Polisy มาเติมในกรณีที่ข้อมูลสร้างใหม่ CLS จะยังไม่มีเลข Polisy
                        if (string.IsNullOrEmpty(temp.generalHeader.polisyClientId) || temp.generalHeader.polisyClientId.Equals("0"))
                        {
                            var lstPolisyClient = PolisyClientService.Instance.FindByCleansingId(temp.generalHeader.cleansingId, contentModel.conditionHeader.clientType.ToUpperIgnoreNull());
                            //List<string> lstPolisyClientId = SearchContactPolisyId(temp.generalHeader.cleansingId);
                            if (lstPolisyClient?.cleansingId != null)
                            {
                                temp.generalHeader.polisyClientId = lstPolisyClient.clientNumber;
                            }

                        }

                    }

                    catch (Exception e)
                    {

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
            return crmInqContent;
        }

        internal bool IsSearchFound(CLSInquiryPersonalClientContentOutputModel content)
        {
            return ((content.success | IsOutputSuccess(content)) & (content.data.Any()));
        }
    }
}