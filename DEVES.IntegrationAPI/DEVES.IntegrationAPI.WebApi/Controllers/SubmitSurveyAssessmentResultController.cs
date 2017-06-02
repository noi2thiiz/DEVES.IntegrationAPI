using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.SubmitSurveyAssessmentResult;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class SubmitSurveyAssessmentResultController : ApiController
    {

        private Guid _transactionId = new Guid();

        public object Post([FromBody]object value)
        {
            _transactionId = Guid.NewGuid();
            
            var outputPass = new SubmitSurveyAssessmentResultOutputModel_Pass();
            var outputFail = new SubmitSurveyAssessmentResultOutputModel_Fail();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<SubmitSurveyAssessmentResultInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/AssignedSurveyorInfo_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                outputPass = new SubmitSurveyAssessmentResultOutputModel_Pass();
                // outputPass = HandleMessage(contentText, contentModel);
                return Request.CreateResponse<SubmitSurveyAssessmentResultOutputModel_Pass>(outputPass);
            }
            else
            {
                outputFail = new SubmitSurveyAssessmentResultOutputModel_Fail();
                outputFail.data = new SubmitSurveyAssessmentResultDataModel_Fail();
                outputFail.data.fieldErrors = new List<SubmitSurveyAssessmentResultFieldErrorsModel>();

                List<string> errorMessage = JsonHelper.getReturnError();
                foreach (var text in errorMessage)
                {
                    string fieldMessage = "";
                    string fieldName = "";
                    if (text.Contains("Required properties"))
                    {
                        int indexEnd = 0;
                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals(":"))
                            {
                                fieldMessage = text.Substring(0, i);
                                indexEnd = i + 1;
                            }
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldName = text.Substring(indexEnd, i - indexEnd).Trim();
                                break;
                            }
                        }

                        string[] splitProp = fieldName.Split(',');
                        for (int i = 0; i < splitProp.Length; i++)
                        {
                            outputFail.data.fieldErrors.Add(new SubmitSurveyAssessmentResultFieldErrorsModel(splitProp[i].Trim(), fieldMessage));
                        }
                    }
                    else if (text.Contains("exceeds maximum length"))
                    {
                        bool isMessage = false;
                        int endMessage = 0;
                        int startName = 0;
                        int endName = 0;
                        for (int i = 0; i < text.Length - 4; i++)
                        {
                            if (text.Substring(i, 4).Equals("Path"))
                            {
                                fieldMessage = text.Substring(0, i - 1);
                                isMessage = true;
                                endMessage = i + "Path".Length;
                            }
                            if (isMessage)
                            {
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (text.Contains("minimum length"))
                    {
                        bool isMessage = false;
                        int startName = 0;
                        int endName = 0;
                        for (int i = 0; i < text.Length - 7; i++)
                        {
                            string check = text.Substring(i, 7);
                            if (text.Substring(i, 7).Equals("minimum"))
                            {
                                fieldMessage = "Required field must not be null";
                                isMessage = true;
                            }
                            if (isMessage)
                            {
                                if (text.Substring(i, 1).Equals("'"))
                                {
                                    if (startName == 0)
                                    {
                                        startName = i + 1;
                                    }
                                    else if (endName == 0)
                                    {
                                        endName = i - 1;
                                    }
                                }
                                if (startName != 0 && endName != 0)
                                {
                                    fieldName = text.Substring(startName, i - startName).Trim();
                                    break;
                                }
                            }
                        }
                    }
                    else if (text.Contains("Invalid type."))
                    {
                        int startIndex = "Invalid type.".Length;
                        int endMessage = 0;
                        int startName = 0;
                        int endName = 0;
                        for (int i = startIndex; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldMessage = text.Substring(0, i);
                                endMessage = i + 1;
                            }
                            if (text.Substring(i, 1).Equals("'"))
                            {
                                if (startName == 0)
                                {
                                    startName = i + 1;
                                }
                                else if (endName == 0)
                                {
                                    endName = i - 1;
                                }
                            }
                            if (startName != 0 && endName != 0)
                            {
                                fieldName = text.Substring(startName, i - startName).Trim();
                                break;
                            }
                        }
                    }
                    else if (text.Contains("not defined in enum"))
                    {
                        int startName = 0;
                        int endName = 0;
                        bool isChecking = true;

                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals(".") && isChecking)
                            {
                                fieldMessage = text.Substring(0, i);
                                isChecking = false;
                            }
                            if (text.Substring(i, 1).Equals("'"))
                            {
                                if (startName == 0)
                                {
                                    startName = i + 1;
                                }
                                else if (endName == 0)
                                {
                                    endName = i - 1;
                                }
                            }
                            if (startName != 0 && endName != 0)
                            {
                                fieldName = text.Substring(startName, i - startName).Trim();
                                break;
                            }
                        }
                    }

                    if (!text.Contains("Required properties"))
                    {
                        outputFail.data.fieldErrors.Add(new SubmitSurveyAssessmentResultFieldErrorsModel(fieldName, fieldMessage));
                    }

                }

                outputFail.code = "400";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = _transactionId.ToString();
                outputFail.transactionDateTime = DateTime.Now.ToString();
                
                // _log.ErrorFormat("ErrorCode: {0} {1} ErrorDescription: {1}", fieldError.name, Environment.NewLine, fieldError.message);
                return Request.CreateResponse<SubmitSurveyAssessmentResultOutputModel_Fail>(outputFail);
            }
        }

    }
}