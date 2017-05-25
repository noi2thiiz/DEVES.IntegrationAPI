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

            if (RoleCode!="G")
            {
                //:Inquiry ASRH;

            }

            //:Inquiry APAR;
           
            APARResult = InquiryAPARPayeeList(searchCondition);
            AllSearchResult.AddRange(APARResult);
           
            if (false /* ASRH or APAR Contran data */)
            {
                // Inquiry CLS

                if (false /* CLS Contran data */)
                {
                    // Inquiry Polisy 400
                }
            }

            if (true /*if search result Contran data  */)
            {
                
                if (true /* if search result < 100*/)
                {
                    //Inquiry SAP
                   // SAPResult = InquerySapVandor(AllSearchResult, iRecordsLimit);

                    //Join Master result and  SAP result;


                }
            }

            if (true /*if search result Contran data  */)
            {
                // MAP CRM 

            }

            //Map output to Output


            // remove duplicate data 


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
        private List<InquiryCRMPayeeListInputModel> InquiryCompClientMaster(int iRecordsLimit, List<InquiryCRMPayeeListInputModel> listSAPSearchCondition,ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
            
                try
                {
  
                    AddDebugInfo("try call method InquiryCompClientMaster ");
                    var tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
                    var i = 0;
                    //  var store = new List<EWIResCOMPInquiryClientMasterContentModel>();

                    while (i < listSAPSearchCondition.Count)
                    {
                        #region Call COMP_Inquiry through ServiceProxy

                        COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                        compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(listSAPSearchCondition[i], compInqClientInput);
                        AddDebugInfo("Not search in  compInqClientInput input ", compInqClientInput);
                    //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                    var retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                            (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);

                        //Found in Polisy400
                        if (retCOMPInqClient?.clientListCollection?.Count > 0)
                        {
                           // Console.WriteLine("289:Found in Polisy400");
                            AddDebugInfo("Found in Polisy400 ", retCOMPInqClient);

                        if (retCOMPInqClient.clientListCollection.Count + listSAPSearchCondition.Count + tmpListSAPSearchCondition.Count > iRecordsLimit)
                            {
                                throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_POLISY400, iRecordsLimit);
                            }

                            //crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(retCOMPInqClient, crmInqPayeeOut);

                            BaseTransformer transformer = TransformerFactory.GetTransformer(typeof(COMPInquiryClientMasterContentClientListModel), typeof(InquiryCRMPayeeListInputModel));
                            foreach (var polData in retCOMPInqClient.clientListCollection)
                            {
                                InquiryCRMPayeeListInputModel t = new InquiryCRMPayeeListInputModel();
                                t = (InquiryCRMPayeeListInputModel)transformer.TransformModel(polData, t);
                                t.requester = listSAPSearchCondition[i].requester;
                                t.emcsCode = listSAPSearchCondition[i].emcsCode;
                                t.roleCode = listSAPSearchCondition[i].roleCode;
                                tmpListSAPSearchCondition.Add(t);

                            }
                            //@TODO AdHoc Add Tranform_retCOMPInqClient_to_crmInqPayeeOut
                            var tranfromer = new Tranform_compInqClient_to_crmInqPayeeOut();
                            crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)tranfromer.TransformModel(retCOMPInqClient, crmInqPayeeOut);
                            listSAPSearchCondition.RemoveAt(0);
                        }
                        else
                        {
                            AddDebugInfo("311:Not Found in Polisy400");
                            i++; // next search
                        }
                       
                       
                        #endregion Call COMP_Inquiry through ServiceProxy
                    }
                    //return output break loop while
                    return tmpListSAPSearchCondition;
                }
                catch (Exception e)
                {
                    AddDebugInfo("Search 400 error"+e.Message,e.StackTrace);
                   
                }
            
            //return empty
            return default(List<InquiryCRMPayeeListInputModel>);

        }
        /// <summary>
        /// InquiryCLSCorporateClient
        /// </summary>
        /// <param name="iRecordsLimit"></param>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        private List<InquiryCRMPayeeListInputModel> InquiryCLSCorporateClient(int iRecordsLimit, List<InquiryCRMPayeeListInputModel> listSAPSearchCondition,ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
           
                    AddDebugInfo("call method", "InquiryCLSCorporateClient");
                    var tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
                    var i = 0;
                    while (i < listSAPSearchCondition.Count)
                    {
                        CLSInquiryCorporateClientInputModel clsCorpInput = new CLSInquiryCorporateClientInputModel();
                        clsCorpInput =
                            (CLSInquiryCorporateClientInputModel) TransformerFactory.TransformModel(
                                listSAPSearchCondition[i], clsCorpInput);

                        CLSInquiryCorporateClientContentOutputModel retCLSInqCorpClient =
                            (CLSInquiryCorporateClientContentOutputModel)
                            CallDevesJsonProxy<EWIResCLSInquiryCorporateClient>
                                (CommonConstant.ewiEndpointKeyCLSInquiryCorporateClient, clsCorpInput);

                        //+ If Success then pour the data from Cleansing to contentOutputModel

                        //if (retCLSInqCorpClient?.data != null && (retCLSInqCorpClient.success | IsOutputSuccess(retCLSInqCorpClient)) && retCLSInqCorpClient.data.Count > 0)
                        if (retCLSInqCorpClient?.data?.Count > 0)
                        {
                            AddDebugInfo("C:Found In Cleansing ");
                            if (retCLSInqCorpClient.data.Count + listSAPSearchCondition.Count +
                                tmpListSAPSearchCondition.Count > iRecordsLimit)
                            {
                                throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_CLS, iRecordsLimit);
                            }
                            BaseTransformer transformer =
                                TransformerFactory.GetTransformer(typeof(CLSInquiryCorporateClientOutputModel),
                                    typeof(InquiryCRMPayeeListInputModel));
                            foreach (var clsData in retCLSInqCorpClient.data)
                            {
                                InquiryCRMPayeeListInputModel t = new InquiryCRMPayeeListInputModel();
                                t = (InquiryCRMPayeeListInputModel) transformer.TransformModel(clsData, t);
                                t.requester = listSAPSearchCondition[i].requester;
                                t.emcsCode = listSAPSearchCondition[i].emcsCode;
                                t.roleCode = listSAPSearchCondition[i].roleCode;
                                t.clientType = listSAPSearchCondition[i].clientType;

                                //หาเลข polisy มาเติม
                                if (string.IsNullOrEmpty(t.polisyClientId) && !string.IsNullOrEmpty(t.cleansingId))
                                {
                                    AddDebugInfo("find polisyClientId by cleansingId" + t.cleansingId);
                                    t.polisyClientId =
                                        PolisyClientService.Instance.FindByCleansingId(t.cleansingId, "C")
                                            ?.clientNumber ?? "";
                                    AddDebugInfo("find polisyClientId result" + t.polisyClientId);
                                }
                                tmpListSAPSearchCondition.Add(t);




                            }
                            //tranform CLS to OUTPUT
                            var tranfromer = new Tranform_clsInqCorporateOut_to_crmInqPayeeOut();
                            crmInqPayeeOut =
                                (CRMInquiryPayeeContentOutputModel) tranfromer.TransformModel(retCLSInqCorpClient,
                                    crmInqPayeeOut);
                            //เอา polisy มาเติมใน output

                            listSAPSearchCondition.RemoveAt(0);

                            //if ((retCLSInqCorpClient.success | IsOutputSuccess(retCLSInqCorpClient)) & (retCLSInqCorpClient.data.Count == 1))
                            //{
                            //    bFound_Cleansing = true;
                            //    //crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)TransformerFactory.TransformModel(retCLSInqCorpClient, crmInqPayeeOut);
                            //    inqCrmPayeeListIn.polisyClientId = retCLSInqCorpClient.data[0].clntnum;
                            //}
                        }
                        else
                        {
                            AddDebugInfo("Not found in CLS");
                            i++; // next search
                        }
                    }

                    return tmpListSAPSearchCondition;
               
            }

        /// <summary>
        /// InquiryMasterASHR
        /// </summary>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="iRecordsLimit"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        public List<InquiryCRMPayeeListInputModel> InquiryMasterASHR(List<InquiryCRMPayeeListInputModel> listSAPSearchCondition, int iRecordsLimit,
            ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
            AddDebugInfo("call method", "InquiryMasterASHR");

            int i;
            List<InquiryCRMPayeeListInputModel> tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
            i = 0;
            while (i < listSAPSearchCondition.Count)
            {
                Console.WriteLine("106:Search in MASTER: ASRH");
                // Search in MASTER: MOTOR_InquiryMasterASRH()
                InquiryMasterASRHDataInputModel inqASRHIn =
                    (InquiryMasterASRHDataInputModel) DataModelFactory.GetModel(typeof(InquiryMasterASRHDataInputModel));
                inqASRHIn =
                    (InquiryMasterASRHDataInputModel) TransformerFactory.TransformModel(listSAPSearchCondition[i], inqASRHIn);
                InquiryMasterASRHContentModel inqASRHOut =
                    CallDevesServiceProxy<InquiryMasterASRHOutputModel, InquiryMasterASRHContentModel>(
                        CommonConstant.ewiEndpointKeyMOTORInquiryMasterASRH, inqASRHIn);

                //ส่งข้อมูลเท่าที่เจอ
                crmInqPayeeOut =
                    (CRMInquiryPayeeContentOutputModel) TransformerFactory.TransformModel(inqASRHOut, crmInqPayeeOut);

                if (inqASRHOut?.ASRHListCollection?.Count > 0)
                {
                    Console.WriteLine("117:Found in MASTER:ASRH");
                    if (inqASRHOut.ASRHListCollection.Count + listSAPSearchCondition.Count + tmpListSAPSearchCondition.Count >
                        iRecordsLimit)
                    {
                        throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_MASTER_ASRH, iRecordsLimit);
                    }
                    BaseTransformer transformer =
                        TransformerFactory.GetTransformer(typeof(InquiryMasterASRHContentASRHListCollectionDataModel),
                            typeof(InquiryCRMPayeeListInputModel));
                    foreach (InquiryMasterASRHContentASRHListCollectionDataModel asrh in inqASRHOut.ASRHListCollection)
                    {
                       
                            InquiryCRMPayeeListInputModel t = new InquiryCRMPayeeListInputModel();
                            t = (InquiryCRMPayeeListInputModel)transformer.TransformModel(asrh, t);
                            t.requester = listSAPSearchCondition[i].requester;
                            t.emcsCode = listSAPSearchCondition[i].emcsCode;
                            t.roleCode = listSAPSearchCondition[i].roleCode;
                            tmpListSAPSearchCondition.Add(t);
                        
                       
                        //bFoundIn_APAR_or_Master = true;
                    }
                    Console.WriteLine("126: FoundIn APAR or Master");
                    listSAPSearchCondition.RemoveAt(0);
                }
                else
                {
                    i++;
                }
                Console.WriteLine(crmInqPayeeOut.ToJson());
            }
            // crmInqPayeeOut.data = crmInqPayeeOut.data.DistinctBy(row => new { row.sourceData, row.sapVendorCode, row.polisyClientId, row.cleansingId }).ToList();
            return tmpListSAPSearchCondition;
        }
        /// <summary>
        /// InquiryCLSPersonalClient
        /// </summary>
        /// <param name="iRecordsLimit"></param>
        /// <param name="listSAPSearchCondition"></param>
        /// <param name="crmInqPayeeOut"></param>
        /// <returns></returns>
        private List<InquiryCRMPayeeListInputModel> InquiryCLSPersonalClient(int iRecordsLimit, List<InquiryCRMPayeeListInputModel> listSAPSearchCondition, ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
            AddDebugInfo("call method", "InquiryCLSPersonalClient");
            var tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
            var i = 0;
            while (i < listSAPSearchCondition.Count)
            {
              
                CLSInquiryPersonalClientInputModel clsPersonalInput = new CLSInquiryPersonalClientInputModel();
                clsPersonalInput = (CLSInquiryPersonalClientInputModel)TransformerFactory.TransformModel(listSAPSearchCondition[i], clsPersonalInput);
                AddDebugInfo("Search in Cleansing(CLS) ", clsPersonalInput);

                //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                CLSInquiryPersonalClientContentOutputModel retCLSInqPersClient = CallDevesServiceProxy<EWIResCLSInquiryPersonalClient, CLSInquiryPersonalClientContentOutputModel>
                    (CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient, clsPersonalInput);

                AddDebugInfo("Search in Cleansing(CLS) ", retCLSInqPersClient);

                //++ If Found records in Cleansing(CLS) then pour the data from Cleansing to contentOutputModel
                //if (retCLSInqPersClient?.data != null && (retCLSInqPersClient.success | IsOutputSuccess(retCLSInqPersClient)) && retCLSInqPersClient.data.Count > 0)

                if (retCLSInqPersClient?.data?.Count > 0)
                {
                    Console.WriteLine("P:Found In Cleansing ");
                    if (retCLSInqPersClient.data.Count + listSAPSearchCondition.Count + tmpListSAPSearchCondition.Count > iRecordsLimit)
                    {
                        throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_CLS, iRecordsLimit);
                    }
                    //bFound_Cleansing = true;
                    crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();

                    var transformerCLs =
                        new
                            TransformCLSInquiryPersonalClientContentOutputModel_to_InquiryCRMPayeeListDataOutputModel();

                    
                    //inqCrmPayeeListIn.polisyClientId = retCLSInqPersClient.data[0].clntnum;

                    BaseTransformer transformer = TransformerFactory.GetTransformer(typeof(CLSInquiryPersonalClientOutputModel), typeof(InquiryCRMPayeeListInputModel));
                    foreach (var clsData in retCLSInqPersClient.data)
                    {
                        InquiryCRMPayeeListInputModel t = new InquiryCRMPayeeListInputModel();
                        t = (InquiryCRMPayeeListInputModel)transformer.TransformModel(clsData, t);
                        t.requester = listSAPSearchCondition[i].requester;
                        t.emcsCode = listSAPSearchCondition[i].emcsCode;
                        t.roleCode = listSAPSearchCondition[i].roleCode;
                        t.clientType = listSAPSearchCondition[i].clientType;

                        //หาเลข polisy มาเติม
                        if (string.IsNullOrEmpty(t.polisyClientId) && !string.IsNullOrEmpty(t.cleansingId))
                        {
                            AddDebugInfo("find polisyClientId by cleansingId"+ t.cleansingId);
                            t.polisyClientId = PolisyClientService.Instance.FindByCleansingId(t.cleansingId, "P")?.clientNumber ?? "";
                            AddDebugInfo("find polisyClientId result" + t.polisyClientId);
                        }


                        tmpListSAPSearchCondition.Add(t);

                      
                    };
                    //@TODO AdHoc Add CLS Tranform_clsInqPersonalOut_to_crmInqPayeeOut
                     var tranfromer = new Tranform_clsInqPersonalOut_to_crmInqPayeeOut();
                     crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)tranfromer.TransformModel( retCLSInqPersClient, crmInqPayeeOut);
                     listSAPSearchCondition.RemoveAt(0);
                }
                else
                {
                    i++;
                }
            }

            return tmpListSAPSearchCondition;
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