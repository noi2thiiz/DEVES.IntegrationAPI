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
using DEVES.IntegrationAPI.WebApi.Logic.Validator;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;
using DEVES.IntegrationAPI.WebApi.Logic.Services;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzCRMRegClientCorporate : BuzCommand
    {
        //จะใช้เก็บค่า CleansingId เอาไว้ เพื่อใช้ลบออกจาก Cleansing หากมี service ใดๆที่ทำงานไม่สำเร็จ
        protected string newCleansingId;

        public RegClientCorporateOutputModel_Fail regFail { get; set; } = new RegClientCorporateOutputModel_Fail();
        protected RegClientCorporateInputModel regClientCorporateInput { get; set; }

        public void TranFormInput()
        {
            if (regClientCorporateInput == null)
            {
                regClientCorporateInput = new RegClientCorporateInputModel();
            }
            if (regClientCorporateInput.addressHeader ==null)
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



                ///////////////////////////////////////////////////////////////////////////////////////////////////

                if (IsASRHValid (regClientCorporateInput) )
                {

                    if (string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId))
                    {

                        BaseDataModel clsCreateCorporateIn = DataModelFactory.GetModel(typeof(CLSCreateCorporateClientInputModel));
                        clsCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, clsCreateCorporateIn);
                        CLSCreateCorporateClientContentOutputModel clsCreateClientContent = CallDevesServiceProxy<CLSCreateCorporateClientOutputModel, CLSCreateCorporateClientContentOutputModel>
                                                                                            (CommonConstant.ewiEndpointKeyCLSCreateCorporateClient, clsCreateCorporateIn);

                            if (clsCreateClientContent.code == CommonConstant.CODE_SUCCESS)
                            {
                               //TODO เอา ค่าที่ได้ไปเป็น output
                                newCleansingId = clsCreateClientContent.data.cleansingId;
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
                            regClientCorporateInput = (RegClientCorporateInputModel)TransformerFactory.TransformModel(clsCreateClientContent, regClientCorporateInput);
                            }

                    if (string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId) && !string.IsNullOrEmpty(regClientCorporateInput.generalHeader.cleansingId) )
                    {

                        BaseDataModel polCreateCorporateIn = DataModelFactory.GetModel(typeof(CLIENTCreateCorporateClientAndAdditionalInfoInputModel));
                        polCreateCorporateIn = TransformerFactory.TransformModel(regClientCorporateInput, polCreateCorporateIn);
                        CLIENTCreateCorporateClientAndAdditionalInfoContentModel polCreateClientContent = CallDevesServiceProxy<CLIENTCreateCorporateClientAndAdditionalInfoOutputModel
                                                                                                            , CLIENTCreateCorporateClientAndAdditionalInfoContentModel>
                                                                                                            (CommonConstant.ewiEndpointKeyCLIENTCreateCorporateClient, polCreateCorporateIn);
                        regClientCorporateInput = (RegClientCorporateInputModel)TransformerFactory.TransformModel(polCreateClientContent, regClientCorporateInput);
                    }
                    //กรณีมาจากหน้าจอ CRM  notCreatePolisyClientFlag == Y ไม่ต้องไปสร้างใน polisyClientId



                        if (!string.IsNullOrEmpty(regClientCorporateInput.generalHeader.polisyClientId))
                        {

                            if ((regClientCorporateInput.generalHeader.assessorFlag == "Y"
                                 || regClientCorporateInput.generalHeader.solicitorFlag == "Y"
                                 || regClientCorporateInput.generalHeader.repairerFlag == "Y"
                                 || regClientCorporateInput.generalHeader.hospitalFlag == "Y"))
                            {
                                if (regClientCorporateInput.generalHeader.notCreatePolisyClientFlag != "Y")
                                {
                                    try
                                    {
                                        COMPInquiryClientMasterInputModel compInqClientInput =
                                            new COMPInquiryClientMasterInputModel();
                                        compInqClientInput =
                                            (COMPInquiryClientMasterInputModel) TransformerFactory.TransformModel(
                                                regClientCorporateInput, compInqClientInput);

                                        EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient =
                                            CallDevesServiceProxy<COMPInquiryClientMasterOutputModel,
                                                    EWIResCOMPInquiryClientMasterContentModel>
                                                (CommonConstant.ewiEndpointKeyCOMPInquiryClient, compInqClientInput);
                                        //Found in Polisy400
                                        if (retCOMPInqClient.clientListCollection != null &&
                                            retCOMPInqClient.clientListCollection.Count == 1)
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

                                            if (inqClientPolisy400Out.clientList.additionalExistFlag == "Y")
                                            {

                                                updateClientPolisy400In =
                                                    (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                                                    TransformerFactory.TransformModel(inqClientPolisy400Out,
                                                        updateClientPolisy400In);
                                                updateClientPolisy400In.checkFlag = "UPDATE";
                                            }
                                            else if (inqClientPolisy400Out.clientList.additionalExistFlag == "N")
                                            {

                                                updateClientPolisy400In =
                                                    (CLIENTUpdateCorporateClientAndAdditionalInfoInputModel)
                                                    TransformerFactory.TransformModel(regClientCorporateInput,
                                                        updateClientPolisy400In);
                                                updateClientPolisy400In.checkFlag = "CREATE";
                                            }
                                            updateClientPolisy400In.assessorFlag =
                                                string.IsNullOrEmpty(regClientCorporateInput.generalHeader.assessorFlag)
                                                    ? updateClientPolisy400In.assessorFlag
                                                    : regClientCorporateInput.generalHeader.assessorFlag;
                                            updateClientPolisy400In.solicitorFlag =
                                                string.IsNullOrEmpty(
                                                    regClientCorporateInput.generalHeader.solicitorFlag)
                                                    ? updateClientPolisy400In.solicitorFlag
                                                    : regClientCorporateInput.generalHeader.solicitorFlag;
                                            updateClientPolisy400In.repairerFlag =
                                                string.IsNullOrEmpty(regClientCorporateInput.generalHeader.repairerFlag)
                                                    ? updateClientPolisy400In.repairerFlag
                                                    : regClientCorporateInput.generalHeader.repairerFlag;
                                            updateClientPolisy400In.hospitalFlag =
                                                string.IsNullOrEmpty(regClientCorporateInput.generalHeader.hospitalFlag)
                                                    ? updateClientPolisy400In.hospitalFlag
                                                    : regClientCorporateInput.generalHeader.hospitalFlag;

                                            CallDevesServiceProxy<
                                                CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel,
                                                CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(
                                                CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient,
                                                updateClientPolisy400In);

                                            //CLIENTUpdateCorporateClientAndAdditionalInfoContentModel updateClientPolisy400Out = CallDevesServiceProxy<CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel, CLIENTUpdateCorporateClientAndAdditionalInfoContentModel>(CommonConstant.ewiEndpointKeyCLIENTUpdateCorporateClient, updateClientPolisy400In);
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                        //เมื่อเกิด error ใด ๆ ใน service อื่นให้ลบ
                                        Console.WriteLine("tyy rollback" + newCleansingId);
                                        var deleteResult = CleansingClientService.Instance.RemoveByCleansingId(newCleansingId, "C");

                                       regClientCorporateOutput.code = CONST_CODE_FAILED;
                                       regClientCorporateOutput.message = e.Message;
                                       if (!deleteResult.success)
                                        {
                                            Console.WriteLine(
                                                "Failed to complete the transaction, and it does not rollback");
                                            regClientCorporateOutput.description =
                                                "Failed to complete the transaction, and it does not rollback";

                                        }
                                        else
                                        {
                                           
                                            regClientCorporateOutput.description = "";
                                        }
                                        RegClientCorporateDataOutputModel_Fail dataOutFail =
                                            new RegClientCorporateDataOutputModel_Fail();
                                        regClientCorporateOutput.data.Add(dataOutFail);
                                    }
                                }
                            }

                            buzCreateCrmClientCorporate cmdCreateCrmClient = new buzCreateCrmClientCorporate();
                            CreateCrmCorporateInfoOutputModel crmContentOutput =
                                (CreateCrmCorporateInfoOutputModel) cmdCreateCrmClient.Execute(regClientCorporateInput);

                            if (crmContentOutput.code == CONST_CODE_SUCCESS)
                            {

                                regClientCorporateOutput.code = CONST_CODE_SUCCESS;
                                regClientCorporateOutput.message = AppConst.MESSAGE_SUCCESS;
                                RegClientCorporateDataOutputModel_Pass dataOutPass =
                                    new RegClientCorporateDataOutputModel_Pass();
                                dataOutPass.cleansingId = regClientCorporateInput.generalHeader.cleansingId;
                                dataOutPass.polisyClientId = regClientCorporateInput.generalHeader.polisyClientId;
                                dataOutPass.crmClientId = crmContentOutput.crmClientId;
                                dataOutPass.corporateName1 = regClientCorporateInput.profileHeader.corporateName1;
                                dataOutPass.corporateName2 = regClientCorporateInput.profileHeader.corporateName2;
                                dataOutPass.corporateBranch = regClientCorporateInput.profileHeader.corporateBranch;

                                regClientCorporateOutput.data.Add(dataOutPass);
                            }
                            else
                            {

                                regClientCorporateOutput.code = CONST_CODE_FAILED;
                                regClientCorporateOutput.message = crmContentOutput.message;
                                regClientCorporateOutput.description = crmContentOutput.description;
                            }

                        }

                }
                else
                {

                    regClientCorporateOutput.code = CONST_CODE_FAILED;
                    regClientCorporateOutput.message = "There is some conflicts among the arguments Look between roleCode and {assessorFlag ,solicitorFlag ,repairerFlag or hospitalFlag}";
                    regClientCorporateOutput.description = "";
                }


            // All Error
            // แก้ตาม ที่ อาจารย์พรชัย บอก เพื่อให้เขาเอา เลข cleansingIdไปซ่อมข้อมูลได้
            if (regClientCorporateOutput.code != AppConst.CODE_SUCCESS)
            {
                if (string.IsNullOrEmpty(regClientCorporateOutput.message))
                {
                    regClientCorporateOutput.message = "Failed client registration did not complete";
                }
                if (regClientCorporateOutput?.data == null || regClientCorporateOutput?.data.Count ==0)
                {
                    regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
                    var dataOutPass =
                        new RegClientCorporateDataOutputModel_Pass
                        {
                            cleansingId = regClientCorporateInput?.generalHeader?.cleansingId??"",
                            polisyClientId = regClientCorporateInput?.generalHeader?.polisyClientId??"",
                            corporateName1 = regClientCorporateInput?.profileHeader?.corporateName1??"",
                            corporateName2 = regClientCorporateInput?.profileHeader?.corporateName2??"",
                            corporateBranch = regClientCorporateInput?.profileHeader?.corporateBranch??""
                        };



                    regClientCorporateOutput.data.Add(dataOutPass);
                }
                //regClientCorporateOutput.data = new List<RegClientCorporateDataOutputModel>();
               // regClientCorporateOutput.data.Add(regClientPersonDataOutput);
            }

            return regClientCorporateOutput;

        }

        bool IsASRHValid(RegClientCorporateInputModel input)
        {
            bool validFlag = true;
            if (

                (input.generalHeader.roleCode == "A" && input.generalHeader.assessorFlag != "Y" ) ||
                (input.generalHeader.roleCode == "S" && input.generalHeader.solicitorFlag != "Y" ) ||
                (input.generalHeader.roleCode == "R" && input.generalHeader.repairerFlag != "Y" ) ||
                (input.generalHeader.roleCode == "H" && input.generalHeader.hospitalFlag != "Y" )
            )
            {

                validFlag = false;
            }
            return validFlag;
        }
    }
}