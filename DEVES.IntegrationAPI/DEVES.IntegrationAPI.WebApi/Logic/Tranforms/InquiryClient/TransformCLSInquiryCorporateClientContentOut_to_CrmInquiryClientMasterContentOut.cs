using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using CLS = DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut: BaseTransformer
    {
        public override BaseDataModel TransformModel(Model.BaseDataModel input, BaseDataModel output)
        {
            CLS.CLSInquiryCorporateClientContentOutputModel srcContent = (CLS.CLSInquiryCorporateClientContentOutputModel)input;
            CRMInquiryClientContentOutputModel trgtContent = (CRMInquiryClientContentOutputModel)output;

            if (srcContent == null)
            {
                return trgtContent;
            }
            if (trgtContent ==null)
            {
                trgtContent = new CRMInquiryClientContentOutputModel();
               
            }
            if (trgtContent?.data == null)
            {
                trgtContent.data = new List<CRMInquiryClientOutputDataModel>();
            }
            if (srcContent?.data != null)
            {
                foreach (CLS.CLSInquiryCorporateClientOutputModel src in srcContent.data)
                {
                    CRMInquiryClientOutputDataModel trgt =
                        new CRMInquiryClientOutputDataModel
                        {
                            addressInfo = new CRMInquiryClientAddressInfoModel(),
                            asrhHeader = new CRMInquiryClientAsrhHeaderModel(),
                            contactInfo = new CRMInquiryClientContactInfoModel(),
                            generalHeader = new CRMInquiryClientGeneralHeaderModel(),
                            profileInfo = new CRMInquiryClientProfileInfoModel()
                        };
                    trgt.generalHeader.cleansingId = src?.cleansing_id?.Trim() ?? "";
                    trgt.generalHeader.polisyClientId = src?.clntnum?.Trim() ?? "";
                    trgt.generalHeader.sourceData = CommonConstant.CONST_SYSTEM_CLS;

                    trgt.profileInfo.name1 = src?.lgivname?.Trim() ?? "";
                    trgt.profileInfo.name2 = src?.lsurname?.Trim() ?? "";
                    trgt.profileInfo.fullName = src?.cls_full_name?.Trim() ?? "";
                    
                    
                    trgt.profileInfo.sex = "";
                    trgt.profileInfo.salutationText = "";

                    




                    // trgt.profileInfo.sex = src.cls_sex;
                    trgt.profileInfo.idTax = src?.cls_tax_no_new?.Trim() ?? "";
                    trgt.profileInfo.corporateBranch = src?.corporate_staff_no?.Trim() ?? "";

                    try
                    {
                        trgt.profileInfo.occupationText=
                            OccupationMasterData.Instance.FindByCode(isNull(src?.cls_occpcode), "00023")?.Name?? "";
                       


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("72"+e.Message);
                    }


                
                    trgt.contactInfo.telephone1 = src?.cltphone01?.Trim() ?? "";
                    trgt.contactInfo.telephone2 = src?.cltphone02?.Trim() ?? "";
                    trgt.contactInfo.fax = src?.cls_fax?.Trim() ?? "";
                    trgt.contactInfo.contactNumber = src?.cls_display_phone?.Trim() ?? "";
                    trgt.contactInfo.emailAddress = src?.email_1?.Trim() ?? "";
                   

                    var addrInfo = src?.addressListsCollection?.FirstOrDefault<Model.CLS.CLSAddressListsCollectionModel>();
                    if (addrInfo != null)
                    {
                        trgt.addressInfo.address = string.Join(CONST_CONCAT, addrInfo.address_1?.Trim() ?? ""
                                                                , addrInfo.address_2?.Trim() ?? ""
                                                                , addrInfo.address_3?.Trim() ?? ""
                                                                , addrInfo.sub_district_display?.Trim() ?? ""
                                                                , addrInfo.district_display?.Trim() ?? ""
                                                                , addrInfo.province_display?.Trim() ?? ""
                                                                , addrInfo.postal_code?.Trim() ?? "");
                        trgt.addressInfo.countryText = addrInfo?.cls_ctrycode_text?.Trim()??"";
                      
                        try
                        {
                            var masterAddressType =
                                AddressTypeMasterData.Instance.FindByCode(isNull(addrInfo?.address_type_code), "01");
                            trgt.addressInfo.addressTypeText = masterAddressType?.Name;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("104:"+e.Message);
                        }
                      
                        trgt.addressInfo.latitude = addrInfo?.lattitude?.Trim() ?? "";
                        trgt.addressInfo.longtitude = addrInfo?.longtitude?.Trim() ?? "";
                    }
                    // trgt.AddDebugInfo("TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut", "");
                    // trgt.AddDebugInfo("Source Data", src);

                  
                    trgtContent.data.Add(trgt);
                   
                }
            }
            // trgtContent.AddDebugInfo("TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut", "");
          
            return trgtContent;
        }
        public string isNull(string a)
        {
            if (a == null)
            {
                return "";
            }
            return a;
        }
    }
}