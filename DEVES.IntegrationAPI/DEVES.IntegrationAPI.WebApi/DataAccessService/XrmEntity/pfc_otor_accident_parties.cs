using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity
{
    public class PfcMotorAccidentParties:Entity
    {
        public PfcMotorAccidentParties()
        {
        }
            public PfcMotorAccidentParties(Guid motorAccidentGuid)
        {
            pfc_parent_motor_accidentid = new EntityReference("pfc_motor_accident", motorAccidentGuid);
        }

        //pfc_motor_accident_parties_name Parties Name single line of text(200)
        public string pfc_motor_accident_parties_name { get; set; }
        //pfc_ref_isurvey_partiesid iSurvey รหัสคู่กรณี single line of text(50)
        public string pfc_ref_isurvey_partiesid { get; set; }

        //pfc_ref_isurvey_eventid iSurvey รหัสการรับการเคลม single line of text(50)
        public string pfc_ref_isurvey_eventid { get; set; }
        //pfc_ref_isurvey_eventitem iSurvey ลำดับการรับแจ้ง single line of text(50)
        public string pfc_ref_isurvey_eventitem { get; set; }
        // pfc_event_code รหัสการเกิดเหตุ single line of text(20)
        public string pfc_event_code { get; set; }

        //pfc_event_sequence ลำดับการเกิดเหตุ    whole number
        public int pfc_event_sequence { get; set; } = 0;

        //pfc_parties_sequence ลำดับคู่กรณี    whole number
        public int pfc_parties_sequence { get; set; }

        //pfc_parent_motor_accidentId Parent Motor Accident   LookUp
        public EntityReference pfc_parent_motor_accidentid { get; set; } = new EntityReference("pfc_motor_accident"); 
        //pfc_parties_fullname    ชื่อคู่กรณี single line of text(100)
        public string pfc_parties_fullname { get; set; }
        //pfc_parties_type ประเภทคู่กรณี   Option Set
        public OptionSetValue pfc_parties_type { get; set; } = new OptionSetValue(0);
        //pfc_licence_provinceId จังหวัดทะเบียนรถ    LookUp
        public EntityReference pfc_licence_provinceId { get; set; } = new EntityReference("pfc_motor_licence_province"); 
        //pfc_licence_province    จังหวัดทะเบียนรถ single line of text(50)
        public string pfc_licence_province { get; set; }
        //pfc_licence_no ทะเบียนรถยนต์   single line of text(50)
        public string pfc_licence_no { get; set; }
        //pfc_brandId ยี่ห้อรถ    LookUp
        public string pfc_brandId { get; set; }
        //pfc_brand   ยี่ห้อรถ single line of text(50)
        public string pfc_brand { get; set; }
        //pfc_modelId รุ่นรถ  LookUp
        public string pfc_modelId { get; set; }
        //pfc_model   รุ่นรถ single line of text(50)
        public string pfc_model { get; set; }
        //pfc_colorId สีรถ    LookUp
        public OptionSetValue pfc_colorId { get; set; }
        //pfc_color   สีรถ single line of text(50)
        public string pfc_color { get; set; }
        // pfc_phoneno เบอร์ติดต่อคู่กรณี  single line of text(50)
        public string pfc_phoneno { get; set; }
        //pfc_insurance_name ชื่อบริษัทประกันภัย single line of text(100)
        public string pfc_insurance_name { get; set; }

        //pfc_policyno เลขที่กรมธรรม์  single line of text(50)
        public string pfc_policyno { get; set; }
        //pfc_policy_type ประเภทกรมธรรม์  single line of text(50)
        public string pfc_policy_type { get; set; }
        //pfc_ref_isurvey_created_date iSurvey วันที่สร้างข้อมูล datetime
        public DateTime pfc_ref_isurvey_created_date { get; set; }
        //pfc_ref_isurvey_modified_date iSurvey วันที่แก้ไขข้อมูล datetime
        public DateTime pfc_ref_isurvey_modified_date { get; set; }

        //pfc_ref_isurvey_isdeleted iSurvey IsDeleted Two Option
        public DateTime pfc_ref_isurvey_isdeleted { get; set; }
        //statuscode  Status Reason   Option set
        public OptionSetValue statuscode { get; set; }
        //pfc_ref_isurvey_isdeleted_date iSurvey IsDeletedDate datetime
        public DateTime pfc_ref_isurvey_isdeleted_date  { get; set; }
        //pfc_ref_rvp_claim_no RVP เลขเคลม single line of text(20)
        public string pfc_ref_rvp_claim_no  { get; set; }
        //pfc_ref_rvp_parties_seq RVP ลำดับคู่กรณี whole number
        public int pfc_ref_rvp_parties_seq  { get; set; }

    }
}