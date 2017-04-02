using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCRMInquiryCRMPayeeListInputModel_to_APARInquiryAPARPayeeListInputModel: BaseTransformer
    {

        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel)input;
            InquiryAPARPayeeListInputModel trgt = (InquiryAPARPayeeListInputModel)output;

            trgt.fullName = src.fullname;
            trgt.polisyClntnum = src.polisyClientId;
            trgt.requester = src.requester;
            trgt.taxBranchCode = src.taxBranchCode;
            trgt.vendorCode = src.sapVendorCode;

            trgt.taxNo = src.taxNo;
            trgt.taxBranchCode  =  src.taxBranchCode;

            // cannot map to trgt
            //src.emcsCode

           /// Console.WriteLine(trgt.ToJson());

            return trgt;
        }

        

      

           

    }
}