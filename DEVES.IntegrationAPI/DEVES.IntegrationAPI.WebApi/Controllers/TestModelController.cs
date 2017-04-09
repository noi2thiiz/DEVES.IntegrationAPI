using System.Collections.Generic;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using Microsoft.Xrm.Sdk;
using DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity;
using System;
using DEVES.IntegrationAPI.WebApi.Services.DataGateWay;
using DEVES.IntegrationAPI.Model.CTI;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    [RoutePrefix("api/test-model")]
    public class TestModelController:ApiController
    {

        [HttpGet]
        [Route("InquiryMasterASRHOutputModel")]
        public IHttpActionResult InquiryMasterASRHOutputModel()
        {
            var model = new InquiryMasterASRHOutputModel();
            var content = new InquiryMasterASRHContentModel();

            InquiryMasterASRHContentASRHListCollectionDataModel data = new InquiryMasterASRHContentASRHListCollectionDataModel()
            {
                ASRHList = new InquiryMasterASRHListModel()
            };
            content.ASRHListCollection = new List<InquiryMasterASRHContentASRHListCollectionDataModel>
            {
                data,
                data
            };
            //content.ASRHListCollection.Add(collection);
            //  content.ASRHListCollection.Add(collection);
            model.content = content;

            return Ok(model);
        }

        [HttpGet]
        [Route("InquiryAPARPayeeOutputModel")]
        public IHttpActionResult InquiryAPARPayeeOutputModel()
        {
            var model = new InquiryAPARPayeeOutputModel()
            {
                content = new InquiryAPARPayeeContentModel()
            };
            model.content.aparPayeeListCollection = new List<InquiryAPARPayeeContentAparPayeeListCollectionDataModel>();
            var colData = new InquiryAPARPayeeContentAparPayeeListCollectionDataModel()
            {
                aparPayeeList = new InquiryAPARPayeeListModel()
            };
            model.content.aparPayeeListCollection.Add(colData);
            model.content.aparPayeeListCollection.Add(colData);

            return Ok(model);
        }

        [HttpGet]
        [Route("COMPInquiryClientMasterOutputModel")]
        public IHttpActionResult COMPInquiryClientMasterOutputModel()
        {
            var model = new COMPInquiryClientMasterOutputModel()
            {
                content = new EWIResCOMPInquiryClientMasterContentModel()
            };
            model.content.clientListCollection = new List<COMPInquiryClientMasterContentClientListModel>();
            var colData = new COMPInquiryClientMasterContentClientListModel()
            {
                clientList = new COMPInquiryClientMasterClientModel()
            };
            model.content.clientListCollection.Add(colData);
            model.content.clientListCollection.Add(colData);

            return Ok(model);
        }


        [HttpGet]
        [Route("SAPInquiryVendorOutputModel")]
        public IHttpActionResult SAPInquiryVendorOutputModel()
        {
            var model = new EWIResSAPInquiryVendorContentModel()
            {
                Status = new SAPInquiryVendorContentStatusModel(),
                VendorInfo = new List<SAPInquiryVendorContentVendorInfoModel>()
            };
            var colData = new SAPInquiryVendorContentVendorInfoModel()
            {
                BankInfo = new List<SAPInquiryVendorBankInfoModel>()
            };
            colData.BankInfo.Add( new SAPInquiryVendorBankInfoModel());
            model.VendorInfo.Add(colData);


            return Ok(model);
        }

        [HttpGet]
        [Route("CRMInquiryPayeeContentOutputModel")]
        public IHttpActionResult CRMInquiryPayeeContentOutputModel()
        {
            var model = new CRMInquiryPayeeContentOutputModel()
            {
                data = new List<InquiryCrmPayeeListDataModel>()
            };
            model.data.Add(new InquiryCrmPayeeListDataModel());



            return Ok(model);
        }


        [HttpGet]
        [Route("SAPMap")]
        public IHttpActionResult SAPMap()
        {
            var model2 = GetCRMInquiryPayeeContentOutputModel();
            var model1 = GetSAPInquiryVendorOutputModel();
            var t = new TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel();
            var output = t.TransformModel(model1, model2);
            return Ok(output);
        }

        public CRMInquiryPayeeContentOutputModel GetCRMInquiryPayeeContentOutputModel()
        {
            var model = new CRMInquiryPayeeContentOutputModel()
            {
                data = new List<InquiryCrmPayeeListDataModel>()
            };
            model.data.Add(new InquiryCrmPayeeListDataModel
            {
                polisyClientId= "77777",name1 = "CRM_NAME_7777",name2 = ""

            });
            model.data.Add(new InquiryCrmPayeeListDataModel
            {
                polisyClientId= "88888",name1 = "CRM_NAME_88888"
            });
            return model;
        }

        public EWIResSAPInquiryVendorContentModel GetSAPInquiryVendorOutputModel()
        {
            var model = new EWIResSAPInquiryVendorContentModel()
            {
                Status = new SAPInquiryVendorContentStatusModel(),
                VendorInfo = new List<SAPInquiryVendorContentVendorInfoModel>()
            };
            var colData = new SAPInquiryVendorContentVendorInfoModel
            {
                PREVACC = "77777",
                NAME1 = "NAME_777777",
                NAME2 = "xxxxxxxxxxx"
            };
            var colData2 = new SAPInquiryVendorContentVendorInfoModel
            {
                PREVACC = "88888",
                NAME1 = "NAME_88888",
                NAME2 = "yyyyyyyyyyy"
            };
            var colData3 = new SAPInquiryVendorContentVendorInfoModel
            {
                PREVACC = "99999",
                NAME1 = "NAME_99999",
                NAME2 = "zzzzzzzzzzzzzz"
            };
            colData.BankInfo = new List<SAPInquiryVendorBankInfoModel>
            {
                new SAPInquiryVendorBankInfoModel
                {
                    ACCTHOLDER = "xxxx",
                    BANKBRANCH = "ccc"
                }
            };
            model.VendorInfo.Add(colData);
            model.VendorInfo.Add(colData2);
            model.VendorInfo.Add(colData3);
            return model;
        }




        [HttpGet]
        [Route("InquiryAPARPayeeContentModel")]
        public IHttpActionResult InquiryAPARPayeeContentModel()
        {
            var model = GetInquiryAPARPayeeContentModel();


            return Ok(model);
        }

        public InquiryAPARPayeeContentModel GetInquiryAPARPayeeContentModel()
        {
            var model = new InquiryAPARPayeeContentModel()
            {
                aparPayeeListCollection = new List<InquiryAPARPayeeContentAparPayeeListCollectionDataModel>
            {
                new InquiryAPARPayeeContentAparPayeeListCollectionDataModel
                {
                    aparPayeeList = new InquiryAPARPayeeListModel()
                }
            }
            };
            return model;
        }

        [HttpGet]
        [Route("CLSCreatePersonalClientOutputModel")]
        public IHttpActionResult CLSCreatePersonalClientOutputModel()
        {
            var model = new CLSCreatePersonalClientOutputModel()
            {
                content = new CLSCreatePersonalClientContentOutputModel()
            };
            model.content.data = new CLSCreatePersonalClientDataOutputModel();


            return Ok(model);
        }

        [HttpGet]
        [Route("CLIENTCreatePersonalClientAndAdditionalInfoOutputModel")]
        public IHttpActionResult CLIENTCreatePersonalClientAndAdditionalInfoOutputModel()
        {
            var model = new CLIENTCreatePersonalClientAndAdditionalInfoOutputModel()
            {
                content = new CLIENTCreatePersonalClientAndAdditionalInfoContentModel()
            };
            return Ok(model);
        }

        [HttpGet]
        [Route("CLIENTCreateCorporateClientAndAdditionalInfoOutputModel")]
        public IHttpActionResult CLIENTCreateCorporateClientAndAdditionalInfoOutputModel()
        {
            var model = new CLIENTCreateCorporateClientAndAdditionalInfoOutputModel()
            {
                content = new CLIENTCreateCorporateClientAndAdditionalInfoContentModel()
            };
            return Ok(model);
        }

        [HttpGet]
        [Route("CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel")]
        public IHttpActionResult CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel()
        {
            var model = new CLIENTUpdateCorporateClientAndAdditionalInfoOutputModel()
            {
                content = new CLIENTUpdateCorporateClientAndAdditionalInfoContentModel()
            };
            return Ok(model);
        }

        [HttpGet]
        [Route("TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel")]
        public IHttpActionResult TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel()
        {
            var src =  new RegClientPersonalInputModel();
            var trgt = new CLSCreatePersonalClientInputModel();

            var tranformer = new TranformRegClientPersonalInputModel_to_CLSCreatePersonalClientInputModel();
            var output = tranformer.TransformModel(src, trgt);

            return Ok(output);
        }

        [HttpGet]
        [Route("TranformRegClientPersonalInputModel_to_CLIENTCreatePersonalClientAndAdditionalInfoInputModel")]
        public IHttpActionResult TranformRegClientPersonalInputModel_to_CLIENTCreatePersonalClientAndAdditionalInfoInputModel()
        {
            var src = new RegClientPersonalInputModel()
            {
                profileInfo = new ProfileInfoModel()
            };
            src.profileInfo.idDriving = "1111";
            var trgt = new CLIENTCreatePersonalClientAndAdditionalInfoInputModel();
            

            var tranformer = new TranformRegClientPersonalInputModel_to_CLIENTCreatePersonalClientAndAdditionalInfoInputModel();
            var output = tranformer.TransformModel(src, trgt);

            return Ok(output);
        }

        [HttpGet]
        [Route("findPolicyMotor")]
        public IHttpActionResult FindPolicyMotor()
        {
            CRMPolicyMotorDataGateWay policyDataGateway = new CRMPolicyMotorDataGateWay();
            var reqPolicy = new DataRequest();
            reqPolicy.AddParam("policyNo", "2010034076831");
            reqPolicy.AddParam("chassisNo", "");
            reqPolicy.AddParam("carRegisNo", "ตม4533");
            reqPolicy.AddParam("carRegisProve", "");
            var result = policyDataGateway.FetchAll(reqPolicy);

            return Ok(result);
        }
        [HttpGet]
        [Route("findCustomerClient")]
        public IHttpActionResult FindCustomerClient()
        {
            var clientDb = new ClientMasterDataGateway();
            var req= new DataRequest();
            req.AddParam("clientType", "P");
            req.AddParam("clientId", "CRM5555");
        
            var result = clientDb.FetchAll(req);

            return Ok(result);
        }

       [HttpGet]
        [Route("createIncident")]
        public IHttpActionResult CreateIncident()
        {
            // B55765F1 - C4A4 - E611 - 80CA - 0050568D1874
            //Account With Id = b55765f1-c4a4-e611-80ca-0050568d1874 Does Not Exist //ACCOUNT
            var policyAdditionalIdGuid = new Guid("3BFCB0A4-DCB6-E611-80CA-0050568D1874");

            var customeGuid = new Guid("b55765f1-c4a4-e611-80ca-0050568d1874");//account
            var informerGuid = new Guid("B55765F1-C4A4-E611-80CA-0050568D1874");//contract
            var driverGuid = new Guid("B55765F1-C4A4-E611-80CA-0050568D1874");//contract
            var policyGuid = new Guid("B55765F1-C4A4-E611-80CA-0050568D1874");//contract
            //e3c8d35e-aeb6-e611-80ca-0050568d1874 Does Not Exist
            var incidentEntity = new IncidentEntity(policyAdditionalIdGuid, customeGuid, informerGuid, driverGuid, policyGuid)
            {
                caseorigincode = null,
                pfc_case_vip = false,
                pfc_policy_additional_number = "C7121677", /* "policyNo" */
                                                           /*(policyAdditional.pfc_policy_vip) (Policy.pfc_policy_mc_nmc) */
                pfc_policy_vip = false, // Default 0 ;
                pfc_policy_mc_nmc = null, //Default(Unassign)
                                          //(policyAdditional.pfc_reg_num)
                                          //(policyAdditional.pfc_reg_num_prov)
                pfc_current_reg_num = "", //api.currentCarRegisterNo
                pfc_current_reg_num_prov = "",//api.currentCarRegisterProv
                casetypecode = new OptionSetValue(2), //fix: 2 ( Service Request )
                pfc_source_data = new OptionSetValue(100000002),
                pfc_customer_vip = false, //default 
                                          //account/contact.pfc_customer_sensitive_level
                pfc_customer_sensitive = new OptionSetValue(100000000)
            };
            ; //Low: 100,000,000, Medium:100,000,001, High:100,000,002
            incidentEntity.pfc_customer_privilege = null;



            PfcIncidentDataGateWay db = new PfcIncidentDataGateWay();
            var guid = db.Create(incidentEntity);
            return Ok(guid);
        }
        [HttpGet]
        [Route("sendVoiceRecord")]
        public IHttpActionResult SendVoiceRecord()
        {
            VoiceRecordDataGateWay db = new VoiceRecordDataGateWay();
            VoiceRecordRequestModel model = new VoiceRecordRequestModel()
            {
                callednumber = "99999"
            };
            model.callednumber = "8888";
            model.callType = "in";
            model.url = "http://test.co.th";
            var reuslt = db.Create("693B360D-C5A4-E611-80CA-0050568D1874", model);
            return Ok(reuslt);
        }
     
    }
}