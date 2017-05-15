﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientPersonal : BuzCommand
    {
        //จะใช้เก็บค่า CleansingId เอาไว้ เพื่อใช้ลบออกจาก Cleansing หากมี service ใดๆที่ทำงานไม่สำเร็จ
        protected string newCleansingId;
       

        protected RegClientPersonalInputModel RegClientPersonalInput { get; set; }

        protected CLSCreatePersonalClientContentOutputModel clsCreateClientContent { get; set; }
        protected RegClientPersonalContentOutputModel regClientPersonOutput { get; set; }
        protected RegClientPersonalDataOutputModel_Pass regClientPersonDataOutput { get; set; }


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
            RegClientPersonalInput = (RegClientPersonalInputModel)input;

            TranFormInput();

            regClientPersonOutput = new RegClientPersonalContentOutputModel
            {
                transactionDateTime = DateTime.Now,
                transactionId = TransactionId,
                code = CONST_CODE_SUCCESS
            };


            regClientPersonDataOutput = new RegClientPersonalDataOutputModel_Pass
            {
                cleansingId = RegClientPersonalInput.generalHeader.cleansingId,
                polisyClientId = RegClientPersonalInput.generalHeader.polisyClientId,
                crmClientId = RegClientPersonalInput.generalHeader.crmClientId,
                personalName = RegClientPersonalInput.profileInfo.personalName,
                personalSurname = RegClientPersonalInput.profileInfo.personalSurname
            };


            ///////////////////////////////////////////////////////////////////////////////////////////////////
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


            if (regClientPersonOutput.code == CONST_CODE_SUCCESS)
            {
                Console.WriteLine("Create:CLIENTCreatePersonalClientAndAdditionalInfo");

                CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreateClientContent =
                    new CLIENTCreatePersonalClientAndAdditionalInfoContentModel();
                if (string.IsNullOrEmpty(RegClientPersonalInput.generalHeader.polisyClientId)
                    && RegClientPersonalInput.generalHeader.notCreatePolisyClientFlag != "Y")
                {
                    BaseDataModel polCreatePersonIn =
                        DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));
                    polCreatePersonIn = TransformerFactory.TransformModel(RegClientPersonalInput, polCreatePersonIn);
                    polCreateClientContent =
                        CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
                                , CLIENTCreatePersonalClientAndAdditionalInfoContentModel>
                            (CommonConstant.ewiEndpointKeyCLIENTCreatePersonalClient, polCreatePersonIn);

                    if (string.IsNullOrEmpty(polCreateClientContent?.clientID))
                    {
                        regClientPersonOutput.code = CONST_CODE_FAILED;
                        regClientPersonOutput.message = $"Polisy400 Error:Cannot create Client in Polisy400.";
                        regClientPersonOutput.description = "";

                        // แก้ตาม ที่ อาจารย์พรชัย บอก เพื่อให้เขาเอา เลข cleansingIdไปซ่อมข้อมูลได้
                    }
                    else
                    {
                        regClientPersonDataOutput.polisyClientId = polCreateClientContent.clientID;

                        RegClientPersonalInput =
                            (RegClientPersonalInputModel)TransformerFactory.TransformModel(polCreateClientContent,
                                RegClientPersonalInput);
                    }
                }


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
                    //เมื่อเกิด error ใด ๆ ใน service อื่นให้ลบ
                    /*
                    var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "P");
                   
               
                    if (!deleteResult.success)
                    {
                        regClientPersonOutput.message = "Failed to complete the transaction, and it does not rollback";
                        regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                        regClientPersonOutput.data.Add(regClientPersonDataOutput);
                    }
                    */
                   

                }
            }
            // Exception Error 
            if (regClientPersonOutput.code != AppConst.CODE_SUCCESS)
            {
                if (string.IsNullOrEmpty(regClientPersonOutput.message))
                {
                    regClientPersonOutput.message = "Failed to complete the transaction";
                }
            }



            return regClientPersonOutput;
        }
    }
}