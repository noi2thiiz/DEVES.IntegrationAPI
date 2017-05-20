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
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using Microsoft.Ajax.Utilities;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzInquiryCRMPayeeList : BuzCommand
    {
        /// <summary>
        /// ใช้ทดสอบว่า หากไม่เจอข้อมูลใน SAP ข้อมูล OUTPUT จะเป็นอย่างไร
        /// production set = false
        /// </summary>
        public bool IgnoreSap = true;
        public bool IgnoreApar = true;
        public override BaseDataModel ExecuteInput(object input)
        {
            /*
                1) sapVendorCode => ใช้ตัวเดียว ใหญ่สุด ถ้ามีเอาอันนี้ Search ใน SAPเลยไม่ต้องสนใจใคร
                ถ้าไม่เจอ => 2) polisyClientId => ไป search ใน SAP ใช้ตัวเดียว ใหญ่รองลงมา 
                ถ้าไม่เจอ => 3) taxNo & taxBranchCode => เป็น Key ในการ Search ใน SAP โดยไม่ต้องสนใจชื่อ
                ถ้าไม่เจอ => 4) (fullname &  taxNo & taxBranchCode) OR (fullname & taxNo) OR (fullname ) => เป็น Key 
                 ถ้า ID ไม่เจอ 
	                4.1) กรณี {ASRH} => ก็ต้องเอาชื่อไปหาที่ APAR เพื่อเอา ID มาลองหาที่ SAP อีกครั้ง
	                4.1) กรณี {G} => ก็ต้องเอาชื่อไปหาที่ APAR เพื่อเอา ID มาลองหาที่ SAP อีกครั้ง
	                4.2) แต่ถ้า APAR ไม่มี ก็ต้องเอาชื่อ ไปหาต่อที่ CLS เพื่อเอา ID ไปลองหาที่ SAP
	                4.3) แต่ถ้า CLS ก็ไม่มี ก็เอาชื่อไปหาต่อที่ Polisy400 แทนเพื่อหา ID เพื่อไปลองหาที่ SAP
	                4.4) ถ้าไม่เจอเลย ก็จบ คือ ไม่เจอ
	                4.5) แต่ถ้าเจอแล้วไม่เจอใน SAP เลยก็จบ
                    -- ข้อ 4.5 คุณบีบอกไม่ถูก ถ้าไม่เจอใน SAP ให้เอาข้อมูลจาก vendor ที่พบข้อมูลก่อน SAP ไป return แทน เป็นลำดับย้อนกลับไป
            */
    

            int iRecordsLimit = int.Parse( GetAppConfigurationSetting("SearchRecordsLimit").ToString());

            bool bFoundIn_APAR_or_Master = false;
            List<InquiryCRMPayeeListInputModel> listSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();

            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = new CRMInquiryPayeeContentOutputModel();
            crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();
            crmInqPayeeOut.description = "";

           
               // Console.WriteLine(input.ToString());

                InquiryCRMPayeeListInputModel inqCrmPayeeListIn = (InquiryCRMPayeeListInputModel)input;
            string isOptionalEmpty = inqCrmPayeeListIn.polisyClientId + inqCrmPayeeListIn.sapVendorCode + inqCrmPayeeListIn.fullname + inqCrmPayeeListIn.taxNo + inqCrmPayeeListIn.taxBranchCode + inqCrmPayeeListIn.emcsCode;
            if(string.IsNullOrEmpty(isOptionalEmpty.Trim()))
            {
                crmInqPayeeOut.code = CONST_CODE_FAILED;
                crmInqPayeeOut.message = "FAILED";
                crmInqPayeeOut.description = "Please fill at least 1 condition";
                crmInqPayeeOut.transactionId = TransactionId;
                crmInqPayeeOut.transactionDateTime = DateTime.Now;

                return crmInqPayeeOut;
            }
            InquiryCRMPayeeListInputModel tempInqCrmPayeeInput = Copy(inqCrmPayeeListIn);

            switch (inqCrmPayeeListIn.requester)
            {
                case "MC": inqCrmPayeeListIn.requester = "MotorClaim"; break;
                default: inqCrmPayeeListIn.requester = "MotorClaim"; break;
            }

            listSAPSearchCondition.Add(tempInqCrmPayeeInput);

                if ( (!listSAPSearchCondition.Exists(x => x.SearchConditionType != ENUM_SAP_SearchConditionType.invalid)) )
                {
                #region IF inqCrmPayeeListIn.ClientType == "G" -> APAR.InquiryAPARPayeeList

                    if (!IgnoreApar)
                    {
                    if (inqCrmPayeeListIn.roleCode.ToUpper() == ENUM_CLIENT_ROLE.G.ToString())
                    {
                        var tmpListSAPSearchCondition = InquiryAPARPayeeList(listSAPSearchCondition, iRecordsLimit, inqCrmPayeeListIn, ref crmInqPayeeOut);
                        listSAPSearchCondition.AddRange(tmpListSAPSearchCondition);

                    }
                }
                    
                    
                    #endregion inqCrmPayeeListIn.roleCode == "G" -> APAR.InquiryAPARPayeeList

                    #region IF inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH
                    else
                    {
                        var tmpListSAPSearchCondition = InquiryMasterASHR(listSAPSearchCondition, iRecordsLimit, ref crmInqPayeeOut);
                        listSAPSearchCondition.AddRange(tmpListSAPSearchCondition);
                    }

                #endregion inqCrmPayeeListIn.roleCode == {A,S,R,H} -> Master.InquiryMasterASRH
                    Console.WriteLine("Check Condition  Cleansing ?");
                //@TODO TEST
                if (!listSAPSearchCondition.Exists(x => x.SearchConditionType != ENUM_SAP_SearchConditionType.invalid) )
                {
                        #region Search in Cleansing(CLS) then search in Polisy400
                        //if (!bFoundIn_APAR_or_Master)
                        //{
                        bool bFound_Cleansing = false;
                        Console.WriteLine("Search In Cleansing ");
                      
                    if (true)
                    {
                        switch (inqCrmPayeeListIn.clientType.ToUpper())
                        {
                            case "P":
                                #region Search Client from Cleansing CLS_InquiryCLSPersonalClient
                                {


                                var tmpListSAPSearchCondition = InquiryCLSPersonalClient(iRecordsLimit, listSAPSearchCondition, ref crmInqPayeeOut);
                                listSAPSearchCondition.AddRange(tmpListSAPSearchCondition);

                               
                                break;
                                }
                                #endregion Search Client from Cleansing

                            case "C":
                                {
                                #region Call CLS_InquiryCLSCorporateClient through ServiceProxy

                                var tmpListSAPSearchCondition  = InquiryCLSCorporateClient(iRecordsLimit, listSAPSearchCondition,ref crmInqPayeeOut);
                                listSAPSearchCondition.AddRange(tmpListSAPSearchCondition);
                                #endregion Call CLS_InquiryCLSCorporateClient through ServiceProxy
                                break;
                                }
                               
                            default:
                                break;
                        }
                    }
                    #endregion Search in Cleansing(CLS) then search in Polisy400

                    //if (!bFound_Cleansing)
                     Console.WriteLine("270:Chech Condition Search in Polisy400?");
                    if (!listSAPSearchCondition.Exists(x => x.SearchConditionType != ENUM_SAP_SearchConditionType.invalid))
                    {
                        Console.WriteLine("273:Search in Polisy400");
                        var tmpListSAPSearchCondition = InquiryCompClientMaster(iRecordsLimit, listSAPSearchCondition, ref crmInqPayeeOut);

                        listSAPSearchCondition.AddRange(tmpListSAPSearchCondition);


                    }
                    //}
                    //}
                }

                }
                else
                {
                    Console.WriteLine("Not search in  Cleansing ");
                }
            var counrSAPResult = 0 ;
                Console.WriteLine("Before Search SAP");

            if (!IgnoreSap)
            {
                if (FilterAndValidateSAPSearchConditions(ref listSAPSearchCondition))
                {
                    #region Search In SAP: SAP_InquiryVendor()
                    Console.WriteLine(listSAPSearchCondition.ToJson());
                    InquerySapVandor(listSAPSearchCondition, crmInqPayeeOut, iRecordsLimit, counrSAPResult);

                    #endregion Search In SAP: SAP_InquiryVendor()
                }
                else
                {
                    Console.WriteLine("Not Found In SAP");
                    //crmInqPayeeOut.content = null;
                }
            }
            
            Console.WriteLine("=========OUTPUT=========");
            Console.WriteLine("=========OUTPUT=========");
            Console.WriteLine(crmInqPayeeOut.ToJson());
            Console.WriteLine("=========OUTPUT=========");
            Console.WriteLine("=========OUTPUT=========");
            //@TODO AdHoc ลบข้อมูลจาก Source อื่นออก ถ้าเจอข้อมูลใน SAP ให้ถือว่าใช้ขอมูลจาก SAP
            if (crmInqPayeeOut.data.Where(row => row.sourceData == "SAP").Distinct().ToList().Count > 0)
                {
                    crmInqPayeeOut.data = crmInqPayeeOut.data.Where(row => row.sourceData == "SAP").DistinctBy(row => row.sapVendorCode).ToList();

                }
            
            else
                if (crmInqPayeeOut.data.Where(row => row.sourceData == "COMP").Distinct().ToList().Count > 0)
                {
                    crmInqPayeeOut.data = crmInqPayeeOut.data.Where(row => row.sourceData == "COMP").DistinctBy(row => row.polisyClientId).ToList();

                }
                 //ตัด CLS ออก
                  else
                  if (crmInqPayeeOut.data.Where(row => row.sourceData == "CLS").Distinct().ToList().Count > 0)
                  {
                    crmInqPayeeOut.data = crmInqPayeeOut.data.Where(row => row.sourceData == "CLS").DistinctBy(row => row.cleansingId).ToList();

                  }

                //MASTER_ASHR
                //APAR






            crmInqPayeeOut.code = CONST_CODE_SUCCESS;                
                crmInqPayeeOut.message = "SUCCESS";
           
            crmInqPayeeOut.transactionId = TransactionId;
            crmInqPayeeOut.transactionDateTime = DateTime.Now;

            return crmInqPayeeOut;
        }

        
       
        private void InquerySapVandor(List<InquiryCRMPayeeListInputModel> listSAPSearchCondition, CRMInquiryPayeeContentOutputModel crmInqPayeeOut,
            int iRecordsLimit, int counrSAPResult)
        {
            Console.WriteLine("========204--------");
            Console.WriteLine(listSAPSearchCondition.ToJson());
            var limit = 20;
            var countLoop = 0;
            while (listSAPSearchCondition.Count > 0 && countLoop <= 20)
            {
                
                InquiryCRMPayeeListInputModel searchCond = listSAPSearchCondition[0];
                Console.WriteLine("==searchCond===");
                Console.WriteLine(searchCond.ToJson());
                SAPInquiryVendorInputModel inqSAPVendorIn =
                    (SAPInquiryVendorInputModel) DataModelFactory.GetModel(typeof(SAPInquiryVendorInputModel));
                inqSAPVendorIn = (SAPInquiryVendorInputModel) TransformerFactory.TransformModel(searchCond, inqSAPVendorIn);

                EWIResSAPInquiryVendorContentModel inqSAPVendorContentOut =
                    CallDevesServiceProxy<SAPInquiryVendorOutputModel, EWIResSAPInquiryVendorContentModel>(
                        CommonConstant.ewiEndpointKeySAPInquiryVendor, inqSAPVendorIn);

                if (inqSAPVendorContentOut?.VendorInfo != null)
                {
                    if (inqSAPVendorContentOut.VendorInfo.Count + listSAPSearchCondition.Count > iRecordsLimit)
                    {
                        throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_SAP, iRecordsLimit);
                    }
                    CRMInquiryPayeeContentOutputModel tmpCrmInqPayeeOut =
                        (CRMInquiryPayeeContentOutputModel) TransformerFactory.TransformModel(inqSAPVendorContentOut,
                            crmInqPayeeOut);
                    foreach (InquiryCrmPayeeListDataModel data in tmpCrmInqPayeeOut.data)
                    {
                        data.emcsMemHeadId = searchCond.emcsMemHeadId;
                        data.emcsMemId = searchCond.emcsMemId;
                        data.contactNumber = searchCond.contactNumber;
                        data.assessorFlag = searchCond.assessorFlag;
                        data.solicitorFlag = searchCond.solicitorFlag;
                        data.repairerFlag = searchCond.repairerFlag;
                        data.hospitalFlag = searchCond.hospitalFlag;
                        data.polisyClientId = searchCond.polisyClientId; // เอา  polisyClientId จากข้อมูลต้นทาง (APAR,ASHR ) มาใช้แทน
                        counrSAPResult += 1;
                    }

                    crmInqPayeeOut.data.AddRange(tmpCrmInqPayeeOut.data);
                }
                listSAPSearchCondition.RemoveAt(0);
                countLoop += 1;
            }
        }

        private List<InquiryCRMPayeeListInputModel> InquiryCompClientMaster(int iRecordsLimit, List<InquiryCRMPayeeListInputModel> listSAPSearchCondition,ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
            var tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
            var i = 0;
            //  var store = new List<EWIResCOMPInquiryClientMasterContentModel>();

            while (i < listSAPSearchCondition.Count)
            {
                #region Call COMP_Inquiry through ServiceProxy

                COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                compInqClientInput = (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(listSAPSearchCondition[i], compInqClientInput);

                //+ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                var retCOMPInqClient = CallDevesServiceProxy<COMPInquiryClientMasterOutputModel, EWIResCOMPInquiryClientMasterContentModel>
                                                                                        (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);

                //Found in Polisy400
                if (retCOMPInqClient?.clientListCollection?.Count > 0)
                {
                    Console.WriteLine("289:Found in Polisy400");


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
                    Console.WriteLine("311:Not Found in Polisy400");
                    i++;
                }
                #endregion Call COMP_Inquiry through ServiceProxy
            }

            return tmpListSAPSearchCondition;
        }

        private List<InquiryCRMPayeeListInputModel> InquiryCLSCorporateClient(int iRecordsLimit, List<InquiryCRMPayeeListInputModel> listSAPSearchCondition,ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
            var tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
            var i = 0;
            while (i < listSAPSearchCondition.Count)
            {
                CLSInquiryCorporateClientInputModel clsCorpInput = new CLSInquiryCorporateClientInputModel();
                clsCorpInput = (CLSInquiryCorporateClientInputModel)TransformerFactory.TransformModel(listSAPSearchCondition[i], clsCorpInput);

                CLSInquiryCorporateClientContentOutputModel retCLSInqCorpClient = (CLSInquiryCorporateClientContentOutputModel)CallDevesJsonProxy<EWIResCLSInquiryCorporateClient>
                    (CommonConstant.ewiEndpointKeyCLSInquiryCorporateClient, clsCorpInput);

                //+ If Success then pour the data from Cleansing to contentOutputModel

                //if (retCLSInqCorpClient?.data != null && (retCLSInqCorpClient.success | IsOutputSuccess(retCLSInqCorpClient)) && retCLSInqCorpClient.data.Count > 0)
                if (retCLSInqCorpClient?.data?.Count > 0)
                {
                    Console.WriteLine("C:Found In Cleansing ");
                    if (retCLSInqCorpClient.data.Count + listSAPSearchCondition.Count + tmpListSAPSearchCondition.Count > iRecordsLimit)
                    {
                        throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_CLS, iRecordsLimit);
                    }
                    BaseTransformer transformer = TransformerFactory.GetTransformer(typeof(CLSInquiryCorporateClientOutputModel), typeof(InquiryCRMPayeeListInputModel));
                    foreach (var clsData in retCLSInqCorpClient.data)
                    {
                        InquiryCRMPayeeListInputModel t = new InquiryCRMPayeeListInputModel();
                        t = (InquiryCRMPayeeListInputModel)transformer.TransformModel(clsData, t);
                        t.requester = listSAPSearchCondition[i].requester;
                        t.emcsCode = listSAPSearchCondition[i].emcsCode;
                        t.roleCode = listSAPSearchCondition[i].roleCode;
                        t.clientType = listSAPSearchCondition[i].clientType;
                        tmpListSAPSearchCondition.Add(t);

                      
                      
                    }
                    //tranform CLS to OUTPUT
                    var tranfromer = new Tranform_clsInqCorporateOut_to_crmInqPayeeOut();
                    crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)tranfromer.TransformModel(retCLSInqCorpClient, crmInqPayeeOut);

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
                    i++;
                }
            }

            return tmpListSAPSearchCondition;
        }

        public List<InquiryCRMPayeeListInputModel> InquiryMasterASHR(List<InquiryCRMPayeeListInputModel> listSAPSearchCondition, int iRecordsLimit,
            ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
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

        private List<InquiryCRMPayeeListInputModel> InquiryCLSPersonalClient(int iRecordsLimit, List<InquiryCRMPayeeListInputModel> listSAPSearchCondition, ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
            var tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
            var i = 0;
            while (i < listSAPSearchCondition.Count)
            {
                CLSInquiryPersonalClientInputModel clsPersonalInput = new CLSInquiryPersonalClientInputModel();
                clsPersonalInput = (CLSInquiryPersonalClientInputModel)TransformerFactory.TransformModel(listSAPSearchCondition[i], clsPersonalInput);

                //++ Call CLS_InquiryCLSPersonalClient through ServiceProxy
                CLSInquiryPersonalClientContentOutputModel retCLSInqPersClient = CallDevesServiceProxy<EWIResCLSInquiryPersonalClient, CLSInquiryPersonalClientContentOutputModel>
                    (CommonConstant.ewiEndpointKeyCLSInquiryPersonalClient, clsPersonalInput);

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

        public List<InquiryCRMPayeeListInputModel> InquiryAPARPayeeList(List<InquiryCRMPayeeListInputModel> listSAPSearchCondition, int iRecordsLimit, InquiryCRMPayeeListInputModel inqCrmPayeeListIn,
            ref CRMInquiryPayeeContentOutputModel crmInqPayeeOut)
        {
            int i;
            List<InquiryCRMPayeeListInputModel> tmpListSAPSearchCondition = new List<InquiryCRMPayeeListInputModel>();
            i = 0;
            while (i < listSAPSearchCondition.Count)
            {
                InquiryAPARPayeeListInputModel inqAPARIn =
                    (InquiryAPARPayeeListInputModel) DataModelFactory.GetModel(typeof(InquiryAPARPayeeListInputModel));
                inqAPARIn =
                    (InquiryAPARPayeeListInputModel) TransformerFactory.TransformModel(listSAPSearchCondition[i], inqAPARIn);
                InquiryAPARPayeeContentModel inqAPAROut = CallDevesServiceProxy<InquiryAPARPayeeOutputModel, InquiryAPARPayeeContentModel>(CommonConstant.ewiEndpointKeyAPARInquiryPayeeList, inqAPARIn);
               // InquiryAPARPayeeContentModel inqAPAROut =
                //    buzInquiryAPARPayeeListPayeeListService.Instance.Execute(inqAPARIn);
                if (inqAPAROut?.aparPayeeListCollection != null
                    && inqAPAROut.aparPayeeListCollection.Count > 0
                   )
                {
                    if (inqAPAROut.aparPayeeListCollection.Count + listSAPSearchCondition.Count +
                        tmpListSAPSearchCondition.Count > iRecordsLimit)
                    {
                        throw new TooManySearchResultsException(CommonConstant.CONST_SYSTEM_APAR, iRecordsLimit);
                    }
                    //if (inqAPAROut.aparPayeeListCollection.Count > 0)
                    //{
                    crmInqPayeeOut =
                        (CRMInquiryPayeeContentOutputModel) TransformerFactory.TransformModel(inqAPAROut, crmInqPayeeOut);
                    //}
                    BaseTransformer transformer =
                        TransformerFactory.GetTransformer(typeof(InquiryAPARPayeeContentAparPayeeListCollectionDataModel),
                            typeof(InquiryCRMPayeeListInputModel));
                    foreach (InquiryAPARPayeeContentAparPayeeListCollectionDataModel apar in inqAPAROut.aparPayeeListCollection)
                    {
                        //if ( && IsValidSAPPolisyClientId(inqCrmPayeeListIn.))
                       // {
                            InquiryCRMPayeeListInputModel t = new InquiryCRMPayeeListInputModel();
                            t = (InquiryCRMPayeeListInputModel)transformer.TransformModel(apar, t);
                            t.requester = inqCrmPayeeListIn.requester;
                            t.emcsCode = inqCrmPayeeListIn.emcsCode;
                            t.roleCode = inqCrmPayeeListIn.roleCode;
                            tmpListSAPSearchCondition.Add(t);
                        //}
                       
                    }

                    listSAPSearchCondition.RemoveAt(0);
                }
                else
                {
                    i++;
                }
            }
            return tmpListSAPSearchCondition;
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
            copy.clientType = model.clientType;
            copy.roleCode = model.roleCode;
            copy.polisyClientId = model.polisyClientId;
            copy.sapVendorCode = model.sapVendorCode;
            copy.fullname = model.fullname;
            copy.taxNo = model.taxNo;
            copy.taxBranchCode = model.taxBranchCode;
            copy.requester = model.requester;
            copy.emcsCode = model.emcsCode;

            return copy;
    }

}
}