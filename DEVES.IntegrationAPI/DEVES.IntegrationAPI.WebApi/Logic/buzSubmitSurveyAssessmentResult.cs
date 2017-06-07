using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzSubmitSurveyAssessmentResult : BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {

            // Preparation Variable
            SubmitSurveyAssessmentResultOutputModel_Pass output = new SubmitSurveyAssessmentResultOutputModel_Pass();
            output.data = new SubmitSurveyAssessmentResultDataModel_Pass();

            // Deserialize Input
            SubmitSurveyAssessmentResultInputModel contentModel = DeserializeJson<SubmitSurveyAssessmentResultInputModel>(input.ToString());

            // Connect SDK and query
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            var query = from c in svcContext.pfc_assessmentSet
                        where c.pfc_assessment_ref_code == contentModel.assessmentrefcode
                        select c;

            // Condition check if ref_code don't have in CRM -> UPDATE
            if(query.FirstOrDefault<pfc_assessment>() == null)
            {
                pfc_assessment create = new pfc_assessment();

                // assessment
                create.pfc_assessment_claim_noti_date = DateTime.Now;
                create.pfc_assessment_claim_noti_score = contentModel.assessmentClaimNotiScore;
                create.pfc_assessment_claim_noti_comment = contentModel.assessmentClaimNotiComment;
                create.pfc_assessment_survey_date = DateTime.Now;
                create.pfc_assessment_survey_score = contentModel.assessmentSurveyScore;
                // create.pfc_assessment_survey_speed_score = contentModel.assessmentSurveySpeedScore;
                create.pfc_assessment_survey_comment = contentModel.assessmentSurveyComment;
                create.pfc_assessment_claim_noti_by = string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid) ? new OptionSetValue(100000000) : new OptionSetValue(100000001);
                create.pfc_assessment_survey_by = string.IsNullOrEmpty(contentModel.assessmentSurveyByUserid) ? new OptionSetValue(100000000) : new OptionSetValue(100000001);
                create.pfc_assessment_claim_noti_by_userid = new EntityReference(pfc_motor_accident_parties.EntityLogicalName, "pfc_assessment_claim_noti_by_userid", contentModel.assessmentSurveyByUserid);
                create.pfc_assessment_survey_by_userid = new EntityReference(pfc_motor_accident_parties.EntityLogicalName, "pfc_assessment_survey_by_userid", contentModel.assessmentSurveyByUserid);


                _serviceProxy.Create(create);

                // WAITING FOR MAPPING DOCUMENT
                output.code = AppConst.CODE_SUCCESS;
                output.message = "รอ LOGIC จากพี่ไกด์";
                output.description = "รอ LOGIC จากพี่ไกด์";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();

                return output;
            }
            // if already have ref_code in CRM -> RETURN message
            else 
            {
                pfc_assessment firstQuery = query.FirstOrDefault<pfc_assessment>();

                output.code = AppConst.CODE_SUCCESS;
                output.message = "ได้ทำการตอบแบบสอบถามนี้แล้ว";
                output.description = "ref_code: " + firstQuery.pfc_assessment_ref_code + " มีอยู่แล้วในระบบ CRM";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();

                return output;
            }

        }


        
    }
}