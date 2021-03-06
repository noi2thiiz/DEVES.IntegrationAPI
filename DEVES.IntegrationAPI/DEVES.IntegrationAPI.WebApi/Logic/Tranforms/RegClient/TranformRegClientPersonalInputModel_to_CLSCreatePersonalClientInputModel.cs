﻿using System;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.CLS;
using DEVES.IntegrationAPI.Model.RegClientPersonal;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class TranformRegClientPersonalInputModel_to_CLSCreatePersonalClientInputModel: BaseTransformer
    {
        public override BaseDataModel TransformModel(BaseDataModel input, BaseDataModel output)
        {
            RegClientPersonalInputModel src = (RegClientPersonalInputModel)input;
            CLSCreatePersonalClientInputModel trgt = (CLSCreatePersonalClientInputModel)output;

           
            
            if (src ==null)
            {
                return trgt;
            }

            if (src.generalHeader != null)
            {
                trgt.roleCode = src.generalHeader.roleCode?.Trim() ?? "";
                //clientId String	8	O
                trgt.clientId = src.generalHeader.polisyClientId?.Trim() ?? "";
                //crmPersonId String	20	O
                trgt.crmPersonId = src.generalHeader.crmClientId?.Trim() ?? "";
                
            }

            if (src.profileInfo != null)
            {
                //salutation  String	8	M คำนำหน้าชื่อ
                trgt.salutation = src.profileInfo.salutation?.Trim() ?? "";
                //personalName String	60	M ชื่อ
                trgt.personalName = src.profileInfo.personalName?.Trim() ?? "";
                //personalSurname String	60	M นามสกุล
                trgt.personalSurname = src.profileInfo.personalSurname?.Trim() ?? "";
                //sex String	1	M เพศลูกค้า
                trgt.sex = src.profileInfo.sex?.Trim() ?? "";
                //idCitizen String	24	O หมายเลขบัตรประจำตัวประชาชน
                trgt.idCitizen = src.profileInfo.idCitizen?.Trim() ?? "";
                //idPassport String	20	O หมายเลขบัตรหนังสือเดินทาง
                trgt.idPassport = src.profileInfo.idPassport?.Trim() ?? "";
                //idAlien String	20	O หมายเลขบัตรต่างด้าว
                trgt.idAlien = src.profileInfo.idAlien?.Trim() ?? "";
                //idDriving String	20	O หมายเลขบัตรใบขับขี่
                trgt.idDriving = src.profileInfo.idDriving?.Trim() ?? "";
                //birthDate String	20	O วันเดือนปีเกิด
                trgt.dtBirthDate = src.profileInfo.birthDate;
                //natioanality String	3	O Nationality
                trgt.natioanality = src.profileInfo.nationality?.Trim() ?? "";
                //language String	1	O ภาษา
                trgt.language = src.profileInfo.language?.Trim() ?? "";
                //married String	1	O สถานะการสมรส
                trgt.married = src.profileInfo.married?.Trim() ?? "";
                //occupation String	3	O อาชีพลูกค้า
                trgt.occupation = src.profileInfo.occupation?.Trim() ?? "";
                //vipStatus String	1	O VIP
                trgt.vipStatus = src.profileInfo.vipStatus?.Trim() ?? "";

               
            }
            if (src.contactInfo != null)
            {
                //telephone1 String	10	O เบอร์ติดต่อที่สะดวก(Contact Number)
                trgt.telephone1 = src.contactInfo.telephone1?.Trim() ?? "";
                //telephone1Ext String	5	O
                trgt.telephone1Ext = src.contactInfo.telephone1Ext?.Trim() ?? "";
                //telephone2  String	10	O โทรศัพท์ลูกค้า(Office)
                trgt.telephone2 = src.contactInfo.telephone2?.Trim() ?? "";
                //telephone2Ext String	5	O
                trgt.telephone2Ext = src.contactInfo.telephone2Ext?.Trim() ?? "";
                //telNo   String	10	O DID Tel No
                trgt.telNo = src.contactInfo.telephone3?.Trim() ?? "";
                //telNoExt String	5	O
                trgt.telNoExt = src.contactInfo.telephone3Ext?.Trim() ?? "";
                //mobilePhone String	16	O
                trgt.mobilePhone = src.contactInfo.mobilePhone?.Trim() ?? "";
                //fax String	16	O
                trgt.fax = src.contactInfo.fax?.Trim() ?? "";
                //emailAddress    String	50	O อีเมล์
                trgt.emailAddress = src.contactInfo.emailAddress?.Trim() ?? "";
                //lineID String	50	O Line ID
                trgt.lineID = src.contactInfo.lineID?.Trim() ?? "";
                //facebook    String	100	O Facebook
                trgt.facebook = src.contactInfo.facebook?.Trim() ?? "";
            }

            if (src.addressInfo != null)
            {
                //address1 String	30	O ที่อยู่ บรรทัดที่ 1
                trgt.address1 = src.addressInfo.address1?.Trim() ?? "";
                //address2 String	30	O ที่อยู่ บรรทัดที่ 2
                trgt.address2 = src.addressInfo.address2?.Trim() ?? "";
                //address3 String	30	O ที่อยู่ บรรทัดที่ 3
                trgt.address3 = src.addressInfo.address3?.Trim() ?? "";
                //subDistrictCode String	6	O ตำบล / แขวง
                trgt.subDistrictCode = src.addressInfo.subDistrictCode?.Trim() ?? "";
                //districtCode    String	4	O อำเภอ / เขต
                trgt.districtCode = src.addressInfo.districtCode?.Trim() ?? "";
                //provinceCode    String	2	O จังหวัด
                trgt.provinceCode = src.addressInfo.provinceCode?.Trim() ?? "";
                //postalCode String	10	O ที่อยู่ -> รหัสไปรษณีย์
                trgt.postalCode = src.addressInfo.postalCode?.Trim() ?? "";
                //country String	3	O ที่อยู่ -> ประเทศ
                trgt.country = src.addressInfo.country?.Trim() ?? "";
                //addressType String	1	O
                trgt.addressType = src.addressInfo.addressType?.Trim() ?? "";
                //latitude    String	20	O
                trgt.latitude = src.addressInfo.latitude?.Trim() ?? "";
                //longtitude  String	20	O
                trgt.longitude = src.addressInfo.longitude?.Trim() ?? "";

            }
            
            return trgt;

        }
    }
}