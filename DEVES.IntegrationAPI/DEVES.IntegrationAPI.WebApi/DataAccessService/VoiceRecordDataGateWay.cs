using System;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using DEVES.IntegrationAPI.Model.CTI;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;

namespace DEVES.IntegrationAPI.WebApi.Services.DataGateWay
{
    public class VoiceRecordDataGateWay : BaseCrmSdkTableGateWay<PfcVoiceRecordEntity>
    {
        public new string EntityName = "pfc_voice_record";

        public Entity BindParams(Entity PfcVoiceRecode, VoiceRecordRequestModel model)
        {
            //  PfcVoiceRecode["pfc_activityid"] = new EntityReference("pfc_activityid",  aircraftGuid);;

            PfcVoiceRecode["pfc_call_transactionid"] = model.callTransactionId;
            PfcVoiceRecode["pfc_sessionid"] = model.sessionId;
           
                PfcVoiceRecode["pfc_event_action"] = model.eventAction.ToString();
            
            // PfcVoiceRecode["pfc_event_action"] = (req.eventAction!=null) ??   req.eventAction.ToString();

            PfcVoiceRecode["pfc_userid"] = model.userid;
            PfcVoiceRecode["pfc_caller_number"] = model.callernumber;
            PfcVoiceRecode["pfc_called_number"] = model.callednumber;

            PfcVoiceRecode["pfc_is_conference"] = (bool) model.isConference;

            
                PfcVoiceRecode["pfc_session_start_date"] = UnixTimeStampToDateTime(model.sessionStartDate);
            

            PfcVoiceRecode["pfc_session_duration"] = (int) model.sessionDuration;

           
                PfcVoiceRecode["pfc_session_state"] = model.sessionState;
           

            PfcVoiceRecode["pfc_url"] = model.url;

            if (model.callType == "out")
            {
                var callIn = new OptionSetValue(1);
                PfcVoiceRecode["pfc_call_type"] = callIn;
            }
            else
            {
                var callOut = new OptionSetValue(2);
                PfcVoiceRecode["pfc_call_type"] = callOut;
            }

            return PfcVoiceRecode;
        }

        public Guid Create(string phoncallId, VoiceRecordRequestModel model)
        {
            Console.WriteLine("===============Create=================");
            var _p = getOrganizationServiceProxy();
            Entity PfcVoiceRecode = new Entity(EntityName);

            var aircraftid = phoncallId;
            var aircraftGuid = new Guid(phoncallId);

            PfcVoiceRecode["pfc_activityid"] = new EntityReference("phonecall", aircraftGuid);
        

            var bindedEntity = this.BindParams(PfcVoiceRecode, model);
            Console.WriteLine(bindedEntity.ToJSON());
            return _p.Create(bindedEntity);
        }
        public Guid CreateEntity(string phoncallId, PfcVoiceRecordEntity model)
        {
            var _p = getOrganizationServiceProxy();
            Entity PfcVoiceRecode = new Entity(EntityName);

            var aircraftid = phoncallId;
            var aircraftGuid = new Guid(phoncallId);

            PfcVoiceRecode["pfc_activityid"] = new EntityReference("phonecall", aircraftGuid);

           // var bindedEntity = this.BindParams(model);

            return _p.Create(BindedEntity);
        }


        public void Update(Entity entity, VoiceRecordRequestModel model)
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

    public class PfcVoiceRecordEntity:Entity
    {
        public string pfc_call_transactionid { get; set; }
        public string pfc_sessionid { get; set; }
        public string pfc_event_action { get; set; }
        public string pfc_userid { get; set; }
        public string pfc_caller_number { get; set; }
        public string pfc_called_number { get; set; }
        public bool pfc_is_conference { get; set; }
        public DateTime pfc_session_start_date { get; set; }
        public int pfc_session_duration { get; set; }
        public string cpfc_url { get; set; }
        public OptionSetValue pfc_call_type { get; set; }

           
    }
}