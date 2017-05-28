using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.InquiryCRMPayeeList;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Ajax.Utilities;

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
                var dataItem = new InquiryCrmPayeeListDataModel
                {
                    sourceData = "COMP",
                    cleansingId = client?.cleansingId,
                    polisyClientId = client?.clientNumber,
                    sapVendorCode = "",
                    sapVendorGroupCode = "",
                    // emcsMemHeadId = ,
                    // emcsMemId ="",
                    //   companyCode ="",
                    title = client?.salutationText,
                    name1 = client?.name1,
                    name2 = client?.name2,
                    fullName = client?.fullName,

                    countryCode = CountryMasterData.Instance.FindByPolisyCode(client?.country)?.CountryCode??"" ,
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
                    hospitalFlag = client?.hospitalFlag

                };
                try
                {
                    dataItem.street1 = client?.address1;
                    dataItem.street2 = client?.address3;
                    dataItem.city = client?.countryText;
                    dataItem.postalCode = client?.postCode;
                    //dataItem.province = client?.address5;
                    dataItem.address = client?.address1 + "|" + client?.address2 + "|" + client?.address3 + "|" +
                                       client?.address4 + "|" + client?.address5 +"|"+ client ?.postCode;

                    // street2 ="",
                    // district = client?.d,
                }
                catch (Exception e)
                {
                    
                }




                //dataItem.AddDebugInfo("COMP JSON Source", polData);
                //dataItem.AddDebugInfo("Transformer", "Tranform_compInqClient_to_crmInqPayeeOut");
                crmInqPayeeOut.data.Add(dataItem);
            }
            /*
            "address1": "222/9 ถนน รนรนรรนรนรนรนรนรนรนร",
            "address2": "ซอย นนสนสนกสนสสนสนสนสนสนสนสนสน",
            "address3": "แยก รบรบบรบรบรบรบรบรบรบรบรบรบร",
            "address4": "ต.คลองตะเคียน อ.พระนครศรีอยุธย",
            "address5": "จ.พระนครศรีอยุธยา",
            "postCode": "13000",
            "countryText": "Thailand",
            */
            crmInqPayeeOut.data = crmInqPayeeOut.data.DistinctBy(row => new { row.sourceData, row.sapVendorCode, row.polisyClientId, row.cleansingId }).ToList();
            return crmInqPayeeOut;

        }

    }
}