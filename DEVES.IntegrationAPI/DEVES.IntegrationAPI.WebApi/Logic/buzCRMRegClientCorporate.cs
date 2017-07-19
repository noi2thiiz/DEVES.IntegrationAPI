using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using DEVES.IntegrationAPI.WebApi.Logic.Services;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientCorporate : BuzCommand
    {
        //จะใช้เก็บค่า CleansingId เอาไว้ เพื่อใช้ลบออกจาก Cleansing หากมี service ใดๆที่ทำงานไม่สำเร็จ
        protected string newCleansingId;
        protected string cleansingId;

        protected string polisyClientId;
        protected string crmClientId;

        public RegClientCorporateOutputModel_Fail regFail { get; set; } = new RegClientCorporateOutputModel_Fail();
        protected RegClientCorporateInputModel regClientCorporateInput { get; set; }

        public void TranFormInput()
        {
            if (regClientCorporateInput == null)
            {
                regClientCorporateInput = new RegClientCorporateInputModel();
            }
            if (regClientCorporateInput.addressHeader == null)
            {
                regClientCorporateInput.addressHeader = new AddressHeaderModel();
            }

            if (regClientCorporateInput.profileHeader == null)
            {
                regClientCorporateInput.profileHeader = new ProfileHeaderModel();
            }

            if (regClientCorporateInput.contactHeader == null)
            {
                regClientCorporateInput.contactHeader = new ContactHeaderModel();
            }

            if (regClientCorporateInput.generalHeader == null)
            {
                regClientCorporateInput.generalHeader = new GeneralHeaderModel();
            }

            // Validate Master Data before sending to other services
            var validator = new MasterDataValidator();




            //"profileHeader.countryOrigin"
            regClientCorporateInput.profileHeader.countryOrigin = validator.TryConvertNationalityCode(
                "profileHeader.countryOrigin",
                regClientCorporateInput?.profileHeader?.countryOrigin);

            //"addressHeader.country"
            regClientCorporateInput.addressHeader.country = validator.TryConvertCountryCode(
                "addressHeader.country",
                regClientCorporateInput?.addressHeader?.country);

            //"addressHeader.provinceCode"
            regClientCorporateInput.addressHeader.provinceCode = validator.TryConvertProvinceCode(
                "addressHeader.provinceCode",
                regClientCorporateInput?.addressHeader?.provinceCode,
                regClientCorporateInput?.addressHeader?.country);

            //"addressHeader.districtCode"
            regClientCorporateInput.addressHeader.districtCode = validator.TryConvertDistrictCode(
                    "addressInfo.districtCode",
                    regClientCorporateInput?.addressHeader?.districtCode,
                    regClientCorporateInput?.addressHeader?.provinceCode)
                ;

            //"addressHeader.subDistrictCode"
            regClientCorporateInput.addressHeader.subDistrictCode = validator.TryConvertSubDistrictCode(
                "addressInfo.subDistrictCode",
                regClientCorporateInput?.addressHeader?.subDistrictCode,
                regClientCorporateInput?.addressHeader?.districtCode,
                regClientCorporateInput?.addressHeader?.provinceCode
            );
            //"addressHeader.addressType"
            regClientCorporateInput.addressHeader.addressType = validator.TryConvertAddressTypeCode(
                "addressInfo.addressType",
                regClientCorporateInput?.addressHeader?.addressType
            );

            //"addressHeader.addressType"
            regClientCorporateInput.addressHeader.addressType = validator.TryConvertAddressTypeCode(
                "addressInfo.addressType",
                regClientCorporateInput?.addressHeader?.addressType
            );

            //"profileHeader.econActivity"
            regClientCorporateInput.profileHeader.econActivity = validator.TryConvertEconActivityCode(
                "profileHeader.econActivity",
                regClientCorporateInput?.profileHeader?.econActivity
            );

            switch (regClientCorporateInput.generalHeader.roleCode)
            {
                case "A":
                    regClientCorporateInput.generalHeader.assessorFlag = "Y";
                    break;
                case "S":
                    regClientCorporateInput.generalHeader.solicitorFlag = "Y";
                    break;
                case "R":
                    regClientCorporateInput.generalHeader.repairerFlag = "Y";
                    break;
                case "H":
                    regClientCorporateInput.generalHeader.hospitalFlag = "Y";
                    break;
            }


            if (validator.Invalid())
            {
                throw new FieldValidationException(validator.GetFieldErrorData());
            }


        }
        public override BaseDataModel ExecuteInput(object input)
        {
            RegClientCorporateContentOutputModel regClientCorporateOutput = new RegClientCorporateContentOutputModel();
            regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
            regClientCorporateOutput.transactionDateTime = DateTime.Now;
            regClientCorporateOutput.transactionId = TransactionId;
            regClientCorporateOutput.code = CONST_CODE_SUCCESS;


            regClientCorporateInput = (RegClientCorporateInputModel)input;
            TranFormInput();

            if (!string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
            {
                //ไม่ต้องไปสร้างใน 400 
                polisyClientId = regClientCorporateInput?.generalHeader?.polisyClientId;
                regClientCorporateInput.generalHeader.notCreatePolisyClientFlag = "Y";
            }

            if (regClientCorporateInput.generalHeader.roleCode == "G")
            {


                AddDebugInfo("regClientCorporateInput.generalHeader.roleCode = G");

                return createASRHCorporate(regClientCorporateInput);
                //return createGeneralCorporate(regClientCorporateInput);
            }
            else
            {
                AddDebugInfo("regClientCorporateInput.generalHeader.roleCode != G");

                return createASRHCorporate(regClientCorporateInput);
            }


            ///////////////////////////////////////////////////////////////////////////////////////////////////


        }

        protected BaseDataModel createGeneralCorporate(RegClientCorporateInputModel input)
        {

            //case PG-02
            // เคสนี้เป็นข้อมูลลูกค้าที่ถูกสร้างตรงๆ ที่<<Polisy400>> ซึ่งข้อมูลนี้จะมีเลข CleansingId ในวันต่อมา 
            // และแค่มีเลข polisyClientId ก็ตอบโจทย์ของ Locus ในการเปิดเคลม
            // ในอนาคตหากจะต้องสร้าง เลข CleansingId ในเคสนี้ ต้องมาคุย Flow กันอีกที
            if (string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.cleansingId) &&
                !string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
            {
                throw new BuzErrorException(
                    "400",
                    "The Service cannot process case:  'cleansingId' null and 'polisyClientId' not null",
                    "",
                    "CRM",
                    TransactionId);
            }
            //PG-05
            //เหตุการณ์นี้ไม่ควรส่งมาเพราะ มี Process การ Inquiry อยู่
            //(north)คิดว่าควรจะ error ตั้งแต่ Cleansing แล้ว เพราะมีข้อมูล dup อยู่ใน cls
            if (!string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.cleansingId) &&
                !string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
            {
                throw new BuzErrorException(
                    "400",
                    "The Service cannot process case:  'cleansingId' not null and 'polisyClientId' not null",
                    "",
                    "CRM",
                    TransactionId);
            }


            /////////////////////////////////PROCESS/////////////////////////////////////////////

            //1:Parameter CleansingId Does Not Contain Data
            if (string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.cleansingId))
            {
                //1.1:Parameter polisy Client Id Does Not Contain Data
                if (string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    //1.1.1:Create Client Cleansing
                    //ถ้า CLS สร้างไม่สำเร็จจะ  throw ไปเลย
                    cleansingId = CreateCorporateClientInCLS(regClientCorporateInput);


                    //1.1.2:Create PersonalClient And Additional Poliisy 400
                    // ยกเว้น notCreatePolisyClientFlag =Y ไม่ต้องสร้าง
                    if (regClientCorporateInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
                    {
                        try
                        {
                            polisyClientId =
                                CreateCorporateClientAndAdditionalInfoInPolisy400(regClientCorporateInput, cleansingId);
                        }
                        catch (Exception)
                        {
                            //ถ้าสร้างไม่สำเร็จจะลบข้อมูลใน  CLS ออก
                            //ระวังอย่างแก้โค้ดจนเอา CleansingId ที่ไม่ได้สร้างใหม่มาลบ
                            CleansingClientService.Instance.RemoveByCleansingId(cleansingId, "C");



                            throw;
                        }
                    }
                }
            }


            //2: Parameter cleansing id Contain Data
            // สร้างเฉพาะใน 400 
            // กรณีนี้  ไม่ได้สร้าง CleansingId ใหม่จะไม่มีการ  rollback ถ้าสร้างใน  Polisy400 ไม่สำเร็จ

            else if (false == string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.cleansingId))
            {
                cleansingId = regClientCorporateInput?.generalHeader?.cleansingId;
                //2.1:Create PersonalClient And Additional Poliisy 400 
                // ยกเว้น notCreatePolisyClientFlag =Y ไม่ต้องสร้าง
                if (regClientCorporateInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
                {
                    CreateCorporateClientAndAdditionalInfoInPolisy400(regClientCorporateInput,
                        cleansingId);
                }
            }



            //3 create crm CleinInfo in CRM เพื่อเก็บ cleansingId   แต่ให้ค้นก่อนถ้าพบจะไม่สร้างซ้ำ
            if (!string.IsNullOrEmpty(cleansingId))
            {
                if (false == SpApiChkCustomerClient.Instance.CheckByCleansingId(cleansingId))
                {

                    crmClientId = CreateClientInCRM(regClientCorporateInput, cleansingId, polisyClientId);
                }

            }



            // return output
            return new RegClientCorporateContentOutputModel
            {
                transactionDateTime = DateTime.Now,
                transactionId = TransactionId,
                code = AppConst.CODE_SUCCESS,
                message = AppConst.MESSAGE_SUCCESS,
                description = "",
                data = new List<RegClientCorporateDataOutputModel>
                {
                    new RegClientCorporateDataOutputModel_Pass
                    {
                        cleansingId = cleansingId,
                        polisyClientId = polisyClientId,
                        crmClientId = crmClientId,
                        corporateName1= regClientCorporateInput?.profileHeader?.corporateName1 ?? "",
                        corporateName2 = regClientCorporateInput?.profileHeader?.corporateName2 ?? "",
                        corporateBranch = regClientCorporateInput?.profileHeader?.corporateBranch ?? ""
                    }
                }
            };

            // return createASRHCorporate(input);
        }

        private string CreateCorporateClientInCLS(RegClientCorporateInputModel regClientCorporateInputModel)
        {
            throw new NotImplementedException();
        }



        public string CreateClientInCRM(RegClientCorporateInputModel regClientPersonalInput, string cleansingId,
            string polisyClientId)
        {
            if (string.IsNullOrEmpty(cleansingId))
            {
                return null;
            }
            //RegClientPersonalInputModel regClientPersonalInput = ObjectCopier.Clone(input);
            regClientPersonalInput.generalHeader.cleansingId = cleansingId;
            regClientPersonalInput.generalHeader.polisyClientId = polisyClientId;
            Console.WriteLine(regClientPersonalInput.ToJson());
            try
            {
                buzCreateCrmClientCorporate cmdCreateCrmClient = new buzCreateCrmClientCorporate();
                CreateCrmCorporateInfoOutputModel crmContentOutput =
                    (CreateCrmCorporateInfoOutputModel)cmdCreateCrmClient.Execute(regClientPersonalInput);

                if (crmContentOutput.code != AppConst.CODE_SUCCESS)
                {
                    throw new BuzErrorException(
                        "500",
                        "CRM Error:" + crmContentOutput.message,
                        crmContentOutput.description,
                        "CRM",
                        TransactionId);
                    //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
                }

                return crmContentOutput.crmClientId;
            }
            catch (BuzErrorException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BuzErrorException(
                    "500",
                    "CRM Error:" + e.Message,
                    e.StackTrace,
                    "CRM",
                    TransactionId);
            }
        }

        private string CreateCorporateClientAndAdditionalInfoInPolisy400(RegClientCorporateInputModel p0, string p1)
        {
            throw new NotImplementedException();
        }

        protected BaseDataModel createASRHCorporate(RegClientCorporateInputModel input)
        {
            RegClientCorporateContentOutputModel regClientCorporateOutput = new RegClientCorporateContentOutputModel();
            regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
            regClientCorporateOutput.transactionDateTime = DateTime.Now;
            regClientCorporateOutput.transactionId = TransactionId;
            regClientCorporateOutput.code = CONST_CODE_SUCCESS;

            RegClientCorporateDataOutputModel_Pass dataOutPass =
                new RegClientCorporateDataOutputModel_Pass();

            if (IsASRHValid(regClientCorporateInput))
            {
                // create CLS if cleansingId and polisyClientId is null
                if (string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId) && string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    AddDebugInfo("create CLS");
                    Console.WriteLine("create CLS");
                    BaseDataModel clsCreateCorporateIn = DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                    clsCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, clsCreateCorporateIn);
                    CLSCreateCorporateClientContentOutputModel clsCreateClientContent = CallDevesServiceProxy<CLSCreateCorporateClientOutputModel, CLSCreateCorporateClientContentOutputModel>
                        (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);

                    if (clsCreateClientContent.code == CommonConstant.CODE_SUCCESS)
                    {
                        //TODO เอา ค่าที่ได้ไปเป็น output
                        newCleansingId = clsCreateClientContent.data.cleansingId;
                        AddDebugInfo("create CLS newCleansingId = " + newCleansingId);


                        //กรณีมาจากหน้าจอ CRM  notCreatePolisyClientFlag == Y ไม่ต้องไปสร้างใน polisyClientId
                        if (regClientCorporateInput.generalHeader.notCreatePolisyClientFlag != "Y")
                        {
                            AddDebugInfo(
                                @"string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId)&& !string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId) ");

                            BaseDataModel polCreateCorporateIn =
                                DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                            polCreateCorporateIn =
                                TransformerFactory.TransformModel(regClientCorporateInput, polCreateCorporateIn);
                            CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent =
                                CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                        , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                    (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                            regClientCorporateInput =
                                (RegClientCorporateInputModel)TransformerFactory.TransformModel(polCreateClientContent,
                                    regClientCorporateInput);
                        }

                    }

                    else
                    {
                        AddDebugInfo("create CLS error  = " + clsCreateClientContent.message, clsCreateClientContent);
                        throw new BuzErrorException(
                            clsCreateClientContent.code,
                            $"CLS Error:{clsCreateClientContent.message}",
                            "An error occurred from the external service (CLSCreateCorporateClient)",

                            "CLS",
                            TransactionId);
                        //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
                    }
                    regClientCorporateInput = (RegClientCorporateInputModel)TransformerFactory.TransformModel(clsCreateClientContent, regClientCorporateInput);
                }

                // กรณีไม่มี เลข polisyClientId  แต่มี cleansingId ให้สร้างเฉพาะ 400
                else if (string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    //กรณีมาจากหน้าจอ CRM  notCreatePolisyClientFlag == Y ไม่ต้องไปสร้างใน polisyClientId
                    if (regClientCorporateInput.generalHeader.notCreatePolisyClientFlag != "Y")
                    {
                        AddDebugInfo(
                            @"string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId)&& !string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId) ");

                        BaseDataModel polCreateCorporateIn =
                            DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                        polCreateCorporateIn =
                            TransformerFactory.TransformModel(regClientCorporateInput, polCreateCorporateIn);
                        CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent =
                            CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                    , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                        regClientCorporateInput =
                            (RegClientCorporateInputModel)TransformerFactory.TransformModel(polCreateClientContent,
                                regClientCorporateInput);
                    }
                }

                //กรณี มี polisyClientId ให้ update
                else if (!string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
                {

                    AddDebugInfo(" to update ");
                    try
                    {
                        COMPInquiryClientMasterInputModel compInqClientInput =
                            new COMPInquiryClientMasterInputModel();
                        compInqClientInput =
                            (COMPInquiryClientMasterInputModel)TransformerFactory.TransformModel(
                                regClientCorporateInput, compInqClientInput);

                        EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient =
                            CallDevesServiceProxy<COMPInquiryClientMasterOutputModel,
                                    EWIResCOMPInquiryClientMasterContentModel>
                                (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);
                        //Found in Polisy400
                        if (retCOMPInqClient != null)
                        {

                            COMPInquiryClientMasterContentClientListModel inqClientPolisy400Out =
                                retCOMPInqClient.clientListCollection.First();

                            CLIENTUpdateCorporateClientAndAdditionalInfoInputModel
                                updateClientPolisy400In =
                                    (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                                    DataModelFactory
                                        .GetModel(
                                            typeof(
                                                CLIENTUpdateCorporateClientAndAdditionalInfoInputModel
                                            ));

                            if (true == retCOMPInqClient?.clientListCollection.Any())
                            {

                                updateClientPolisy400In =
                                    (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                                    TransformerFactory.TransformModel(inqClientPolisy400Out,
                                        updateClientPolisy400In);
                                updateClientPolisy400In.checkFlag = "UPDATE";
                            }
                            else
                            {

                                updateClientPolisy400In =
                                    (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                                    TransformerFactory.TransformModel(regClientCorporateInput,
                                        updateClientPolisy400In);
                                updateClientPolisy400In.checkFlag = "CREATE";
                            }
                            if (regClientCorporateInput?.generalHeader?.assessorFlag == "Y")
                            {
                                updateClientPolisy400In.assessorFlag = "Y";
                                updateClientPolisy400In.assessorBlackListFlag = regClientCorporateInput.asrhHeader.assessorBlackListFlag;
                                updateClientPolisy400In.assessorDelistFlag = regClientCorporateInput.asrhHeader.assessorDelistFlag;
                                updateClientPolisy400In.assessorOregNum = regClientCorporateInput.asrhHeader.assessorOregNum;
                                updateClientPolisy400In.assessorTerminateDate = regClientCorporateInput.asrhHeader.assessorTerminateDate; ;
                            }

                            if (regClientCorporateInput?.generalHeader?.solicitorFlag == "Y")
                            {
                                updateClientPolisy400In.solicitorFlag = "Y";
                                updateClientPolisy400In.solicitorBlackListFlag = regClientCorporateInput.asrhHeader.solicitorBlackListFlag;
                                updateClientPolisy400In.solicitorDelistFlag = regClientCorporateInput.asrhHeader.solicitorDelistFlag;
                                updateClientPolisy400In.solicitorOregNum = regClientCorporateInput.asrhHeader.solicitorOregNum;
                                updateClientPolisy400In.solicitorTerminateDate = regClientCorporateInput.asrhHeader.solicitorTerminateDate; ;

                            }

                            if (regClientCorporateInput?.generalHeader?.repairerFlag == "Y")
                            {
                                updateClientPolisy400In.repairerFlag = "Y";
                                updateClientPolisy400In.repairerBlackListFlag = regClientCorporateInput.asrhHeader.repairerBlackListFlag;
                                updateClientPolisy400In.repairerDelistFlag = regClientCorporateInput.asrhHeader.repairerDelistFlag;
                                updateClientPolisy400In.repairerOregNum = regClientCorporateInput.asrhHeader.repairerOregNum;
                                updateClientPolisy400In.repairerTerminateDate = regClientCorporateInput.asrhHeader.repairerTerminateDate; ;

                            }

                            if (regClientCorporateInput?.generalHeader?.hospitalFlag == "Y")
                            {
                                updateClientPolisy400In.hospitalFlag = "Y";

                            }

                            CallDevesServiceProxy<
                                CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel,
                                CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(
                                CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient,
                                updateClientPolisy400In);


                        }
                        else
                        {
                            regClientCorporateOutput.code = CONST_CODE_FAILED;
                            regClientCorporateOutput.message = "Your Policy Client  Id  cannot be found";

                        }
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("400 Error" + e.Message);

                        if (!string.IsNullOrEmpty(newCleansingId))
                        {
                            //เมื่อเกิด error ใด ๆ ใน service อื่นให้ลบ

                            AddDebugInfo("try rollback" + newCleansingId);

                            var deleteResult =
                                CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "C");

                            regClientCorporateOutput.code = CONST_CODE_FAILED;
                            regClientCorporateOutput.message = e.Message;
                            if (!deleteResult.success)
                            {
                                AddDebugInfo(
                                    "Failed to complete the transaction, and it does not rollback");
                                regClientCorporateOutput.description =
                                    "Failed to complete the transaction, and it does not rollback";

                            }
                            else
                            {

                                regClientCorporateOutput.description = "";
                            }
                        }


                        regClientCorporateOutput.code = CONST_CODE_FAILED;
                        regClientCorporateOutput.message = e.Message;

                    }



                }
                else
                {
                    AddDebugInfo(" not Create PolisyClient ");
                }



            }
            else
            {
                AddDebugInfo("There is some conflicts among the arguments Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}");
                regClientCorporateOutput.code = AppConst.CODE_INVALID_INPUT;
                regClientCorporateOutput.message = "There is some conflicts among the arguments Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}";
                regClientCorporateOutput.description = "";
            }

            //3 create crm CleinInfo in CRM เพื่อเก็บ cleansingId   แต่ให้ค้นก่อนถ้าพบจะไม่สร้างซ้ำ
            polisyClientId = regClientCorporateInput?.generalHeader?.polisyClientId ?? "";
            cleansingId = newCleansingId ?? regClientCorporateInput?.generalHeader?.cleansingId;
            Console.WriteLine(polisyClientId + ":" + cleansingId);
            if (!string.IsNullOrEmpty(cleansingId))
            {
                if (false == SpApiChkCustomerClient.Instance.CheckByCleansingId(cleansingId))
                {
                    try
                    {
                        crmClientId = CreateClientInCRM(regClientCorporateInput, cleansingId, polisyClientId);
                    }
                    catch (Exception e)
                    {
                        AddDebugInfo(e.Message, "CRM Error");
                        throw;
                    }

                }

            }

            // All Error
            // แก้ตาม ที่ อาจารย์พรชัย บอก เพื่อให้เขาเอา เลข cleansingIdไปซ่อมข้อมูลได้
            if (regClientCorporateOutput.code != AppConst.CODE_SUCCESS)
            {



                if (string.IsNullOrEmpty(regClientCorporateOutput.message))
                {
                    regClientCorporateOutput.message = "Failed client registration did not complete";
                }
                if (regClientCorporateOutput?.data == null || regClientCorporateOutput?.data.Count == 0)
                {
                    regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
                    dataOutPass =
                        new RegClientCorporateDataOutputModel_Pass
                        {
                            cleansingId = regClientCorporateInput?.generalHeader?.cleansingId ?? "",
                            polisyClientId = regClientCorporateInput?.generalHeader?.polisyClientId ?? "",
                            corporateName1 = regClientCorporateInput?.profileHeader?.corporateName1 ?? "",
                            corporateName2 = regClientCorporateInput?.profileHeader?.corporateName2 ?? "",
                            corporateBranch = regClientCorporateInput?.profileHeader?.corporateBranch ?? ""
                        };



                    regClientCorporateOutput.data.Add(dataOutPass);
                }
                //regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
                // regClientCorporateOutput.data.Add(regClientPersonDataOutput);
            }
            else
            {
                regClientCorporateOutput.code = CONST_CODE_SUCCESS;
                regClientCorporateOutput.message = AppConst.MESSAGE_SUCCESS;

                dataOutPass.cleansingId = regClientCorporateInput.generalHeader.cleansingId;
                dataOutPass.polisyClientId = regClientCorporateInput.generalHeader.polisyClientId;

                dataOutPass.corporateName1 = regClientCorporateInput.profileHeader.corporateName1;
                dataOutPass.corporateName2 = regClientCorporateInput.profileHeader.corporateName2;
                dataOutPass.corporateBranch = regClientCorporateInput.profileHeader.corporateBranch;

                regClientCorporateOutput.data.Add(dataOutPass);
            }

            return regClientCorporateOutput;
        }

        private bool IsShouldToUpdate(RegClientCorporateInputModel regClientCorporateInput)
        {
            if (!string.IsNullOrEmpty(regClientCorporateInput?.generalHeader?.polisyClientId))
            {
                AddDebugInfo("polisyClientId != null");

                if ((regClientCorporateInput?.generalHeader?.assessorFlag == "Y"
                     || regClientCorporateInput?.generalHeader?.solicitorFlag == "Y"
                     || regClientCorporateInput?.generalHeader?.repairerFlag == "Y"
                     || regClientCorporateInput?.generalHeader?.hospitalFlag == "Y"))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsASRHValid(RegClientCorporateInputModel input)
        {
            bool validFlag = true;
            if (

                (input.generalHeader.roleCode == "A" && input.generalHeader.assessorFlag != "Y") ||
                (input.generalHeader.roleCode == "S" && input.generalHeader.solicitorFlag != "Y") ||
                (input.generalHeader.roleCode == "R" && input.generalHeader.repairerFlag != "Y") ||
                (input.generalHeader.roleCode == "H" && input.generalHeader.hospitalFlag != "Y")
            )
            {

                validFlag = false;
            }
            return validFlag;
        }
    }
}