using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using CLS = DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLSInquiryCorporateClientContentOut_to_CrmInquiryClientMasterContentOut: BaseTransformer
    {
        public override BaseDataModel TransformModel(Model.BaseDataModel input, BaseDataModel output)
        {
            CLS.CLSInquiryCorporateClientContentOutputModel srcContent = (CLS.CLSInquiryCorporateClientContentOutputModel)input;
            CLS.CLSInquiryCorporateClientOutputModel src = srcContent.data.First<CLS.CLSInquiryCorporateClientOutputModel>();
            CRMInquiryClientContentOutputModel trgtContent = (CRMInquiryClientContentOutputModel)output;
            CRMInquiryClientOutputDataModel trgt = trgtContent.data.First<CRMInquiryClientOutputDataModel>();

            trgt.generalHeader.cleansingId = src.cleansing_id;
            trgt.generalHeader.polisyClientId = src.clntnum;

            trgt.profileInfo.name1 = src.lgivname;
            trgt.profileInfo.name2 = src.lsurname;
            trgt.profileInfo.fullName = src.cls_full_name;
            trgt.profileInfo.salutationText = src.salutl;
            trgt.profileInfo.sex = src.cls_sex;
            trgt.profileInfo.idTax = src.cls_citizen_id_new;
            trgt.profileInfo.occupationText = src.cls_occpcode;

            trgt.contactInfo.telephone1 = src.cltphone01;
            trgt.contactInfo.telephone2 = src.cltphone02;
            trgt.contactInfo.fax = src.cls_fax;
            trgt.contactInfo.contactNumber = src.cls_display_phone;
            trgt.contactInfo.emailAddress = src.email_1;

            Model.CLS.CLSAddressListsCollectionModel addrInfo = src.addressListsCollection.First<Model.CLS.CLSAddressListsCollectionModel>();
            trgt.addressInfo.address = string.Join(CONST_CONCAT, addrInfo.address_1
                                                        , addrInfo.address_2
                                                        , addrInfo.address_3
                                                        , addrInfo.sub_district_display
                                                        , addrInfo.district_display
                                                        , addrInfo.province_display
                                                        , addrInfo.postal_code);
            trgt.addressInfo.countryText = addrInfo.cls_ctrycode_text;
            trgt.addressInfo.addressTypeText = addrInfo.address_type_code;
            trgt.addressInfo.latitude = addrInfo.lattitude;
            trgt.addressInfo.longtitude = addrInfo.longtitude;

            return trgtContent;
        }
    }
}