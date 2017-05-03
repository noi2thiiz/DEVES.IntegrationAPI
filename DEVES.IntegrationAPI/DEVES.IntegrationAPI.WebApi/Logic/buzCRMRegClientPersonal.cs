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

namespace DEVES.IntegrationAPI.WebApi.Logic
{

    public class buzCRMRegClientPersonal : BaseCommand
    {

        RegClientPersonalOutputModel_Fail regFail = new RegClientPersonalOutputModel_Fail();
        protected CLSCreatePersonalClientContentOutputModel clsCreateClientContent { get; set; }
        protected   RegClientPersonalContentOutputModel regClientPersonOutput  { get; set; }
        protected  RegClientPersonalDataOutputModel_Pass regClientPersonDataOutput  { get; set; }

        public override BaseDataModel Execute(object input)
        {
            regClientPersonOutput = new RegClientPersonalContentOutputModel
            {
                transactionDateTime = DateTime.Now,
                transactionId = TransactionId,
                code = CONST_CODE_SUCCESS
            };


            // regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();


            try
            {

                RegClientPersonalInputModel regClientPersonalInput = (RegClientPersonalInputModel)input;
                regClientPersonDataOutput = new RegClientPersonalDataOutputModel_Pass
                {
                    cleansingId = regClientPersonalInput.generalHeader.cleansingId,
                    polisyClientId = regClientPersonalInput.generalHeader.polisyClientId,
                    crmClientId = regClientPersonalInput.generalHeader.crmClientId,
                    personalName = regClientPersonalInput.profileInfo.personalName,
                    personalSurname = regClientPersonalInput.profileInfo.personalSurname
                };

                // Validate Master Data before sending to other services

                regFail.data = new RegClientPersonalDataOutputModel_Fail();
                regFail.data.fieldErrors = new List<RegClientPersonalFieldErrors>();

                var master_salutation = PersonalTitleMasterData.Instance.FindByCode(regClientPersonalInput.profileInfo.salutation);
                if (master_salutation == null)
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Salutation",
                            regClientPersonalInput.profileInfo.salutation);
                    regFail.data.fieldErrors.Add(new RegClientPersonalFieldErrors("profileInfo.salutation", errorMessage));
                }
                else
                {
                    regClientPersonalInput.profileInfo.salutation = master_salutation.PolisyCode;
                }

                var master_nationality = NationalityMasterData.Instance.FindByCode(regClientPersonalInput.profileInfo.nationality, "00203");
                if(master_nationality == null)
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Nationality",
                            regClientPersonalInput.profileInfo.nationality);
                    regFail.data.fieldErrors.Add(new RegClientPersonalFieldErrors("profileInfo.nationality", errorMessage));
                }
                else
                {
                    regClientPersonalInput.profileInfo.nationality = master_nationality.PolisyCode;
                }

                var master_occupation = OccupationMasterData.Instance.FindByCode(regClientPersonalInput.profileInfo.occupation, "00023");
                if (master_occupation == null)
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Occupation",
                            regClientPersonalInput.profileInfo.occupation);
                    regFail.data.fieldErrors.Add(new RegClientPersonalFieldErrors("profileInfo.occupation",errorMessage));
                }
                else
                {
                    regClientPersonalInput.profileInfo.occupation = master_occupation.PolisyCode;
                }

                var master_country = CountryMasterData.Instance.FindByCode(regClientPersonalInput.addressInfo.country, "00220");
                if (master_country == null)
                {
                    var errorMessage =
                        MessageBuilder.Instance.GetInvalidMasterMessage("Country",
                            regClientPersonalInput.addressInfo.country);
                    regFail.data.fieldErrors.Add(new RegClientPersonalFieldErrors("addressInfo.country", errorMessage));
                }
                else
                {
                    regClientPersonalInput.addressInfo.country = master_country.PolisyCode;
                }

                if(regFail.data.fieldErrors.Count > 0)
                {
                    throw new FieldValidationException();
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////
                if (string.IsNullOrEmpty(regClientPersonalInput.generalHeader.cleansingId))
                {
                    BaseDataModel clsCreatePersonIn =
                        DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
                    clsCreatePersonIn = TransformerFactory.TransformModel(regClientPersonalInput, clsCreatePersonIn);
                     clsCreateClientContent =
                        CallDevesServiceProxy<CLSCreatePersonalClientOutputModel,
                                CLSCreatePersonalClientContentOutputModel>
                            (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonIn);
                    if (clsCreateClientContent.code == CONST_CODE_SUCCESS)
                    {
                        Console.WriteLine("102  : CLS-"+CONST_CODE_SUCCESS);
                        if (clsCreateClientContent.data != null)
                        {
                            regClientPersonDataOutput.cleansingId = clsCreateClientContent.data.cleansingId;

                            regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                            regClientPersonOutput.data.Add(regClientPersonDataOutput);

                        }
                        regClientPersonalInput =
                            (RegClientPersonalInputModel) TransformerFactory.TransformModel(clsCreateClientContent,
                                regClientPersonalInput);
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
                        Console.WriteLine("134 : CLS-"+clsCreateClientContent.code);
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

                    CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreateClientContent = new CLIENTCreatePersonalClientAndAdditionalInfoContentModel();
                    if (string.IsNullOrEmpty(regClientPersonalInput.generalHeader.polisyClientId)
                        && regClientPersonalInput.generalHeader.notCreatePolisyClientFlag !="Y")
                    {
                        BaseDataModel polCreatePersonIn = DataModelFactory.GetModel(typeof(CLIENTCreatePersonalClientAndAdditionalInfoInputModel));
                        polCreatePersonIn = TransformerFactory.TransformModel(regClientPersonalInput, polCreatePersonIn);
                        polCreateClientContent = CallDevesServiceProxy<CLIENTCreatePersonalClientAndAdditionalInfoOutputModel
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

                            regClientPersonalInput = (RegClientPersonalInputModel)TransformerFactory.TransformModel(polCreateClientContent, regClientPersonalInput);
                        }
                    }


                    if (regClientPersonOutput.code == AppConst.CODE_SUCCESS)
                    {
                        buzCreateCrmClientPersonal cmdCreateCrmClient = new buzCreateCrmClientPersonal();
                        CreateCrmPersonInfoOutputModel crmContentOutput =
                            (CreateCrmPersonInfoOutputModel) cmdCreateCrmClient.Execute(regClientPersonalInput);

                        if (crmContentOutput.code == AppConst.CODE_SUCCESS)
                        {
                            regClientPersonOutput.code = AppConst.CODE_SUCCESS;
                            regClientPersonOutput.message = AppConst.MESSAGE_SUCCESS;
                            regClientPersonOutput.description = "Client registration complete";
                            RegClientPersonalDataOutputModel_Pass dataOutPass =
                                new RegClientPersonalDataOutputModel_Pass();
                            dataOutPass.cleansingId = regClientPersonalInput.generalHeader.cleansingId;
                            //dataOutPass.polisyClientId = regClientPersonalInput.generalHeader.polisyClientId;
                            dataOutPass.polisyClientId = polCreateClientContent.clientID;
                            dataOutPass.crmClientId = crmContentOutput.crmClientId;
                            dataOutPass.personalName = regClientPersonalInput.profileInfo.personalName;
                            dataOutPass.personalSurname = regClientPersonalInput.profileInfo.personalSurname;
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
                            regClientPersonOutput.data.Add( regClientPersonDataOutput);
                        }
                    }
                    else
                    {
                        regClientPersonOutput.code = AppConst.CODE_FAILED;

                    }
                }
                // All Error
                // แก้ตาม ที่ อาจารย์พรชัย บอก เพื่อให้เขาเอา เลข cleansingIdไปซ่อมข้อมูลได้
                if (regClientPersonOutput.code !=AppConst.CODE_SUCCESS)
                {
                    if (string.IsNullOrEmpty(regClientPersonOutput.message ))
                    {
                        regClientPersonOutput.message = "Failed client registration did not complete";
                    }
                    regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                    regClientPersonOutput.data.Add( regClientPersonDataOutput);
                }
            }
            catch (FieldValidationException e)
            {
                
                regFail.code = AppConst.CODE_INVALID_INPUT;
                regFail.message =AppConst.MESSAGE_INVALID_INPUT;
                regFail.description = AppConst.DESC_INVALID_INPUT;
                regFail.transactionId = TransactionId;
                regFail.transactionDateTime = DateTime.Now;

                return regFail;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
                regClientPersonOutput.code = AppConst.CODE_FAILED;
                regClientPersonOutput.message = AppConst.MESSAGE_INTERNAL_ERROR;
                regClientPersonOutput.description = e.StackTrace;

                RegClientPersonalDataOutputModel_Fail dataOutFail = new RegClientPersonalDataOutputModel_Fail();
                //fix Object reference not set to an instance of an object.
                regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
                regClientPersonOutput.data.Add(dataOutFail);
            }


            return regClientPersonOutput;

        }
    }
}