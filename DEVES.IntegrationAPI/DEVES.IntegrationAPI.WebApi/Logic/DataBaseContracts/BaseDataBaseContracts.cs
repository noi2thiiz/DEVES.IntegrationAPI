using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts
{
    public class BaseDataBaseContracts<TEntityClass>
        where TEntityClass : new()
    {
        protected IDataReader DataReader { get; set; }
        protected string StoreName { get; set; }

        protected string readerName { get; set; } = "auto";

        public BaseDataBaseContracts(string storeName)
        {
            StoreName = storeName;

            if (readerName == "Rest")
            {
                DataReader = new RestDataReader();

            }else
            if (AppConst.IS_SERVER)
            {
                var conectionString = AppConfig.Instance.GetCRMDBConfigurationString();
                DataReader = new StoreDataReader(conectionString);
            }
            else
            {
                DataReader = new RestDataReader();
               
            }

            
           

        }

        public DbResult Excecute(Dictionary<string, string> reqParams)
        {
            try
            {
               
                var req = new DbRequest()
                {
                    StoreName = StoreName


                };
                foreach (var param in reqParams)
                {
                    req.AddParam(param.Key, param.Value);

                }

                DbResult result = DataReader.Execute(req);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TEntityClass Tranform(dynamic item)
        {
            var obj = (TEntityClass)Activator.CreateInstance(typeof(TEntityClass));
            foreach (PropertyInfo pi in typeof(TEntityClass).GetProperties())
            {
                var piName = pi.Name;
                if (!pi.CanWrite) continue;

               
                try
                {
                    //AdHoc  Convert เฉพาะที่ใช้เท่านั้น
                    Console.WriteLine(pi.PropertyType.Name);
                    if (pi.PropertyType.Name == "System.String" && item[piName].GetType() == "System.Guid")
                    {
                        pi.SetValue(obj, ((Guid)item[piName]).ToString("D"), null);
                    }
                    else if (pi.PropertyType.Name == "System.Guid" && item[piName].GetType() == "System.String")
                    {
                        pi.SetValue(obj, new Guid(item[piName]?.ToString()), null);
                    }
                    else if (pi.PropertyType.Name == "System.String")
                    {
                        pi.SetValue(obj, new Guid(item[piName]?.ToString()), null);
                    }
                    else
                    {
                        pi.SetValue(obj, item[piName], null);
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Fail Tranform Entity in BaseDataBaseContracts:"+e.Message + "===>" + piName);
                }
            }

            return obj;
        }

    }

}