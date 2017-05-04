using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public abstract class BuzCommand:BaseCommand
    {
        public override BaseDataModel Execute(object input)
        {
            try
            {

                //for remove redundant  try catch
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
                    
                };
                if (e.SourceData != null)
                {
                    regFail.data = e.SourceData;
                }
               
                return regFail;
            }
            catch (FieldValidationException e)
            {
                var regFail = new OutputModelFail
                {
                    code = AppConst.CODE_INVALID_INPUT,
                    description = AppConst.DESC_INVALID_INPUT,
                    transactionId = TransactionId,
                    transactionDateTime = DateTime.Now,
                    message = AppConst.MESSAGE_INVALID_INPUT
                };

                if (e.fieldErrorData.fieldErrors.Any())
                {

                    regFail.data = e.fieldErrorData;

                }else if (!string.IsNullOrEmpty(e.fieldError))
                {
                    regFail.code = AppConst.CODE_FAILED;
                    regFail.AddFieldError(e.fieldError, e.fieldMessage);
                    regFail.description = e.fieldMessage;
                }
                if (string.IsNullOrEmpty(e.message)) return regFail;

                regFail.message = e.message;
                regFail.description = e.fieldMessage;

                return regFail;
            }
            catch (Exception e)
            {
                var regFail = new OutputModelFail
                {
                    code = AppConst.CODE_FAILED,
                    message = e.Message,
                    description = e.StackTrace
                };

                return regFail;


            }

           
        }
        public abstract Model.BaseDataModel ExecuteInput(object input);
    }
}