using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformInquiryCRMPayeeListInputModel_to_InquiryAPARPayeeListInputModel : BaseTransformer
    {

        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            InquiryCRMPayeeListInputModel src = (InquiryCRMPayeeListInputModel)input;
            InquiryAPARPayeeListInputModel trgt = (InquiryAPARPayeeListInputModel)output;

            trgt.fullName = src.fullname ?? "";
            trgt.polisyClntnum = src.polisyClientId ?? "";
            trgt.requester = src.requester ?? "";
           
            trgt.taxBranchCode = src.taxBranchCode ?? "";
            trgt.vendorCode = src.sapVendorCode ?? "";

            trgt.cleansingId = "" + src.cleansingId?.Trim() ?? "";

            trgt.taxNo = src.taxNo ?? "";
            trgt.taxBranchCode  =  src.taxBranchCode ?? "";


            // cannot map to trgt
            //src.emcsCode

            /// Console.WriteLine(trgt.ToJson());

            switch (src.requester)
            {
                case "MC": trgt.requester = "MotorClaim"; break;
                default: trgt.requester = "MotorClaim"; break;
            }

            return trgt;
        }

        

      

           

    }
}