using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using DEVES.IntegrationAPI.Core.Helper;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientPersonal : BuzCommand
    {
        //จะใช้เก็บค่า CleansingId เอาไว้ เพื่อใช้ลบออกจาก Cleansing หากมี service ใดๆที่ทำงานไม่สำเร็จ
        protected string cleansingId;

        protected string polisyClientId;
        protected string crmClientId;


        protected RegClientPersonalInputModel RegClientPersonalInput { get; set; }

      

        public void TranFormInput()
        {
            // ป้องกันปัญหา locus ส่ง json มาไม่ครบ
            if (RegClientPersonalInput.addressInfo == null)
            {
                RegClientPersonalInput.addressInfo = new AddressInfoModel();
            }
            if (RegClientPersonalInput.contactInfo == null)
            {
                RegClientPersonalInput.contactInfo = new ContactInfoModel();
            }
            if (RegClientPersonalInput.generalHeader == null)
            {
                RegClientPersonalInput.generalHeader = new GeneralHeaderModel();
            }
            if (RegClientPersonalInput.profileInfo == null)
            {
                RegClientPersonalInput.profileInfo = new ProfileInfoModel();
            }

            // Validate Master Data before sending to other services
            var validator = new MasterDataValidator();


            //profileInfo.salutation
            RegClientPersonalInput.profileInfo.salutation = validator.TryConvertSalutationCode(
                "profileInfo.salutation",
                RegClientPersonalInput?.profileInfo?.salutation);

            //"profileInfo.nationality"
            RegClientPersonalInput.profileInfo.nationality = validator.TryConvertNationalityCode(
                "profileInfo.nationality",
                RegClientPersonalInput?.profileInfo?.nationality);

            //"profileInfo.occupation"
            RegClientPersonalInput.profileInfo.occupation = validator.TryConvertOccupationCode(
                "profileInfo.occupation",
                RegClientPersonalInput?.profileInfo?.occupation);


            //"profileInfo.country"
            RegClientPersonalInput.addressInfo.country = validator.TryConvertCountryCode(
                "addressInfo.country",
                RegClientPersonalInput?.addressInfo?.country);

            //"profileInfo.provinceCode"
            RegClientPersonalInput.addressInfo.provinceCode = validator.TryConvertProvinceCode(
                "addressInfo.provinceCode",
                RegClientPersonalInput?.addressInfo?.provinceCode,
                RegClientPersonalInput?.addressInfo?.country);

            //"profileInfo.districtCode"
            RegClientPersonalInput.addressInfo.districtCode = validator.TryConvertDistrictCode(
                    "addressInfo.districtCode",
                    RegClientPersonalInput?.addressInfo?.districtCode,
                    RegClientPersonalInput?.addressInfo?.provinceCode)
                ;

            //"profileInfo.subDistrictCode"
            RegClientPersonalInput.addressInfo.subDistrictCode = validator.TryConvertSubDistrictCode(
                "addressInfo.subDistrictCode",
                RegClientPersonalInput?.addressInfo?.subDistrictCode,
                RegClientPersonalInput?.addressInfo?.districtCode,
                RegClientPersonalInput?.addressInfo?.provinceCode
            );
            //"profileInfo.addressType"
            RegClientPersonalInput.addressInfo.addressType = validator.TryConvertAddressTypeCode(
                "addressInfo.addressType",
                RegClientPersonalInput?.addressInfo?.addressType
            );


            if (validator.Invalid())
            {
                throw new FieldValidationException(validator.GetFieldErrorData());
            }
        }

        public override BaseDataModel ExecuteInput(object input)
        {
            RegClientPersonalInput = (RegClientPersonalInputModel) input;

            TranFormInput();

            //control flow อาจจะดู งง ๆ แค่พยายามเขียนให้ตรงกับเอกสาร
           
            if (!string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.polisyClientId))
            {
                //ไม่ต้องไปสร้างใน 400 
                polisyClientId = RegClientPersonalInput?.generalHeader?.polisyClientId;
                RegClientPersonalInput.generalHeader.notCreatePolisyClientFlag = "Y";
            }
            //case PG-02
            // เคสนี้เป็นข้อมูลลูกค้าที่ถูกสร้างตรงๆ ที่<<Polisy400>> ซึ่งข้อมูลนี้จะมีเลข CleansingId ในวันต่อมา 
            // และแค่มีเลข polisyClientId ก็ตอบโจทย์ของ Locus ในการเปิดเคลม
            // ในอนาคตหากจะต้องสร้าง เลข CleansingId ในเคสนี้ ต้องมาคุย Flow กันอีกที
            if (string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.cleansingId) &&
                !string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.polisyClientId))
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
            if (!string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.cleansingId) &&
                !string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.polisyClientId))
            {
                throw new BuzErrorException(
                    "400",
                    "The Service cannot process case:  'cleansingId' not null and 'polisyClientId' not null",
                    "",
                    "CRM",
                    TransactionId);
            }



            //1:Parameter CleansingId Does Not Contain Data
            if (string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.cleansingId))
            {
                //1.1:Parameter polisy Client Id Does Not Contain Data
                if (string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.polisyClientId))
                {
                    //1.1.1:Create Client Cleansing
                    //ถ้า CLS สร้างไม่สำเร็จจะ  throw ไปเลย
                    cleansingId = CreatePersonalClientInCLS(RegClientPersonalInput);


                    //1.1.2:Create PersonalClient And Additional Poliisy 400
                    // ยกเว้น notCreatePolisyClientFlag =Y ไม่ต้องสร้าง
                    if (RegClientPersonalInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
                    {
                        try
                        {
                            polisyClientId =
                                CreatePersonalClientAndAdditionalInfoInPolisy400(RegClientPersonalInput, cleansingId);
                        }
                        catch (Exception)
                        {
                            //ถ้าสร้างไม่สำเร็จจะลบข้อมูลใน  CLS ออก
                            //ระวังอย่างแก้โค้ดจนเอา CleansingId ที่ไม่ได้สร้างใหม่มาลบ
                            DeleteNewClientInCLS(cleansingId);
                            throw;
                        }
                    }
                }
            }


            //2: Parameter cleansing id Contain Data
            // สร้างเฉพาะใน 400 
            // กรณีนี้  ไม่ได้สร้าง CleansingId ใหม่จะไม่มีการ  rollback ถ้าสร้างใน  Polisy400 ไม่สำเร็จ
            
            else if (false==string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.cleansingId))
            {
                cleansingId = RegClientPersonalInput?.generalHeader?.cleansingId;
                //2.1:Create PersonalClient And Additional Poliisy 400 
                // ยกเว้น notCreatePolisyClientFlag =Y ไม่ต้องสร้าง
                if (RegClientPersonalInput?.generalHeader?.notCreatePolisyClientFlag != "Y")
                {
                    CreatePersonalClientAndAdditionalInfoInPolisy400(RegClientPersonalInput,
                        cleansingId);
                }
            }



            //3 create crm CleinInfo in CRM เพื่อเก็บ cleansingId   แต่ให้ค้นก่อนถ้าพบจะไม่สร้างซ้ำ
            if (!string.IsNullOrEmpty(cleansingId))
            {
                crmClientId = CreateClientInCRM(RegClientPersonalInput, cleansingId, polisyClientId);
            }
           


            // return output
            return new RegClientPersonalContentOutputModel
            {
                transactionDateTime = DateTime.Now,
                transactionId = TransactionId,
                code = AppConst.CODE_SUCCESS,
                message = AppConst.MESSAGE_SUCCESS,
                description = "",
                data = new List<RegClientPersonalDataOutputModel>
                {
                    new RegClientPersonalDataOutputModel_Pass
                    {
                        cleansingId = cleansingId,
                        polisyClientId = polisyClientId,
                        crmClientId = crmClientId,
                        personalName = RegClientPersonalInput?.profileInfo?.personalName ?? "",
                        personalSurname = RegClientPersonalInput?.profileInfo?.personalSurname ?? ""
                    }
                }
            };
        }

        public void DeleteNewClientInCLS(string newCleansingId)
        {
            Console.WriteLine("try rollback" + newCleansingId);
            try
            {
                var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "P");

                if (!deleteResult.success)
                {
                    throw new BuzErrorException(
                        "500",
                        "CLS Error Failed to complete the transaction, and it does not rollback",
                        "An error occurred from the external service (CLSDeletePersonalClient)",
                        "CLS",
                        TransactionId);
                }
            }
            catch (BuzErrorException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BuzErrorException(
                    "500",
                    "CLS Error Failed to complete the transaction, and it does not rollback",
                    "An error occurred from the external service (CLSDeletePersonalClient)",
                    "CLS",
                    TransactionId);
            }
        }

        public string CreatePersonalClientAndAdditionalInfoInPolisy400(
            RegClientPersonalInputModel RegClientPersonalInput, string cleansingId)
        {
            if (string.IsNullOrEmpty(cleansingId))
            {
                return null;
            }
            //RegClientPersonalInputModel RegClientPersonalInput = ObjectCopier.Clone(input);
            RegClientPersonalInput.generalHeader.cleansingId = cleansingId;

            Console.WriteLine("Create:CLIENTCreatePersonalClientAndAdditionalInfo");
            CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreateClientContent =
                new CLIENTCreatePersonalClientAndAdditionalInfoContentModel();
            BaseDataModel polCreatePersonIn =
                DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));
            polCreatePersonIn = TransformerFactory.TransformModel(RegClientPersonalInput, polCreatePersonIn);
            polCreateClientContent =
                CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
                        , CLIENTCreatePersonalClientAndAdditionalInfoContentModel>
                    (CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient, polCreatePersonIn);

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

        public string CreatePersonalClientInCLS(RegClientPersonalInputModel RegClientPersonalInput)
        {
            if (!string.IsNullOrEmpty(RegClientPersonalInput?.generalHeader?.cleansingId))
            {
                return null;
            }


            BaseDataModel clsCreatePersonIn =
                DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
            clsCreatePersonIn = TransformerFactory.TransformModel(RegClientPersonalInput, clsCreatePersonIn);

            var clsCreateClientContent =
                CallDevesServiceProxy<CLSCreatePersonalClientOutputModel,
                        CLSCreatePersonalClientContentOutputModel>
                    (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonIn);


            if (clsCreateClientContent.code != CONST_CODE_SUCCESS)
            {
                throw new BuzErrorException(
                    "500",
                    $"CLS Error {clsCreateClientContent.code}:{clsCreateClientContent.message}",
                    clsCreateClientContent.description,
                    "CLS",
                    TransactionId);
                //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
            }

            if (string.IsNullOrEmpty(clsCreateClientContent?.data?.cleansingId))
            {
                throw new BuzErrorException(
                    "500",
                    $"CLS Error: CLS return success but not return cleansingId",
                    "An error occurred from the external service (CLSCreatePersonalClient)",
                    "CLS",
                    TransactionId);
                //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
            }


            //success
            return clsCreateClientContent?.data?.cleansingId;
        }

        public string CreateClientInCRM(RegClientPersonalInputModel regClientPersonalInput, string cleansingId,
            string polisyClientId)
        {
            if (string.IsNullOrEmpty(cleansingId) )
            {
                return null;
            }
            //RegClientPersonalInputModel regClientPersonalInput = ObjectCopier.Clone(input);
            regClientPersonalInput.generalHeader.cleansingId = cleansingId;
            regClientPersonalInput.generalHeader.polisyClientId = polisyClientId;
            Console.WriteLine(regClientPersonalInput.ToJson());
            try
            {
                buzCreateCrmClientPersonal cmdCreateCrmClient = new buzCreateCrmClientPersonal();
                CreateCrmPersonInfoOutputModel crmContentOutput =
                    (CreateCrmPersonInfoOutputModel) cmdCreateCrmClient.Execute(regClientPersonalInput);

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
    }
}
/*
  if (regClientPersonOutput.code == AppConst.CODE_SUCCESS)
                {
                    buzCreateCrmClientPersonal cmdCreateCrmClient = new buzCreateCrmClientPersonal();
                    CreateCrmPersonInfoOutputModel crmContentOutput =
                        (CreateCrmPersonInfoOutputModel)cmdCreateCrmClient.Execute(RegClientPersonalInput);

                    if (crmContentOutput.code == AppConst.CODE_SUCCESS)
                    {
                        regClientPersonOutput.code = AppConst.CODE_SUCCESS;
                        regClientPersonOutput.message = AppConst.MESSAGE_SUCCESS;
                        regClientPersonOutput.description = "Client registration complete";
                        RegClientPersonalDataOutputModel_Pass dataOutPass =
                            new RegClientPersonalDataOutputModel_Pass();
                        dataOutPass.cleansingId = RegClientPersonalInput.generalHeader.cleansingId;
                        //dataOutPass.polisyClientId = regClientPersonalInput.generalHeader.polisyClientId;
                        dataOutPass.polisyClientId = polCreateClientContent.clientID;
                        dataOutPass.crmClientId = crmContentOutput.crmClientId;
                        dataOutPass.personalName = RegClientPersonalInput.profileInfo.personalName;
                        dataOutPass.personalSurname = RegClientPersonalInput.profileInfo.personalSurname;
                        //Object reference not set to an instance of an object
                        regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                        regClientPersonOutput.data.Add(dataOutPass);
                    }
                    else
                    {
                        regClientPersonOutput.code = AppConst.CODE_FAILED;
                        regClientPersonOutput.message = "CRM Error:Cannot create Client in CRM.";
                        regClientPersonOutput.description = crmContentOutput.description;
                        regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                        regClientPersonOutput.data.Add(regClientPersonDataOutput);
                    }
                }
                else
                {
                    regClientPersonOutput.code = AppConst.CODE_FAILED;


                }
     
     */

/*
  if (string.IsNullOrEmpty(RegClientPersonalInput.generalHeader.cleansingId))
        {
            BaseDataModel clsCreatePersonIn =
                DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
            clsCreatePersonIn = TransformerFactory.TransformModel(RegClientPersonalInput, clsCreatePersonIn);
            clsCreateClientContent =
                CallDevesServiceProxy<CLSCreatePersonalClientOutputModel,
                        CLSCreatePersonalClientContentOutputModel>
                    (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonIn);
            if (clsCreateClientContent.code == CONST_CODE_SUCCESS)
            {
                Console.WriteLine("102  : CLS-" + CONST_CODE_SUCCESS);
                if (clsCreateClientContent.data != null)
                {
                    regClientPersonDataOutput.cleansingId = clsCreateClientContent.data.cleansingId;


                    regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                    regClientPersonOutput.data.Add(regClientPersonDataOutput);

                    newCleansingId = clsCreateClientContent.data.cleansingId;

                }
                RegClientPersonalInput =
                    (RegClientPersonalInputModel)TransformerFactory.TransformModel(clsCreateClientContent,
                        RegClientPersonalInput);
            }
            else if (clsCreateClientContent.code == "CLS-1109")
            {
                Console.WriteLine("108 : CLS-1109");

                regClientPersonOutput.code = clsCreateClientContent.code;
                regClientPersonOutput.message = clsCreateClientContent.message;
                regClientPersonOutput.description = clsCreateClientContent.description;

                if (clsCreateClientContent.data != null)
                {
                    regClientPersonDataOutput.cleansingId = clsCreateClientContent.data.cleansingId;
                }
            }
            else
            {
                throw new BuzErrorException(
                    "500",
                    $"CLS Error {clsCreateClientContent.code}:{clsCreateClientContent.message}",
                    "An error occurred from the external service (CLSCreateCorporateClient)",

                    "CLS",
                    TransactionId);
                //throw new Exception(String.Format("Error:{0}, Message:{1}", ewiRes.responseCode , ewiRes.responseMessage));
            }

        }
        else
        {
            //AdHoc  ถ้าระบุ  cleansingId ให้ถิแว่า success ไปก่น
            Console.WriteLine("regClientPersonOutput Is Existing ");
            regClientPersonOutput.code = CONST_CODE_SUCCESS;
        }
 */

