using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.Polisy400;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformRegClientPersonalInputModel_to_CLIENTCreatePersonalClientAndAdditionalInfoInputModel : BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientPersonalInputModel src = (RegClientPersonalInputModel)input;
            CLIENTCreatePersonalClientAndAdditionalInfoInputModel trgt = (CLIENTCreatePersonalClientAndAdditionalInfoInputModel)output;



            if (src == null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {
                
                trgt.cleansingId = src.generalHeader.cleansingId;

            }

            if (src.profileInfo != null)
            {
                //salutation  String	8	M คำนำหน้าชื่อ
                trgt.salutation = src.profileInfo.salutation;
                //personalName String	60	M ชื่อ
                trgt.personalName = src.profileInfo.personalName;
                //personalSurname String	60	M นามสกุล
                trgt.personalSurname = src.profileInfo.personalSurname;
                //sex String	1	M เพศลูกค้า
                trgt.sex = src.profileInfo.sex;
                //idCitizen String	24	O หมายเลขบัตรประจำตัวประชาชน
                trgt.idCard = src.profileInfo.idCitizen;
                //idPassport String	20	O หมายเลขบัตรหนังสือเดินทาง
                trgt.passportId = src.profileInfo.idPassport;
                //idAlien String	20	O หมายเลขบัตรต่างด้าว
                trgt.alientId = src.profileInfo.idAlien;
                //idDriving String	20	O หมายเลขบัตรใบขับขี่
                trgt.driverlicense = src.profileInfo.idDriving;
                //birthDate String	20	O วันเดือนปีเกิด
                trgt.birthDate = src.profileInfo.birthDate;
                //natioanality String	3	O Nationality
                trgt.natioanality = src.profileInfo.nationality;
                //language String	1	O ภาษา
                trgt.language = src.profileInfo.language;
                //married String	1	O สถานะการสมรส
                trgt.married = src.profileInfo.married;
                //occupation String	3	O อาชีพลูกค้า
                trgt.occupation = src.profileInfo.occupation;

                trgt.riskLevel = src.profileInfo.riskLevel;
                //vipStatus String	1	O VIP
                trgt.vipStatus = src.profileInfo.vipStatus;

               
               
            }
            if (src.contactInfo != null)
            {
                //telephone1 String	10	O เบอร์ติดต่อที่สะดวก(Contact Number)
                trgt.telephone1 = src.contactInfo.telephone1;
             
                //telephone2  String	10	O โทรศัพท์ลูกค้า(Office)
                trgt.telephone2 = src.contactInfo.telephone2;
              
                //telNo   String	10	O DID Tel No
                trgt.telNo = src.contactInfo.telephone3;
           
                //mobilePhone String	16	O
                trgt.mobilePhone = src.contactInfo.mobilePhone;
                //fax String	16	O
                trgt.fax = src.contactInfo.fax;
                //emailAddress    String	50	O อีเมล์
                trgt.emailAddress = src.contactInfo.emailAddress;
                //lineID String	50	O Line ID
                trgt.lineId = src.contactInfo.lineID;
                //facebook    String	100	O Facebook
                trgt.facebook = src.contactInfo.facebook;
            }

            if (src.addressInfo != null)
            {
                //address1 String	30	O ที่อยู่ บรรทัดที่ 1
                trgt.address1 = src.addressInfo.address1;
                //address2 String	30	O ที่อยู่ บรรทัดที่ 2
                trgt.address2 = src.addressInfo.address2;
                //address3 String	30	O ที่อยู่ บรรทัดที่ 3
                trgt.address3 = src.addressInfo.address3;
                //subDistrictCode String	6	O ตำบล / แขวง
                trgt.address4 = src.addressInfo.subDistrictCode;
                
                //provinceCode    String	2	O จังหวัด
                trgt.address5 = src.addressInfo.provinceCode;
                //postalCode String	10	O ที่อยู่ -> รหัสไปรษณีย์
                trgt.postCode = src.addressInfo.postalCode;
                //country String	3	O ที่อยู่ -> ประเทศ
                trgt.country = src.addressInfo.country;
                //addressType String	1	O
                trgt.busRes = src.addressInfo.addressType;
                //latitude    String	20	O
                trgt.latitude = src.addressInfo.latitude;
                //longtitude  String	20	O
                trgt.longtitude = src.addressInfo.longtitude;

            }

            return trgt;

        }
    }
}