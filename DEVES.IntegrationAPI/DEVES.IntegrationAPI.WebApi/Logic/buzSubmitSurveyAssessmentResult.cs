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
using Newtonsoft.Json;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzSubmitSurveyAssessmentResult : BuzCommand
    {
        public override BaseDataModel ExecuteInput(object input)
        {
            {

                // Preparation Variable
                SubmitSurveyAssessmentResultOutputModel_Pass output = new SubmitSurveyAssessmentResultOutputModel_Pass();
                output.data = new SubmitSurveyAssessmentResultDataModel_Pass();

                // Deserialize Input
                // SubmitSurveyAssessmentResultInputModel contentModel = DeserializeJson<SubmitSurveyAssessmentResultInputModel>(input.ToString());
                SubmitSurveyAssessmentResultInputModel contentModel = (SubmitSurveyAssessmentResultInputModel)input;

                // Connect SDK and query
                var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
                OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
                ServiceContext svcContext = new ServiceContext(_serviceProxy);

                int type = 100000000 + contentModel.assessmentType - 1;
                var query = from c in svcContext.pfc_assessmentSet
                            where c.pfc_assessment_ref_code == contentModel.assessmentRefCode && c.pfc_assessment_type.Value == type
                            select c;


                // Condition check if ref_code don't have in CRM -> RETURN ERROR
                if (query.FirstOrDefault<pfc_assessment>() == null)
                {
                    output.code = AppConst.CODE_FAILED;
                    output.message = "ไม่สามารถบันทึกคะแนนได้";
                    output.description = "ไม่สามารถบันทึกคะแนนได้เนื่องจากไม่พบแบบสำรวจความพึงพอใจ";
                    output.transactionId = TransactionId;
                    output.transactionDateTime = DateTime.Now;

                    return output;
                }
                // if already have ref_code in CRM -> RETURN message
                else
                {
                    pfc_assessment firstQuery = query.FirstOrDefault<pfc_assessment>();
                    Guid guid = new Guid(firstQuery.Id.ToString());
                    pfc_assessment retrievedAssessment = (pfc_assessment)_serviceProxy.Retrieve(pfc_assessment.EntityLogicalName, guid, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));

                    if (firstQuery.pfc_assessment_status.Value != 100000000)
                    {
                        output.code = AppConst.CODE_FAILED;
                        output.message = "สถานะไม่ถูกต้อง: (" + firstQuery.pfc_assessment_status.Value + ")";
                        output.description = "แบบสอบถามนี้ได้ถูกประเมินหรือพ้นช่วงเวลาประเมินแล้ว";
                        output.transactionId = TransactionId;
                        output.transactionDateTime = DateTime.Now;

                        return output;
                    }
                    else
                    {
                        var queryQuestionnair = from c in svcContext.pfc_questionnairSet
                                                where c.pfc_questionnairId == contentModel.assessmentQuestionnaireId
                                                select c;

                        if (queryQuestionnair.FirstOrDefault<pfc_questionnair>() == null)
                        {
                            output.code = AppConst.CODE_FAILED;
                            output.message = "ไม่สามารถบันทึกคะแนนได้";
                            output.description = "ไม่สามารถบันทึกคะแนนได้เนื่องจากไม่พบแบบสำรวจความพึงพอใจ";
                            output.transactionId = TransactionId;
                            output.transactionDateTime = DateTime.Now;

                            return output;
                        }
                        else
                        {
                            retrievedAssessment.pfc_assessment_date = DateTime.Now;
                            retrievedAssessment.pfc_assessment_score1 = contentModel.assessmentScore1;
                            retrievedAssessment.pfc_assessment_score2 = contentModel.assessmentScore2;
                            retrievedAssessment.pfc_assessment_score3 = contentModel.assessmentScore3;
                            retrievedAssessment.pfc_assessment_score4 = contentModel.assessmentScore4;
                            retrievedAssessment.pfc_assessment_score5 = contentModel.assessmentScore5;
                            retrievedAssessment.pfc_assessment_score6 = contentModel.assessmentScore6;
                            retrievedAssessment.pfc_assessment_score7 = contentModel.assessmentScore7;
                            retrievedAssessment.pfc_assessment_score8 = contentModel.assessmentScore8;
                            retrievedAssessment.pfc_assessment_score9 = contentModel.assessmentScore9;
                            retrievedAssessment.pfc_assessment_score10 = contentModel.assessmentScore10;
                            retrievedAssessment.pfc_assessment_comment = contentModel.assessmentComment;
                            retrievedAssessment.pfc_assessment_status = new OptionSetValue(100000001);
                            if (retrievedAssessment.pfc_assessment_by_userid == null)
                            {
                                retrievedAssessment.pfc_assessment_by = new OptionSetValue(100000000);
                            }
                            else
                            {
                                retrievedAssessment.pfc_assessment_by = new OptionSetValue(100000001);
                            }
                            retrievedAssessment.pfc_assessment_survey_ipaddress = HttpContext.Current.Request.UserHostAddress;
                            retrievedAssessment.pfc_assessee_code = contentModel.assesseeCode;
                            retrievedAssessment.pfc_assessee_name = contentModel.assesseeName;

                            try
                            {
                                _serviceProxy.Update(retrievedAssessment);
                            }
                            catch (Exception e)
                            {
                                output.code = AppConst.CODE_FAILED;
                                output.message = "ไม่สามารถบันทึกคะแนนได้";
                                output.description = e.Message;
                                output.transactionId = TransactionId;
                                output.transactionDateTime = DateTime.Now;
                                return output;
                            }

                            output.code = AppConst.CODE_SUCCESS;
                            output.message = AppConst.MESSAGE_SUCCESS;
                            output.description = "บันทึกคะแนนเรียบร้อยแล้ว";
                            output.transactionId = TransactionId;
                            output.transactionDateTime = DateTime.Now;
                            return output;
                        }

                    }
                }

            }
        }
        

        
    }
}