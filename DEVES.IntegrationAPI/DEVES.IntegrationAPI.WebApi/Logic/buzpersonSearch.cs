using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.personSearchModel;
using DEVES.IntegrationAPI.WebApi.Logic.Converter;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzpersonSearch: BuzCommand
    {
        private System.Data.DataTable dt = new System.Data.DataTable();
        public override BaseDataModel ExecuteInput(object input)
        {
            personSearchInputModel personS = new personSearchInputModel();
            personS = (personSearchInputModel)input;
            string jsonValue = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", string.IsNullOrEmpty(personS.name1) ? "" : personS.name1
                   , string.IsNullOrEmpty(personS.idCard) ? "" : personS.idCard
                   , string.IsNullOrEmpty(personS.phoneNumber) ? "" : personS.phoneNumber
                   , string.IsNullOrEmpty(personS.cleansingId) ? "" : personS.cleansingId
                   , string.IsNullOrEmpty(personS.crmClientId) ? "" : personS.crmClientId
                   , string.IsNullOrEmpty(personS.email) ? "" : personS.email
                   );
         

            QueryInfo newQuery = new QueryInfo();
            dt = newQuery.Queryinfo_searchPerson(jsonValue);

            personSearchOutputModel output = new personSearchOutputModel();
            output.data = new List<personSearchDataOutput>();

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
                dataOutput.idDriving =  string.IsNullOrEmpty(dt.Rows[i]["DriverId"].ToString()) ? "" : dt.Rows[i]["DriverId"].ToString();
                dataOutput.idAlien = string.IsNullOrEmpty(dt.Rows[i]["AlienId"].ToString()) ? "" : dt.Rows[i]["AlienId"].ToString();
                dataOutput.language = string.IsNullOrEmpty(dt.Rows[i]["Language"].ToString()) ? "" : dt.Rows[i]["Language"].ToString();
                string occupationchk = dt.Rows[i]["Occupation"].ToString();
                if (occupationchk != null && occupationchk != "")
                {
                    dataOutput.occupationText = OccupationMasterData.Instance.Find(dt.Rows[i]["Occupation"].ToString())?.Name ?? "";
                }
                string nationalchk = dt.Rows[i]["Nationality"].ToString();
                if(nationalchk != null && nationalchk != "")
                {
                    dataOutput.nationalityText = NationalityMasterData.Instance.Find(dt.Rows[i]["Nationality"].ToString())?.Name ?? "";
                }
                string birthdatechk = dt.Rows[i]["Birth"].ToString();
                string deathdatechk = dt.Rows[i]["Death"].ToString();
                if (birthdatechk != null && birthdatechk != "")
                {
                    dataOutput.dateOfBirth = (DateTime)dt.Rows[i]["Birth"];
                }
                if (deathdatechk != null && deathdatechk != "")
                {
                    dataOutput.dateOfDeath = (DateTime)dt.Rows[i]["Death"];
                }
                output.data.Add(dataOutput);
            }
            
            //loop

            return output;// newQuery.Queryinfo_searchPerson(jsonValue);
        }
    }
}