using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.XrmEntity
{
    public class IncidentEntity:Entity
    {
       // public string pfc_ref_isurvey_total_event { get; set; }

        public IncidentEntity()
        {

        }

            public IncidentEntity(Guid policyAdditionalId, Guid accountGuid, Guid informerGuid, Guid driverGuid,Guid policyGuid)
        {
             pfc_policy_additionalid = new EntityReference("pfc_policy_additional", policyAdditionalId);
            //    pfc_policy_additionalId = new EntityReference("pfc_category", new Guid("1DCB2B21-0AAB-E611-80CA-0050568D1874"));

           
             customerid = new EntityReference("contact", accountGuid);
             pfc_informer_name = new EntityReference("contact", informerGuid);
             pfc_driver_name = new EntityReference("contact", driverGuid);
            pfc_policyid = new EntityReference("pfc_polycy", policyGuid);
            //customerid //contact // account //B55765F1-C4A4-E611-80CA-0050568D1874
            //pfc_informer_name //pfc_driver_client_name account
            //pfc_driver_client_name

        }
        

        public EntityReference customerid { get; set; }
        public EntityReference pfc_informer_name { get; set; }
        public EntityReference pfc_driver_name { get; set; }

        // public Guid incidentId { get; set; }
        public string ticketnumber { get; set; }
        public string title { get; set; }
       // public string pfc_parent_caseId { get; set; }

        public OptionSetValue caseorigincode { get; set; } = new OptionSetValue(100000002);


        public bool pfc_case_vip { get; set; }

    //LookUp pfc_policy_additional_incident_policy_additionalId
       public EntityReference pfc_policy_additionalid { get; set; }

        public string pfc_policy_additional_number { get; set; }

        //LookUp pfc_policy_incident_policyId
        public EntityReference pfc_policyid { get; set; } = new EntityReference("pfc_policy");

        public string pfc_policy_client_number { get; set; }
        public string pfc_policy_number { get; set; }

        public bool pfc_policy_vip { get; set; }
        public OptionSetValue pfc_policy_mc_nmc { get; set; } = new OptionSetValue();
        public string pfc_current_reg_num { get; set; }
        public string pfc_current_reg_num_prov { get; set; }

        public string pfc_current_reg_num_provid { get; set; }

        //LookUp pfc_claim_incident_claimId
        public EntityReference pfc_claimid { get; set; } = new EntityReference("pfc_claim");

       
        public string pfc_claim_number { get; set; } 

        public OptionSetValue casetypecode { get; set; } = new OptionSetValue(2);

        //LookUp
        public EntityReference pfc_categoryid { get; set; }

        //LookUp
        public EntityReference pfc_sub_categoryid { get; set; }

        public OptionSetValue pfc_source_data { get; set; } = new OptionSetValue(100000000);

        public string description { get; set; }

        public string pfc_claim_noti_number { get; set; }
        public DateTime pfc_claim_noti_numberOn { get; set; }
        public string pfc_request_claim_noti_number_by { get; set; }
        public DateTime pfc_claim_loss_date { get; set; }

        public string pfc_customer_client_number { get; set; }



        // pfc_customer_vip Customer VIP Two Option
        public bool pfc_customer_vip { get; set; }
        // pfc_customer_sensitive  Customer Sensitive  Option set
        public OptionSetValue pfc_customer_sensitive { get; set; }
        //  pfc_customer_privilege Customer Privilege Option set
        public OptionSetValue pfc_customer_privilege { get; set; }

        // pfc_notification_date วันเวลาที่รับแจ้ง   datetime
        public DateTime pfc_notification_date { get; set; }
        // pfc_accident_on วันเวลาที่เกิดเหตุ datetime
        public DateTime pfc_accident_on { get; set; }
        // pfc_accident_desc_code รหัสลักษณะการเกิดเหตุ   single line of text(100)
        public string pfc_accident_desc_code { get; set; }
        // pfc_accident_desc รายละเอียดกลักษณะการเกิดเหตุ    Multiple Line of text(2000)
        public string pfc_accident_desc { get; set; }
        // pfc_num_of_expect_injuries จำนวนผู้บาดเจ็บ(คาดการณ์)   Option set
        public OptionSetValue pfc_num_of_expect_injuries { get; set; }
        public OptionSetValue pfc_num_of_death { get;  set; }
        public OptionSetValue pfc_accident_legal_result { get;  set; }
        public string pfc_police_station { get;  set; }
        public string pfc_police_record_id { get;  set; }
        public DateTime pfc_police_record_date { get;  set; }
        public bool pfc_police_bail_flag { get;  set; }
        public string pfc_driver_mobile { get;  set; }
        public string pfc_driver_client_number { get;  set; }
        public OptionSetValue pfc_relation_cutomer_accident_party { get;  set; }
        public string pfc_accident_place { get;  set; }
        public string pfc_accident_latitude { get;  set; }
        public string pfc_accident_longitude { get;  set; }
        public OptionSetValue pfc_claim_type { get;  set; }
        public OptionSetValue pfc_send_out_surveyor { get;  set; }
        public OptionSetValue pfc_isurvey_status { get; set; }
        public string pfc_accident_province { get;  set; }
        public string pfc_accident_district { get;  set; }
        public OptionSetValue pfc_num_of_accident_injuries { get; set; }
        public decimal pfc_excess_fee { get;  set; }
        public decimal pfc_deductable_fee { get;  set; }
        public int pfc_motor_accident_sum { get; set; }
        //public bool pfc_legal_case_flag { get;  set; }
        // public int pfc_excess_fee { get;  set; }
        //  public int pfc_deductable_fee { get;  set; }
        //public DateTime reportAccidentResultDate { get;  set; }
    }
}