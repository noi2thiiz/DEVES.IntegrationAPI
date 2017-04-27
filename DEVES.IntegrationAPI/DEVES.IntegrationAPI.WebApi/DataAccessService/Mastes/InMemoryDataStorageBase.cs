﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.DataAdapter;
using Microsoft.Xrm.Sdk.Deployment;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class InMemoryDataStorageBase<TEntityClass>
        where TEntityClass : new()

    {
        protected Dictionary<string, dynamic> DataList { get; set; } 
        protected Dictionary<string, dynamic> DataList2 { get; set; }


        protected void Load(string storeName, string fieldCodeName)
        {
            if (null != DataList2) return;
            DataList = new Dictionary<string, dynamic>();
            DataList2 = new Dictionary<string, dynamic>();

            //if (System.Environment.MachineName == "DESKTOP-Q30CAGJ")
            //{
                //LoadMockData(storeName, fieldCodeName);
                //return;;
            //}

            Console.WriteLine($"{storeName}/{fieldCodeName}");
         
          //  DataList = new Dictionary<string, dynamic>();


            //var conectionString = CrmConfigurationService.AppConfig.Get("CRMDB");
          
            
            var reader = new StoreDataReader();
            var req = new DbRequest {StoreName = storeName};
            var result = reader.Execute(req);
            if (result.Count > 0)
            {
                foreach (Dictionary<string, dynamic> item in result.Data)
                {
                    // Console.WriteLine(item.ToJSON());
                    Console.WriteLine(fieldCodeName +"="+ item[fieldCodeName]);
                    var code = (string) item[fieldCodeName];
                    if (string.IsNullOrEmpty(code)) continue;
                    Console.WriteLine(code);
                    if (!DataList.ContainsKey(code))
                    {
                        DataList.Add(code.ToString(), item);
                        DataList2.Add(((Guid)item["Id"]).ToString(), item);
                    }
                   
                }
            }
            else
            {
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
            Console.WriteLine(guid);
            if (!DataList2.ContainsKey(guid))
            {
                return new TEntityClass();
            }
            var provinceRow = ((Dictionary<string, dynamic>) DataList2[guid]);
          
            return Tranform(provinceRow);
        }

        public TEntityClass FindByCode(string code)
        {
           
            if (!DataList.ContainsKey(code))
            {
                Console.WriteLine(" not Contains Key "+ code);
                return new TEntityClass();
            }

            var provinceRow = ((Dictionary<string, dynamic>) DataList[code]);
            return Tranform(provinceRow);
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
            int num_rows = lines.GetUpperBound(0) + 1;
            int num_cols = lines.GetUpperBound(1) + 1;
            var props = typeof(TEntityClass).GetProperties();
            var propName = new List<string>();
            foreach (PropertyInfo pi in props)
            {
                propName.Add(pi.Name);
            }
        
            for (int i = 0; i < num_rows; i++)
            {
                var id = lines[i,0];
                if (!DataList2.ContainsKey(id))
                {
                  
                   
                    var code = "";
                    var item = new Dictionary<string, dynamic>();
                    for (var j = 0 ; j < num_cols; j++)
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