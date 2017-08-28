using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi.TechnicalService;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter
{
    public class StoreDataReader : DataReaderBase, IDataReader
    {
        public string ConnectionString { get; set; }
        public StoreDataReader(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public StoreDataReader()
        {
            this.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB_SERVER"];
        }

        public DbResult Execute(DbRequest req)
        {
            TraceDebugLogger.Instance.AddLog("Execute StoreDataReader input:", req);
            int fieldcount = 0;
            int rowscount = 0;

            try
            {

                Dictionary<string, object> paras = new Dictionary<string, object>();

                foreach (var pr in req.StoreParams)
                {
                    paras.Add(pr.Key, pr.Value);
                }



                List<object> rows = new List<object>();
                List<object> fieldInfo = new List<object>();


                using (SqlDataReader reader = ExecuteProcedure(req.StoreName, paras))
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

                        fieldcount = reader.FieldCount;
                        Dictionary<string, object> item = new Dictionary<string, object>();


                        for (int index = 0; index < fieldcount; index++)
                        { // iterate through all columns

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
                TraceDebugLogger.Instance.AddLog("ExecuteProcedure StoreDataReader Success: (Success = true)",  dbResult);
                return dbResult;
            }
            catch (System.Exception e)
            {
                TraceDebugLogger.Instance.AddLog("Execute StoreDataReader Exception: (Success = false)" + e.Message, e.StackTrace);
                var dbResult = new DbResult();
                dbResult.Success = false;
                dbResult.Code = "0";
                dbResult.Message = e.ToString();
                return dbResult;
            }


        }

        public SqlDataReader ExecuteProcedure(string commandName, Dictionary<string, object> paras)
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = commandName;
                if (paras != null)
                {
                    foreach (KeyValuePair<string, object> kvp in paras)
                        comm.Parameters.Add(new SqlParameter(kvp.Key, kvp.Value));
                }
                return comm.ExecuteReader(); //System.Data.CommandBehavior.CloseConnection
            }
            catch (Exception e)
            {
                TraceDebugLogger.Instance.AddLog("ExecuteProcedure StoreDataReader Exception:" + e.Message, e.StackTrace);
                throw;
            }
           
        }

        public DbResult Execute<T>(DbRequest req)
        {

            DbResult result = Execute(req);
            if (result.Success)
            {
                if (result.Count > 0)
                {
                    var sources = result.Data.ToList();
                    result.Data.Clear();
                    foreach (var item in sources)
                    {

                        result.Data.Add(Tranform<T>(item));
                    }



                }

            }
            return result;
        }

    }
}