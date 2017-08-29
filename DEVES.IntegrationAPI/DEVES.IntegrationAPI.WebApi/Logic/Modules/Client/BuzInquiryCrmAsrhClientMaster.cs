using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using Newtonsoft.Json;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class BuzInquiryCrmAsrhClientMaster : BuzInquiryCrmGeneralClient
    {
        //const string ewiEndpointKeyCLSInquiryCorporateClient = "EWI_ENDPOINT_CLSInquiryCorporateClient";

        public override BaseDataModel ExecuteInput(object input)
        {
            string clientType = "";
            string roleCode = "";

            CRMInquiryClientContentOutputModel crmInqContent = (CRMInquiryClientContentOutputModel)Model.DataModelFactory.GetModel(typeof(CRMInquiryClientContentOutputModel));
            crmInqContent.transactionDateTime = DateTime.Now;
            crmInqContent.transactionId = TransactionId;

            bool bFoundIn_APAR_or_Master = false;

            //+ Deserialize Input
            InquiryClientMasterInputModel contentModel = (InquiryClientMasterInputModel)input;// DeserializeJson<InquiryClientMasterInputModel>(input.ToString());

            string var1 = contentModel?.conditionDetail?.cleansingId??"";
            string var2 = contentModel?.conditionDetail?.polisyClientId??"";
            string var3 = contentModel?.conditionDetail?.crmClientId??"";
            string var4 = contentModel?.conditionDetail?.clientName1??"";
            string var5 = contentModel?.conditionDetail?.clientName2??"";
            string var6 = contentModel?.conditionDetail?.clientFullname??"";
            string var7 = contentModel?.conditionDetail?.idCard??"";
            string var8 = contentModel?.conditionDetail?.corporateBranch??"";
            string var9 = contentModel?.conditionDetail?.emcsCode??"";
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
            //ลบ บริษัทออกจากชื่อ
            AddDebugInfo("Input raw:", contentModel?.ToJson());
            if (contentModel?.conditionDetail != null && contentModel?.conditionHeader.clientType=="C")
            {
                contentModel.conditionDetail.clientFullname = contentModel?.conditionDetail?.clientFullname?.Replace("ห้างหุ้นส่วนจำกัด", "");
                contentModel.conditionDetail.clientFullname = contentModel?.conditionDetail?.clientFullname?.Replace("บริษัท.", "");
                contentModel.conditionDetail.clientFullname = contentModel?.conditionDetail?.clientFullname?.Replace("บริษัท", "");
                contentModel.conditionDetail.clientFullname = contentModel?.conditionDetail?.clientFullname?.Replace("บ.", "");
               // contentModel.conditionDetail.clientFullname = contentModel?.conditionDetail?.clientFullname?.Replace("จำกัด", "");
                

                contentModel.conditionDetail.clientFullname = contentModel.conditionDetail.clientFullname
                    .ReplaceMultiplSpacesWithSingleSpace();

                contentModel.conditionDetail.clientName1 = contentModel?.conditionDetail?.clientName1?.Replace("ห้างหุ้นส่วนจำกัด", "");
                contentModel.conditionDetail.clientName2 = contentModel?.conditionDetail?.clientName2?.Replace("ห้างหุ้นส่วนจำกัด", "");
                contentModel.conditionDetail.clientName1 = contentModel?.conditionDetail?.clientName1?.Replace("บริษัท.", "");
                contentModel.conditionDetail.clientName2 = contentModel?.conditionDetail?.clientName2?.Replace("บริษัท", "");
                contentModel.conditionDetail.clientName1 = contentModel?.conditionDetail?.clientName1?.Replace("บ.", "");
                //contentModel.conditionDetail.clientName1 = contentModel?.conditionDetail?.clientName1?.Replace("จำกัด", "");
                //contentModel.conditionDetail.clientName2 = contentModel?.conditionDetail?.clientName2?.Replace("จำกัด", "");


                contentModel.conditionDetail.clientName1 = contentModel?.conditionDetail?.clientName1
                    .ReplaceMultiplSpacesWithSingleSpace();

                contentModel.conditionDetail.clientName2 = contentModel?.conditionDetail?.clientName2
                    .ReplaceMultiplSpacesWithSingleSpace();
            }
            AddDebugInfo("Input after:", contentModel?.ToJson());
            //Start Process
            
                #region IF inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH
                crmInqContent = InquiryMasterASRH(contentModel, crmInqContent, ref bFoundIn_APAR_or_Master);

                #endregion inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH
            

            //ถ้าไม่พยให้ไปค้นที่ 400
            if(false==crmInqContent?.data?.Any())
            {
                #region Call COMP_Inquiry through ServiceProxy

                crmInqContent = InquiryCOMPClientMaster(contentModel, crmInqContent);

                #endregion Call COMP_Inquiry through ServiceProxy
            }

            


            bool crmDbError = false;
            foreach (CRMInquiryClientOutputDataModel temp in crmInqContent.data)
            {
                #region map roleCode
                temp.generalHeader.roleCode = contentModel?.conditionHeader?.roleCode?.ToUpper();

                /* ส่วนการ map ตาม Flag นี้ได้เสนอแอรโรแล้วแต่ไม่ให้ทำ
                if (temp.generalHeader.clientType == "P")
                {
                    temp.generalHeader.roleCode = "G";
                }
               
                else
                if (temp.asrhHeader.assessorFlag == "Y" && temp.asrhHeader.solicitorFlag == "N" && temp.asrhHeader.repairerFlag == "N" && temp.asrhHeader.hospitalFlag == "N")
                {
                    temp.generalHeader.roleCode = "A";
                }
                else
                if (temp.asrhHeader.assessorFlag == "N" && temp.asrhHeader.solicitorFlag == "Y" && temp.asrhHeader.repairerFlag == "N" && temp.asrhHeader.hospitalFlag == "N")
                {
                    temp.generalHeader.roleCode = "S";
                }
                else
                if (temp.asrhHeader.assessorFlag == "N" && temp.asrhHeader.solicitorFlag == "N" && temp.asrhHeader.repairerFlag == "Y" && temp.asrhHeader.hospitalFlag == "N")
                {
                    temp.generalHeader.roleCode = "R";
                }
                else if (temp.asrhHeader.assessorFlag == "N" && temp.asrhHeader.solicitorFlag == "N" &&
                         temp.asrhHeader.repairerFlag == "N" && temp.asrhHeader.hospitalFlag == "Y")
                {
                    temp.generalHeader.roleCode = "H";
                }
                 else
                 if (temp.asrhHeader.assessorFlag == "N" && temp.asrhHeader.solicitorFlag == "N" && temp.asrhHeader.repairerFlag == "N" && temp.asrhHeader.hospitalFlag == "N")
                 {
                     temp.generalHeader.roleCode = ""; 
                 }
                else
                {
                  
                    temp.generalHeader.roleCode = contentModel?.conditionHeader?.roleCode?.ToUpper();
                }
                */


                #endregion


                #region Search crmClientId by CleansingId

                if (!string.IsNullOrEmpty(temp.generalHeader.cleansingId))
                {
                    try
                    {
                        if (false == crmDbError)
                        {
                            // //AdHoc fix timeout: เพื่อป้องกันกรณีที่เกิด error จากฐานข้อมูล crm แล้วจะทำให้ api ช่าไปหมด
                            List<string> crmData = SearchCrmClientId(temp.generalHeader.cleansingId,
                                contentModel.conditionHeader.clientType);
                            if (crmData != null && crmData.Count == 1)
                            {
                                temp.generalHeader.crmClientId = crmData.First();
                            }
                            else
                            {
                                AddDebugInfo("Error on search crmClientId: cleansingId (" +
                                             temp.generalHeader.cleansingId + ") not found");
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

            crmInqContent.code = AppConst.CODE_SUCCESS;
            crmInqContent.message = AppConst.MESSAGE_SUCCESS;
            

            if (crmInqContent.data != null)
            {
                crmInqContent.data = crmInqContent.data.Where(row => row?.profileInfo?.name1.Trim() != "" || row?.profileInfo?.fullName.Trim() != "" ).ToList();
            }

            return crmInqContent;
        }

        private CRMInquiryClientContentOutputModel InquiryMasterASRH(InquiryClientMasterInputModel contentModel,
            CRMInquiryClientContentOutputModel crmInqContent, ref bool bFoundIn_APAR_or_Master)
        {
            string clientType;
            string roleCode;
            // Search in MASTER: MOTOR_InquiryMasterASRH()
            var service = new MOTORInquiryMasterASRH(TransactionId, ControllerName);

            InquiryMasterASRHDataInputModel inqASRHIn =
                (InquiryMasterASRHDataInputModel) DataModelFactory.GetModel(typeof(InquiryMasterASRHDataInputModel));
            inqASRHIn = (InquiryMasterASRHDataInputModel) TransformerFactory.TransformModel(contentModel, inqASRHIn);

            InquiryMasterASRHContentModel inqASRHOut = service.Execute(inqASRHIn);


            AddDebugInfo("InquiryMasterASRH);", inqASRHIn);
            clientType = "C";
            roleCode = inqASRHIn.asrhType;

            if (inqASRHOut != null && inqASRHOut.ASRHListCollection != null)
            {
                if (inqASRHOut.ASRHListCollection.Count > 0)
                {
                    crmInqContent =
                        (CRMInquiryClientContentOutputModel) TransformerFactory.TransformModel(inqASRHOut,
                            crmInqContent);
                    bFoundIn_APAR_or_Master = true;
                    AddDebugInfo(" found in InquiryMasterASRH);");
                }
                else
                {
                    AddDebugInfo("not found in InquiryMasterASRH);");
                }
            }
            else
            {
                AddDebugInfo("not found in InquiryMasterASRH);");
            }
            return crmInqContent;
        }

        private void FillCrmClientIdbyCleansingId(CRMInquiryClientContentOutputModel crmInqContent,
            InquiryClientMasterInputModel contentModel)
        {
            foreach (CRMInquiryClientOutputDataModel data in crmInqContent.data)
            {
                if (string.IsNullOrEmpty(data?.generalHeader?.crmClientId))
                {
                    try
                    {
                        List<string> crmData = SearchCrmContactClientId(contentModel?.conditionDetail?.cleansingId);
                        if (crmData != null && crmData.Count == 1)
                        {
                            data.generalHeader.crmClientId = crmData.First();
                        }
                    }
                    catch (Exception e)
                    {
                        AddDebugInfo("Error on search crmClientId", "Error: " + e.Message + "--" + e.StackTrace);
                    }
                }
            }
        }

        private CRMInquiryClientContentOutputModel InquiryCOMPClientMaster(InquiryClientMasterInputModel contentModel,
            CRMInquiryClientContentOutputModel crmInqContent)
        {
            var compService = new COMPInquiryClientMaster(TransactionId, ControllerName);

            COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
            compInqClientInput =
                (COMPInquiryClientMasterInputModel) TransformerFactory.TransformModel(contentModel, compInqClientInput);

            EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = compService.Execute(compInqClientInput);

            AddDebugInfo("Call COMP_Inquiry);", compInqClientInput);
            //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy

            //Found in Polisy400
            if (retCOMPInqClient.clientListCollection != null)
            {
                crmInqContent =
                    (CRMInquiryClientContentOutputModel) TransformerFactory.TransformModel(retCOMPInqClient,
                        crmInqContent);
            }
            else
            {
                AddDebugInfo("not found COMP_Inquiry);");
            }
            return crmInqContent;
        }

        private bool IsSearchFound(CLSInquiryCorporateClientContentOutputModel content)
        {
            if (content?.data == null)
            {
                return false;
            }
           
            return ((content.success | IsOutputSuccess(content)) & (content.data.Any()));
        }


    }
}