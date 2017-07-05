using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.personSearchModel;
using DEVES.IntegrationAPI.WebApi.Logic.Converter;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzpersonSearch: BuzCommand
    {
        private System.Data.DataTable dt = new System.Data.DataTable();
        public override BaseDataModel ExecuteInput(object input)
        {
            personSearchInputModel personS = new personSearchInputModel();
            personS = (personSearchInputModel)input;
            string jsonValue = string.Format("{0}|{1}|{2}|{3}", string.IsNullOrEmpty(personS.name1) ? "" : personS.name1
                   , string.IsNullOrEmpty(personS.idCard) ? "" : personS.idCard
                   , string.IsNullOrEmpty(personS.phoneNumber) ? "" : personS.phoneNumber
                   , string.IsNullOrEmpty(personS.cleansingId) ? "" : personS.cleansingId);


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
                dataOutput.mobilePhone = dt.Rows[i]["PhoneNumber"].ToString();
                dataOutput.emailAddress = dt.Rows[i]["Email"].ToString();
                dataOutput.salutationText = dt.Rows[i]["Salutation"].ToString();
                dataOutput.sex = OptionSetConvertor.GetSex(dt.Rows[i]["Sex"].ToString());
                dataOutput.idDriving = dt.Rows[i]["DriverId"].ToString();
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