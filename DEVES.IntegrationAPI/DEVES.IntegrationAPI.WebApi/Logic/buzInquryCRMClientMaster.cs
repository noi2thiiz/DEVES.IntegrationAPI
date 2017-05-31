﻿using System;
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
    public class BuzInquiryCrmClientMaster : BuzCommand
    {

        public List<CRMInquiryClientOutputDataModel> AllSearchResult = new List<CRMInquiryClientOutputDataModel>();
        public List<CRMInquiryClientOutputDataModel> CLSResult = new List<CRMInquiryClientOutputDataModel>();


        public BaseTransformer CLSPersonalInputTransform = new TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCLSPersonalClientInput();
        public BaseTransformer CLSPersonalOutputTransform = new TransformCLSInquiryPersonalClientContentOut_to_CrmInquiryClientMasterContentOut();

        public BaseTransformer CLSCorporateInputTransform =
            new TransformCRMInquiryCRMClientMasterInput_to_CLSInquiryCorporateClientInputModel();
        public BaseTransformer CLSCorporateOutputTransform = new TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut();


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

            if (AllSearchResult.Any())
            {
                FulFillEmptyPolisyClientId(AllSearchResult, InputModel?.conditionHeader?.clientType);
            }


            //ถ้าไม่พบให้ไปค้นหาต่อที่ 400


            crmInqContent.code = AppConst.CODE_SUCCESS;
            crmInqContent.message = AppConst.MESSAGE_SUCCESS;
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = TransactionId;
            crmInqContent.data = AllSearchResult;
            if (crmInqContent.data != null)
            {
                crmInqContent.data = crmInqContent.data.Where(row => row?.profileInfo?.name1.Trim() != "" || row?.profileInfo?.fullName.Trim() != "").ToList();
            }
            crmInqContent.AddListDebugInfo(GetDebugInfoList());
          
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
            var service = new CLSInquiryCLSPersonalClient(TransactionId, ControllerName);

            var clssearchCondition = (CLSInquiryPersonalClientInputModel)
                CLSPersonalInputTransform.TransformModel(searchCondition, new CLSInquiryPersonalClientInputModel());


            var inqClsOutput = service.Execute(clssearchCondition);


            var crmInqOutput = new CRMInquiryClientContentOutputModel();
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
            var service = new CLSInquiryCLSCorporateClient(TransactionId, ControllerName);

            var clssearchCondition = (CLSInquiryCorporateClientInputModel)
                CLSCorporateInputTransform.TransformModel(searchCondition, new CLSInquiryCorporateClientInputModel());


            var inqClsOutput = service.Execute(clssearchCondition);


            var crmInqOutput = new CRMInquiryClientContentOutputModel();
            if (inqClsOutput?.data != null)
            {

                crmInqOutput =
                    (CRMInquiryClientContentOutputModel)CLSCorporateOutputTransform.TransformModel(inqClsOutput, crmInqOutput);


            }

            return crmInqOutput?.data;
        }

        /// <summary>
        /// ดึงค่าจาก Polisy มาเติมในกรณีที่ข้อมูลสร้างใหม่ CLS จะยังไม่มีเลข Polisy
        /// </summary>
        /// <param name="listSearchResult"></param>
        /// <param name="clientType">P or C</param>
        /// <returns></returns>
        public List<CRMInquiryClientOutputDataModel> FulFillEmptyPolisyClientId(List<CRMInquiryClientOutputDataModel> listSearchResult,string clientType)
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
                        //debugInfo.AddDebugInfo("found Polisy for new client =" + lstPolisyClient.clientNumber, "");
                        if (temp?.generalHeader != null)
                            temp.generalHeader.polisyClientId = lstPolisyClient.clientNumber;
                    }
                    else
                    {
                        AddDebugInfo(" not found Polisy client id", "");
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