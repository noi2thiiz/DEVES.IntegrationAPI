using System;
using System.Collections.Generic;
using System.Linq;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientCorporate;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Commands.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class BuzCrmRegClientCorporate : BuzCommand
    {
        /// <summary>
        /// ใช้เก็บค่า  CleansingId กรณีสร้างใหม่
        /// </summary>
        protected string NewCleansingId;
        /// <summary>
        /// 
        /// </summary>
        protected string CleansingId;
        /// <summary>
        /// 
        /// </summary>
        protected string PolisyClientId;
        /// <summary>
        /// 
        /// </summary>
        protected string CrmClientId;


        /// <summary>
        /// 
        /// </summary>
        protected RegClientCorporateInputModel RegClientCorporateInput { get; set; }

        /// <summary>
        /// จัดการกับ nput ก่อนส่งเข้า process
        /// </summary>
        /// <exception cref="FieldValidationException"></exception>
        public void TranFormInput()
        {
            if (RegClientCorporateInput == null)
            {
                RegClientCorporateInput = new RegClientCorporateInputModel();
            }
            if (RegClientCorporateInput.addressHeader == null)
            {
                RegClientCorporateInput.addressHeader = new AddressHeaderModel();
            }

            if (RegClientCorporateInput.profileHeader == null)
            {
                RegClientCorporateInput.profileHeader = new ProfileHeaderModel();
            }

            if (RegClientCorporateInput.contactHeader == null)
            {
                RegClientCorporateInput.contactHeader = new ContactHeaderModel();
            }

            if (RegClientCorporateInput.generalHeader == null)
            {
                RegClientCorporateInput.generalHeader = new GeneralHeaderModel();
            }

            // Validate Master Data before sending to other services
            var validator = new MasterDataValidator();


            //"profileHeader.countryOrigin"
            RegClientCorporateInput.profileHeader.countryOrigin = validator.TryConvertNationalityCode(
                "profileHeader.countryOrigin",
                RegClientCorporateInput?.profileHeader?.countryOrigin);

            //"addressHeader.country"
            RegClientCorporateInput.addressHeader.country = validator.TryConvertCountryCode(
                "addressHeader.country",
                RegClientCorporateInput?.addressHeader?.country);

            //"addressHeader.provinceCode"
            RegClientCorporateInput.addressHeader.provinceCode = validator.TryConvertProvinceCode(
                "addressHeader.provinceCode",
                RegClientCorporateInput?.addressHeader?.provinceCode,
                RegClientCorporateInput?.addressHeader?.country);

            //"addressHeader.districtCode"
            RegClientCorporateInput.addressHeader.districtCode = validator.TryConvertDistrictCode(
                    "addressInfo.districtCode",
                    RegClientCorporateInput?.addressHeader?.districtCode,
                    RegClientCorporateInput?.addressHeader?.provinceCode)
                ;

            //"addressHeader.subDistrictCode"
            RegClientCorporateInput.addressHeader.subDistrictCode = validator.TryConvertSubDistrictCode(
                "addressInfo.subDistrictCode",
                RegClientCorporateInput?.addressHeader?.subDistrictCode,
                RegClientCorporateInput?.addressHeader?.districtCode,
                RegClientCorporateInput?.addressHeader?.provinceCode
            );
            //"addressHeader.addressType"
            RegClientCorporateInput.addressHeader.addressType = validator.TryConvertAddressTypeCode(
                "addressInfo.addressType",
                RegClientCorporateInput?.addressHeader?.addressType
            );


            //"profileHeader.econActivity"
            RegClientCorporateInput.profileHeader.econActivity = validator.TryConvertEconActivityCode(
                "profileHeader.econActivity",
                RegClientCorporateInput?.profileHeader?.econActivity
            );

            switch (RegClientCorporateInput.generalHeader.roleCode)
            {
                case "A":
                    RegClientCorporateInput.generalHeader.assessorFlag = "Y";
                    break;
                case "S":
                    RegClientCorporateInput.generalHeader.solicitorFlag = "Y";
                    break;
                case "R":
                    RegClientCorporateInput.generalHeader.repairerFlag = "Y";
                    break;
                case "H":
                    RegClientCorporateInput.generalHeader.hospitalFlag = "Y";
                    break;
            }


            if (validator.Invalid())
            {
                throw new FieldValidationException(validator.GetFieldErrorData());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="BuzErrorException"></exception>
        public override BaseDataModel ExecuteInput(object input)
        {
            RegClientCorporateContentOutputModel regClientCorporateOutput = new RegClientCorporateContentOutputModel();
            regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
            regClientCorporateOutput.transactionDateTime = DateTime.Now;
            regClientCorporateOutput.transactionId = TransactionId;
            regClientCorporateOutput.code = CONST_CODE_SUCCESS;


            RegClientCorporateInput = (RegClientCorporateInputModel) input;
            TranFormInput();


            


            if (!IsASRHValid(RegClientCorporateInput))
            {
                // จริงๆ error นี้จะไม่ควรเกิดขึ้น แต่เขียนดักไว้ก่อน
                AddDebugInfo(
                    "There is some conflicts among the arguments Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}");
               
                regClientCorporateOutput.description = "";
                throw  new BuzErrorException(AppConst.CODE_INVALID_INPUT, "There is some conflicts among the arguments Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}","");
            }

            // ตัวแปลลับ เพื่อบังคับให้สร้าง client ใน CLS เท่านั้น
                //if (regClientCorporateInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
                //{
                //    cleansingId = CreateCorporateClientInCLS(regClientCorporateInput);

                //}

            if (!string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
            {
                //ไม่ต้องไปสร้างใน 400 
                PolisyClientId = RegClientCorporateInput?.generalHeader?.polisyClientId;
                //RegClientCorporateInput.generalHeader.notCreatePolisyClientFlag = "Y";
            }

            if (RegClientCorporateInput?.generalHeader?.roleCode == "G")
            {
                AddDebugInfo("roleCode = G");

                //case PG-02
                // เคสนี้เป็นข้อมูลลูกค้าที่ถูกสร้างตรงๆ ที่<<Polisy400>> ซึ่งข้อมูลนี้จะมีเลข CleansingId ในวันต่อมา 
                // และแค่มีเลข polisyClientId ก็ตอบโจทย์ของ Locus ในการเปิดเคลม
                // ในอนาคตหากจะต้องสร้าง เลข CleansingId ในเคสนี้ ต้องมาคุย Flow กันอีกที
                if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.cleansingId) &&
                    !string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
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
                if (!string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.cleansingId) &&
                    !string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    throw new BuzErrorException(
                        "400",
                        "The Service cannot process case:  'cleansingId' not null and 'polisyClientId' not null",
                        "",
                        "CRM",
                        TransactionId);
                }

                // return CreateCorporateAsrhAndG(regClientCorporateInput);
                return CreateGeneralCorporate(RegClientCorporateInput);
            }
            else
            {
                AddDebugInfo("roleCode IN A S R H");

                //return createGASRHCorporate(RegClientCorporateInput);
                return CreateAsrhCorporate(RegClientCorporateInput);
            }


            ///////////////////////////////////////////////////////////////////////////////////////////////////
        }

        /// <summary>
        /// สร้าง Client ที่ Role = G
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected BaseDataModel CreateGeneralCorporate(RegClientCorporateInputModel input)
        {
            /////////////////////////////////PROCESS/////////////////////////////////////////////

            //1:Parameter CleansingId Does Not Contain Data
            if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.cleansingId))
            {
                //1.1:Parameter polisy Client Id Does Not Contain Data
                if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    //1.1.1:Create Client Cleansing
                    //ถ้า CLS สร้างไม่สำเร็จจะ  throw ไปเลย
                    CleansingId = CreateCorporateClientInCls(RegClientCorporateInput);


                    //1.1.2:Create PersonalClient And Additional Poliisy 400
                    // ยกเว้น notCreatePolisyClientFlag =Y ไม่ต้องสร้าง
                    if (RegClientCorporateInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
                    {
                        try
                        {
                            PolisyClientId =
                                CreateCorporateClientAndAdditionalInfoInPolisy400(RegClientCorporateInput, CleansingId);
                        }
                        catch (Exception)
                        {
                            //ถ้าสร้างไม่สำเร็จจะลบข้อมูลใน  CLS ออก
                            //ระวังอย่างแก้โค้ดจนเอา CleansingId ที่ไม่ได้สร้างใหม่มาลบ
                            var delService = new CleansingClientService(TransactionId, ControllerName);
                            delService.RemoveByCleansingId(CleansingId, "C");
                            throw;
                        }
                    }
                }
            }

            //2: Parameter cleansing id Contain Data
            // สร้างเฉพาะใน 400 
            // กรณีนี้  ไม่ได้สร้าง CleansingId ใหม่จะไม่มีการ  rollback ถ้าสร้างใน  Polisy400 ไม่สำเร็จ

            else if (false == string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.cleansingId))
            {
                CleansingId = RegClientCorporateInput?.generalHeader?.cleansingId;
                //2.1:Create PersonalClient And Additional Poliisy 400 
                // ยกเว้น notCreatePolisyClientFlag =Y ไม่ต้องสร้าง
                if (RegClientCorporateInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
                {
                    try
                    {
                        PolisyClientId =
                            CreateCorporateClientAndAdditionalInfoInPolisy400(RegClientCorporateInput, CleansingId);
                    }
                    catch (Exception)
                    {
                        //ถ้าสร้างไม่สำเร็จจะลบข้อมูลใน  CLS ออก
                        //ระวังอย่างแก้โค้ดจนเอา CleansingId ที่ไม่ได้สร้างใหม่มาลบ
                        var delService = new CleansingClientService(TransactionId, ControllerName);
                        delService.RemoveByCleansingId(CleansingId, "C");
                        throw;
                    }
                }
            }

            AddDebugInfo("cleansingId : " + CleansingId);

            //3 create crm CleinInfo in CRM เพื่อเก็บ cleansingId   แต่ให้ค้นก่อนถ้าพบจะไม่สร้างซ้ำ
            if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.crmClientId))
            {
                try
                {
                    if (!string.IsNullOrEmpty(CleansingId))
                    {
                        if (false == SpApiChkCustomerClient.Instance.CheckByCleansingId(CleansingId))
                        {
                            AddDebugInfo("CheckByCleansingId == false  ");
                            CrmClientId = CreateClientInCrm(RegClientCorporateInput, CleansingId, PolisyClientId);
                        }
                    }
                }
                catch (Exception e)
                {
                    AddDebugInfo("Cannot create Client in CRM  : " + e.Message, e.StackTrace);
                }
            }
            else
            {
                CrmClientId = RegClientCorporateInput?.generalHeader?.crmClientId;
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
                        cleansingId = CleansingId,
                        polisyClientId = PolisyClientId,
                        crmClientId = CrmClientId,
                        corporateName1 = RegClientCorporateInput?.profileHeader?.corporateName1 ?? "",
                        corporateName2 = RegClientCorporateInput?.profileHeader?.corporateName2 ?? "",
                        corporateBranch = RegClientCorporateInput?.profileHeader?.corporateBranch ?? ""
                    }
                }
            };

            // return createASRHCorporate(input);
        }

        private string CreateCorporateClientInCls(RegClientCorporateInputModel regClientCorporateInputModel)
        {
            if (!string.IsNullOrEmpty(regClientCorporateInputModel?.generalHeader?.cleansingId))
            {
                return null;
            }


            BaseDataModel clsCreateCorporateIn = DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
            clsCreateCorporateIn =
                TransformerFactory.TransformModel(regClientCorporateInputModel, clsCreateCorporateIn);

            // var clsCreateClientContent =
            //     CallDevesServiceProxy<CLSCreatePersonalClientOutputModel,
            //             CLSCreatePersonalClientContentOutputModel>
            //         (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonIn);
            var clsService = new CLSCreateCorporateClientService(TransactionId, ControllerName);
            var clsCreateClientContent = clsService.Execute((CLSCreateCorporateClientInputModel) clsCreateCorporateIn);

            if (clsCreateClientContent.code != CONST_CODE_SUCCESS)
            {
                throw new BuzErrorException(
                    clsCreateClientContent.code ?? AppConst.CODE_FAILED,
                    $"CLS:{clsCreateClientContent.message}",
                    clsCreateClientContent.description,
                    "CLS",
                    TransactionId);
                //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
            }

            if (string.IsNullOrEmpty(clsCreateClientContent?.data?.cleansingId))
            {
                throw new BuzErrorException(
                    "500",
                    $"CLS Error: The service return success but not return cleansingId",
                    "An error occurred from the external service (CLSCreateCorporateClient)",
                    "CLS",
                    TransactionId);
                //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
            }


            //success
            return clsCreateClientContent?.data?.cleansingId;
        }

        private string CreateCorporateClientAndAdditionalInfoInPolisy400(
            RegClientCorporateInputModel regClientCorporateInput, string cleansingId)
        {
            if (string.IsNullOrEmpty(cleansingId))
            {
                return null;
            }
            //RegClientPersonalInputModel RegClientPersonalInput = ObjectCopier.Clone(input);
            regClientCorporateInput.generalHeader.cleansingId = cleansingId;

            Console.WriteLine("Create:CLIENTCreateCorporateClientAndAdditionalInfo");

            BaseDataModel polCreateCorporateIn =
                DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
            polCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, polCreateCorporateIn);
            // polCreateClientContent =
            //     CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
            //             , CLIENTCreatePersonalClientAndAdditionalInfoContentModel>
            //         (CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient, polCreatePersonIn);
            var clientService = new CLIENTCreateCorporateClientAndAdditionalInfoService(TransactionId, ControllerName);
            CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent =
                clientService.Execute((CLIENTCreateCorporateClientAndAdditionalInfoInputModel) polCreateCorporateIn);

            if (string.IsNullOrEmpty(polCreateClientContent?.clientID))
            {
                throw new BuzErrorException(
                    "500",
                    "Polisy400 Error :Cannot create Client in Polisy400",
                    "",
                    "Polisy400",
                    TransactionId);
                //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
            }


            return polCreateClientContent?.clientID;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected BaseDataModel CreateAsrhCorporate(RegClientCorporateInputModel input)
        {
            string newCleansingId = "";
            Console.WriteLine("CreateAsrhCorporate");

            //parameter.polisyClientId does not contain data
            // ถ้าไม่ได้ระบุ polisyClientId แสดงว่าต้อง create
            if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
            {
              
                //parameter.cleansingId does not contain data
                // กรณีที่ไม่มีเลข polisyClientId แสดงว่าต้องการสร้าง client แต่ก่อนสร้างต้องไปสร้าง cleansing ก่อนเพื่อเอาเลข cleansingId
                // ในกรณีที่มีการสร้างเลข cleansingId ขึ้นมาใหม่ หากเกิด error  ต้องลบออกด้วย
                if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.cleansingId))
                {
                    Console.WriteLine("CreateCorporateClientInCls");
                    newCleansingId  = CreateCorporateClientInCls(RegClientCorporateInput);
                    CleansingId = newCleansingId;
                }
                else
                {
                    CleansingId = RegClientCorporateInput?.generalHeader?.cleansingId;
                }


                try
                {
                    Console.WriteLine("CreateCorporateClientAndAdditionalInfoInPolisy400");
                    PolisyClientId =CreateCorporateClientAndAdditionalInfoInPolisy400(RegClientCorporateInput, CleansingId);
                }
                catch (Exception)
                {
                    //ถ้าสร้างไม่สำเร็จจะลบข้อมูลใน  CLS ออก
                    //ระวังอย่างแก้โค้ดจนเอา CleansingId ที่ไม่ได้สร้างใหม่มาลบ
                    if (!string.IsNullOrEmpty(newCleansingId))
                    {
                        var delService = new CleansingClientService(TransactionId, ControllerName);
                        delService.RemoveByCleansingId(newCleansingId, "C");
                    }
                    
                    throw;
                }
            }
            //parameter.polisyClientId  contain data
            // ถ้าระบุ polisyClientId มาด้วยให้ Update   //แล้วเป็น A S R H ด้วย จะต้อง Update ? 
            //parameter A S R H FLAG does not contain data เงื่อนไขนี้ยังไม่เข้าใจอยู่ดี
            else
            {
               
                try
                {
                    Console.WriteLine("UpdateCorporateClientAndAdditionalInfoInPolisy400");
                    UpdateCorporateClientAndAdditionalInfoInPolisy400(RegClientCorporateInput);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    //ถ้าสร้างไม่สำเร็จจะลบข้อมูลใน  CLS ออก
                    //ระวังอย่างแก้โค้ดจนเอา CleansingId ที่ไม่ได้สร้างใหม่มาลบ
                   // var delService = new CleansingClientService(TransactionId, ControllerName);
                    //delService.RemoveByCleansingId(cleansingId, "C");
                    throw;
                }

            }

            AddDebugInfo("cleansingId : " + CleansingId);

            //3 create crm CleinInfo in CRM เพื่อเก็บ cleansingId   แต่ให้ค้นก่อนถ้าพบจะไม่สร้างซ้ำ
            if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.crmClientId))
            {
                try
                {
                    if (!string.IsNullOrEmpty(CleansingId))
                    {
                        if (false == SpApiChkCustomerClient.Instance.CheckByCleansingId(CleansingId))
                        {
                            AddDebugInfo("CheckByCleansingId == false  ");
                            CrmClientId = CreateClientInCrm(RegClientCorporateInput, CleansingId, PolisyClientId);
                        }
                    }
                }
                catch (Exception e)
                {
                    AddDebugInfo("Cannot create Client in CRM  : " + e.Message, e.StackTrace);
                }
            }
            else
            {
                CrmClientId = RegClientCorporateInput?.generalHeader?.crmClientId;
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
                        cleansingId = CleansingId,
                        polisyClientId = PolisyClientId,
                        crmClientId = CrmClientId,
                        corporateName1 = RegClientCorporateInput?.profileHeader?.corporateName1 ?? "",
                        corporateName2 = RegClientCorporateInput?.profileHeader?.corporateName2 ?? "",
                        corporateBranch = RegClientCorporateInput?.profileHeader?.corporateBranch ?? ""
                    }
                }
            };
        }

        private void UpdateCorporateClientAndAdditionalInfoInPolisy400(RegClientCorporateInputModel regClientCorporateInput)
        {
            if (RegClientCorporateInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
            {
                AddDebugInfo(" to update ");

                COMPInquiryClientMasterInputModel compInqClientInput = new COMPInquiryClientMasterInputModel();
                compInqClientInput =
                    (COMPInquiryClientMasterInputModel) TransformerFactory.TransformModel(regClientCorporateInput,
                        compInqClientInput);



                var polService = new COMPInquiryClientMasterService(TransactionId, ControllerName);
                EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = polService.Execute((COMPInquiryClientMasterInputModel) compInqClientInput);

               
                Console.WriteLine("retCOMPInqClient");
                Console.WriteLine(retCOMPInqClient?.ToJson());
                if (retCOMPInqClient != null && retCOMPInqClient.clientListCollection.Any() )
                {  // success InquiryClientMaster

                    COMPInquiryClientMasterContentClientListModel inqClientPolisy400Out =retCOMPInqClient?.clientListCollection?.FirstOrDefault();
                    Console.WriteLine("inqClientPolisy400Out.ToJson()");
                    Console.WriteLine(inqClientPolisy400Out?.ToJson());

                    


                    CLIENTUpdateCorporateClientAndAdditionalInfoInputModel
                        updateClientPolisy400In =
                            (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                            DataModelFactory
                                .GetModel(
                                    typeof(
                                        CLIENTUpdateCorporateClientAndAdditionalInfoInputModel
                                    ));

                    if ((inqClientPolisy400Out?.clientList?.assessorFlag == "Y"
                         || inqClientPolisy400Out?.clientList?.solicitorFlag == "Y"
                         || inqClientPolisy400Out?.clientList?.repairerFlag == "Y"
                         || inqClientPolisy400Out?.clientList?.hospitalFlag == "Y") )
                    {
                        updateClientPolisy400In = (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                            TransformerFactory.TransformModel(inqClientPolisy400Out,updateClientPolisy400In);
                        updateClientPolisy400In.checkFlag = "UPDATE";

                       
                    }
                    else
                    {
                        updateClientPolisy400In =
                            (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                            TransformerFactory.TransformModel(RegClientCorporateInput,
                                updateClientPolisy400In);
                        updateClientPolisy400In.checkFlag = "CREATE";
                    }
                    CleansingId = inqClientPolisy400Out?.clientList?.cleansingId;
                    RegClientCorporateInput.generalHeader.cleansingId = inqClientPolisy400Out?.clientList?.cleansingId;
                    RegClientCorporateInput.profileHeader.corporateName1 = inqClientPolisy400Out?.clientList?.name1;
                    RegClientCorporateInput.profileHeader.corporateName2 = inqClientPolisy400Out?.clientList?.name2;
                    RegClientCorporateInput.profileHeader.corporateBranch = inqClientPolisy400Out?.clientList?.corporateStaffNo;

                    //ต้อง Map field retCOMPInqClient ไปเป็น  updateClientPolisy400In  ไม่ใช่เอามาจาก user input

                    if (regClientCorporateInput?.generalHeader?.assessorFlag == "Y")
                    {
                        updateClientPolisy400In.assessorFlag = "Y";
                        updateClientPolisy400In.assessorBlackListFlag =
                            regClientCorporateInput?.asrhHeader?.assessorBlackListFlag;
                        updateClientPolisy400In.assessorDelistFlag =
                            regClientCorporateInput?.asrhHeader?.assessorDelistFlag;
                        updateClientPolisy400In.assessorOregNum = regClientCorporateInput?.asrhHeader?.assessorOregNum;
                        updateClientPolisy400In.assessorTerminateDate =
                            regClientCorporateInput?.asrhHeader?.assessorTerminateDate;

                    }

                    if (regClientCorporateInput?.generalHeader?.solicitorFlag == "Y")
                    {
                        updateClientPolisy400In.solicitorFlag = "Y";
                        updateClientPolisy400In.solicitorBlackListFlag =
                            regClientCorporateInput?.asrhHeader?.solicitorBlackListFlag;
                        updateClientPolisy400In.solicitorDelistFlag =
                            regClientCorporateInput?.asrhHeader?.solicitorDelistFlag;
                        updateClientPolisy400In.solicitorOregNum =
                            regClientCorporateInput?.asrhHeader?.solicitorOregNum;
                        updateClientPolisy400In.solicitorTerminateDate =
                            regClientCorporateInput?.asrhHeader?.solicitorTerminateDate;

                    }

                    if (regClientCorporateInput?.generalHeader?.repairerFlag == "Y")
                    {
                        updateClientPolisy400In.repairerFlag = "Y";
                        updateClientPolisy400In.repairerBlackListFlag =
                            regClientCorporateInput?.asrhHeader?.repairerBlackListFlag;
                        updateClientPolisy400In.repairerDelistFlag =
                            regClientCorporateInput?.asrhHeader?.repairerDelistFlag;
                        updateClientPolisy400In.repairerOregNum = regClientCorporateInput?.asrhHeader?.repairerOregNum;
                        updateClientPolisy400In.repairerTerminateDate =
                            regClientCorporateInput?.asrhHeader?.repairerTerminateDate;

                    }

                    if (regClientCorporateInput?.generalHeader?.hospitalFlag == "Y")
                    {
                        updateClientPolisy400In.hospitalFlag = "Y";
                    }

                    Console.WriteLine("updateClientPolisy400In.ToJson()");
                    Console.WriteLine(updateClientPolisy400In?.ToJson());
                    var clientService =
                        new CLIENTUpdateCorporateClientAndAdditionalInfoService(TransactionId, ControllerName);
                    clientService.Execute(updateClientPolisy400In);
                }
                else
                {
                    Console.WriteLine("Your Policy Client  Id  cannot be found in polisy400");
                    // regClientCorporateOutput.code = CONST_CODE_FAILED;
                    //regClientCorporateOutput.message = "Your Policy Client  Id  cannot be found";
                    throw new BuzErrorException("500", "Your Policy Client  Id  cannot be found in polisy400");
                }
            }
            else
            {
                Console.WriteLine("notCreatePolisyClientFlag = Y");
            }
          
           
        }

        /// <summary>
        /// สร้าง Corporate Client ใน CRM  
        /// </summary>
        /// <param name="regClientCorporateInput"></param>
        /// <param name="cleansingId"></param>
        /// <param name="polisyClientId"></param>
        /// <returns></returns>
        /// <exception cref="BuzErrorException"></exception>
        private string CreateClientInCrm(RegClientCorporateInputModel regClientCorporateInput, string cleansingId,
            string polisyClientId)
        {
            if ("Y" == regClientCorporateInput?.generalHeader?.notCreateCrmClientFlag)
            {
                return null;
            }
            if (string.IsNullOrEmpty(cleansingId))
            {
                return null;
            }
            //RegClientPersonalInputModel regClientPersonalInput = ObjectCopier.Clone(input);
            regClientCorporateInput.generalHeader.cleansingId = cleansingId;
            regClientCorporateInput.generalHeader.polisyClientId = polisyClientId;
            Console.WriteLine(regClientCorporateInput.ToJson());
            try
            {
                buzCreateCrmClientCorporate cmdCreateCrmClient = new buzCreateCrmClientCorporate();
                cmdCreateCrmClient.TransactionId = TransactionId;
                CreateCrmCorporateInfoOutputModel crmContentOutput =
                    (CreateCrmCorporateInfoOutputModel) cmdCreateCrmClient.Execute(regClientCorporateInput);

                if (crmContentOutput.code != AppConst.CODE_SUCCESS)
                {
                    AddDebugInfo("Create CRM Error BuzErrorException " + crmContentOutput.message,
                        crmContentOutput.description);
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
                AddDebugInfo("Create CRM Error Exception " + e.Message, e.StackTrace);
                throw new BuzErrorException(
                    "500",
                    "CRM Error:" + e.Message,
                    e.StackTrace,
                    "CRM",
                    TransactionId);
            }
        }


        protected BaseDataModel createGASRHCorporate(RegClientCorporateInputModel input)
        {
            RegClientCorporateContentOutputModel regClientCorporateOutput = new RegClientCorporateContentOutputModel();
            regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
            regClientCorporateOutput.transactionDateTime = DateTime.Now;
            regClientCorporateOutput.transactionId = TransactionId;
            regClientCorporateOutput.code = CONST_CODE_SUCCESS;

            RegClientCorporateDataOutputModel_Pass dataOutPass =
                new RegClientCorporateDataOutputModel_Pass();

            if (IsASRHValid(RegClientCorporateInput))
            {
                // create CLS if cleansingId and polisyClientId is null
                if (string.IsNullOrEmpty(RegClientCorporateInput.generalHeader.cleansingId) &&
                    string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    AddDebugInfo("create CLS");
                    Console.WriteLine("create CLS");
                    BaseDataModel clsCreateCorporateIn =
                        DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                    clsCreateCorporateIn =
                        TransformerFactory.TransformModel(RegClientCorporateInput, clsCreateCorporateIn);
                    //  CLSCreateCorporateClientContentOutputModel clsCreateClientContent = CallDevesServiceProxy<CLSCreateCorporateClientOutputModel, CLSCreateCorporateClientContentOutputModel>
                    //      (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);
                    var clsService = new CLSCreateCorporateClientService(TransactionId, ControllerName);
                    CLSCreateCorporateClientContentOutputModel clsCreateClientContent =
                        clsService.Execute((CLSCreateCorporateClientInputModel) clsCreateCorporateIn);

                    if (clsCreateClientContent.code == CommonConstant.CODE_SUCCESS)
                    {
                        //TODO เอา ค่าที่ได้ไปเป็น output
                        if (!string.IsNullOrEmpty(clsCreateClientContent?.data?.cleansingId))
                        {
                            NewCleansingId = clsCreateClientContent?.data?.cleansingId;
                        }
                        if (!string.IsNullOrEmpty(clsCreateClientContent?.data?.cleansingId))
                        {
                            NewCleansingId = clsCreateClientContent?.data?.cleansingId;
                        }

                        AddDebugInfo("create CLS newCleansingId = " + NewCleansingId);
                        RegClientCorporateInput.generalHeader.cleansingId = NewCleansingId;

                        //กรณีมาจากหน้าจอ CRM  notCreatePolisyClientFlag == Y ไม่ต้องไปสร้างใน polisyClientId
                        if (RegClientCorporateInput.generalHeader.notCreatePolisyClientFlag != "Y")
                        {
                            AddDebugInfo(
                                "กรณีมาจากหน้าจอ CRM  notCreatePolisyClientFlag == Y ไม่ต้องไปสร้างใน polisyClientId",
                                @"string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId)&& !string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId) ");

                            BaseDataModel polCreateCorporateIn =
                                DataModelFactory.GetModel(
                                    typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                            polCreateCorporateIn =
                                TransformerFactory.TransformModel(RegClientCorporateInput, polCreateCorporateIn);
                            /*
                             CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent =
                                 CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                         , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                     (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                             
                                     */

                            var clientService =
                                new CLIENTCreateCorporateClientAndAdditionalInfoService(TransactionId, ControllerName);
                            CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent =
                                clientService.Execute(
                                    (CLIENTCreateCorporateClientAndAdditionalInfoInputModel) polCreateCorporateIn);

                            RegClientCorporateInput =
                                (RegClientCorporateInputModel) TransformerFactory.TransformModel(polCreateClientContent,
                                    RegClientCorporateInput);
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
                    RegClientCorporateInput =
                        (RegClientCorporateInputModel) TransformerFactory.TransformModel(clsCreateClientContent,
                            RegClientCorporateInput);
                }

                // กรณีไม่มี เลข polisyClientId  แต่มี cleansingId ให้สร้างเฉพาะ 400
                else if (string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    //กรณีมาจากหน้าจอ CRM  notCreatePolisyClientFlag == Y ไม่ต้องไปสร้างใน polisyClientId
                    if (RegClientCorporateInput.generalHeader.notCreatePolisyClientFlag != "Y")
                    {
                        AddDebugInfo("กรณีไม่มี เลข polisyClientId  แต่มี cleansingId ให้สร้างเฉพาะ 400",
                            @"string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId)&& !string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId) ");

                        BaseDataModel polCreateCorporateIn =
                            DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                        polCreateCorporateIn =
                            TransformerFactory.TransformModel(RegClientCorporateInput, polCreateCorporateIn);
                        /*CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent =
                            CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                    , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);*/
                        var clientService =
                            new CLIENTCreateCorporateClientAndAdditionalInfoService(TransactionId, ControllerName);
                        CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent =
                            clientService.Execute(
                                (CLIENTCreateCorporateClientAndAdditionalInfoInputModel) polCreateCorporateIn);

                        RegClientCorporateInput =
                            (RegClientCorporateInputModel) TransformerFactory.TransformModel(polCreateClientContent,
                                RegClientCorporateInput);
                    }
                }

                //กรณี มี polisyClientId ให้ update
                else if (!string.IsNullOrEmpty(RegClientCorporateInput?.generalHeader?.polisyClientId))
                {
                    AddDebugInfo(" to update ");
                    try
                    {
                        COMPInquiryClientMasterInputModel compInqClientInput =
                            new COMPInquiryClientMasterInputModel();
                        compInqClientInput =
                            (COMPInquiryClientMasterInputModel) TransformerFactory.TransformModel(
                                RegClientCorporateInput, compInqClientInput);

                        /*  EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient =
                              CallDevesServiceProxy<COMPInquiryClientMasterOutputModel,
                                      EWIResCOMPInquiryClientMasterContentModel>
                                  (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);*/

                        var polService = new COMPInquiryClientMasterService(TransactionId, ControllerName);
                        EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient =
                            polService.Execute((COMPInquiryClientMasterInputModel) compInqClientInput);

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

                            if 
                                (inqClientPolisy400Out?.clientList?.assessorFlag == "Y"
                                  || inqClientPolisy400Out?.clientList?.solicitorFlag == "Y"
                                  || inqClientPolisy400Out?.clientList?.repairerFlag == "Y"
                                  || inqClientPolisy400Out?.clientList?.hospitalFlag == "Y")
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
                                    TransformerFactory.TransformModel(inqClientPolisy400Out,
                                        updateClientPolisy400In);
                                updateClientPolisy400In.checkFlag = "CREATE";
                            }
                            if (RegClientCorporateInput?.generalHeader?.assessorFlag == "Y")
                            {
                                updateClientPolisy400In.assessorFlag = "Y";
                                updateClientPolisy400In.assessorBlackListFlag =
                                    RegClientCorporateInput.asrhHeader.assessorBlackListFlag;
                                updateClientPolisy400In.assessorDelistFlag =
                                    RegClientCorporateInput.asrhHeader.assessorDelistFlag;
                                updateClientPolisy400In.assessorOregNum =
                                    RegClientCorporateInput.asrhHeader.assessorOregNum;
                                updateClientPolisy400In.assessorTerminateDate =
                                    RegClientCorporateInput.asrhHeader.assessorTerminateDate;
                                ;
                            }

                            if (RegClientCorporateInput?.generalHeader?.solicitorFlag == "Y")
                            {
                                updateClientPolisy400In.solicitorFlag = "Y";
                                updateClientPolisy400In.solicitorBlackListFlag =
                                    RegClientCorporateInput.asrhHeader.solicitorBlackListFlag;
                                updateClientPolisy400In.solicitorDelistFlag =
                                    RegClientCorporateInput.asrhHeader.solicitorDelistFlag;
                                updateClientPolisy400In.solicitorOregNum =
                                    RegClientCorporateInput.asrhHeader.solicitorOregNum;
                                updateClientPolisy400In.solicitorTerminateDate =
                                    RegClientCorporateInput.asrhHeader.solicitorTerminateDate;
                                ;
                            }

                            if (RegClientCorporateInput?.generalHeader?.repairerFlag == "Y")
                            {
                                updateClientPolisy400In.repairerFlag = "Y";
                                updateClientPolisy400In.repairerBlackListFlag =
                                    RegClientCorporateInput.asrhHeader.repairerBlackListFlag;
                                updateClientPolisy400In.repairerDelistFlag =
                                    RegClientCorporateInput.asrhHeader.repairerDelistFlag;
                                updateClientPolisy400In.repairerOregNum =
                                    RegClientCorporateInput.asrhHeader.repairerOregNum;
                                updateClientPolisy400In.repairerTerminateDate =
                                    RegClientCorporateInput.asrhHeader.repairerTerminateDate;
                                ;
                            }

                            if (RegClientCorporateInput?.generalHeader?.hospitalFlag == "Y")
                            {
                                updateClientPolisy400In.hospitalFlag = "Y";
                            }

                            // CallDevesServiceProxy<
                            //     CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel,
                            //     CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(
                            //     CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient,
                            //     updateClientPolisy400In);
                            var clientService =
                                new CLIENTUpdateCorporateClientAndAdditionalInfoService(TransactionId, ControllerName);
                            clientService.Execute(
                                (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel) updateClientPolisy400In);
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

                        if (!string.IsNullOrEmpty(NewCleansingId))
                        {
                            //เมื่อเกิด error ใด ๆ ใน service อื่นให้ลบ

                            AddDebugInfo("try rollback" + NewCleansingId);

                            var deleteResult =
                                CleansingClientService.Instance.RemoveByCleansingId(NewCleansingId, "C");

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
                AddDebugInfo(
                    "There is some conflicts among the arguments Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}");
                regClientCorporateOutput.code = AppConst.CODE_INVALID_INPUT;
                regClientCorporateOutput.message =
                    "There is some conflicts among the arguments Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}";
                regClientCorporateOutput.description = "";
            }

            //3 create crm CleinInfo in CRM เพื่อเก็บ cleansingId   แต่ให้ค้นก่อนถ้าพบจะไม่สร้างซ้ำ
            PolisyClientId = RegClientCorporateInput?.generalHeader?.polisyClientId ?? "";
            CleansingId = NewCleansingId ?? RegClientCorporateInput?.generalHeader?.cleansingId;
            Console.WriteLine(PolisyClientId + ":" + CleansingId);
            AddDebugInfo("create crm CleinInfo in CRM เพื่อเก็บ cleansingId   แต่ให้ค้นก่อนถ้าพบจะไม่สร้างซ้ำ",
                PolisyClientId + ":" + CleansingId);
            if (!string.IsNullOrEmpty(CleansingId))
            {
                AddDebugInfo("เริ่มสร้าง crm ");
                if (false == SpApiChkCustomerClient.Instance.CheckByCleansingId(CleansingId))
                {
                    try
                    {
                        CrmClientId = CreateClientInCrm(RegClientCorporateInput, CleansingId, PolisyClientId);
                        AddDebugInfo("สร้างข้อมูลแล้ว =" + CrmClientId);
                    }
                    catch (Exception e)
                    {
                        AddDebugInfo("CRM Error" + e.Message, e.StackTrace);
                    }
                }
                else
                {
                    AddDebugInfo("พบข้อมูลในระบบ crm อยู่แล้ว");
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
                            cleansingId = RegClientCorporateInput?.generalHeader?.cleansingId ?? "",
                            polisyClientId = RegClientCorporateInput?.generalHeader?.polisyClientId ?? "",
                            corporateName1 = RegClientCorporateInput?.profileHeader?.corporateName1 ?? "",
                            corporateName2 = RegClientCorporateInput?.profileHeader?.corporateName2 ?? "",
                            corporateBranch = RegClientCorporateInput?.profileHeader?.corporateBranch ?? ""
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

                dataOutPass.cleansingId = RegClientCorporateInput.generalHeader.cleansingId;
                dataOutPass.polisyClientId = RegClientCorporateInput.generalHeader.polisyClientId;
                dataOutPass.crmClientId = CrmClientId;
                dataOutPass.corporateName1 = RegClientCorporateInput.profileHeader.corporateName1;
                dataOutPass.corporateName2 = RegClientCorporateInput.profileHeader.corporateName2;
                dataOutPass.corporateBranch = RegClientCorporateInput.profileHeader.corporateBranch;

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