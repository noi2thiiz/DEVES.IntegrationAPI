<<<<<<< .mine
using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformAPARInquiryAPARPayeeListContentOutputModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        private Dictionary<string, InquiryAPARPayeeContentAparPayeeListCollectionDataModel> _tmpInquiryAPARPayeeOutputModel;
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryAPARPayeeContentModel srcContent = (InquiryAPARPayeeContentModel) input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel) output;


            _tmpInquiryAPARPayeeOutputModel = new Dictionary<string, InquiryAPARPayeeContentAparPayeeListCollectionDataModel>();
            CRMInquiryPayeeContentOutputModel outputContent =
                new CRMInquiryPayeeContentOutputModel {data = new List<InquiryCrmPayeeListDataModel>()};



            foreach (var aparPayeeListDataModel in srcContent.aparPayeeListCollection)
            {
                if (aparPayeeListDataModel.aparPayeeList != null)
                {
                    if (!string.IsNullOrEmpty(aparPayeeListDataModel.aparPayeeList.polisyClntnum ))
                    {
                        _tmpInquiryAPARPayeeOutputModel.Add(aparPayeeListDataModel.aparPayeeList.polisyClntnum, aparPayeeListDataModel);
                    }
                }

            }

            foreach (var dataItrm in trgtContent.data)
            {
                if (dataItrm.polisyClientId != null &&
                    _tmpInquiryAPARPayeeOutputModel.ContainsKey(dataItrm.polisyClientId))
                {
                    var _srcContentData = _tmpInquiryAPARPayeeOutputModel[dataItrm.polisyClientId];


                    outputContent.data.Add(TransformDataModel(_srcContentData, dataItrm));
                }
                else
                {
                    outputContent.data.Add(TransformDataModel(new InquiryAPARPayeeContentAparPayeeListCollectionDataModel(), dataItrm));
                }
            }

            return outputContent;

            return trgtContent;
        }

        public InquiryCrmPayeeListDataModel TransformDataModel(InquiryAPARPayeeContentAparPayeeListCollectionDataModel input,
            InquiryCrmPayeeListDataModel output)
        {
            if (input.aparPayeeList != null)
            {

                output.polisyClientId = input.aparPayeeList.polisyClntnum;
                output.sapVendorCode = input.aparPayeeList.vendorCode;
                output.sapVendorGroupCode = input.aparPayeeList.vendorGroupCode;
                //dataItrm.emcsMemHeadId = "";
                //dataItrm.emcsMemId = "";
                //  output.companyCode = input.COMPANY;
                //  output.title = input.TITLE;
                // output.name1 = input.NAME1;
                // output.name2 = input.NAME2;
                // dataItrm.fullName = "";
                // output.street1 = input.STREET1;
                // output.street2 = input.STREET2;
                // output.district = input.DISTRICT;

                // output.city = input.CITY;
                // output.postalCode = input.POSTCODE;
                // output.countryCode = input.COUNTRY;
                // output.countryCodeDesc = input.COUNTRY_DESC;
                output.address = input.aparPayeeList.address;
                output.telephone1 = input.aparPayeeList.telephone1;
                output.telephone2 = input.aparPayeeList.telephone2;

                output.faxNo = input.aparPayeeList.faxNo;
                //output.contactNumber = "";
                //  output.taxNo = input.TAX3;
                output.taxBranchCode = input.aparPayeeList.taxBranchCode;


                // output.paymentTerm = input.PAYTERM;
                // output.paymentTermDesc = input.PAYTERM_DESC;
                // output.paymentMethods = input.PAYMETHOD;
                //output.inactive = input.INACTIVE;

            }




            return output;
        }



    }
}
=======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformAPARInquiryAPARPayeeListContentOutputModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            throw new NotImplementedException();
        }
    }
}




























































































>>>>>>> .theirs
