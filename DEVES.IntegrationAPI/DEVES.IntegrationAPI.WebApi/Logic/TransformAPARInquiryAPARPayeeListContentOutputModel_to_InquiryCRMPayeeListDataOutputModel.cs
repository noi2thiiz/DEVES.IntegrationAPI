using System;
using System.Collections.Generic;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.APAR;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.SAP;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class
        TransformAPARInquiryAPARPayeeListContentOutputModel_to_InquiryCRMPayeeListDataOutputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            Console.WriteLine("process : TransformAPARInquiryAPARPayeeListContentOutputModel_to_InquiryCRMPayeeListDataOutputModel");
            InquiryAPARPayeeContentModel srcContent = (InquiryAPARPayeeContentModel) input;
            CRMInquiryPayeeContentOutputModel trgtContent = (CRMInquiryPayeeContentOutputModel) output;

            Console.WriteLine("===========InquiryAPARPayeeContentModel Tranform output==========");
            Console.WriteLine("===========Tranform==========");
            Console.WriteLine("===========Tranform==========");

            Console.WriteLine(srcContent.ToJson());


            Console.WriteLine("===========CRMInquiryPayeeContentOutputModel Tranform output==========");
            Console.WriteLine("===========Tranform==========");
            Console.WriteLine("===========Tranform==========");
            Console.WriteLine(trgtContent.ToJson());


            trgtContent.data = new List<InquiryCrmPayeeListDataModel>();


            foreach (var aparPayeeListDataModel in srcContent.aparPayeeListCollection)
            {
                if (aparPayeeListDataModel.aparPayeeList != null)
                {
                    if (aparPayeeListDataModel.aparPayeeList != null)
                    {
                        var aparPayeeList = aparPayeeListDataModel.aparPayeeList;
                        trgtContent.data.Add(new InquiryCrmPayeeListDataModel
                        {
                            polisyClientId = aparPayeeList.polisyClntnum,
                            sapVendorCode = aparPayeeList.vendorCode,
                            sapVendorGroupCode = aparPayeeList.vendorGroupCode,

                            address = aparPayeeList.address,
                            telephone1 = aparPayeeList.telephone1,
                            telephone2 = aparPayeeList.telephone2,

                            faxNo = aparPayeeList.faxNo,

                            taxBranchCode = aparPayeeList.taxBranchCode,
                            fullName =  aparPayeeList.fullName
                        });
                    }
                }
            }


            return trgtContent;
        }
    }
}