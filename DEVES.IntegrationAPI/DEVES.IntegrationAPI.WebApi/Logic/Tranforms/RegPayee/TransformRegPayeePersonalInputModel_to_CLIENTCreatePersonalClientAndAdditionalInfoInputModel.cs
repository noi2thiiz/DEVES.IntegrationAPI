using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.RegPayeePersonal;
using DEVES.IntegrationAPI.Model.Polisy400;
using System.Globalization;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TransformRegPayeePersonalInputModel_to_CLIENTCreatePersonalClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegPayeePersonalInputModel src = (RegPayeePersonalInputModel)input;
            CLIENTCreatePersonalClientAndAdditionalInfoInputModel trgt = (CLIENTCreatePersonalClientAndAdditionalInfoInputModel)output;



            if (src == null)
            {
                return trgt;
            }
            
            if (src.generalHeader != null)
            {
                trgt.cleansingId = src.generalHeader.cleansingId ?? "";
            }
            if (src.profileInfo != null)
            {
                trgt.salutation = src.profileInfo.salutation ?? "";
                trgt.personalName = src.profileInfo.personalName ?? "";
                trgt.personalSurname = src.profileInfo.personalSurname ?? "";
                trgt.sex = src.profileInfo.sex ?? "";
                trgt.idCard = src.profileInfo.idCitizen ?? "";
                trgt.passportId = src.profileInfo.idPassport ?? "";
                trgt.alientId = src.profileInfo.idAlien ?? "";
                trgt.driverlicense = src.profileInfo.idDriving ?? "";
                trgt.dtBirthDtate = src.profileInfo.birthDate;
                trgt.natioanality = src.profileInfo.nationality ?? "";
                trgt.language = src.profileInfo.language ?? "";
                trgt.married = src.profileInfo.married ?? "";
                trgt.occupation = src.profileInfo.occupation ?? "";
                trgt.riskLevel = src.profileInfo.riskLevel ?? "";
                trgt.vipStatus = src.profileInfo.vipStatus ?? "";
                trgt.deathDate = "";
            }
            if (src.contactInfo != null)
            {
                if (string.IsNullOrEmpty(src.contactInfo.telephone1Ext))
                {
                    trgt.telephone1 = src.contactInfo.telephone1 ?? "";
                }
                else
                {
                    trgt.telephone1 = src.contactInfo.telephone1 + "#" + src.contactInfo.telephone1Ext ?? "";
                }
                if (string.IsNullOrEmpty(src.contactInfo.telephone2Ext))
                {
                    trgt.telephone2 = src.contactInfo.telephone2 ?? "";
                }
                else
                {
                    trgt.telephone2 = src.contactInfo.telephone2 + "#" + src.contactInfo.telephone2Ext ?? "";
                }
                if (string.IsNullOrEmpty(src.contactInfo.telephone3Ext))
                {
                    trgt.telNo = src.contactInfo.telephone3 ?? "";
                }
                else
                {
                    trgt.telNo = src.contactInfo.telephone3 + "#" + src.contactInfo.telephone3Ext ?? "";
                }
                trgt.mobilePhone = src.contactInfo.mobilePhone ?? "";
                trgt.fax = src.contactInfo.fax ?? "";
                trgt.emailAddress = src.contactInfo.emailAddress ?? "";
                trgt.lineId = src.contactInfo.lineID ?? "";
                trgt.facebook = src.contactInfo.facebook ?? "";
            }
            if (src.addressInfo != null)
            {
                trgt.address1 = src.addressInfo.address1 ?? "";
                trgt.address2 = src.addressInfo.address2 ?? "";
                trgt.address3 = src.addressInfo.address3 ?? "";

                string districtName = "";
                string subDistrictName = "";
                if (!string.IsNullOrEmpty(src.addressInfo.districtCode))
                {
                    var district = DistricMasterData.Instance.FindByCode(src.addressInfo.districtCode);
                    if (district != null)
                    {
                        districtName = district.DistrictName;
                    }

                }


                if (!string.IsNullOrEmpty(src.addressInfo?.subDistrictCode))
                {
                    var subDistrict = SubDistrictMasterData.Instance.FindByCode(src.addressInfo.subDistrictCode);
                    if (subDistrict != null)
                    {
                        subDistrictName = subDistrict.SubDistrictName;
                    }
                }

                trgt.address4 = "" + subDistrictName + " " + districtName;

                //provinceCode    String	2	O จังหวัด
                if (!string.IsNullOrEmpty(src.addressInfo?.provinceCode))
                {
                    var province = ProvinceMasterData.Instance.FindByCode(src.addressInfo.provinceCode);
                    if (province != null)
                    {
                        trgt.address5 = province.ProvinceName;
                    }
                }

                trgt.postCode = src.addressInfo.postalCode ?? "";
                trgt.country = src.addressInfo.country ?? "";
                trgt.busRes = "";//src.addressInfo.addressType
                trgt.latitude = src.addressInfo.latitude ?? "";
                trgt.longtitude = src.addressInfo.longtitude ?? "";
            }
            if (src?.addressInfo?.addressType != null)
            {
                switch (src.addressInfo.addressType)
                {
                    case "01": trgt.busRes = ""; break; //ไม่ระบุ
                    case "02": trgt.busRes = "R"; break; //ที่อยู่ตามบัตรประจำตัวประชาชน
                    case "03": trgt.busRes = "R"; break; //ที่อยู่ตามทะเบียนบ้าน
                    case "04": trgt.busRes = "B"; break; //ที่อยู่ที่ทำงาน
                    case "05": trgt.busRes = "R"; break; //ที่อยู่ปัจจุบัน
                    case "06": trgt.busRes = ""; break; //ที่อยู่ในสัญญา
                    case "07": trgt.busRes = "P"; break; //ที่อยู่สำหรับจัดส่งเอกสาร
                    default: trgt.busRes = ""; break;
                }
            }

            //@TODO AdHoc Fix กรณีส่ง 00002 แล้ว ตามด้วย post code จะ error
            if (trgt.country=="004")
            {
                trgt.postCode = "";
            }
            /*
             B = Business Address
                 P = Postal Address
                 R = Residential Address
                 */
            return trgt;
        }

    }
}