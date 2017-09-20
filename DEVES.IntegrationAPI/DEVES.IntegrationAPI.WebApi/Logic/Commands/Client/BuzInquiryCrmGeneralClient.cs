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
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.RegPayeeCorporate;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using WebGrease.Css.Visitor;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzInquiryCrmGeneralClient : BuzCommand
    {

        public List<CRMInquiryClientOutputDataModel> AllSearchResult = new List<CRMInquiryClientOutputDataModel>();
        public List<CRMInquiryClientOutputDataModel> CLSResult = new List<CRMInquiryClientOutputDataModel>();
        public List<CRMInquiryClientOutputDataModel> COMPResult = new List<CRMInquiryClientOutputDataModel>();



        public BaseTransformer CLSPersonalOutputTransform = new TransformCLSInquiryPersonalClientContentOut_to_CrmInquiryClientMasterContentOut();
        public BaseTransformer CLSCorporateOutputTransform = new TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut();
        public BaseTransformer COMPOutputTransform = new TransformCOMPInquiryClientMasterContentOutputModel_to_CrmInquiryClientMasterContentOut();


        public override BaseDataModel ExecuteInput(object input)
        {

            #region Prepare box for output 
            CRMInquiryClientContentOutputModel crmInqContent = (CRMInquiryClientContentOutputModel)Model.DataModelFactory.GetModel(typeof(CRMInquiryClientContentOutputModel));
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = TransactionId;
            #endregion Prepare box for output 


            //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
            InquiryClientMasterInputModel InputModel = (InquiryClientMasterInputModel)input;

            string var1 = InputModel?.conditionDetail?.cleansingId ?? "";
            string var2 = InputModel?.conditionDetail?.polisyClientId ?? "";
            string var3 = InputModel?.conditionDetail?.crmClientId ?? "";
            string var4 = InputModel?.conditionDetail?.clientName1 ?? "";
            string var5 = InputModel?.conditionDetail?.clientName2 ?? "";
            string var6 = InputModel?.conditionDetail?.clientFullname ?? "";
            string var7 = InputModel?.conditionDetail?.idCard ?? "";
            string var8 = InputModel?.conditionDetail?.corporateBranch ?? "";
            string var9 = InputModel?.conditionDetail?.emcsCode ?? "";
            string isOptionalEmpty = var1 + var2 + var3 + var4 + var5 + var6 + var7 + var8 + var9;
            if (string.IsNullOrEmpty(isOptionalEmpty.Trim()))
            {
                crmInqContent.code = AppConst.CODE_INVALID_INPUT;
                crmInqContent.message = "Please fill at least 1 condition";
                crmInqContent.description = "";
                crmInqContent.transactionId = TransactionId;
                crmInqContent.transactionDateTime = DateTime.Now;

                return crmInqContent;
            }

            if (InputModel?.conditionDetail != null && InputModel?.conditionHeader.clientType == "C")
            {
                InputModel.conditionDetail.clientFullname = InputModel?.conditionDetail?.clientFullname?.Replace("ห้างหุ้นส่วนจำกัด", "");
                InputModel.conditionDetail.clientFullname = InputModel?.conditionDetail?.clientFullname?.Replace("บริษัท.", "");
                InputModel.conditionDetail.clientFullname = InputModel?.conditionDetail?.clientFullname?.Replace("บริษัท", "");
                InputModel.conditionDetail.clientFullname = InputModel?.conditionDetail?.clientFullname?.Replace("บ.", "");
                // contentModel.conditionDetail.clientFullname = contentModel?.conditionDetail?.clientFullname?.Replace("จำกัด", "");


                InputModel.conditionDetail.clientFullname = InputModel.conditionDetail.clientFullname
                    .ReplaceMultiplSpacesWithSingleSpace();

                InputModel.conditionDetail.clientName1 = InputModel?.conditionDetail?.clientName1?.Replace("ห้างหุ้นส่วนจำกัด", "");
                InputModel.conditionDetail.clientName2 = InputModel?.conditionDetail?.clientName2?.Replace("ห้างหุ้นส่วนจำกัด", "");
                InputModel.conditionDetail.clientName1 = InputModel?.conditionDetail?.clientName1?.Replace("บริษัท.", "");
                InputModel.conditionDetail.clientName2 = InputModel?.conditionDetail?.clientName2?.Replace("บริษัท", "");
                InputModel.conditionDetail.clientName1 = InputModel?.conditionDetail?.clientName1?.Replace("บ.", "");
                //contentModel.conditionDetail.clientName1 = contentModel?.conditionDetail?.clientName1?.Replace("จำกัด", "");
                //contentModel.conditionDetail.clientName2 = contentModel?.conditionDetail?.clientName2?.Replace("จำกัด", "");


                InputModel.conditionDetail.clientName1 = InputModel?.conditionDetail?.clientName1
                    .ReplaceMultiplSpacesWithSingleSpace();

                InputModel.conditionDetail.clientName2 = InputModel?.conditionDetail?.clientName2
                    .ReplaceMultiplSpacesWithSingleSpace();
            }


            //Search Client from Cleansing

            if (InputModel?.conditionHeader?.clientType == "C")
            {
                CLSResult = InquiryCLSCorporateClient(InputModel);
                if (CLSResult != null)
                {
                    AllSearchResult.AddRange(CLSResult);
                }
            }
            else if (InputModel?.conditionHeader?.clientType == "P")
            {
                CLSResult = InquiryCLSPersonalClient(InputModel);
                if (CLSResult != null)
                {
                    AllSearchResult.AddRange(CLSResult);
                }
            }
            //ถ้าพบ จะซ่อม PolisyClientId เพราะ CLS อาจจะไม่มี PolisyClientId โดยเฉพาะ  รายการที่พึ่งสร้างใหม่
            if (AllSearchResult.Any())
            {
                FullFillEmptyPolisyClientId(AllSearchResult, InputModel?.conditionHeader?.clientType);
            }


            //ถ้าไม่พบให้ไปค้นหาต่อที่ 400
            if (!AllSearchResult.Any())
            {
                COMPResult = InquiryCOMPClientMaster(InputModel);
                if (COMPResult != null)
                {
                    AllSearchResult.AddRange(COMPResult);
                }
            }
            //ซ่อม Role Code เนื่องจาก Source  ไม่มี field นี้
            bool crmDbError = false;
            foreach (CRMInquiryClientOutputDataModel temp in AllSearchResult)
            {
                AddDebugInfo("search crmClientId: cleansingId (" +temp.generalHeader.cleansingId + ")");
                temp.generalHeader.roleCode = "G";

                #region Search crmClientId by CleansingId

                if (!string.IsNullOrEmpty(temp.generalHeader.cleansingId))
                {
                    try
                    {
                        if (false == crmDbError)
                        {
                            List<string> crmData = SearchCrmClientId(temp.generalHeader.cleansingId,
                                InputModel.conditionHeader.clientType);
                            
                            if (crmData != null && crmData.Count == 1)
                            {
                                temp.generalHeader.crmClientId = crmData.First();
                            }
                            else
                            {
                                AddDebugInfo("Error on search crmClientId: cleansingId (" +
                                             temp.generalHeader.cleansingId + ")not found", crmData);
                            }

                        }
                        else
                        {
                            AddDebugInfo("xrmDbError:ป้องกันกรณีที่เกิด error จากฐานข้อมูล crm แล้วจะทำให้ api ช่าไปหมด จึงข้าม crm ไปทั้งหมดเลย ");
                        }
                    }
                    catch (Exception e)
                    {
                        crmDbError = true;
                        AddDebugInfo("Error on search crmClientId", "Error: " + e.Message + "--" + e.StackTrace);
                    }

                }
                else
                {
                    AddDebugInfo("Error on search crmClientId: cleansingId is null");
                }
                #endregion Search crmClientId by CleansingId
            }




            //output
            crmInqContent.code = AppConst.CODE_SUCCESS;
            crmInqContent.message = AppConst.MESSAGE_SUCCESS;
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = TransactionId;
            crmInqContent.data = AllSearchResult;

            // ลบรายการ ที่ชื่อ(name1,name2,fullName) เป็น emtpy string ออกเพราะไม่มีประโยชน์
            if (crmInqContent.data != null)
            {
                crmInqContent.data = crmInqContent.data.Where(row => row?.profileInfo?.name1?.Trim() != "" || row?.profileInfo?.name2?.Trim() != ""  || row?.profileInfo?.fullName?.Trim() != "").ToList();
            }
          
          
            return crmInqContent;
        }

        private bool IsSearchFound(CLSInquiryPersonalClientContentOutputModel content)
        {
            if (null == content?.data)
            {
                return false;
            }
            return ((content.success | IsOutputSuccess(content)) & (content.data.Any()));
        }


        private List<CRMInquiryClientOutputDataModel> InquiryCLSPersonalClient(InquiryClientMasterInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryCLSPersonalClient", searchCondition);
            var service = new CLSInquiryCLSPersonalClientService(TransactionId, ControllerName);
            var inqClsOutput = service.Execute(searchCondition);


            var crmInqOutput = new CRMInquiryClientContentOutputModel();

            //Found in CLS
            if (inqClsOutput?.data != null)
            {
                crmInqOutput =
                    (CRMInquiryClientContentOutputModel)CLSPersonalOutputTransform.TransformModel(inqClsOutput, crmInqOutput);
            }

            return crmInqOutput?.data;
        }

        private List<CRMInquiryClientOutputDataModel> InquiryCLSCorporateClient(InquiryClientMasterInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryCLSCorporateClient", searchCondition);
            var service = new CLSInquiryCLSCorporateClientService(TransactionId, ControllerName);
            var inqClsOutput = service.Execute(searchCondition);


            var crmInqOutput = new CRMInquiryClientContentOutputModel();
            //Found in CLS
            if (inqClsOutput?.data != null)
            {
                crmInqOutput =
                    (CRMInquiryClientContentOutputModel)CLSCorporateOutputTransform.TransformModel(inqClsOutput, crmInqOutput);
            }

            return crmInqOutput?.data;
        }

        private List<CRMInquiryClientOutputDataModel> InquiryCOMPClientMaster(InquiryClientMasterInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryCOMPClientMaster", searchCondition);
            var service = new COMPInquiryClientMasterService(TransactionId, ControllerName);

            var inqOutput = service.Execute(searchCondition);

            var crmInqOutput = new CRMInquiryClientContentOutputModel();

            //Found in Polisy400
            if (inqOutput?.clientListCollection != null)
            {
                crmInqOutput =
                    (CRMInquiryClientContentOutputModel)COMPOutputTransform.TransformModel(inqOutput, crmInqOutput);

            }
            return crmInqOutput?.data;
        }

        /// <summary>
        /// ดึงค่าจาก Polisy มาเติมในกรณีที่ข้อมูลสร้างใหม่ CLS จะยังไม่มีเลข Polisy
        /// </summary>
        /// <param name="listSearchResult"></param>
        /// <param name="clientType">P or C</param>
        /// <returns></returns>
        public List<CRMInquiryClientOutputDataModel> FullFillEmptyPolisyClientId(List<CRMInquiryClientOutputDataModel> listSearchResult,string clientType)
        {

            if (listSearchResult == null)
            {
                return default(List<CRMInquiryClientOutputDataModel>);
            }
            var polisyClientService = new PolisyClientService(TransactionId, ControllerName);
            foreach (var temp in listSearchResult)
            {
                if (string.IsNullOrEmpty(temp?.generalHeader?.polisyClientId) || temp?.generalHeader?.polisyClientId == "0")
                {
                    AddDebugInfo("Find Polisy for new client  " + temp?.generalHeader?.polisyClientId ?? "", "");

                    var lstPolisyClient = polisyClientService.FindByCleansingId(temp?.generalHeader?.cleansingId, clientType);

                    if (lstPolisyClient?.cleansingId != null)
                    {
                        
                        if (temp?.generalHeader != null)
                            temp.generalHeader.polisyClientId = lstPolisyClient.clientNumber;
                    }
                    else
                    {
                        AddDebugInfo(" not found Polisy client id");
                    }
                    if (temp?.generalHeader != null && (temp.generalHeader.polisyClientId == "0"))
                    {
                        temp.generalHeader.polisyClientId = "";
                    }

                }
            }
            return listSearchResult;
        }
    }

}