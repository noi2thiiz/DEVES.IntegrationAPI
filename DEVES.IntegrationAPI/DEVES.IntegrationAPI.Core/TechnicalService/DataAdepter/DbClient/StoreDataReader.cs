using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DEVES.IntegrationAPI.WebApi.Core.DataAdepter
{
    public class StoreDataReader
    {
        public string ConnectionString { get; set; }
        public StoreDataReader(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public DbResult Execute(DbRequest req){
            int fieldcount = 0;
            int rowscount = 0;

            try
            {

                Dictionary<string, object> paras = new Dictionary<string, object>();

                foreach (var pr in req.StoreParams)
                {
                    paras.Add(pr.key, pr.value);
                }



                List<object> rows = new List<object>();
                List<object> fieldInfo = new List<object>();
                
                
                using(SqlDataReader reader = ExecuteProcedure(req.StoreName, paras))
                {

                    for (int index = 0; index < fieldcount; index++)
                    { // iterate through all columns

                        var fieldName = reader.GetName(index);
                        var fieldType = reader.GetFieldType(index);
                        fieldInfo.Add(new { Name = fieldName });
                          
                    }

                    while (reader.Read())
                    {
                        rowscount++;
                        //http://stackoverflow.com/questions/5554472/create-json-string-from-sqldatareader
                         fieldcount = reader.FieldCount;
                        Dictionary<string, object> item = new Dictionary<string, object>();

                   
                        for (int index = 0; index < fieldcount; index++) { // iterate through all columns

                            var fieldName = reader.GetName(index);
                            var fieldValue = reader[index];
                            var fieldType = reader.GetFieldType(index);

                            item[fieldName] = fieldValue;

                        }
                        rows.Add(item);
                        
                    }
                    //result.Add(rows);
                }


                var dbResult = new DbResult
                {
                    Data = rows,
                    Success = true,
                    Code = "1",
                    Message = "success",
                    Count = rowscount,
                    Total = rowscount,
                    Fieldcount = fieldcount,
                    FieldInfo = fieldInfo,
                    ReqParams = paras
                };
                return dbResult;
            }
            catch (System.Exception e)
            {
                var dbResult = new DbResult();
                dbResult.Success = false;
                dbResult.Code = "0";
                dbResult.Message = e.ToString();
                return dbResult;
            }


        }

        public  SqlDataReader ExecuteProcedure(string commandName,Dictionary<string, object> paras)
        {
            var _connectionString = ConnectionString;
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