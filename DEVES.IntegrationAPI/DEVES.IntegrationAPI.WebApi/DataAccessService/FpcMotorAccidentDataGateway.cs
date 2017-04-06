using DEVES.IntegrationAPI.Model.RegClaimRequestFromRVP;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService
{
    public class FpcMotorAccidentDataGateway : CrmSdkTableGateWayAbstract
    {
        public string EntityName = "pfc_motor_accident";

        public void BindParams(CRMregClaimRequestFromRVPInputModel model)
        {

            //pfc_motor_accident_name Motor Accident Name single line of text(200)
            AddAttribute("pfc_motor_accident_name", model.ToString());
            //pfc_parent_caseId Parent Case LookUp
            AddAttribute("pfc_parent_caseId", model.ToString());
            //pfc_activity_date วันที่เกิดเหตุ  datetime
            AddAttribute("pfc_activity_date", model.ToString());
            //pfc_event_code  รหัสการเกิดเหตุ single line of text(20)
            AddAttribute("pfc_event_code", model.ToString());
            //pfc_event_sequence ลำดับการเกิดเหตุ    whole number
            AddAttribute("pfc_event_sequence", model.ToString());
            //pfc_ref_isurvey_eventid iSurvey รหัสการรับการเคลม single line of text(20)
            AddAttribute("pfc_ref_isurvey_eventid", model.ToString());
            //pfc_ref_motor_type_of _lossid   ลักษณะการเกิดเหตุ LookUp
            AddAttribute("pfc_ref_motor_type_of", model.ToString());
            //pfc_accident_event_detail ลักษณะการเกิดเหตุ   Multiple Line of text(2000)
            AddAttribute("pfc_accident_event_detail", model.ToString());
            //pfc_more_document เอกสารเพิ่มเติม Multiple Line of text(2000)
            AddAttribute("pfc_more_document", model.ToString());
            //pfc_accident_latitude Latitude  ที่เกิดเหตุบนแผนที่ single line of text(20)
            AddAttribute("pfc_accident_latitude", model.ToString());
            //pfc_accident_Longitude Longitude  ที่เกิดเหตุบนแผนที่ single line of text(20)
            AddAttribute("pfc_accident_Longitude", model.ToString());
            //pfc_accident_location สถานที่เกิดเหตุ Multiple Line of text(2000)
            AddAttribute("pfc_accident_location", model.ToString());
            //pfc_accident_remark หมายเหตุ    Multiple Line of text(2000)
            AddAttribute("pfc_accident_remark", model.ToString());
            //pfc_ref_isurvey_total_event iSurvey จำนวนเหตุการณ์ single line of text(20)
            AddAttribute("pfc_ref_isurvey_total_event", model.ToString());
            //pfc_ref_isurvey_created_date iSurvey วันที่สร้างข้อมูล datetime
            AddAttribute("pfc_ref_isurvey_created_date", model.ToString());
            //pfc_ref_isurvey_modified_date iSurvey วันที่แก้ไขข้อมูล datetime
            AddAttribute("pfc_ref_isurvey_modified_date", model.ToString());
            //pfc_ref_isurvey_isdeleted iSurvey IsDeleted Two Option
            AddAttribute("pfc_ref_isurvey_modified_date", model.ToString());
            //pfc_ref_isurvey_isdeleted_date  iSurvey IsDeletedDate   datetime
            AddAttribute("pfc_ref_isurvey_isdeleted_date", model.ToString());
            //pfc_motor_accident_parties_sum  จำนวนคู่กรณี whole number
            AddAttribute("pfc_ref_isurvey_isdeleted_date", model.ToString());


            // return pfc_ref_rvp_claim_no    RVP เลขเคลม single line of text(20)
            // AddAttribute("pfc_ref_rvp_claim_no", model.ToString());

        }

       

        public Guid Create(string phoncallId, CRMregClaimRequestFromRVPInputModel model)
        {
            var _p = getOrganizationServiceProxy();
            Entity PfcVoiceRecode = new Entity(EntityName);

            var aircraftid = phoncallId;
            var aircraftGuid = new Guid(phoncallId);

            PfcVoiceRecode["pfc_activityid"] = new EntityReference("phonecall", aircraftGuid);

            var bindedEntity = this.BindParams(PfcVoiceRecode, model);

            return _p.Create(bindedEntity);
        }


        public void Update(Entity entity, CRMregClaimRequestFromRVPInputModel model)
        {
            var _p = getOrganizationServiceProxy();
            var bindedEntity = this.BindParams(entity, model);

            _p.Update(bindedEntity);
        }

        public void Find()
        {
        }

        public void FetchAll()
        {
        }

        //https://msdn.microsoft.com/en-us/library/microsoft.xrm.sdk.query.queryexpression.aspx
        public Entity FindBySessionId(string sessionid)
        {
            var _p = getOrganizationServiceProxy();

            var condition1 = new ConditionExpression
            {
                AttributeName = "pfc_sessionid",
                Operator = ConditionOperator.Equal
            };

            condition1.Values.Add(sessionid);

            var filter1 = new FilterExpression();
            filter1.Conditions.Add(condition1);

            QueryExpression query = new QueryExpression(this.EntityName);

            query.ColumnSet.AddColumns("pfc_voice_recordid");
            query.Criteria.AddFilter(filter1);

            var _result1 = _p.RetrieveMultiple(query);


            return _result1.Entities.FirstOrDefault();
        }

    }
}