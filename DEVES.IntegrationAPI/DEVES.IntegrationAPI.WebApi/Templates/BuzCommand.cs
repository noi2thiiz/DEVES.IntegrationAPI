using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public abstract class BuzCommand:BaseCommand
    {
      
        /*
        public void AddDebugInfo(string message, dynamic info)
        {
            TraceDebugLogger.Instance.AddDebugLogInfo(TransactionId,message,info);
           // debugInfo.AddDebugInfo(message, info);
        }
        public void AddDebugInfo(string message)
        {
            TraceDebugLogger.Instance.AddDebugLogInfo(TransactionId, message, message);
           // debugInfo.AddDebugInfo(message, message);
        }
        */
        
        public override BaseDataModel Execute(object input)
        {
            try
            {

                //for remove redundant  try catch
                AddDebugInfo("ExecuteInput", input);
                return ExecuteInput(input);


            }
            catch (BuzErrorException e)
            {
                if (e.OutputModel != null)
                {
                   
                    return (BaseDataModel)e.OutputModel;
                }

                var regFail = new OutputGenericDataModel<object>
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message?.Trim(),
                    description = e.Description?.Trim(),
                    stackTrace = e.StackTrace
                };

                if (string.IsNullOrEmpty(e.Code))
                {
                    regFail.code = e.Code;
                }

                if (e.SourceData != null)
                {
                    regFail.data = e.SourceData;
                }
                regFail.transactionId = !string.IsNullOrEmpty(e.TransactionId) ? e.TransactionId : TransactionId;

                AddDebugInfo("BuzErrorException", regFail);
                return regFail;
            }
            catch (FieldValidationException e)
            {
                var regFail = new OutputModelFail
                {
                    code = AppConst.CODE_INVALID_INPUT,
                    description =  e.description,
                    transactionId = TransactionId,
                    transactionDateTime = DateTime.Now,
                    message = AppConst.MESSAGE_INVALID_INPUT,

               };

                if (e.fieldErrorData.fieldErrors.Any())
                {

                    regFail.data = e.fieldErrorData;
                    regFail.description = e.description;



                }
                else if (!string.IsNullOrEmpty(e.fieldError))
                {
                    regFail.code = AppConst.CODE_FAILED;
                    regFail.AddFieldError(e.fieldError, e.fieldMessage);
                    regFail.description = e.fieldMessage;
                }

                if (!string.IsNullOrEmpty(e.message))
                {
                    regFail.message = e.message;
                }

               


                if (string.IsNullOrEmpty(regFail.description))
                {
                    regFail.description = e.fieldMessage;
                }

                

                if (string.IsNullOrEmpty(regFail.description))
                {
                   regFail.description = AppConst.DESC_INVALID_INPUT;
                }

                AddDebugInfo("FieldValidationException", regFail);
                return regFail;
            }
            catch (Exception e)
            {
                var regFail = new OutputModelFail
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message,
                    description = "",
                    stackTrace = e.StackTrace,
                };
                AddDebugInfo("Exception", regFail);
                return regFail;


            }

           
        }

        public List<string> SearchCrmContactClientId(string cleansingId)
        {

            // Console.WriteLine("SearchCrmContactClientId");
            // For performance, until we found the way to cache the ServiceProxy, we prefer SQL rather than Crm
            List<string> result = new List<string>();
            try
            {
                result= SpApiCustomerClient.Instance.SearchCrmContactClientId("P", cleansingId);
            }
            catch (Exception e)
            {
                AddDebugInfo(e.Message);
                throw;
            }
            return result;

        }
        internal List<string> SearchCrmAccountClientId(string cleansingId)
        {


            // For performance, until we found the way to cache the ServiceProxy, we prefer SQL rather than Crm
            List<string> result = new List<string>();
            try
            {
                result = SpApiCustomerClient.Instance.SearchCrmContactClientId("C", cleansingId);
            }
            catch (Exception e)
            {
                AddDebugInfo("SearchCrmAccountClientId Error:"+e.Message,e.StackTrace);
                throw;
            }
            return result;


        }
        internal List<string> SearchCrmClientId(string cleansingId, string clienType)
        {


            // For performance, until we found the way to cache the ServiceProxy, we prefer SQL rather than Crm
            List<string> result = new List<string>();
            try
            {
                
                result = SpApiCustomerClient.Instance.SearchCrmContactClientId(clienType, cleansingId);
               
               
            }
            catch (Exception e)
            {
                AddDebugInfo("SearchCrmClientId:"+e.Message,e.StackTrace);
                throw;
            }
            return result;


        }
        

        public abstract Model.BaseDataModel ExecuteInput(object input);
    }
}