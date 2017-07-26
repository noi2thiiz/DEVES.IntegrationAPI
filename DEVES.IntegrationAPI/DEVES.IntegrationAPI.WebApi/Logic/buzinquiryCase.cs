using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Logic.Converter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.InquiryCaseModel;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzinquiryCase :BuzCommand
    {
        private System.Data.DataTable dt = new System.Data.DataTable();
        public override BaseDataModel ExecuteInput(object input)
        {
            inquiryCaseInputModel caseS = new inquiryCaseInputModel();
            caseS = (inquiryCaseInputModel)input;
            string jsonValue = string.Format("{0}|{1}", string.IsNullOrEmpty(caseS.conditions.cleansingId) ? "" : caseS.conditions.cleansingId
                   , string.IsNullOrEmpty(caseS.conditions.ticketNo) ? "" : caseS.conditions.ticketNo
                   );


            QueryInfo newQuery = new QueryInfo();
            dt = newQuery.Queryinfo_inquiryCase(jsonValue);

            inquiryCaseOutputModel output = new inquiryCaseOutputModel();
            output.data = new List<inquiryCaseDataOutput>();

            int datarow = dt.Rows.Count;
            //loop
            for (int i = 0; i < datarow; i++)
            {
                inquiryCaseDataOutput dataOutput = new inquiryCaseDataOutput();

                dataOutput.caseNo = string.IsNullOrEmpty(dt.Rows[i]["caseNo"].ToString()) ? "" : dt.Rows[i]["caseNo"].ToString();
                dataOutput.caseTitle = string.IsNullOrEmpty(dt.Rows[i]["caseTitle"].ToString()) ? "" : dt.Rows[i]["caseTitle"].ToString();
                dataOutput.caseType = string.IsNullOrEmpty(dt.Rows[i]["caseType"].ToString()) ? "" : dt.Rows[i]["caseType"].ToString();
                dataOutput.category = string.IsNullOrEmpty(dt.Rows[i]["category"].ToString()) ? "" : dt.Rows[i]["category"].ToString();
                dataOutput.subCategory = string.IsNullOrEmpty(dt.Rows[i]["subCategory"].ToString()) ? "" : dt.Rows[i]["subCategory"].ToString();
                dataOutput.priority = string.IsNullOrEmpty(dt.Rows[i]["priority"].ToString()) ? "" : dt.Rows[i]["priority"].ToString();
                dataOutput.origin = string.IsNullOrEmpty(dt.Rows[i]["origin"].ToString()) ? "" : dt.Rows[i]["origin"].ToString();
                dataOutput.description = string.IsNullOrEmpty(dt.Rows[i]["description"].ToString()) ? "" : dt.Rows[i]["description"].ToString();               
                dataOutput.policyAdditionalId = string.IsNullOrEmpty(dt.Rows[i]["policyAdditionalId"].ToString()) ? "" : dt.Rows[i]["policyAdditionalId"].ToString();
                dataOutput.policyNo = string.IsNullOrEmpty(dt.Rows[i]["policyNo"].ToString()) ? "" : dt.Rows[i]["policyNo"].ToString();
                dataOutput.policyAdditionalNo = string.IsNullOrEmpty(dt.Rows[i]["policyAdditionalNo"].ToString()) ? "" : dt.Rows[i]["policyAdditionalNo"].ToString();               
                output.data.Add(dataOutput);
                #region [condition]
                var duedatechk = string.IsNullOrEmpty(dt.Rows[i]["dueDate"].ToString()) ? "" : dt.Rows[i]["dueDate"].ToString();
                if (duedatechk != null && duedatechk != "")
                {
                    dataOutput.dueDate = (DateTime)dt.Rows[i]["dueDate"];
                }

                #endregion
            }

            //loop
            output.code = AppConst.CODE_SUCCESS;
            output.message = AppConst.MESSAGE_SUCCESS;
            output.description = "";
            output.transactionId = TransactionId;
            output.transactionDateTime = DateTime.Now;
            return output;// newQuery.Queryinfo_searchPerson(jsonValue);

        }
    }
}