using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.personSearchModel;
using DEVES.IntegrationAPI.WebApi.Logic.Converter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzpersonSearch: BuzCommand
    {
        private System.Data.DataTable dt = new System.Data.DataTable();
        public override BaseDataModel ExecuteInput(object input)
        {
            personSearchInputModel personS = new personSearchInputModel();
            personS = (personSearchInputModel)input;
            string jsonValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", string.IsNullOrEmpty(personS.conditions.fullName) ? "" : personS.conditions.fullName
                   , string.IsNullOrEmpty(personS.conditions.idCard) ? "" : personS.conditions.idCard
                   , string.IsNullOrEmpty(personS.conditions.phoneNumber) ? "" : personS.conditions.phoneNumber
                   , string.IsNullOrEmpty(personS.conditions.cleansingId) ? "" : personS.conditions.cleansingId
                   , string.IsNullOrEmpty(personS.conditions.crmClientId) ? "" : personS.conditions.crmClientId
                   , string.IsNullOrEmpty(personS.conditions.email) ? "" : personS.conditions.email
                   , string.IsNullOrEmpty(personS.conditions.customerType) ? "" : personS.conditions.customerType
                   );
             
            personSearchOutputModel output = new personSearchOutputModel();
            if (personS.conditions.customerType == "P")
            {
                output.Persondata = new List<personSearchDataOutput>();
                QueryInfo newQuery = new QueryInfo();
                dt = newQuery.Queryinfo_searchPerson(jsonValue);
                int datarow = dt.Rows.Count;
                //loop
                for (int i = 0; i < datarow; i++)
                {
                    personSearchDataOutput dataOutput = new personSearchDataOutput();
                    dataOutput.fullName = string.IsNullOrEmpty(dt.Rows[i]["FullName"].ToString()) ? "" : dt.Rows[i]["FullName"].ToString();
                    dataOutput.idCard = string.IsNullOrEmpty(dt.Rows[i]["CitizenID"].ToString()) ? "" : dt.Rows[i]["CitizenID"].ToString();
                    dataOutput.cleansingId = string.IsNullOrEmpty(dt.Rows[i]["cleansingID"].ToString()) ? "" : dt.Rows[i]["cleansingID"].ToString();
                    dataOutput.crmClientId = string.IsNullOrEmpty(dt.Rows[i]["CRMClientID"].ToString()) ? "" : dt.Rows[i]["CRMClientID"].ToString();
                    dataOutput.polisyClientId = string.IsNullOrEmpty(dt.Rows[i]["PolisyClientID"].ToString()) ? "" : dt.Rows[i]["PolisyClientID"].ToString();
                    dataOutput.mobilePhone = string.IsNullOrEmpty(dt.Rows[i]["PhoneNumber"].ToString()) ? "" : dt.Rows[i]["PhoneNumber"].ToString();
                    dataOutput.emailAddress = string.IsNullOrEmpty(dt.Rows[i]["Email"].ToString()) ? "" : dt.Rows[i]["Email"].ToString();
                    dataOutput.salutationText = string.IsNullOrEmpty(dt.Rows[i]["Salutation"].ToString()) ? "" : dt.Rows[i]["Salutation"].ToString();
                    dataOutput.sex = OptionSetConvertor.GetSex(string.IsNullOrEmpty(dt.Rows[i]["Sex"].ToString()) ? "" : dt.Rows[i]["Sex"].ToString());
                    dataOutput.idDriving = string.IsNullOrEmpty(dt.Rows[i]["DriverId"].ToString()) ? "" : dt.Rows[i]["DriverId"].ToString();
                    dataOutput.idAlien = string.IsNullOrEmpty(dt.Rows[i]["AlienId"].ToString()) ? "" : dt.Rows[i]["AlienId"].ToString();
                    dataOutput.language = string.IsNullOrEmpty(dt.Rows[i]["Language"].ToString()) ? "" : dt.Rows[i]["Language"].ToString();
                    dataOutput.idPassport = string.IsNullOrEmpty(dt.Rows[i]["PassportId"].ToString()) ? "" : dt.Rows[i]["PassportId"].ToString();
                    dataOutput.fax = string.IsNullOrEmpty(dt.Rows[i]["Fax"].ToString()) ? "" : dt.Rows[i]["Fax"].ToString();
                    dataOutput.clientStatus = string.IsNullOrEmpty(dt.Rows[i]["clientStatus"].ToString()) ? "" : dt.Rows[i]["clientStatus"].ToString();
                    #region [condition data]
                    string occupationchk = dt.Rows[i]["Occupation"].ToString();
                    if (occupationchk != null && occupationchk != "")
                    {
                        dataOutput.occupationText = OccupationMasterData.Instance.Find(dt.Rows[i]["Occupation"].ToString())?.Name ?? "";
                    }
                    else
                    {
                        dataOutput.occupationText = "";
                    }

                    string nationalchk = dt.Rows[i]["Nationality"].ToString();
                    if (nationalchk != null && nationalchk != "")
                    {
                        dataOutput.nationalityText = NationalityMasterData.Instance.Find(dt.Rows[i]["Nationality"].ToString())?.Name ?? "";
                    }
                    else
                    {
                        dataOutput.nationalityText = "";
                    }

                    string birthdatechk = dt.Rows[i]["Birth"].ToString();
                    if (birthdatechk != null && birthdatechk != "")
                    {
                        dataOutput.dateOfBirth = (DateTime)dt.Rows[i]["Birth"];
                    }

                    string deathdatechk = dt.Rows[i]["Death"].ToString();
                    if (deathdatechk != null && deathdatechk != "")
                    {
                        dataOutput.dateOfDeath = (DateTime)dt.Rows[i]["Death"];
                    }

                    string vipchk = dt.Rows[i]["VIP"].ToString();
                    if (vipchk == "VIP")
                    {
                        dataOutput.vipStatus = "Y";
                    }
                    else
                    {
                        dataOutput.vipStatus = "N";
                    }
                    #endregion
                    output.Persondata.Add(dataOutput);
                }

                //loop
                output.code = AppConst.CODE_SUCCESS;
                output.message = AppConst.MESSAGE_SUCCESS;
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
            }
            else if (personS.conditions.customerType == "C")
            {
                output.Corpdata = new List<corpSearchDataOutput>();
                QueryInfo newQuery = new QueryInfo();
                dt = newQuery.Queryinfo_searchPerson(jsonValue);
                int datarow = dt.Rows.Count;
                //loop
                for (int i = 0; i < datarow; i++)
                {
                    corpSearchDataOutput dataOutput = new corpSearchDataOutput();
                    dataOutput.fullName = string.IsNullOrEmpty(dt.Rows[i]["fullName"].ToString()) ? "" : dt.Rows[i]["fullName"].ToString();
                    dataOutput.idRegCorp = string.IsNullOrEmpty(dt.Rows[i]["idRegCorp"].ToString()) ? "" : dt.Rows[i]["idRegCorp"].ToString();
                    dataOutput.idTax = string.IsNullOrEmpty(dt.Rows[i]["idTax"].ToString()) ? "" : dt.Rows[i]["idTax"].ToString();
                    dataOutput.corporateBranch = string.IsNullOrEmpty(dt.Rows[i]["corporateBranch"].ToString()) ? "" : dt.Rows[i]["corporateBranch"].ToString();
                    dataOutput.riskLevelText = string.IsNullOrEmpty(dt.Rows[i]["riskLevelText"].ToString()) ? "" : dt.Rows[i]["riskLevelText"].ToString();
                    dataOutput.language = string.IsNullOrEmpty(dt.Rows[i]["language"].ToString()) ? "" : dt.Rows[i]["language"].ToString();
                    dataOutput.clientStatus = string.IsNullOrEmpty(dt.Rows[i]["clientStatus"].ToString()) ? "" : dt.Rows[i]["clientStatus"].ToString();
                    #region [condition data]
                    string birthdatechk = dt.Rows[i]["corporateDate"].ToString();
                    if (birthdatechk != null && birthdatechk != "")
                    {
                        dataOutput.corporateDate = (DateTime)dt.Rows[i]["corperateDate"];
                    }
                    string vipchk = dt.Rows[i]["VIP"].ToString();
                    if (vipchk == "VIP")
                    {
                        dataOutput.vipStatus = "Y";
                    }
                    else
                    {
                        dataOutput.vipStatus = "N";
                    }
                    #endregion
                    output.Corpdata.Add(dataOutput);
                }

                //loop
                output.code = AppConst.CODE_SUCCESS;
                output.message = AppConst.MESSAGE_SUCCESS;
                output.description = "";
                output.transactionId = TransactionId;
                output.transactionDateTime = DateTime.Now;
            }
            return output;// newQuery.Queryinfo_searchPerson(jsonValue);

        }
    }
}