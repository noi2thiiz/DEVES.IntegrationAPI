using System.Collections.Generic;
using System.Web.Http;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.MASTER;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Logic;

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

            InquiryMasterASRHContentASRHListCollectionDataModel data = new InquiryMasterASRHContentASRHListCollectionDataModel();
            data.ASRHList = new InquiryMasterASRHListModel();
            content.ASRHListCollection = new List<InquiryMasterASRHContentASRHListCollectionDataModel>();
            content.ASRHListCollection.Add(data);
            content.ASRHListCollection.Add(data);
            //content.ASRHListCollection.Add(collection);
            //  content.ASRHListCollection.Add(collection);
            model.content = content;

            return Ok(model);
        }

        [HttpGet]
        [Route("InquiryAPARPayeeOutputModel")]
        public IHttpActionResult InquiryAPARPayeeOutputModel()
        {
            var model = new InquiryAPARPayeeOutputModel();
            model.content = new InquiryAPARPayeeContentModel();
            model.content.aparPayeeListCollection = new List<InquiryAPARPayeeContentAparPayeeListCollectionDataModel>();
            var colData = new InquiryAPARPayeeContentAparPayeeListCollectionDataModel();
            colData.aparPayeeList = new InquiryAPARPayeeListModel();
            model.content.aparPayeeListCollection.Add(colData);
            model.content.aparPayeeListCollection.Add(colData);

            return Ok(model);
        }

        [HttpGet]
        [Route("COMPInquiryClientMasterOutputModel")]
        public IHttpActionResult COMPInquiryClientMasterOutputModel()
        {
            var model = new COMPInquiryClientMasterOutputModel();
            model.content = new EWIResCOMPInquiryClientMasterContentModel();
            model.content.clientListCollection = new List<COMPInquiryClientMasterContentClientListModel>();
            var colData = new COMPInquiryClientMasterContentClientListModel();
            colData.clientList = new COMPInquiryClientMasterClientModel();
            model.content.clientListCollection.Add(colData);
            model.content.clientListCollection.Add(colData);

            return Ok(model);
        }


        [HttpGet]
        [Route("SAPInquiryVendorOutputModel")]
        public IHttpActionResult SAPInquiryVendorOutputModel()
        {
            var model = new EWIResSAPInquiryVendorContentModel();
            model.Status = new SAPInquiryVendorContentStatusModel();
            model.VendorInfo = new List<SAPInquiryVendorContentVendorInfoModel>();
            var colData = new SAPInquiryVendorContentVendorInfoModel();
            colData.BankInfo = new List<SAPInquiryVendorBankInfoModel>();
            colData.BankInfo.Add( new SAPInquiryVendorBankInfoModel());
            model.VendorInfo.Add(colData);


            return Ok(model);
        }

        [HttpGet]
        [Route("CRMInquiryPayeeContentOutputModel")]
        public IHttpActionResult CRMInquiryPayeeContentOutputModel()
        {
            var model = new CRMInquiryPayeeContentOutputModel();
            model.data = new List<InquiryCrmPayeeListDataModel>();

            model.data.Add(new InquiryCrmPayeeListDataModel());



            return Ok(model);
        }


        [HttpGet]
        [Route("SAPMap")]
        public IHttpActionResult SAPMap()
        {
            var model2 = getCRMInquiryPayeeContentOutputModel();
            var model1 = getSAPInquiryVendorOutputModel();
            var t = new TransformSAPInquiryVendorOutputModel_to_InquiryCRMPayeeListDataOutputModel();
            var output = t.TransformModel(model1, model2);
            return Ok(output);
        }

        public CRMInquiryPayeeContentOutputModel getCRMInquiryPayeeContentOutputModel()
        {
            var model = new CRMInquiryPayeeContentOutputModel();
            model.data = new List<InquiryCrmPayeeListDataModel>();

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

        public EWIResSAPInquiryVendorContentModel getSAPInquiryVendorOutputModel()
        {
            var model = new EWIResSAPInquiryVendorContentModel();
            model.Status = new SAPInquiryVendorContentStatusModel();
            model.VendorInfo = new List<SAPInquiryVendorContentVendorInfoModel>();
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
            colData.BankInfo = new List<SAPInquiryVendorBankInfoModel>();
            colData.BankInfo.Add( new SAPInquiryVendorBankInfoModel
            {
                ACCTHOLDER = "xxxx",
                BANKBRANCH = "ccc"
            });
            model.VendorInfo.Add(colData);
            model.VendorInfo.Add(colData2);
            model.VendorInfo.Add(colData3);
            return model;
        }




        [HttpGet]
        [Route("InquiryAPARPayeeContentModel")]
        public IHttpActionResult InquiryAPARPayeeContentModel()
        {
            var model = getInquiryAPARPayeeContentModel();


            return Ok(model);
        }

        public InquiryAPARPayeeContentModel getInquiryAPARPayeeContentModel()
        {
            var model = new InquiryAPARPayeeContentModel();
            model.aparPayeeListCollection = new List<InquiryAPARPayeeContentAparPayeeListCollectionDataModel>();

            model.aparPayeeListCollection.Add(new InquiryAPARPayeeContentAparPayeeListCollectionDataModel
            {
                aparPayeeList = new InquiryAPARPayeeListModel()
            });
            return model;
        }

        [HttpGet]
        [Route("CLSCreatePersonalClientOutputModel")]
        public IHttpActionResult CLSCreatePersonalClientOutputModel()
        {
            var model = new CLSCreatePersonalClientOutputModel();
            model.content = new CLSCreatePersonalClientContentOutputModel();
            model.content.data = new CLSCreatePersonalClientDataOutputModel();


            return Ok(model);
        }

        [HttpGet]
        [Route("CLIENTCreatePersonalClientAndAdditionalInfoOutputModel")]
        public IHttpActionResult CLIENTCreatePersonalClientAndAdditionalInfoOutputModel()
        {
            var model = new CLIENTCreatePersonalClientAndAdditionalInfoOutputModel();
            model.content = new CLIENTCreatePersonalClientAndAdditionalInfoContentModel();
           

            return Ok(model);
        }

        [HttpGet]
        [Route("CLIENTCreateCorporateClientAndAdditionalInfoOutputModel")]
        public IHttpActionResult CLIENTCreateCorporateClientAndAdditionalInfoOutputModel()
        {
            var model = new CLIENTCreateCorporateClientAndAdditionalInfoOutputModel();
            model.content = new CLIENTCreateCorporateClientAndAdditionalInfoContentModel();


            return Ok(model);
        }


        

    }
}