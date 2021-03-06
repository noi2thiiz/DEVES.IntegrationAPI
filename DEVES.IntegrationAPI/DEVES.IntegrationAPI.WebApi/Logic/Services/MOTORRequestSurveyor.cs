﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model.RequestSurveyor;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services
{
    public class MOTORRequestSurveyor : BaseEwiServiceProxy
    {
       
        public MOTORRequestSurveyor(string globalTransactionID, string controllerName = "") : base(globalTransactionID, controllerName)
        {
            serviceName = "MOTOR_RequestSurveyor";
            systemName = "MOTOR";
            serviceEndpoint = AppConfig.Instance.Get(CommonConstant.ewiEndpointKeyMOTORRequestSurveyor);
        }

        public RequestSurveyorContentOutputModel Execute(RequestSurveyorInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<RequestSurveyorOutputModel>(result.Content);
            return contentObj?.content;
        }

        public RequestSurveyorOutputModel ExecuteEWI(RequestSurveyorInputModel input)
        {

            var result = SendRequest(input, serviceEndpoint);


            var jss = new JavaScriptSerializer();
            var contentObj = jss.Deserialize<RequestSurveyorOutputModel>(result.Content);
            return contentObj;
        }

        public RequestSurveyorContentOutputModel Execute(BaseDataModel inputData)
        {
            return Execute((RequestSurveyorInputModel)inputData);
        }

    }
}