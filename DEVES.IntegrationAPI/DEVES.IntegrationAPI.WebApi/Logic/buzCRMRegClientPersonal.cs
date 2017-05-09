using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientPersonal : BuzCommand
    {
        protected RegClientPersonalInputModel RegClientPersonalInput { get; set; }
     
        protected CLSCreatePersonalClientContentOutputModel clsCreateClientContent { get; set; }
        protected RegClientPersonalContentOutputModel regClientPersonOutput { get; set; }
        protected RegClientPersonalDataOutputModel_Pass regClientPersonDataOutput { get; set; }

        public void TranFormInput(RegClientPersonalInputModel regPayeePersonalInput)
        {
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

           


            if (validator.Invalid())
            {
                throw new FieldValidationException(validator.GetFieldErrorData());
            }
        }

        public override BaseDataModel ExecuteInput(object input)
        {
            RegClientPersonalInput = (RegClientPersonalInputModel) input;

            TranFormInput(RegClientPersonalInput);

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
                    }
                    RegClientPersonalInput =
                        (RegClientPersonalInputModel) TransformerFactory.TransformModel(clsCreateClientContent,
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
                    Console.WriteLine("134 : CLS-" + clsCreateClientContent.code);
                    regClientPersonOutput.code = clsCreateClientContent.code;
                    regClientPersonOutput.message = clsCreateClientContent.message;
                    regClientPersonOutput.description = clsCreateClientContent.description;


                    //return regClientPersonOutput;o
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
                        regClientPersonOutput.message = "Cannot create Client in Polisy400.";
                        regClientPersonOutput.description = "";

                        // แก้ตาม ที่ อาจารย์พรชัย บอก เพื่อให้เขาเอา เลข cleansingIdไปซ่อมข้อมูลได้
                    }
                    else
                    {
                        regClientPersonDataOutput.polisyClientId = polCreateClientContent.clientID;

                        RegClientPersonalInput =
                            (RegClientPersonalInputModel) TransformerFactory.TransformModel(polCreateClientContent,
                                RegClientPersonalInput);
                    }
                }


                if (regClientPersonOutput.code == AppConst.CODE_SUCCESS)
                {
                    buzCreateCrmClientPersonal cmdCreateCrmClient = new buzCreateCrmClientPersonal();
                    CreateCrmPersonInfoOutputModel crmContentOutput =
                        (CreateCrmPersonInfoOutputModel) cmdCreateCrmClient.Execute(RegClientPersonalInput);

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
                        regClientPersonOutput.message = "Cannot create Client in CRM.";
                        regClientPersonOutput.description = crmContentOutput.description;
                        regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                        regClientPersonOutput.data.Add(regClientPersonDataOutput);
                    }
                }
                else
                {
                    regClientPersonOutput.code = AppConst.CODE_FAILED;
                }
            }
            // All Error
            // แก้ตาม ที่ อาจารย์พรชัย บอก เพื่อให้เขาเอา เลข cleansingIdไปซ่อมข้อมูลได้
            if (regClientPersonOutput.code != AppConst.CODE_SUCCESS)
            {
                if (string.IsNullOrEmpty(regClientPersonOutput.message))
                {
                    regClientPersonOutput.message = "Failed client registration did not complete";
                }
                regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                regClientPersonOutput.data.Add(regClientPersonDataOutput);
            }


            return regClientPersonOutput;
        }
    }
}