using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;
using System.Linq;
using System.Collections;
using System.Data.Common;
using System.Globalization;
using System.Linq.Expressions;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using Microsoft.Ajax.Utilities;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzInquiryCRMPayeeListNew : BuzCommand
    {
        /// <summary>
        /// ใช้ทดสอบว่า หากไม่เจอข้อมูลใน SAP ข้อมูล OUTPUT จะเป็นอย่างไร
        /// production set = false
        /// </summary>
        public bool IgnoreSap = false;
        public bool IgnoreApar = false;
        public bool IgnoreCls = false;
        public InquiryCRMPayeeListInputModel inqCrmPayeeInput;
        public List<InquiryCrmPayeeListDataModel> SAPResult = new List<InquiryCrmPayeeListDataModel>();
        public List<InquiryCrmPayeeListDataModel> CLSResult = new List<InquiryCrmPayeeListDataModel>();
        public List<InquiryCrmPayeeListDataModel> COMPResult = new List<InquiryCrmPayeeListDataModel>();
        public List<InquiryCrmPayeeListDataModel> APARResult = new List<InquiryCrmPayeeListDataModel>();
        public List<InquiryCrmPayeeListDataModel> ASHRResult = new List<InquiryCrmPayeeListDataModel>();
        public List<InquiryCrmPayeeListDataModel> AllSearchResult = new List<InquiryCrmPayeeListDataModel>();

        public BaseTransformer APAROutTranformer = new TransformAPARInquiryAPARPayeeListContentOutputModel_to_InquiryCRMPayeeListDataOutputModel();

        public void TranformInput()
        {
            switch (inqCrmPayeeInput.requester)
            {
                case "MC": inqCrmPayeeInput.requester = "MotorClaim"; break;
                default: inqCrmPayeeInput.requester = "MotorClaim"; break;
            }
         
            //ลบ บริษัทออกจากชื่อ
            if (!string.IsNullOrEmpty(inqCrmPayeeInput.fullname))
            {
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("บริษัท.", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("บริษัท", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("บ.", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("จำกัด", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname.ReplaceMultiplSpacesWithSingleSpace();

            }

        }
        public override BaseDataModel ExecuteInput(object input)
        {

            //config
            int iRecordsLimit = int.Parse( GetAppConfigurationSetting("SearchRecordsLimit").ToString());


            
            // request
            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();

            List<InquiryCRMPayeeListInputModel> listSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
          
           // listSAPSearchCondition.Add(searchCondition);

            Console.WriteLine(listSAPSearchCondition);
           
            inqCrmPayeeInput = (InquiryCRMPayeeListInputModel)input;
            InquiryCRMPayeeListInputModel searchCondition = Copy(inqCrmPayeeInput);
            //validate input
            TranformInput();

            string RoleCode = inqCrmPayeeInput?.roleCode?.ToUpper();

            // สาย A S R H ให้ค้นที่ MASTERASRH ก่อน
            if (RoleCode!="G")
            {
                //:Inquiry ASRH;
                ASHRResult = InquiryMasterASHR(searchCondition);
                AllSearchResult.AddRange(ASHRResult);

            }

            //กรณีไม่พบข้อมูล ให้ไปค้นที่ APAR
            if (!AllSearchResult.Any())
            {
                APARResult = InquiryAPARPayeeList(searchCondition);
                AllSearchResult.AddRange(APARResult);
            }

            //กรณีไม่พบข้อมูล ให้ไปค้นที่ CLS
            if (!AllSearchResult.Any())
            {
                CLSResult = InquiryAPARPayeeList(searchCondition);
                AllSearchResult.AddRange(CLSResult);
            }

            //กรณีไม่พบข้อมูล ให้ไปค้นที่ 400
            if (!AllSearchResult.Any())
            {
                COMPResult = InquiryAPARPayeeList(searchCondition);
                AllSearchResult.AddRange(COMPResult);
            }

            //ถ้าข้อมูลมากกว่า 100 รายการ จะ 
            if (AllSearchResult.Count>100)
            {
                throw new  BuzErrorException("500", "คุณระบุเงื่อนไขในการสืบค้นน้อยเกินไป", "คุณระบุเงื่อนไขในการสืบค้นน้อยเกินไป ทำให้พบข้อมูลจำนวนมากเกินกว่าที่ระบบอนุญาต กรุณาระบุเงื่อนไขที่ชัดเจนมากขึ้น เช่นการระบุชื่อและนามสกุล");
            }

            //กรณีที่ไม่พบข้อมูล
            if (!AllSearchResult.Any())
            {
                throw new BuzErrorException("200", "ไม่พบข้อมูล", " กรุณาตรวจสอบเงื่อนไขในการค้นหาอีกครั้ง หรือใช้เงื่อนไขอื่นในการค้นหา ข้อควรระวัง ไม่ควรค้นจากการระบุเงื่อนไขหลายๆอย่างพร้อมกัน เช่นการค้นทั้งชื่อนามสกุลพร้อมกับเลขประจำตัวประชาชน");
            }


            //กรณีที่พบข้อมูลค้นหาต่อที่ SAP
            // * foreach item in result
            //     if item contain VCODE then Search by VCODE
            //     if not found and item contain  PolisyClientID then search by PREVACC
            //     if not found and item contain  Tax ID then search by Tax(TAX3+TAX4)
  

            // ค้นข้อมูลใน CRM เพื่อเอาเลข CRM Client ID มาเติม



           
            // remove duplicate data 


            //order by by vcode,cleasing id desc



            //return output


            crmInqPayeeOut.AddListDebugInfo(GetDebugInfoList());
            
            crmInqPayeeOut.data.AddRange(AllSearchResult);
            return crmInqPayeeOut;
        }


        /// <summary>
        /// listSAPSearchCondition
        /// </summary>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <param name="iRecordsLimit"></param>
        /// <param name="counrSAPResult"></param>
        ///  public List<InquiryCrmPayeeListDataModel> InquiryAPARPayeeList(InquiryCRMPayeeListInputModel searchCondition)
       /* private List<InquiryCrmPayeeListDataModel> InquerySapVandor(List<InquiryCrmPayeeListDataModel> listSAPSearchCondition, int iRecordsLimit)
        {
            CRMInquiryPayeeContentOutputModel tmpCrmInqPayeeOut = null;
          AddDebugInfo("call method", "listSAPSearchCondition");
            SAPResult = new List<InquiryCrmPayeeListDataModel>();
            var limit = iRecordsLimit;
            var countLoop = 0;
             AddDebugInfo("watch listSAPSearchCondition ", listSAPSearchCondition);
            //sapSearchResultCash จะใช้เก็บข้อมูลที่จะ search sap โดยหากเป็น sapVendorCode จะไม่ search ซ้ำ
            var sapSearchResultCash =  new Dictionary<string, EWIResSAPInquiryVendorContentModel>();

            foreach (var searchCond in listSAPSearchCondition)
            {
                
            }
            while (listSAPSearchCondition.Count > 0 && countLoop <= limit)
            {
                AddDebugInfo("Loop while search sap " + countLoop, countLoop);
              
                InquiryCRMPayeeListInputModel searchCond = listSAPSearchCondition[0];
                string polisyClientId = searchCond.polisyClientId;
                AddDebugInfo("searchCond polisyClientId="+ polisyClientId, searchCond);
              
                SAPInquiryVendorInputModel inqSAPVendorIn =
                    (SAPInquiryVendorInputModel)DataModelFactory.GetModel(typeof(SAPInquiryVendorInputModel));

                inqSAPVendorIn =
                    (SAPInquiryVendorInputModel)TransformerFactory.TransformModel(searchCond, inqSAPVendorIn);
                AddDebugInfo("inqSAPVendorIn ", inqSAPVendorIn);

               
                var sapSearchKeyCode = (inqSAPVendorIn.PREVACC + inqSAPVendorIn.TAX3 + inqSAPVendorIn.TAX4 + inqSAPVendorIn.VCODE).ReplaceMultiplSpacesWithSingleSpace();
                EWIResSAPInquiryVendorContentModel inqSAPVendorContentOut;
                if (!sapSearchResultCash.ContainsKey(sapSearchKeyCode))
                {
                    

                     inqSAPVendorContentOut =
                        CallDevesServiceProxy<SAPInquiryVendorOutputModel, EWIResSAPInquiryVendorContentModel>(
                            CommonConstant.ewiEndpointKeySAPInquiryVendor, inqSAPVendorIn);
                    sapSearchResultCash[sapSearchKeyCode] = inqSAPVendorContentOut;
                }
                else
                {
                    //ดึงค่า Cash
                    inqSAPVendorContentOut = sapSearchResultCash[sapSearchKeyCode];
                }
                AddDebugInfo("searchResult", inqSAPVendorContentOut);

                if (inqSAPVendorContentOut?.VendorInfo != null)
                {
                    if (inqSAPVendorContentOut.VendorInfo.Count + listSAPSearchCondition.Count > iRecordsLimit)
                    {
                        throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_SAP, iRecordsLimit);
                    }
                     tmpCrmInqPayeeOut =
                        (CRMInquiryPayeeContentOutputModel) TransformerFactory.TransformModel(inqSAPVendorContentOut,
                            new CRMInquiryPayeeContentOutputModel());
                   
                    foreach (InquiryCrmPayeeListDataModel data in tmpCrmInqPayeeOut.data)
                    {
                        data.emcsMemHeadId = searchCond.emcsMemHeadId;
                        data.emcsMemId = searchCond.emcsMemId;
                        data.contactNumber = searchCond.contactNumber;
                        data.assessorFlag = searchCond.assessorFlag;
                        data.solicitorFlag = searchCond.solicitorFlag;
                        data.repairerFlag = searchCond.repairerFlag;
                        data.hospitalFlag = searchCond.hospitalFlag;
                        data.polisyClientId = (!string.IsNullOrEmpty(polisyClientId)?  polisyClientId: data.polisyClientId); // เอา  polisyClientId จากข้อมูลต้นทาง (APAR,ASHR ) มาใช้แทน
                  
                        // AddDebugInfo("watch tmpCrmInqPayeeOut ", data);
                        // AddDebugInfo("watch searchCond ", searchCond);
                        // data.AddDebugInfo("searchCond", searchCond);
                        AddDebugInfo("search Transformed result", data);
                    }

                    SAPResult.AddRange(tmpCrmInqPayeeOut.data);
                }
                listSAPSearchCondition.RemoveAt(0);
                countLoop += 1;
            }
            return SAPResult;
        }*/
        /// <summary>
        /// InquiryCompClientMaster
        /// </summary>
        /// <param name="iRecordsLimit"></param>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        private List<InquiryCrmPayeeListDataModel> InquiryCompClientMaster(InquiryCRMPayeeListInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryCompClientMaster", searchCondition);

            var inqAparOut = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
            {
                taxNo = searchCondition.taxNo,
                taxBranchCode = searchCondition.taxBranchCode,
                fullName = searchCondition.fullname,
                polisyClntnum = searchCondition.polisyClientId,
                vendorCode = searchCondition.sapVendorCode,
                requester = searchCondition.requester,

            });


            var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            if (inqAparOut?.aparPayeeListCollection != null)
            {

                crmInqPayeeOut =
                    (CRMInquiryPayeeContentOutputModel)APAROutTranformer.TransformModel(inqAparOut, crmInqPayeeOut);
            }

            return crmInqPayeeOut.data;
        }
        /// <summary>
        /// InquiryCLSCorporateClient
        /// </summary>
        /// <param name="iRecordsLimit"></param>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        private List<InquiryCrmPayeeListDataModel> InquiryCLSCorporateClient(InquiryCRMPayeeListInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryCLSCorporateClient", searchCondition);

            var inqAparOut = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
            {
                taxNo = searchCondition.taxNo,
                taxBranchCode = searchCondition.taxBranchCode,
                fullName = searchCondition.fullname,
                polisyClntnum = searchCondition.polisyClientId,
                vendorCode = searchCondition.sapVendorCode,
                requester = searchCondition.requester,

            });


            var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            if (inqAparOut?.aparPayeeListCollection != null)
            {

                crmInqPayeeOut =
                    (CRMInquiryPayeeContentOutputModel)APAROutTranformer.TransformModel(inqAparOut, crmInqPayeeOut);
            }

            return crmInqPayeeOut.data;
        }

        /// <summary>
        /// InquiryMasterASHR
        /// </summary>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="iRecordsLimit"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        public List<InquiryCrmPayeeListDataModel> InquiryMasterASHR(InquiryCRMPayeeListInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryMasterASHR", searchCondition);

            var inqAparOut = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
            {
                taxNo = searchCondition.taxNo,
                taxBranchCode = searchCondition.taxBranchCode,
                fullName = searchCondition.fullname,
                polisyClntnum = searchCondition.polisyClientId,
                vendorCode = searchCondition.sapVendorCode,
                requester = searchCondition.requester,

            });


            var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            if (inqAparOut?.aparPayeeListCollection != null)
            {

                crmInqPayeeOut =
                    (CRMInquiryPayeeContentOutputModel)APAROutTranformer.TransformModel(inqAparOut, crmInqPayeeOut);
            }

            return crmInqPayeeOut.data;
        }
        /// <summary>
        /// InquiryCLSPersonalClient
        /// </summary>
        /// <param name="iRecordsLimit"></param>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        private List<InquiryCrmPayeeListDataModel> InquiryCLSPersonalClient(InquiryCRMPayeeListInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryCLSPersonalClient", searchCondition);

            var inqAparOut = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
            {
                taxNo = searchCondition.taxNo,
                taxBranchCode = searchCondition.taxBranchCode,
                fullName = searchCondition.fullname,
                polisyClntnum = searchCondition.polisyClientId,
                vendorCode = searchCondition.sapVendorCode,
                requester = searchCondition.requester,

            });


            var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            if (inqAparOut?.aparPayeeListCollection != null)
            {

                crmInqPayeeOut =
                    (CRMInquiryPayeeContentOutputModel)APAROutTranformer.TransformModel(inqAparOut, crmInqPayeeOut);
            }

            return crmInqPayeeOut.data;
        }

        public bool IsValidSAPPolisyClientId(string polisyClientNum)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <param name="iRecordsLimit"></param>
        /// <returns></returns>
        public List<InquiryCrmPayeeListDataModel> InquiryAPARPayeeList(InquiryCRMPayeeListInputModel searchCondition)
        {
            AddDebugInfo("call method InquiryAPARPayeeList", searchCondition);
            



                var inqAparOut = MotorInquiryAparPayeeList.Instance.Execute(new InquiryAPARPayeeListInputModel
                {
                    taxNo = searchCondition.taxNo,
                    taxBranchCode = searchCondition.taxBranchCode,
                    fullName = searchCondition.fullname,
                    polisyClntnum = searchCondition.polisyClientId,
                    vendorCode = searchCondition.sapVendorCode,
                    requester = searchCondition.requester,

                });


            var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            if (inqAparOut?.aparPayeeListCollection != null)
            {

                crmInqPayeeOut =
                    (CRMInquiryPayeeContentOutputModel) APAROutTranformer.TransformModel(inqAparOut, crmInqPayeeOut);
            }

            return crmInqPayeeOut.data;
        }


        private bool FilterAndValidateSAPSearchConditions(ref List<InquiryCRMPayeeListInputModel> lstParam)
        {
            int i = 0;
            if( lstParam.Count> 0 )
            {
                while ( i < lstParam.Count )
                {
                    if (lstParam[i].SearchConditionType == ENUM_SAP_SearchConditionType.invalid)
                    {
                        lstParam.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            return (lstParam.Count > 0);
        }

        private InquiryCRMPayeeListInputModel Copy(InquiryCRMPayeeListInputModel model)
        {
            InquiryCRMPayeeListInputModel copy = new InquiryCRMPayeeListInputModel();
            if (model != null)
            {
                copy.clientType = model?.clientType??"";
                copy.roleCode = model.roleCode ?? "";
                copy.polisyClientId = model.polisyClientId ?? "";
                copy.sapVendorCode = model.sapVendorCode ?? "";
                copy.fullname = model.fullname ?? "";
                copy.taxNo = model.taxNo ?? "";
                copy.taxBranchCode = model.taxBranchCode ?? "";
                copy.requester = model.requester ?? "";
                copy.emcsCode = model.emcsCode ?? "";
            }
            
           

            return copy;
    }

}
}