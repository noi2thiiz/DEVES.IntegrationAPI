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

            if (srcContent.data != null)
            {
                foreach (CLS.CLSInquiryCorporateClientOutputModel src in srcContent.data)
                {
                    CRMInquiryClientOutputDataModel trgt = new CRMInquiryClientOutputDataModel();
                    trgt.addressInfo = new CRMInquiryClientAddressInfoModel();
                    trgt.asrhHeader = new CRMInquiryClientAsrhHeaderModel();
                    trgt.contactInfo = new CRMInquiryClientContactInfoModel();
                    trgt.generalHeader = new CRMInquiryClientGeneralHeaderModel();
                    trgt.profileInfo = new CRMInquiryClientProfileInfoModel();


                    trgt.generalHeader.cleansingId = src.cleansing_id;
                    trgt.generalHeader.polisyClientId = src.clntnum;

                    trgt.profileInfo.name1 = src.lgivname;
                    trgt.profileInfo.name2 = src.lsurname;
                    trgt.profileInfo.fullName = src.cls_full_name;
                    // trgt.profileInfo.salutationText = src.salutl;
                    if(!src.cls_sex.Equals("M") || !src.cls_sex.Equals("F"))
                    {
                        trgt.profileInfo.sex = "U";
                    }
                    // trgt.profileInfo.sex = src.cls_sex;
                    trgt.profileInfo.idTax = src.cls_tax_no_new;

                    var master_occupation = OccupationMasterData.Instance.FindByCode(isNull(src.cls_occpcode), "00023");
                    trgt.profileInfo.occupationText = master_occupation.Name;

                    trgt.contactInfo.telephone1 = src.cltphone01;
                    trgt.contactInfo.telephone2 = src.cltphone02;
                    trgt.contactInfo.fax = src.cls_fax;
                    trgt.contactInfo.contactNumber = src.cls_display_phone;
                    trgt.contactInfo.emailAddress = src.email_1;

                    var addrInfo = src.addressListsCollection.FirstOrDefault<Model.CLS.CLSAddressListsCollectionModel>();
                    if (addrInfo != null)
                    {
                        trgt.addressInfo.address = string.Join(CONST_CONCAT, addrInfo.address_1
                                                                , addrInfo.address_2
                                                                , addrInfo.address_3
                                                                , addrInfo.sub_district_display
                                                                , addrInfo.district_display
                                                                , addrInfo.province_display
                                                                , addrInfo.postal_code);
                        trgt.addressInfo.countryText = addrInfo.cls_ctrycode_text;

                        var master_addressType = AddressTypeMasterData.Instance.FindByCode(isNull(addrInfo.address_type_code), "01");
                        trgt.addressInfo.addressTypeText = master_addressType.Name;
                        trgt.addressInfo.latitude = addrInfo.lattitude;
                        trgt.addressInfo.longtitude = addrInfo.longtitude;
                    }

                    trgtContent.data.Add(trgt);
                }
            }

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