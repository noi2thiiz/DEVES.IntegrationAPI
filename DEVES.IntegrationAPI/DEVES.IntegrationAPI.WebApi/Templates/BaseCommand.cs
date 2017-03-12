using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.Model.EWI;
using DEVES.IntegrationAPI.Model;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Globalization;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DEVES.IntegrationAPI.WebApi.Templates
{
    public abstract class BaseCommand
    {
        //This is like the Main() function. And need to be implemented.
        public abstract void Execute();

        //private T getData<Nullable<T>>(T x)
        //{
        //    Nullable<T> o= null;
        //    try {
        //        o = (T)x;
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    return o;
        //}

        public T GetField<T>(SqlDataReader dr, string fieldName)
        {
            var value = dr.GetValue(dr.GetOrdinal(fieldName));
            return value is DBNull ? default(T) : (T)value;
        }

        //private DateTime? isDateNull(string a)
        //{
        //    DateTime? d = null;
        //    try
        //    {
        //        d = (DateTime)dt.Rows[0][a];
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    return d;
        //}
        //private string isStringNull(string a)
        //{
        //    if (dt.Rows[0][a] == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return dt.Rows[0][a].ToString();
        //    }
        //}
        //private int isIntNull(string a)
        //{
        //    if (dt.Rows[0][a] == null)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        return Convert.ToInt32(dt.Rows[0][a]);
        //    }
        //}

        private bool ValidateJSON(string strJSON)
        {
            string typeName = this.GetType().Name;
            throw new NotImplementedException();
        }

        internal EWIResponseContent CallEWIService(string EWIendpoint, BaseDataModel JSON)
        {
            EWIRequest reqModel = new EWIRequest()
            {
                username = "ClaimMotor",
                password = "1234",
                gid = "ClaimMotor",
                token = "",
                content = JSON
            };

            string jsonReqModel = JsonConvert.SerializeObject(reqModel, Formatting.Indented, new EWIDatetimeConverter());

            HttpClient client = new HttpClient();

            // URL
            //client.BaseAddress = new Uri("http://192.168.3.194/ServiceProxy/ClaimMotor/jsonproxy/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Accept-Encoding", "utf-8");

            // + ENDPOINT
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, EWIendpoint);
            request.Content = new StringContent(jsonReqModel, System.Text.Encoding.UTF8, "application/json");

            // เช็ค check reponse 
            HttpResponseMessage response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            EWIResponse ewiRes = response.Content.ReadAsAsync<EWIResponse>().Result;
            EWIResponseContent locusOutput = ewiRes.content;
            return locusOutput;
        }

        internal void FillModelUsingSQL(ref Object target, string storedProcName, List<CommandParameter> listParams)
        {
            /*
             * For better performance, revise the code to avoid using DataTable
            */
            System.Data.DataTable dt = CallSQLStoredProc(storedProcName, listParams);
            FillObjectFromDataTable(ref target, dt);
        }

        internal System.Data.DataTable CallSQLStoredProc(string storedProcName, List<CommandParameter> listParams)
        {
            //string strSql = string.Format(SQL_01, ticketNo);
            using (SqlConnection cnnSQL = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString()))
            {

                SqlCommand cmdSQL = new SqlCommand(storedProcName, cnnSQL);
                cmdSQL.CommandTimeout = 600;
                cmdSQL.CommandType = CommandType.StoredProcedure ;
                foreach (CommandParameter p in listParams)
                {
                    cmdSQL.Parameters.Add ( new SqlParameter(p.Name, p.Value));
                }

                cnnSQL.Open();
                SqlDataReader dr = cmdSQL.ExecuteReader();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Load(dr);
                return dt;
            }
        }

        internal void FillObjectFromDataTable(ref Object target, DataTable dt)
        {
            Type objType = target.GetType();
            if (objType.IsDefined(typeof(CrmClassToMapDataAttribute), false))
            {
                Object targetChild = target;
                FillObjectFromDataTable(ref targetChild, dt);
            }
            foreach (PropertyInfo prop in objType.GetProperties())
            {
                
            }
        }


    }
}