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
                // WAITING FOR MAPPING DOCUMENT
                return output;
            }
            // if already have ref_code in CRM -> RETURN message
            else 
            {
                output.code = AppConst.CODE_SUCCESS;
                output.message = "ได้ทำการตอบแบบสอบถามนี้แล้ว";
                output.description = "ref_code ที่กรอกมีอยู่แล้วในระบบ CRM";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now.ToString();

                return output;
            }

        }


        
    }
}