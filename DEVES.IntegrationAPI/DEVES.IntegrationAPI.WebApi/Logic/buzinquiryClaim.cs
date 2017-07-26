using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.WebApi.Logic.Converter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.Model.inquiryClaimModel;


namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzinquiryClaim : BuzCommand
    {
        private System.Data.DataTable dt = new System.Data.DataTable();
        public override BaseDataModel ExecuteInput(object input)
        {
           
             inquiryClaimInputModel claimSearch = new inquiryClaimInputModel();
            //personS = (personSearchInputModel)input;
            claimSearch = (inquiryClaimInputModel)input;
            string jsonValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", string.IsNullOrEmpty(claimSearch.conditions.cleansingId) ? "" : claimSearch.conditions.cleansingId
                   , string.IsNullOrEmpty(claimSearch.conditions.claimNotiNo) ? "" : claimSearch.conditions.claimNotiNo
                   , string.IsNullOrEmpty(claimSearch.conditions.claimNo) ? "" : claimSearch.conditions.claimNo
                   , string.IsNullOrEmpty(claimSearch.conditions.policyNo) ? "" : claimSearch.conditions.policyNo
                   , string.IsNullOrEmpty(claimSearch.conditions.policyCarRegisterNo) ? "" : claimSearch.conditions.policyCarRegisterNo
                   , string.IsNullOrEmpty(claimSearch.conditions.parentPolicyId) ? "" : claimSearch.conditions.parentPolicyId
                   );


            QueryInfo newQuery = new QueryInfo();
            dt = newQuery.Queryinfo_inquiryClaim(jsonValue);
            inquiryClaimOutputModel output = new inquiryClaimOutputModel();
            output.data = new List<inquiryClaimDataOutput>();
            int datarow = dt.Rows.Count;
            //loop
            for (int i = 0; i < datarow; i++)
            {
                inquiryClaimDataOutput dataOutput = new inquiryClaimDataOutput();
                dataOutput.policyInfo = new inquiryClaimDataOutputPolicyinfo();
                dataOutput.claimInfo = new inquiryClaimDataOutputClaiminfo();
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
                dataOutput.policyInfo.policyGarageFlag = string.IsNullOrEmpty(dt.Rows[i]["policyGarageFlag"].ToString()) ? "" : dt.Rows[i]["policyGarageFlag"].ToString();
                dataOutput.policyInfo.policyPaymentStatus = string.IsNullOrEmpty(dt.Rows[i]["policyPaymentStatus"].ToString()) ? "" : dt.Rows[i]["policyPaymentStatus"].ToString();
                dataOutput.policyInfo.policyCarRegisterNo = string.IsNullOrEmpty(dt.Rows[i]["regnum"].ToString()) ? "" : dt.Rows[i]["regnum"].ToString();
                dataOutput.policyInfo.policyCarRegisterProv = string.IsNullOrEmpty(dt.Rows[i]["regnumprov"].ToString()) ? "" : dt.Rows[i]["regnumprov"].ToString();
                dataOutput.policyInfo.policyStatus = string.IsNullOrEmpty(dt.Rows[i]["PolicyStatus"].ToString()) ? "" : dt.Rows[i]["PolicyStatus"].ToString();
                dataOutput.claimInfo.driverFullName = string.IsNullOrEmpty(dt.Rows[i]["driverFullName"].ToString()) ? "" : dt.Rows[i]["driverFullName"].ToString();
                dataOutput.claimInfo.driverClientId = string.IsNullOrEmpty(dt.Rows[i]["driverClientId"].ToString()) ? "" : dt.Rows[i]["driverClientId"].ToString();
                dataOutput.claimInfo.driverMobile = string.IsNullOrEmpty(dt.Rows[i]["driverMobile"].ToString()) ? "" : dt.Rows[i]["driverMobile"].ToString();
                dataOutput.claimInfo.ticketNumber = string.IsNullOrEmpty(dt.Rows[i]["caseNumber"].ToString()) ? "" : dt.Rows[i]["caseNumber"].ToString();
                dataOutput.claimInfo.claimNotiNo = string.IsNullOrEmpty(dt.Rows[i]["claimNotiNo"].ToString()) ? "" : dt.Rows[i]["claimNotiNo"].ToString();
                dataOutput.claimInfo.claimNo = string.IsNullOrEmpty(dt.Rows[i]["claimNo"].ToString()) ? "" : dt.Rows[i]["claimNo"].ToString();
                dataOutput.claimInfo.agentCode = string.IsNullOrEmpty(dt.Rows[i]["claimAgentCode"].ToString()) ? "" : dt.Rows[i]["claimAgentCode"].ToString();
                dataOutput.claimInfo.agentName = string.IsNullOrEmpty(dt.Rows[i]["claimAgentName"].ToString()) ? "" : dt.Rows[i]["claimAgentName"].ToString();
                dataOutput.claimInfo.agentBranch = string.IsNullOrEmpty(dt.Rows[i]["claimAgentBranch"].ToString()) ? "" : dt.Rows[i]["claimAgentBranch"].ToString();
                dataOutput.claimInfo.informerCleintId = string.IsNullOrEmpty(dt.Rows[i]["informerClientId"].ToString()) ? "" : dt.Rows[i]["informerClientId"].ToString();
                dataOutput.claimInfo.informerFullName = string.IsNullOrEmpty(dt.Rows[i]["informerFullName"].ToString()) ? "" : dt.Rows[i]["informerFullName"].ToString();
                dataOutput.claimInfo.informerMobile = string.IsNullOrEmpty(dt.Rows[i]["informerMobile"].ToString()) ? "" : dt.Rows[i]["informerMobile"].ToString();
                dataOutput.claimInfo.accidentDescCode = string.IsNullOrEmpty(dt.Rows[i]["accidentDescCode"].ToString()) ? "" : dt.Rows[i]["accidentDescCode"].ToString();
                dataOutput.claimInfo.accidnetDesc = string.IsNullOrEmpty(dt.Rows[i]["accidentDesc"].ToString()) ? "" : dt.Rows[i]["accidentDesc"].ToString();
                dataOutput.claimInfo.numOfExpectInjury = string.IsNullOrEmpty(dt.Rows[i]["numOfExpectInjury"].ToString()) ? "" : dt.Rows[i]["numOfExpectInjury"].ToString();
                dataOutput.claimInfo.accidentPlace = string.IsNullOrEmpty(dt.Rows[i]["accidentPlace"].ToString()) ? "" : dt.Rows[i]["accidentPlace"].ToString();
                dataOutput.claimInfo.accidentLatitude = string.IsNullOrEmpty(dt.Rows[i]["accidentLatitude"].ToString()) ? "" : dt.Rows[i]["accidentLatitude"].ToString();
                dataOutput.claimInfo.accidentLongitude = string.IsNullOrEmpty(dt.Rows[i]["accidentLongitude"].ToString()) ? "" : dt.Rows[i]["accidentLongitude"].ToString();
                dataOutput.claimInfo.accidentProvName = string.IsNullOrEmpty(dt.Rows[i]["accidentProv"].ToString()) ? "" : dt.Rows[i]["accidentProv"].ToString();
                dataOutput.claimInfo.accidentDistName = string.IsNullOrEmpty(dt.Rows[i]["accidentDist"].ToString()) ? "" : dt.Rows[i]["accidentDist"].ToString();
                dataOutput.claimInfo.sendOutSurveyorCode = string.IsNullOrEmpty(dt.Rows[i]["sendOutSurveyorCode"].ToString()) ? "" : dt.Rows[i]["sendOutSurveyorCode"].ToString();


                #region [condition data]
                //    string occupationchk = dt.Rows[i]["Occupation"].ToString();
                //    if (occupationchk != null && occupationchk != "")
                //    {
                //        dataOutput.occupationText = OccupationMasterData.Instance.Find(dt.Rows[i]["Occupation"].ToString())?.Name ?? "";
                //    }
                //    else
                //    {
                //        dataOutput.occupationText = "";
                //    }

                //    string nationalchk = dt.Rows[i]["Nationality"].ToString();
                //    if (nationalchk != null && nationalchk != "")
                //    {
                //        dataOutput.nationalityText = NationalityMasterData.Instance.Find(dt.Rows[i]["Nationality"].ToString())?.Name ?? "";
                //    }
                //    else
                //    {
                //        dataOutput.nationalityText = "";
                //    }
                string IssueDatechk = dt.Rows[i]["policyIssueDate"].ToString();
                if (IssueDatechk != null && IssueDatechk != "")
                {
                    dataOutput.policyInfo.policyIssueDate = (DateTime)dt.Rows[i]["policyIssueDate"];
                }
                string EffectiveDatechk = dt.Rows[i]["policyEffectiveDate"].ToString();
                if (EffectiveDatechk != null && EffectiveDatechk != "")
                {
                    dataOutput.policyInfo.policyEffectiveDate = (DateTime)dt.Rows[i]["policyEffectiveDate"];
                }
                string ExpiryDatechk = dt.Rows[i]["policyExpiryDate"].ToString();
                if (ExpiryDatechk != null && ExpiryDatechk != "")
                {
                    dataOutput.policyInfo.policyExpiryDate = (DateTime)dt.Rows[i]["policyExpiryDate"];
                }
                string informerOnchk = dt.Rows[i]["informerOn"].ToString();
                if (informerOnchk != null && informerOnchk != "")
                {
                    dataOutput.claimInfo.informerOn = (DateTime)dt.Rows[i]["informerOn"];
                }
                string accidentOnchk = dt.Rows[i]["accidentOn"].ToString();
                if (accidentOnchk != null && accidentOnchk != "")
                {
                    dataOutput.claimInfo.accidentOn = (DateTime)dt.Rows[i]["accidentOn"];
                }
                //    string vipchk = dt.Rows[i]["VIP"].ToString();
                //    if (vipchk == "VIP")
                //    {
                //        dataOutput.vipStatus = "Y";
                //    }
                //    else
                //    {
                //        dataOutput.vipStatus = "N";
                //    }
                #endregion
                output.data.Add(dataOutput);
            }

            ////loop
            output.code = AppConst.CODE_SUCCESS;
            output.message = AppConst.MESSAGE_SUCCESS;
            output.description = "";
            output.transactionId = TransactionId;
            output.transactionDateTime = DateTime.Now;
            return output;

        }
    }
}