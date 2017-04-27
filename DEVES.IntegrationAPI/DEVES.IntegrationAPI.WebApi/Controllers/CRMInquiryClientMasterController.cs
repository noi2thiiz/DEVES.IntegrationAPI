using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Newtonsoft.Json;

using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.Model.InquiryClientMaster;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;

namespace DEVES.IntegrationAPI.WebApi.Controllers
{
    public class CRMInquiryClientMasterController : ApiController
    {
        public object Post([FromBody]object value)
        {
            //+ Deserialize Input
            //InquiryClientMasterInputModel contentModel = DeserializeJson<InquiryClientMasterInputModel>(input.ToString());

            buzCrmInquiryClientMaster inqClientCmd = new buzCrmInquiryClientMaster();

            if (Request.Properties["TransactionID"] == null)
            {
                inqClientCmd.TransactionId = Guid.NewGuid().ToString();
                Request.Properties["TransactionID"] = inqClientCmd.TransactionId;
            }
            else
            {
                inqClientCmd.TransactionId = Request.Properties["TransactionID"].ToString();
            }
            // Request.Properties["TransactionID"].ToString();

            var contentText = value.ToString();
            var contentModel = JsonConvert.DeserializeObject<InquiryClientMasterInputModel>(contentText);

            string outvalidate = string.Empty;
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/JsonSchema/InquiryClientMaster_Input_Schema.json");

            if (JsonHelper.TryValidateJson(contentText, filePath, out outvalidate))
            {
                var content = inqClientCmd.Execute(inqClientCmd.DeserializeJson<InquiryClientMasterInputModel>(value.ToString()));
                return Request.CreateResponse(content);
            }
            else
            {
                var outputFail = new InquiryClientMasterOutputModel_Fail();
                outputFail.data = new InquiryClientMasterDataOutputModel_Fail();
                outputFail.data.fieldErrors = new List<InquiryClientMasterListFieldErrors>();
                
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

                        for (int i = 0; i < text.Length - 1; i++)
                        {
                            if (text.Substring(i, 1).Equals("."))
                            {
                                fieldMessage = text.Substring(0, i);
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

                    outputFail.data.fieldErrors.Add(new InquiryClientMasterListFieldErrors(fieldName, fieldMessage));
                }

                outputFail.code = "500";
                outputFail.message = "Invalid Input(s)";
                outputFail.description = "Some of your input is invalid. Please recheck again.";
                outputFail.transactionId = Request.Properties["TransactionID"].ToString();
                outputFail.transactionDateTime = DateTime.Now.ToString();

                return Request.CreateResponse<InquiryClientMasterOutputModel_Fail>(outputFail);
            }

        }
    }
}
