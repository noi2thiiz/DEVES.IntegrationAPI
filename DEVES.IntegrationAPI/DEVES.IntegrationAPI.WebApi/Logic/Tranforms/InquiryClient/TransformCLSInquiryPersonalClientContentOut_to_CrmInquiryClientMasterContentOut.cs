using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using CLS = DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.Core.Helper;
//TODO เพิ่ม Source Data= Integration
namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformCLSInquiryPersonalClientContentOut_to_CrmInquiryClientMasterContentOut : BaseTransformer
    {
        public override BaseDataModel TransformModel (Model.BaseDataModel input, BaseDataModel output)
        {
            /*
             * ToDo:    Correct the case that there are many records input
             *          1. Loop through the input, 
             *          2. create a new output, 
             *          3. transfer data from input to output
            */

            CLS.CLSInquiryPersonalClientContentOutputModel srcContent = (CLS.CLSInquiryPersonalClientContentOutputModel)input;
            CRMInquiryClientContentOutputModel trgtContent = (CRMInquiryClientContentOutputModel)output;
            if (trgtContent == null)
            {
                trgtContent = new CRMInquiryClientContentOutputModel();
            }
            if (trgtContent.data == null)
            {
                trgtContent.data = new List<CRMInquiryClientOutputDataModel>();

            }
           
            foreach( CLS.CLSInquiryPersonalClientOutputModel src in srcContent.data)
            {
                try
                {
                    CRMInquiryClientOutputDataModel trgt = new CRMInquiryClientOutputDataModel();
                    trgt.addressInfo = new CRMInquiryClientAddressInfoModel();
                    trgt.asrhHeader = new CRMInquiryClientAsrhHeaderModel();
                    trgt.contactInfo = new CRMInquiryClientContactInfoModel();
                    trgt.generalHeader = new CRMInquiryClientGeneralHeaderModel();
                    trgt.profileInfo = new CRMInquiryClientProfileInfoModel();


                    trgt.generalHeader.cleansingId = src?.cleansing_id?.Trim() ?? "";
                    trgt.generalHeader.polisyClientId = src?.clntnum?.Trim() ?? "";
                    trgt.generalHeader.sourceData = CommonConstant.CONST_SYSTEM_CLS;
                    trgt.generalHeader.clientType = src?.clientType;
                    trgt.profileInfo.clientStatus = src?.cltstat?.Trim().ToUpper() ?? "";


                    trgt.profileInfo.name1 = src?.lgivname?.Trim() ?? "";
                    trgt.profileInfo.name2 = src?.lsurname?.Trim() ?? "";
                    trgt.profileInfo.fullName = src?.cls_full_name?.Trim() ?? "";

                    try
                    {
                        trgt.profileInfo.salutationText = PersonalTitleMasterData.Instance.FindByCode(src?.salutl)?.Name??"";
                      

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    

                    trgt.profileInfo.sex = ((new[] {"M", "F","U"}).Contains(src.cls_sex.ToUpperIgnoreNull()))
                        ? src.cls_sex.ToUpperIgnoreNull()
                        : "U";

                    //trgt.profileInfo.sex = src.cls_sex;
                    trgt.profileInfo.idCard = src.cls_citizen_id_new;

                    trgt.profileInfo.vipStatus = ((new[] {"Y", "N"}).Contains(src.cls_vip))
                        ? src.cls_vip.ToUpperIgnoreNull()
                        : "N";



                    try
                    {
                        trgt.profileInfo.occupationText = OccupationMasterData.Instance.FindByCode(src?.cls_occpcode, "00023")?.Name??"";
                        

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    
                    trgt.contactInfo.telephone1 = src?.cltphone01?.Trim() ?? "";
                    trgt.contactInfo.telephone2 = src?.cltphone02?.Trim() ?? "";
                    trgt.contactInfo.fax = src?.cls_fax?.Trim() ?? "";
                    trgt.contactInfo.contactNumber = src?.cls_display_phone?.Trim() ?? "";
                    trgt.contactInfo.emailAddress = src?.email_1?.Trim() ?? "";

                    var addrInfo = src.addressListsCollection.FirstOrDefault<Model.CLS.CLSAddressListsCollectionModel>();
                    if (addrInfo != null)
                    {
                        trgt.addressInfo.address = string.Join(CONST_CONCAT, addrInfo.address_1?.Trim() ?? ""
                                                                    , addrInfo.address_2?.Trim() ?? ""
                                                                    , addrInfo.address_3?.Trim() ?? ""
                                                                    , addrInfo.sub_district_display?.Trim() ?? ""
                                                                    , addrInfo.district_display?.Trim() ?? ""
                                                                    , addrInfo.province_display?.Trim() ?? ""
                                                                    , addrInfo.postal_code)?.ReplaceMultiplSpacesWithSingleSpace();
                        
                        trgt.addressInfo.countryText = addrInfo?.cls_ctrycode_text?.Trim()??"";

                        try
                        {
                            trgt.addressInfo.addressTypeText = AddressTypeMasterData.Instance.FindByCode(isNull(addrInfo?.address_type_code), "01")?.Name??"";
                          
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                       
                        trgt.addressInfo.latitude = addrInfo.lattitude?.Trim() ?? "";
                        trgt.addressInfo.longitude = addrInfo.longitude?.Trim() ?? "";
                    }
                    //trgt.AddDebugInfo("TransformCLSInquiryPersonalClientContentOut_to_CrmInquiryClientMasterContentOut","");
                   // trgt.AddDebugInfo("Source Data", src);
                    trgtContent.data.Add(trgt);
                }
                catch (Exception e)
                {
                    Console.WriteLine( e.Message );
                }
            }
           // trgtContent.AddDebugInfo("TransformCLSInquiryPersonalClientContentOut_to_CrmInquiryClientMasterContentOut", "");
            return trgtContent;
        }

        public string isNull(string a)
        {
            if( a == null)
            {
                return "";
            }
            return a;
        }
    }

    
}