using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Sdk.Deployment;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class InMemoryDataStorageBase<TEntityClass, TEntityFieldEnum>
        where TEntityClass : new()
        where TEntityFieldEnum : struct, IComparable, IConvertible, IFormattable

    {
        protected Dictionary<string, dynamic> DataList { get; set; } 
        protected Dictionary<string, dynamic> DataList2 { get; set; }
        protected Dictionary<string, dynamic> DataList3 { get; set; }

        protected IDataReader DataReader { get; set; }
        protected string StoreName { get; set; }
        protected string FieldCodeName { get; set; }


        protected void Load(string storeName, string fieldCodeName)
        {
            Console.WriteLine("Load "+ storeName);
             StoreName = storeName;
             FieldCodeName = fieldCodeName;
            if (null != DataList2) return;
            if (System.Environment.MachineName == AppConst.QA_SERVER_NAME)
            {
                DataReader = new StoreDataReader();
                Console.WriteLine("StoreDataReader");
            }
            else
            {
                Console.WriteLine("RestDataReader");
                DataReader = new RestDataReader();
            }
           
           
            DataList = new Dictionary<string, dynamic>();
            DataList2 = new Dictionary<string, dynamic>();
           



            var reader = DataReader;
            var req = new DbRequest {StoreName = storeName};
            var result = reader.Execute(req);
            //Console.WriteLine(result.ToJson());
            if (result.Count > 0)
            {
                foreach (Dictionary<string, dynamic> item in result.Data)
                {
                    // Console.WriteLine(item.ToJSON());
                   
                    var code = (string) item[fieldCodeName];
                    if (string.IsNullOrEmpty(code)) continue;
                   
                    if (!DataList.ContainsKey(code))
                    {
                        DataList.Add(code.ToString(), item);
                        if (item["Id"] is string)
                        {
                            item["Id"] = new Guid(item["Id"]);
                        }
                        DataList2.Add(((Guid)item["Id"]).ToString(), item);
                        
                    }
                   
                }
            }
            else
            {
                Console.WriteLine("Load " + storeName+ " Data Not Found!!");
                // throw new Exception("Data Not Found!!");
            }
        }

        public void Init()
        {
            
        }

        public TEntityClass Tranform(dynamic provinceRow)
        {
            var obj = (TEntityClass) Activator.CreateInstance(typeof(TEntityClass));
            foreach (PropertyInfo pi in typeof(TEntityClass).GetProperties())
            {
                var piName = pi.Name;
                if (!pi.CanWrite) continue;
                try
                {
                    pi.SetValue(obj, provinceRow[piName], null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "===>" + piName);
                }
            }

            return obj;
        }

        public TEntityClass Find(string guid)
        {
          
            if (!DataList2.ContainsKey(guid))
            {
                return default(TEntityClass);
            }
            var provinceRow = ((Dictionary<string, dynamic>) DataList2[guid]);
          
            return Tranform(provinceRow);
        }

        public TEntityClass FindByCode(string code)
        {

            if (!DataList.ContainsKey(code))
            {
              //  Console.WriteLine(" not Contains Key " + code);
                return default(TEntityClass);
            }

            var provinceRow = ((Dictionary<string, dynamic>)DataList[code]);
            return Tranform(provinceRow);
        }

        public TEntityClass FindByCode(string code, string defaulCode)
        {
            if(string.IsNullOrEmpty(code))
            {
                code = defaulCode;
            }
            return FindByCode(code);
        }

        public TEntityClass FindByPolisyCode(string code)
        {
            //@TO adHoc Load All Data
            if (DataList3 != null) return DataList3[code];
            DataList3 = new Dictionary<string, dynamic>();
            foreach (var item in DataList)
            {
                   
                if (!DataList3.ContainsKey(item.Value["PolisyCode"]))
                {
                    DataList3.Add(item.Value["PolisyCode"], item);
                }
            }
            return DataList3[code];
        }

        public TEntityClass FindByPolisyCode(string code, string defaulCode)
        {
            if (code == null || code.Equals(""))
            {
                code = defaulCode;
            }
            return FindByPolisyCode(code);
        }

        public TEntityClass FindByField(TEntityFieldEnum fieldEnum, string fieldValue)
        {
            if (typeof(TEntityFieldEnum).IsEnum)
            {
               
                return FindByField(fieldEnum.ToString() ,fieldValue);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

        

        }
        public TEntityClass FindByField(string fieldName, string fieldValue)
        {

            var reader = DataReader;
            var req = new DbRequest { StoreName = StoreName };
            req.AddParam(fieldName.ToString(), fieldValue);
            var result = reader.Execute(req);
            if (result.Count == 1)
            {
                var item = ((Dictionary<string, dynamic>)result.Data[0]);

                return Tranform(item);

            }
            else
            {
                return default(TEntityClass);
            }



        }

        // Load a CSV file into an array of rows and columns.
        // Assume there may be blank lines but every line has
        // the same number of fields.
        private string[,] LoadCsv(string filename)
        {
            // Get the file's text.
            string wholeFile = System.IO.File.ReadAllText(filename);

            // Split into lines.
            wholeFile = wholeFile.Replace('\n', '\r');
            string[] lines = wholeFile.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int numRows = lines.Length;
            int numCols = lines[0].Split(',').Length;

            // Allocate the data array.
            string[,] values = new string[numRows, numCols];

            // Load the array.
            for (var r = 0; r < numRows; r++)
            {
                string[] lineR = lines[r].Split(',');
                for (int c = 0; c < numCols; c++)
                {
                    values[r, c] = lineR[c];
                }
            }

            // Return the values.
            return values;
        }
        public  void LoadMockData(string fileName, string fieldCodeName)
        {
            
            var filepath = @"C:\Users\patiw\Source\RVP2\DEVES.IntegrationAPI.WebApi\App_Data\MockData\" + fileName+".csv";
            Console.WriteLine(filepath);
            var lines = LoadCsv(filepath);
            int numRows = lines.GetUpperBound(0) + 1;
            int numCols = lines.GetUpperBound(1) + 1;
            var props = typeof(TEntityClass).GetProperties();
            var propName = new List<string>();
            foreach (PropertyInfo pi in props)
            {
                propName.Add(pi.Name);
            }
        
            for (int i = 0; i < numRows; i++)
            {
                var id = lines[i,0];
                if (!DataList2.ContainsKey(id))
                {
                  
                   
                    var code = "";
                    var item = new Dictionary<string, dynamic>();
                    for (var j = 0 ; j < numCols; j++)
                    {
                        var piName = propName[j];
                        
                     
                        try
                        {      if(piName == "Id") { 
                                    
                                      item.Add(piName, new Guid(lines[i, j]));
                                 }
                            else{
                                   
                                item.Add(piName,(string) lines[i, j]);
                            }
                            if (piName == fieldCodeName)
                            {
                                code = (string)lines[i, j];
                            }
                            Console.WriteLine(i+ ": "+piName+" = "+ lines[i, j]);

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "===>" + piName);
                        }
                        ++j;
                    }
                    if (!DataList.ContainsKey(code))
                    {
                        DataList.Add(code, item);
                       
                    }
                    DataList2.Add(id, item);


                }
            }
            
        }

        public static void InitAll()
        {
            

        }
        
    }
}