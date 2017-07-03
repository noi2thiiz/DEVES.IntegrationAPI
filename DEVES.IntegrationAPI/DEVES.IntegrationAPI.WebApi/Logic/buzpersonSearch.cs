using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Model;
using DEVES.IntegrationAPI.Model.personSearchModel;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public class buzpersonSearch: BuzCommand
    {
        private System.Data.DataTable dt = new System.Data.DataTable();
        public override BaseDataModel ExecuteInput(object input)
        {
            personSearchInputModel personS = new personSearchInputModel();
            personS = (personSearchInputModel)input;
            string jsonValue = string.Format("{0}|{1}|{2}|{3}", string.IsNullOrEmpty(personS.fullName) ? "" : personS.name1
                   , string.IsNullOrEmpty(personS.citizenId) ? "" : personS.idCard
                   , string.IsNullOrEmpty(personS.phoneNum) ? "" : personS.phoneNumber
                   , string.IsNullOrEmpty(personS.clsId) ? "" : personS.cleansingId);


            QueryInfo newQuery = new QueryInfo();
            dt = newQuery.Queryinfo_searchPerson(jsonValue);

            personSearchOutputModel output = new personSearchOutputModel();
            output.data = new List<personSearchDataOutput>();
            personSearchDataOutput data = new personSearchDataOutput();
            int datarow = dt.Rows.Count;
            //loop
            for (int i = 0; i < datarow; i++)
            {
                data.fullName = dt.Rows[i]["FullName"].ToString();
                data.citizenId = dt.Rows[i]["FullName"].ToString();
                data.clsId = dt.Rows[i]["FullName"].ToString();
                data.phoneNum = dt.Rows[i]["FullName"].ToString();
                output.data.Add(data);
            }
            
            //loop

            return output;// newQuery.Queryinfo_searchPerson(jsonValue);
        }
    }
}