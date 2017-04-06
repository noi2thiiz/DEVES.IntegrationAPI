using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity
{
    public class PfcMortorAccident:Entity
    {
        public PfcMortorAccident(Guid motorAccidentGuid)
        {

            pfc_parent_caseid = new EntityReference("incident", motorAccidentGuid);
            pfc_motor_accident_parties_sum = 0;
        }
        public string pfc_motor_accident_name { get; set; }
        //pfc_parent_caseId Parent Case LookUp  Ref: incident

        //pfc_ref_motor_type_of _lossid   ลักษณะการเกิดเหตุ LookUp  Ref.pfc_motor_type_of _loss
        public EntityReference pfc_ref_motor_type_of_lossid { get; set; }


        //pfc_motor_accident_name Motor Accident Name single line of text(200)
      
        public EntityReference pfc_parent_caseid { get; set; }
        //pfc_activity_date   วันที่เกิดเหตุ datetime
        public DateTime pfc_activity_date { get; set; }
        //pfc_event_code รหัสการเกิดเหตุ single line of text(20) "ในกรณี Inhouse ให้ใช้ eventdetail.EventID
        public string pfc_event_code { get; set; }
        //ในกรณี Outsource ให้ใช้ Incident.pfc_isurvey_params_event_code"

        //pfc_event_sequence ลำดับการเกิดเหตุ    whole number	"ในกรณี Inhouse = 1 (เสมอ และ Update Incident.pfc_motor_accident_sum = 1) 
        public int pfc_event_sequence { get; set; }
        //ในกรณี Outsource ให้ดึงIncident.pfc_motor_accident_sum +1"
       
        //pfc_ref_isurvey_eventid iSurvey รหัสการรับการเคลม single line of text(20)
        public DateTime pfc_ref_isurvey_eventid { get; set; }
        
        //pfc_accident_event_detail ลักษณะการเกิดเหตุ   Multiple Line of text(2000)    กรณีกดปุ่ม ให้เอาลักษณะการเกิดเหตุ pfc_ref_motor_type_of _lossid พร้อม Template มาไว้
        public string pfc_accident_event_detail { get; set; }
        //pfc_more_document   เอกสารเพิ่มเติม Multiple Line of text(2000)
        public string pfc_more_document { get; set; }
        //pfc_accident_latitude Latitude  ที่เกิดเหตุบนแผนที่ single line of text(20) "กรณี Inhouse มาจาก eventdetail.accidentLocation
        public string pfc_accident_latitude { get; set; }
        //กรณี Outsource ให้ Default จาก Incident.pfc_accident_latitude"
        //pfc_accident_Longitude Longitude  ที่เกิดเหตุบนแผนที่ single line of text(20) "กรณี Inhouse มาจาก eventdetail.accidentLocation
        public string pfc_accident_Longitude { get; set; }
        //กรณี Outsource ให้ Default จาก Incident.pfc_accident_longitude"
        //pfc_accident_location สถานที่เกิดเหตุ Multiple Line of text (2000)    "กรณี Inhouse มาจาก eventdetail.accidentLocation
        public string pfc_accident_location { get; set; }
        //กรณี Outsource ให้ Default จาก Incident.pfc_accident_place"
        //pfc_accident_remark หมายเหตุ    Multiple Line of text (2000)
        public string pfc_accident_remark { get; set; }
        //pfc_ref_isurvey_total_event iSurvey จำนวนเหตุการณ์ single line of text(20)
        public string pfc_ref_isurvey_total_event { get; set; }
        //pfc_ref_isurvey_created_date iSurvey วันที่สร้างข้อมูล datetime
        public DateTime pfc_ref_isurvey_created_date { get; set; }
        //pfc_ref_isurvey_modified_date iSurvey วันที่แก้ไขข้อมูล datetime
        public DateTime pfc_ref_isurvey_modified_date { get; set; }
        //pfc_ref_isurvey_isdeleted iSurvey IsDeleted Two Option	"Default (0)
        //0 : Active
        //1 : Deleted"
        public bool pfc_ref_isurvey_isdeleted { get; set; } = false;
        //pfc_ref_isurvey_isdeleted_date iSurvey IsDeletedDate datetime
        public DateTime pfc_ref_isurvey_isdeleted_date { get; set; }
        //pfc_motor_accident_parties_sum จำนวนคู่กรณี    whole number    Default = 0
        public int pfc_motor_accident_parties_sum { get; set; } = 0;
        //pfc_ref_rvp_claim_no RVP เลขเคลม single line of text(20)
        public string pfc_ref_rvp_claim_no { get; set; }

        
    }
}