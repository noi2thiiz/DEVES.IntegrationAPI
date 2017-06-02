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
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic;
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
        public List<InquiryCrmPayeeListDataModel> FinalSearchResult = new List<InquiryCrmPayeeListDataModel>();

        public BaseTransformer APAROutTransformer = new TransformAPARInquiryAPARPayeeListContentOutputModel_to_InquiryCRMPayeeListDataOutputModel();
        public BaseTransformer PolisyOutTransformer = new Tranform_compInqClient_to_crmInqPayeeOut();
        public BaseTransformer CLSCorporateOutTransformer = new Tranform_clsInqCorporateOut_to_crmInqPayeeOut();
        public BaseTransformer ASRHOutTransformer = new TransformInquiryMasterASRHContentOutputModel_to_InquiryCRMPayeeListDataOutputModel();
        public BaseTransformer CLSPersonalOutTransform = new Tranform_clsInqPersonalOut_to_crmInqPayeeOut();

        public void TranformInput()
        {
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();
            //ลบ บริษัทออกจากชื่อ
            if (!string.IsNullOrEmpty(inqCrmPayeeInput.fullname))
            {
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("บริษัท.", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("บริษัท", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("บ.", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname?.Replace("จำกัด", "");
                inqCrmPayeeInput.fullname = inqCrmPayeeInput.fullname.ReplaceMultiplSpacesWithSingleSpace();

            }

            timer.Stop();
            TimeSpan t = timer.Elapsed;
            AddDebugInfo("TranformInput="+t.TotalMilliseconds);

        }
        public override BaseDataModel ExecuteInput(object input)
        {
            
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();
            //config
            int iRecordsLimit = int.Parse( GetAppConfigurationSetting("SearchRecordsLimit").ToString());


            
            // request
            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();

           


            List<InquiryCRMPayeeListInputModel> listSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
            Console.WriteLine(listSAPSearchCondition);
           
            inqCrmPayeeInput = (InquiryCRMPayeeListInputModel)input;
            InquiryCRMPayeeListInputModel searchCondition = Copy(inqCrmPayeeInput);

            string isOptionalEmpty = inqCrmPayeeInput.polisyClientId + inqCrmPayeeInput.sapVendorCode + inqCrmPayeeInput.fullname + inqCrmPayeeInput.taxNo + inqCrmPayeeInput.taxBranchCode + inqCrmPayeeInput.emcsCode;
            if (string.IsNullOrEmpty(isOptionalEmpty.Trim()))
            {
                crmInqPayeeOut.code = CONST_CODE_FAILED;
                crmInqPayeeOut.message = "Please fill at least 1 condition";
                crmInqPayeeOut.description = "";
                crmInqPayeeOut.transactionId = TransactionId;
                crmInqPayeeOut.transactionDateTime = DateTime.Now;

                return crmInqPayeeOut;
            }


            //validate and  tranform input
            TranformInput();
            string RoleCode = inqCrmPayeeInput?.roleCode?.ToUpper();

            //Start Process

            // สาย A S R H ให้ค้นที่ MASTERASRH ก่อน
            if (RoleCode!="G")
            {
                //:Inquiry ASRH;
                ASHRResult = InquiryMasterASHR(searchCondition);

                if (ASHRResult != null)
                {
                    AllSearchResult.AddRange(ASHRResult);
                }

            }

            //กรณีไม่พบข้อมูล ให้ไปค้นที่ APAR
            if (!AllSearchResult.Any())
            {
                APARResult = InquiryAPARPayeeList(searchCondition);

                if(APARResult != null)
                {
                    AllSearchResult.AddRange(APARResult);
                }
                
            }

            //กรณีไม่พบข้อมูล ให้ไปค้นที่ CLS
            if (!AllSearchResult.Any())
            {
                if (inqCrmPayeeInput.clientType == "C")
                {
                    CLSResult = InquiryCLSCorporateClient(searchCondition);
                    if (CLSResult != null)
                    {

                        AllSearchResult.AddRange(CLSResult);
                    }
                }
                else if (inqCrmPayeeInput.clientType == "P")
                {
                    CLSResult = InquiryCLSPersonalClient(searchCondition);
                    if (CLSResult != null)
                    {
                        AllSearchResult.AddRange(CLSResult);
                    }
                }
                // AllSearchResult.AddRange(CLSResult);
            }

            //กรณีไม่พบข้อมูล ให้ไปค้นที่ 400
            if (!AllSearchResult.Any())
            {
                COMPResult = InquiryCompClientMaster(searchCondition);
                if (COMPResult != null)
                {
                    AllSearchResult.AddRange(COMPResult);
                }
                //AllSearchResult.AddRange(COMPResult);
            }

            //ถ้าข้อมูลมากกว่า 100 รายการ จะ throw error
            if (AllSearchResult.Count> iRecordsLimit)
            {
                throw new  BuzErrorException("500", "คุณระบุเงื่อนไขในการสืบค้นน้อยเกินไป", "คุณระบุเงื่อนไขในการสืบค้นน้อยเกินไป ทำให้พบข้อมูลจำนวนมากเกินกว่าที่ระบบอนุญาต กรุณาระบุเงื่อนไขที่ชัดเจนมากขึ้น เช่นการระบุชื่อและนามสกุล");
            }

            //กรณีที่ไม่พบข้อมูล
            if (AllSearchResult.Any())
            {
                // throw new BuzErrorException("200", "ไม่พบข้อมูล", " กรุณาตรวจสอบเงื่อนไขในการค้นหาอีกครั้ง หรือใช้เงื่อนไขอื่นในการค้นหา ข้อควรระวัง ไม่ควรค้นจากการระบุเงื่อนไขหลายๆอย่างพร้อมกัน เช่นการค้นทั้งชื่อนามสกุลพร้อมกับเลขประจำตัวประชาชน");

                //ซ่อมข้อมูล  Client ID 
                AllSearchResult = FixEmptyPolisyClientId(AllSearchResult, inqCrmPayeeInput.clientType);


                //กรณีที่พบข้อมูลค้นหาต่อที่ SAP
                SAPResult = InquerySapVandor(AllSearchResult);

                // ค้นข้อมูลใน CRM เพื่อเอาเลข CRM Client ID มาเติม ??? ต้องทำมัย


                // remove duplicate data 
                // ตัวที่อยู่ใน search condition ทั้งหมดจะต้องไม่ซ้ำกันเลย


                //order by vcode,cleasing id desc
                var distinctResult = ProcessDistinct(SAPResult);
                FinalSearchResult = ProcessOrderBy(distinctResult);

                if (FinalSearchResult!= null && FinalSearchResult.Any())
                {
                    crmInqPayeeOut.data.AddRange(FinalSearchResult);

                }
                
            }

           

            //return output
            timer.Stop();
            TimeSpan t = timer.Elapsed;
            AddDebugInfo("ExecuteInput=" + t.TotalMilliseconds);
            crmInqPayeeOut.code = AppConst.CODE_SUCCESS;
            crmInqPayeeOut.message = AppConst.MESSAGE_SUCCESS;
            crmInqPayeeOut.description = "";
            crmInqPayeeOut.transactionId = TransactionId;
            crmInqPayeeOut.transactionDateTime = DateTime.Now;
           
           



           

            return crmInqPayeeOut;
        }

        public List<InquiryCrmPayeeListDataModel> ProcessDistinct(List<InquiryCrmPayeeListDataModel> sapResult)
        {
            return sapResult
                .DistinctBy(row => new
                {
                    row.fullName,
                    row.name1,
                    row.name2,
                    row.cleansingId,
                    row.polisyClientId,
                    row.sapVendorCode,
                    row.sapVendorGroupCode,
                    row.taxNo,
                    row.taxBranchCode
                   
                }).ToList();
        }

        public List<InquiryCrmPayeeListDataModel> ProcessOrderBy(List<InquiryCrmPayeeListDataModel> sapResult)
        {
            return sapResult
               
                .OrderBy(row => row.fullName)
                
                .ThenByDescending(row => row.sapVendorCode)
                .ThenBy(row => row.sapVendorGroupCode)
                .ThenByDescending(row => row.polisyClientId)
                .ThenBy(row => row.taxNo)
                .ThenBy(row => row.taxBranchCode)
                .ToList();

            
        }

        public List<InquiryCrmPayeeListDataModel> FixEmptyPolisyClientId( List<InquiryCrmPayeeListDataModel> listSearchResult,string defaultClientType)
        {
            // ดึงค่าจาก Polisy มาเติมในกรณีที่ข้อมูลสร้างใหม่ CLS จะยังไม่มีเลข Polisy
            if (listSearchResult==null)
            {
                return default(List<InquiryCrmPayeeListDataModel>);
            }
            var polisyClientService = new PolisyClientService(TransactionId,ControllerName);
            foreach (var temp in listSearchResult)
            {
                if (string.IsNullOrEmpty(temp.polisyClientId) || temp?.polisyClientId== "0")
                {
                    //debugInfo.AddDebugInfo("Find Polisy for new client  " + temp.polisyClientId, "");



                    var lstPolisyClient = polisyClientService.FindByCleansingId(temp.cleansingId, defaultClientType);
                   
                    if (lstPolisyClient?.cleansingId != null)
                    {

                        //debugInfo.AddDebugInfo("found Polisy for new client =" + lstPolisyClient.clientNumber, "");
                        temp.polisyClientId = lstPolisyClient.clientNumber;
                    }
                    else
                    {
                        //debugInfo.AddDebugInfo(" not found Polisy for new client =", "");
                    }
                    if (temp.polisyClientId == "0")
                    {
                        temp.polisyClientId = "";
                    }

                }
            }
            return listSearchResult;
        }

        /// <summary>
        ///  // * foreach item in result
        //     if item contain VCODE then Search by VCODE
        //     if not found and item contain  PolisyClientID then search by PREVACC
        //     if not found and item contain  Tax ID then search by Tax(TAX3+TAX4)
        /// </summary>
        /// <param name="listSAPSearchConditions"></param>
        /// <returns></returns>
        private List<InquiryCrmPayeeListDataModel> InquerySapVandor(List<InquiryCrmPayeeListDataModel> listSAPSearchConditions)
        {

            try
            {
                AddDebugInfo("InquerySapVandor", listSAPSearchConditions);


                var allSearchResult = new List<InquiryCrmPayeeListDataModel>();

                var service = new SAPInquiryVendor(TransactionId, ControllerName);


                foreach (var condition in listSAPSearchConditions)
                {
                    condition.sapResults = new List<InquiryCrmPayeeListDataModel>();
                    var polisyClientId = condition?.polisyClientId;
                    var searchResult = new EWIResSAPInquiryVendorContentModel();
                    if (!string.IsNullOrEmpty(condition?.sapVendorCode))
                    {
                        var sapSearchCondition = new SAPInquiryVendorInputModel
                        {
                            VCODE = condition?.sapVendorCode
                        };
                        condition.sapSearchCondition = sapSearchCondition;
                        searchResult = service.Execute(sapSearchCondition);


                    }
                    AddDebugInfo("searchResult 1", searchResult);

                    if ((searchResult?.VendorInfo == null || !searchResult.VendorInfo.Any()) && !string.IsNullOrEmpty(condition?.polisyClientId))
                    {
                        var sapSearchCondition = new SAPInquiryVendorInputModel
                        {
                            PREVACC = condition?.polisyClientId
                        };
                        condition.sapSearchCondition = sapSearchCondition;
                        searchResult = service.Execute(sapSearchCondition);
                    }
                    AddDebugInfo("searchResult 2", searchResult);
                    if ((searchResult?.VendorInfo == null || !searchResult.VendorInfo.Any()) && !string.IsNullOrEmpty(condition?.taxNo))
                    {
                        var sapSearchCondition = new SAPInquiryVendorInputModel
                        {
                            TAX3 = condition?.taxNo,
                            TAX4 = condition?.taxBranchCode
                        };
                        condition.sapSearchCondition = sapSearchCondition;
                        searchResult = service.Execute(sapSearchCondition);
                    }

                    if (searchResult?.VendorInfo != null && searchResult.VendorInfo.Any())
                    {

                        CRMInquiryPayeeContentOutputModel tmpCrmInqPayeeOut =
                            (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(searchResult,
                                new CRMInquiryPayeeContentOutputModel());
                        if (tmpCrmInqPayeeOut?.data != null && tmpCrmInqPayeeOut.data.Any())
                        {
                            foreach (InquiryCrmPayeeListDataModel data in tmpCrmInqPayeeOut.data)
                            {
                                data.emcsMemHeadId = condition.emcsMemHeadId;
                                data.emcsMemId = condition.emcsMemId;
                                data.contactNumber = condition.contactNumber;
                                data.assessorFlag = condition.assessorFlag;
                                data.solicitorFlag = condition.solicitorFlag;
                                data.repairerFlag = condition.repairerFlag;
                                data.hospitalFlag = condition.hospitalFlag;
                                data.polisyClientId = (!string.IsNullOrEmpty(polisyClientId)
                                    ? polisyClientId
                                    : data.polisyClientId); // เอา  polisyClientId จากข้อมูลต้นทาง (APAR,ASHR ) มาใช้แทน

                                //   AddDebugInfo("search Transformed result", data);

                                allSearchResult.Add(data);
                            }
                        }



                    }
                    else
                    {
                        allSearchResult.Add(condition);
                    }

                }

                return allSearchResult;
            }
            catch (Exception e)
            {
                AddDebugInfo("Error Exception InquerySapVandor: "+e.Message,e.StackTrace);
                throw;
            }
          
        }

        /// <summary>
        /// InquiryCompClientMaster
        /// </summary>
        /// <param name="iRecordsLimit"></param>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        private List<InquiryCrmPayeeListDataModel> InquiryCompClientMaster(InquiryCRMPayeeListInputModel searchCondition)
        {
            AddDebugInfo("call method COMPInquiryClientMaster", searchCondition);
            var service = new COMPInquiryClientMaster(TransactionId, ControllerName);
            var inqPolisyOut = service.Execute(new COMPInquiryClientMasterInputModel
            {
                cltType = searchCondition?.clientType??"",
                asrType = searchCondition?.roleCode??"",
                clntnum = searchCondition?.polisyClientId??"",
                fullName = searchCondition?.fullname??"",
                idcard = searchCondition?.taxNo??"",
                branchCode = searchCondition?.taxBranchCode??"",
                // backDay = searchCondition.,
                cleansingId = searchCondition?.cleansingId??""
            });


            var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            if (inqPolisyOut?.clientListCollection != null)
            {

                crmInqPayeeOut =
                    (CRMInquiryPayeeContentOutputModel)PolisyOutTransformer.TransformModel(inqPolisyOut, crmInqPayeeOut);
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
            AddDebugInfo("call method CLSInquiryCLSCorporateClient", searchCondition);
            try
            {
                var service = new CLSInquiryCLSCorporateClient(TransactionId, ControllerName);
                var inqClsCorporateOut = service.Execute(new CLSInquiryCorporateClientInputModel
                {
                    clientId = searchCondition?.polisyClientId ?? "",
                    roleCode = searchCondition?.roleCode ?? "",
                    corporateFullName = searchCondition?.fullname ?? "",
                    taxNo = searchCondition?.taxNo ?? ""

                    // telephone = searchCondition.tele,
                    // emailAddress = searchCondition.e,
                    // backDay = searchCondition.backDay

                });


                var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
                if (inqClsCorporateOut?.data != null)
                {

                    crmInqPayeeOut =
                        (CRMInquiryPayeeContentOutputModel)CLSCorporateOutTransformer.TransformModel(inqClsCorporateOut, crmInqPayeeOut);
                }

                return crmInqPayeeOut.data;
            }
            catch (Exception e)
            {
                AddDebugInfo("Error Exception InquiryCLSCorporateClient: " + e.Message, e.StackTrace);
                throw;
            }
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
            try
            {

                AddDebugInfo("call method MOTORInquiryMasterASRH", searchCondition);
                var service = new MOTORInquiryMasterASRH(TransactionId, ControllerName);
                var inqASRHOut = service.Execute(new InquiryMasterASRHDataInputModel
                {
                    vendorCode = searchCondition?.sapVendorCode ?? "",
                    taxNo = searchCondition?.taxNo ?? "",
                    taxBranchCode = searchCondition?.taxBranchCode ?? "",
                    asrhType = searchCondition?.roleCode ?? "",
                    polisyClntnum = searchCondition?.polisyClientId ?? "",
                    fullName = searchCondition?.fullname ?? "",
                    emcsCode = searchCondition?.emcsCode ?? ""

                });


                var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
                if (inqASRHOut?.ASRHListCollection != null)
                {

                    crmInqPayeeOut =
                        (CRMInquiryPayeeContentOutputModel)ASRHOutTransformer.TransformModel(inqASRHOut, crmInqPayeeOut);
                }

                return crmInqPayeeOut.data;
            }
            catch (Exception e)
            {
                AddDebugInfo("Error Exception MOTORInquiryMasterASRH: " + e.Message, e.StackTrace);
                throw;
            }
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
            try
            {
                AddDebugInfo("call method InquiryCLSPersonalClient", searchCondition);
                var service = new CLSInquiryCLSPersonalClient(TransactionId, ControllerName);
                var clssearchCondition = new CLSInquiryPersonalClientInputModel
                {

                    clientId = searchCondition?.polisyClientId ?? "",
                    roleCode = searchCondition?.roleCode ?? "",
                    personalFullName = searchCondition?.fullname ?? "",
                    idCitizen = searchCondition?.taxNo ?? "",
                    //telephone = searchCondition.telephone,



                };

                // AddDebugInfo("call method CLSInquiryCLSPersonalClient", clssearchCondition);
                var inqClsPesonalOut = service.Execute(clssearchCondition);


                var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
                if (inqClsPesonalOut?.data != null)
                {

                    crmInqPayeeOut =
                        (CRMInquiryPayeeContentOutputModel)CLSPersonalOutTransform.TransformModel(inqClsPesonalOut, crmInqPayeeOut);

                    //ถ้า output ไม่ polisy client id ให้ไปหามาเติม
                }


                return crmInqPayeeOut.data;
            }
            catch (Exception e)
            {
                AddDebugInfo("Error Exception InquiryCLSPersonalClient: " + e.Message, e.StackTrace);
                throw;
            }
           
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
            try
            {
                AddDebugInfo("call method InquiryAPARPayeeList", searchCondition);
                var crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
                // return crmInqPayeeOut?.data;

                var service = new MotorInquiryAparPayeeList(TransactionId, ControllerName);
                var inqAparOut = service.Execute(new InquiryAPARPayeeListInputModel
                {
                    taxNo = searchCondition?.taxNo ?? "",
                    taxBranchCode = searchCondition?.taxBranchCode ?? "",
                    fullName = searchCondition?.fullname ?? "",
                    polisyClntnum = searchCondition?.polisyClientId ?? "",
                    vendorCode = searchCondition?.sapVendorCode ?? "",
                    clientType = searchCondition?.clientType ?? "",
                    requester = searchCondition?.requester ?? ""

                });



                if (inqAparOut?.aparPayeeListCollection != null)
                {

                    crmInqPayeeOut =
                        (CRMInquiryPayeeContentOutputModel)APAROutTransformer.TransformModel(inqAparOut, crmInqPayeeOut);
                }

                return crmInqPayeeOut.data;
            }
            catch (Exception e)
            {
                AddDebugInfo("Error Exception InquiryAPARPayeeList: " + e.Message, e.StackTrace);
                throw;
            }
           
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