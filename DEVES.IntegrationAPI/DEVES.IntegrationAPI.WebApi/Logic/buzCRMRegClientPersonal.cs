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
        public override BaseDataModel Execute(object input)
        {
            RegClientPersonalContentOutputModel regClientPersonOutput = new RegClientPersonalContentOutputModel();
            // regClientPersonOutput.data = new List<RegClientPersonalDataOutputModel>();
            regClientPersonOutput.transactionDateTime = DateTime.Now;
            regClientPersonOutput.transactionId = TransactionId;
            regClientPersonOutput.code = CONST_CODE_SUCCESS;
            try
            {

                RegClientPersonalInputModel regClientPersonalInput = (RegClientPersonalInputModel)input;

                // Validate Master Data before sending to other services
                var master_salutation = PersonalTitleMasterData.Instance.FindByCode(regClientPersonalInput.profileInfo.salutation);
                if (master_salutation == null)
                {
                    throw new FieldValidationException("profileInfo.salutation", "PersonalTitleMasterData is invalid");
                }
                else
                {
                    regClientPersonalInput.profileInfo.salutation = master_salutation.PolisyCode;
                }

                var master_nationality = NationalityMasterData.Instance.FindByCode(regClientPersonalInput.profileInfo.nationality);
                if(master_nationality == null)
                {
                    throw new FieldValidationException("profileInfo.nationality", "NationalityMasterData is invalid");
                }
                else
                {
                    regClientPersonalInput.profileInfo.nationality = master_nationality.PolisyCode;
                }

                var master_occupation = OccupationMasterData.Instance.FindByCode(regClientPersonalInput.profileInfo.occupation);
                if (master_occupation == null)
                {
                    throw new FieldValidationException("profileInfo.occupation", "OccupationMasterData is invalid");
                }
                else
                {
                    regClientPersonalInput.profileInfo.occupation = master_occupation.PolisyCode;
                }

                var master_country = CountryMasterData.Instance.FindByCode(regClientPersonalInput.addressInfo.country);
                if (master_country == null)
                {
                    throw new FieldValidationException("addressInfo.country", "CountryMasterData is invalid");
                }
                else
                {
                    regClientPersonalInput.addressInfo.country = master_country.PolisyCode;
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////
                if (string.IsNullOrEmpty(regClientPersonalInput.generalHeader.cleansingId))
                {
                    BaseDataModel clsCreatePersonIn = DataModelFactory.GetModel(typeof(CLSCreatePersonalClientInputModel));
                    clsCreatePersonIn = TransformerFactory.TransformModel(regClientPersonalInput, clsCreatePersonIn);
                    CLSCreatePersonalClientContentOutputModel clsCreateClientContent = CallDevesServiceProxy<CLSCreatePersonalClientOutputModel, CLSCreatePersonalClientContentOutputModel>
                                                                                        (CommonConstant.ewiEndpointKeyCLSCreatePersonalClient, clsCreatePersonIn);
                    if (clsCreateClientContent.code == CONST_CODE_SUCCESS)
                    {
                        regClientPersonalInput = (RegClientPersonalInputModel)TransformerFactory.TransformModel(clsCreateClientContent, regClientPersonalInput);
                    }
                    else
                    {
                        regClientPersonOutput.code = clsCreateClientContent.code;
                        regClientPersonOutput.message = clsCreateClientContent.message;
                        regClientPersonOutput.description = clsCreateClientContent.description;
                        //return regClientPersonOutput;o
                    }
                }

                if (regClientPersonOutput.code == CONST_CODE_SUCCESS)
                {

                    CLIENTCreatePersonalClientAndAdditionalInfoContentModel polCreateClientContent = new CLIENTCreatePersonalClientAndAdditionalInfoContentModel();
                    if (string.IsNullOrEmpty(regClientPersonalInput.generalHeader.polisyClientId))
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
                        }
                        else
                        {
                            regClientPersonalInput = (RegClientPersonalInputModel)TransformerFactory.TransformModel(polCreateClientContent, regClientPersonalInput);
                        }
                    }


                    if (regClientPersonOutput.code == CONST_CODE_SUCCESS)
                    {
                        buzCreateCrmClientPersonal cmdCreateCrmClient = new buzCreateCrmClientPersonal();
                        CreateCrmPersonInfoOutputModel crmContentOutput = (CreateCrmPersonInfoOutputModel)cmdCreateCrmClient.Execute(regClientPersonalInput);

                        if (crmContentOutput.code == CONST_CODE_SUCCESS)
                        {
                            regClientPersonOutput.code = CONST_CODE_SUCCESS;
                            regClientPersonOutput.message = "SUCCESS";
                            RegClientPersonalDataOutputModel_Pass dataOutPass = new RegClientPersonalDataOutputModel_Pass();
                            dataOutPass.cleansingId = regClientPersonalInput.generalHeader.cleansingId;
                            //dataOutPass.polisyClientId = regClientPersonalInput.generalHeader.polisyClientId;
                            dataOutPass.polisyClientId = polCreateClientContent.clientID;
                            dataOutPass.crmClientId = crmContentOutput.crmClientId;
                            dataOutPass.personalName = regClientPersonalInput.profileInfo.personalName;
                            dataOutPass.personalSurname = regClientPersonalInput.profileInfo.personalSurname;
                            regClientPersonOutput.data.Add(dataOutPass);
                        }
                        else
                        {
                            regClientPersonOutput.code = CONST_CODE_FAILED;
                            regClientPersonOutput.message = crmContentOutput.message;
                            regClientPersonOutput.description = crmContentOutput.description;
                        }
                    }
                }
            }
            catch (FieldValidationException e)
            {
                RegClientPersonalOutputModel_Fail regFail = new RegClientPersonalOutputModel_Fail();
                regFail.code = CONST_CODE_FAILED;
                regFail.message = e.Message;
                regFail.description = e.StackTrace;
                regFail.transactionId = TransactionId;
                regFail.transactionDateTime = DateTime.Now.ToString();
                regFail.data = new RegClientPersonalDataOutputModel_Fail();
                regFail.data.fieldErrors = new List<RegClientPersonalFieldErrors>();
                regFail.data.fieldErrors.Add(new RegClientPersonalFieldErrors(e.fieldError, e.message));

                return regFail;
            }
            catch (Exception e)
            {
                regClientPersonOutput.code = CONST_CODE_FAILED;
                regClientPersonOutput.message = e.Message;
                regClientPersonOutput.description = e.StackTrace;

                RegClientPersonalDataOutputModel_Fail dataOutFail = new RegClientPersonalDataOutputModel_Fail();
                regClientPersonOutput.data.Add(dataOutFail);
            }


            return regClientPersonOutput;

        }
    }
}