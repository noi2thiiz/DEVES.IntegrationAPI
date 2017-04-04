using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter
{
    public class StoreDataReader
    {

        public DbResult Execute(DbRequest req){


            try
            {

                Dictionary<string, object> paras = new Dictionary<string, object>();

                foreach (var pr in req.storeParams)
                {
                    paras.Add(pr.key, pr.value);
                }



                List<object> result = new List<object>();
                int i = 0;
                using(SqlDataReader reader = executeProcedure(req.storeName, paras))
                {

                    List<object> rows = new List<object>();
                    while (reader.Read())
                    {
                        //http://stackoverflow.com/questions/5554472/create-json-string-from-sqldatareader
                        int fieldcount = reader.FieldCount;
                        object[] values = new object[fieldcount];
                        reader.GetValues(values);
                        Dictionary<string, object> item = new Dictionary<string, object>();
                        for (int index = 0; index < fieldcount; index++) { // iterate through all columns

                            var fieldName = reader.GetName(index);
                            var fieldValue = reader[index];
                            item[fieldName] = fieldValue;

                        }
                        rows.Add(item);
                        ++i;
                    }
                    result.Add(rows);
                }


                var dbResult = new DbResult();
                dbResult.data = result;
                dbResult.code = "200";
                dbResult.message = "success";
                dbResult.total = i;
               

                return dbResult;
            }
            catch (System.Exception e)
            {
                var dbResult = new DbResult();

                dbResult.code = "500";
                dbResult.message = e.ToString();
                return dbResult;
            }


        }

        public static SqlDataReader executeProcedure(string commandName,Dictionary<string, object> paras)
        {
            var _connectionString = XrmConfigurationSettings.AppSettings.Get("CRMDB");
            SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            SqlCommand comm = conn.CreateCommand();
            comm.CommandType = CommandType.StoredProcedure;
            comm.CommandText = commandName;
            if (paras != null)
            {
                foreach(KeyValuePair<string, object> kvp in paras)
                    comm.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value));
            }
            return comm.ExecuteReader(); //System.Data.CommandBehavior.CloseConnection
        }

    }
}