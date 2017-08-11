using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.InquiryPolicyModel;
using DEVES.IntegrationAPI.WebApi.Logic.Converter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Ajax.Utilities;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzinquiryPolicy : BuzCommand
    {

        private System.Data.DataTable dt = new System.Data.DataTable();

        public List<inquiryPolicyDataOutput> processDistinct(List<inquiryPolicyDataOutput> data)
        {
            return data
                .DistinctBy(row => new
                {
                  row.policyGuid
                }).ToList();
        }
        public List<inquiryPolicyDataOutput> getclaimList(List<inquiryPolicyDataOutput> data,string policyGuid)
        {
            return data.Where(
                                row => row?.policyGuid == policyGuid &&
                                !string.IsNullOrEmpty(row?.rawData["claimNo"]?.ToString())
                              ).ToList();     
            //crmInqContent.data = crmInqContent.data.Where(row => row?.profileInfo?.name1.Trim() != "" || row?.profileInfo?.fullName.Trim() != "").ToList();
        }
        public override BaseDataModel ExecuteInput(object input)
        {
            
            inquiryPolicyInputModel policyS = new inquiryPolicyInputModel();
            policyS = (inquiryPolicyInputModel)input;

            if (string.IsNullOrEmpty(policyS.conditions.cleansingId) && string.IsNullOrEmpty(policyS.conditions.crmClientId)) 
                {
                    var data = new OutputModelFailData();
                    data.AddFieldError("conditions.cleansingId", "cleansingId cannot null or empty");
                    data.AddFieldError("conditions.crmCleintId", "crmClientId cannot null or empty");
                    throw new FieldValidationException(data);
                }
            string jsonValue = string.Format("{0}|{1}|{2}|{3}|{4}", string.IsNullOrEmpty(policyS.conditions.crmClientId) ? "" : policyS.conditions.crmClientId
                   , string.IsNullOrEmpty(policyS.conditions.cleansingId) ? "" : policyS.conditions.cleansingId
                   , string.IsNullOrEmpty(policyS.conditions.policyCarRegisterNo) ? "" : policyS.conditions.policyCarRegisterNo
                   , string.IsNullOrEmpty(policyS.conditions.policyNo) ? "" : policyS.conditions.policyNo
                   , string.IsNullOrEmpty(policyS.conditions.chassisNo) ? "" : policyS.conditions.chassisNo
                   ); 


            QueryInfo newQuery = new QueryInfo();
            dt = newQuery.Queryinfo_inquiryPolicy(jsonValue);           
            inquiryPolicyOutputModel output = new inquiryPolicyOutputModel();
            output.data = new List<inquiryPolicyDataOutput>();
            int datarow = dt.Rows.Count;
            #region [comment]

            for (int i = 0; i < datarow; i++)
            {
                inquiryPolicyDataOutput dataOutput = new inquiryPolicyDataOutput();
                dataOutput.policyInfo = new inquiryPolicyDataOutputPolicy();
                dataOutput.claims = new List<inquiryPolicyDataOutputClaims>();
                dataOutput.policyGuid = string.IsNullOrEmpty(dt.Rows[i]["crmPolicyDetailId"].ToString()) ? "" : dt.Rows[i]["crmPolicyDetailId"].ToString();
                dataOutput.rawData = dt.Rows[i];
                dataOutput.policyInfo.crmPolicyDetailId = string.IsNullOrEmpty(dt.Rows[i]["crmPolicyDetailId"].ToString()) ? "" : dt.Rows[i]["crmPolicyDetailId"].ToString();
                dataOutput.policyInfo.policyNo = string.IsNullOrEmpty(dt.Rows[i]["PolicyNo"].ToString()) ? "" : dt.Rows[i]["PolicyNo"].ToString();
                dataOutput.policyInfo.renewalNo = string.IsNullOrEmpty(dt.Rows[i]["renewalNo"].ToString()) ? "" : dt.Rows[i]["renewalNo"].ToString();
                dataOutput.policyInfo.fleetCarNo = string.IsNullOrEmpty(dt.Rows[i]["fleetCarNo"].ToString()) ? "" : dt.Rows[i]["fleetCarNo"].ToString();
                dataOutput.policyInfo.barcode = string.IsNullOrEmpty(dt.Rows[i]["barCode"].ToString()) ? "" : dt.Rows[i]["barCode"].ToString();
                dataOutput.policyInfo.insureCardNo = string.IsNullOrEmpty(dt.Rows[i]["insureCardNo"].ToString()) ? "" : dt.Rows[i]["insureCardNo"].ToString();
                dataOutput.policyInfo.policySeqNo = string.IsNullOrEmpty(dt.Rows[i]["policySeqNo"].ToString()) ? "" : dt.Rows[i]["policySeqNo"].ToString();
                dataOutput.policyInfo.branchCode = string.IsNullOrEmpty(dt.Rows[i]["branchCode"].ToString()) ? "" : dt.Rows[i]["branchCode"].ToString();
                dataOutput.policyInfo.contractType = string.IsNullOrEmpty(dt.Rows[i]["contractType"].ToString()) ? "" : dt.Rows[i]["contractType"].ToString();
                dataOutput.policyInfo.policyProductTypeCode = string.IsNullOrEmpty(dt.Rows[i]["policyProductTypeCode"].ToString()) ? "" : dt.Rows[i]["policyProductTypeCode"].ToString();
                dataOutput.policyInfo.policyProductTypeName = string.IsNullOrEmpty(dt.Rows[i]["policyProductTypeName"].ToString()) ? "" : dt.Rows[i]["policyProductTypeName"].ToString();
                string policyissuechk = dt.Rows[i]["policyIssueDate"].ToString();
                if (policyissuechk != null && policyissuechk != "")
                {
                    dataOutput.policyInfo.policyIssueDate = (DateTime)dt.Rows[i]["policyIssueDate"];
                }
                string policyeffectivechk = dt.Rows[i]["policyEffectiveDate"].ToString();
                if (policyeffectivechk != null && policyeffectivechk != "")
                {
                    dataOutput.policyInfo.policyEffectiveDate = (DateTime)dt.Rows[i]["policyEffectiveDate"];
                }
                string policyexpirychk = dt.Rows[i]["policyExpiryDate"].ToString();
                if (policyexpirychk != null && policyexpirychk != "")
                {
                    dataOutput.policyInfo.policyExpiryDate = (DateTime)dt.Rows[i]["policyExpiryDate"];
                }
                dataOutput.policyInfo.policyGarageFlag = string.IsNullOrEmpty(dt.Rows[i]["policyGarageFlag"].ToString()) ? "" : dt.Rows[i]["policyGarageFlag"].ToString();
                dataOutput.policyInfo.policyPaymentStatus = string.IsNullOrEmpty(dt.Rows[i]["policyPaymentStatus"].ToString()) ? "" : dt.Rows[i]["policyPaymentStatus"].ToString();
                dataOutput.policyInfo.policyCarRegisterNo = string.IsNullOrEmpty(dt.Rows[i]["regnum"].ToString()) ? "" : dt.Rows[i]["regnum"].ToString();
                dataOutput.policyInfo.policyCarRegisterProv = string.IsNullOrEmpty(dt.Rows[i]["regnumprov"].ToString()) ? "" : dt.Rows[i]["regnumprov"].ToString();
                dataOutput.policyInfo.carChassisNo = string.IsNullOrEmpty(dt.Rows[i]["carChassisNo"].ToString()) ? "" : dt.Rows[i]["carChassisNo"].ToString();
                dataOutput.policyInfo.carVehicleType = string.IsNullOrEmpty(dt.Rows[i]["carVehicleType"].ToString()) ? "" : dt.Rows[i]["carVehicleType"].ToString();
                dataOutput.policyInfo.carVehicleModel = string.IsNullOrEmpty(dt.Rows[i]["carVehicleModel"].ToString()) ? "" : dt.Rows[i]["carVehicleModel"].ToString();
                dataOutput.policyInfo.carVehicleYear = string.IsNullOrEmpty(dt.Rows[i]["carVehicleYear"].ToString()) ? "" : dt.Rows[i]["carVehicleYear"].ToString();
                dataOutput.policyInfo.carVehicleBody = string.IsNullOrEmpty(dt.Rows[i]["carVehicleBody"].ToString()) ? "" : dt.Rows[i]["carVehicleBody"].ToString();
                dataOutput.policyInfo.carVehicleSize = string.IsNullOrEmpty(dt.Rows[i]["carVehicleSize"].ToString()) ? "" : dt.Rows[i]["carVehicleSize"].ToString();
                dataOutput.policyInfo.policyDeduct = string.IsNullOrEmpty(dt.Rows[i]["policyDeduct"].ToString()) ? "" : dt.Rows[i]["policyDeduct"].ToString();
                dataOutput.policyInfo.agentCode = string.IsNullOrEmpty(dt.Rows[i]["agentCode"].ToString()) ? "" : dt.Rows[i]["agentCode"].ToString();
                dataOutput.policyInfo.agentName = string.IsNullOrEmpty(dt.Rows[i]["agentName"].ToString()) ? "" : dt.Rows[i]["agentName"].ToString();
                dataOutput.policyInfo.agentBranch = string.IsNullOrEmpty(dt.Rows[i]["agentBranch"].ToString()) ? "" : dt.Rows[i]["agentBranch"].ToString();
                dataOutput.policyInfo.insuredCleansingId = string.IsNullOrEmpty(dt.Rows[i]["insuredCleansingId"].ToString()) ? "" : dt.Rows[i]["insuredCleansingId"].ToString();
                dataOutput.policyInfo.insuredClientId = string.IsNullOrEmpty(dt.Rows[i]["insuredClientId"].ToString()) ? "" : dt.Rows[i]["insuredClientId"].ToString();
                dataOutput.policyInfo.insuredClientType = string.IsNullOrEmpty(dt.Rows[i]["insuredClientType"].ToString()) ? "" : dt.Rows[i]["insuredClientType"].ToString();
                dataOutput.policyInfo.insuredFullName = string.IsNullOrEmpty(dt.Rows[i]["insuredFullName"].ToString()) ? "" : dt.Rows[i]["insuredFullName"].ToString();
                dataOutput.policyInfo.policyStatus = string.IsNullOrEmpty(dt.Rows[i]["PolicyStatus"].ToString()) ? "" : dt.Rows[i]["PolicyStatus"].ToString();
                //#region [claim]               
                //dataOutput.claims.driverFullName = string.IsNullOrEmpty(dt.Rows[i]["driverFullName"].ToString()) ? "" : dt.Rows[i]["driverFullName"].ToString();
                //dataOutput.claims.driverCleansingId = string.IsNullOrEmpty(dt.Rows[i]["driverCleansingId"].ToString()) ? "" : dt.Rows[i]["driverCleansingId"].ToString();
                //dataOutput.claims.driverClientId = string.IsNullOrEmpty(dt.Rows[i]["driverClientId"].ToString()) ? "" : dt.Rows[i]["driverClientId"].ToString();
                //dataOutput.claims.claimNo = string.IsNullOrEmpty(dt.Rows[i]["claimNo"].ToString()) ? "" : dt.Rows[i]["claimNo"].ToString();
                //dataOutput.claims.customerName = string.IsNullOrEmpty(dt.Rows[i]["customerName"].ToString()) ? "" : dt.Rows[i]["customerName"].ToString();
                //dataOutput.claims.claimNotiNo = string.IsNullOrEmpty(dt.Rows[i]["claimNotiNo"].ToString()) ? "" : dt.Rows[i]["claimNotiNo"].ToString();
                //dataOutput.claims.claimAgentCode = string.IsNullOrEmpty(dt.Rows[i]["claimAgentCode"].ToString()) ? "" : dt.Rows[i]["claimAgentCode"].ToString();
                //dataOutput.claims.claimAgentName = string.IsNullOrEmpty(dt.Rows[i]["claimAgentName"].ToString()) ? "" : dt.Rows[i]["claimAgentName"].ToString();
                //dataOutput.claims.claimAgentBranch = string.IsNullOrEmpty(dt.Rows[i]["claimAgentBranch"].ToString()) ? "" : dt.Rows[i]["claimAgentBranch"].ToString();
                //dataOutput.claims.regNo = string.IsNullOrEmpty(dt.Rows[i]["regNo"].ToString()) ? "" : dt.Rows[i]["regNo"].ToString();
                //dataOutput.claims.regNoProvince = string.IsNullOrEmpty(dt.Rows[i]["regNoProvince"].ToString()) ? "" : dt.Rows[i]["regNoProvince"].ToString();
                //#endregion

                //loop


                // .regNoProvince = string.IsNullOrEmpty(dt.Rows[i]["regNoProvince"].ToString()) ? "" : dt.Rows[i]["regNoProvince"].ToString();

                output.data.Add(dataOutput);
                 }
            #region
            var bigdata = output.data;
                output.data = processDistinct(bigdata);
                foreach(var policydata in output.data)
                {
                    var claimList = getclaimList(bigdata, policydata.policyGuid);
                    policydata.claims = new List<inquiryPolicyDataOutputClaims>();
                    foreach (var claimdata in claimList)
                    {
                        inquiryPolicyDataOutputClaims claimItem = new inquiryPolicyDataOutputClaims();
                        claimItem.driverFullName = string.IsNullOrEmpty(claimdata.rawData["driverFullName"].ToString()) ? "" : claimdata.rawData["driverFullName"].ToString();
                        claimItem.driverCleansingId = string.IsNullOrEmpty(claimdata.rawData["driverCleansingId"].ToString()) ? "" : claimdata.rawData["driverCleansingId"].ToString();
                        claimItem.driverClientId = string.IsNullOrEmpty(claimdata.rawData["driverClientId"].ToString()) ? "" : claimdata.rawData["driverClientId"].ToString();
                        claimItem.claimNo = string.IsNullOrEmpty(claimdata.rawData["claimNo"].ToString()) ? "" : claimdata.rawData["claimNo"].ToString();
                        claimItem.customerName = string.IsNullOrEmpty(claimdata.rawData["customerName"].ToString()) ? "" : claimdata.rawData["customerName"].ToString();
                        claimItem.claimNotiNo = string.IsNullOrEmpty(claimdata.rawData["claimNotiNo"].ToString()) ? "" : claimdata.rawData["claimNotiNo"].ToString();
                        claimItem.claimAgentCode = string.IsNullOrEmpty(claimdata.rawData["claimAgentCode"].ToString()) ? "" : claimdata.rawData["claimAgentCode"].ToString();
                        claimItem.claimAgentName = string.IsNullOrEmpty(claimdata.rawData["claimAgentName"].ToString()) ? "" : claimdata.rawData["claimAgentName"].ToString();
                        claimItem.claimAgentBranch = string.IsNullOrEmpty(claimdata.rawData["claimAgentBranch"].ToString()) ? "" : claimdata.rawData["claimAgentBranch"].ToString();
                        string claimOpenchk = claimdata.rawData["claimOpenDate"].ToString();
                        if(claimOpenchk != null && claimOpenchk != "")
                        {
                        claimItem.claimOpenDate = (DateTime)claimdata.rawData["claimOpenDate"];
                        }
                    claimItem.claimStatus = claimdata.rawData["claimStatus"].ToString();
                        policydata.claims.Add(claimItem);
                    }

                }
            #endregion
            #endregion



            output.code = AppConst.CODE_SUCCESS;
            output.message = AppConst.MESSAGE_SUCCESS;
            output.description = "";
            output.transactionId = TransactionId;
            output.transactionDateTime = DateTime.Now;
            return output;// newQuery.Queryinfo_searchPerson(jsonValue);
        }

        //public List<InquiryCrmPayeeListDataModel> ProcessDistinct(List<InquiryCrmPayeeListDataModel> sapResult)
        //{
        //    return sapResult
        //        .DistinctBy(row => new
        //        {
        //            row.fullName,
        //            row.name1,
        //            row.name2,
        //            row.cleansingId,
        //            row.polisyClientId,
        //            row.sapVendorCode,
        //            row.sapVendorGroupCode,
        //            row.taxNo,
        //            row.taxBranchCode

        //        }).ToList();
        //}

    }
}
