using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class Tranform_compInqClient_to_crmInqPayeeOut : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            EWIResCOMPInquiryClientMasterContentModel retCOMPInqClient = (EWIResCOMPInquiryClientMasterContentModel)input;
            CRMInquiryPayeeContentOutputModel crmInqPayeeOut = (CRMInquiryPayeeContentOutputModel)output
                                                            ?? (CRMInquiryPayeeContentOutputModel)DataModelFactory.GetModel(typeof(CRMInquiryPayeeContentOutputModel));

            #region prevent null reference
            if (retCOMPInqClient?.clientListCollection == null)
            {
                return crmInqPayeeOut;
            }
            if (crmInqPayeeOut.data == null)
            {
                crmInqPayeeOut.data = new List<InquiryCrmPayeeListDataModel>();
            }
            #endregion

            foreach (var polData in retCOMPInqClient.clientListCollection)
            {
                var client = polData?.clientList;
                crmInqPayeeOut.data.Add(new InquiryCrmPayeeListDataModel
                {
                     sourceData = "COMP",
                     cleansingId = client?.cleansingId,
                     polisyClientId = client?.clientNumber,
                     sapVendorCode ="",
                     sapVendorGroupCode ="",
                    // emcsMemHeadId = ,
                    // emcsMemId ="",
                  //   companyCode ="",
                     title = client?.salutation,
                     name1 = client?.name1,
                     name2 = client?.name2,
                     fullName = client?.fullName,
                    // street1="",
                    // street2 ="",
                    // district ="",
                     //city = client?.,
                     postalCode = client?.postCode,
                     countryCode = client?.country,
                     countryCodeDesc = client?.countryText,
                    // address ="",
                     telephone1 = client?.telephone1,
                     telephone2 = client?.telephone2,
                     faxNo = client?.fax,
                 //  contactNumber ="",
                     taxNo = client?.taxIdNumber,
                   //taxBranchCode ="",
                   //paymentTerm="",
                   //paymentTermDesc ="",
                   //paymentMethods = ,
                   //inactive = ,
                     assessorFlag = client?.assessorFlag,
                     solicitorFlag = client?.solicitorFlag,
                     repairerFlag = client?.repairerFlag,
                     hospitalFlag= client?.hospitalFlag

                });
            }
           
            return crmInqPayeeOut;

        }

    }
}