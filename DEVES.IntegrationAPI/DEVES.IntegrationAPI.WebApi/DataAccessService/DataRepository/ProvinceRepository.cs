using DEVES.IntegrationAPI.WebApi.TechnicalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi.DataAccessService;
using DEVES.IntegrationAPI.Core.ExtensionMethods;
using DEVES.IntegrationAPI.WebApi.Core.ExtensionMethods;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataRepository
{
    public class ProvinceRepository
    {

        private static ProvinceRepository instance;
        private ProvinceRepository() { }
        public static ProvinceRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProvinceRepository();
                    instance.Load();
                }
                return instance;
            }
        }

        protected Dictionary<string, dynamic> ProvinceList { get; set; }
        protected void Load()
        {
            if (null == ProvinceList)
            {
                ProvinceList = new Dictionary<string, dynamic>();


                SpQueryProvinceDataGateWay dg = new SpQueryProvinceDataGateWay();
                var result = dg.FetchAll();
                if (result.Count > 0)
                {
                  
                    foreach (Dictionary<string, dynamic> item in result.Data)
                    {
                        // Console.WriteLine(item.ToJSON());
                        var ProvinceCode = (string)item["ProvinceCode"];
                        if (!string.IsNullOrEmpty(ProvinceCode))
                        {
                            ProvinceList.Add((string)item["ProvinceCode"], item);
                        }
                        
                       
                    }
                }
                else
                {
                    
                    throw new Exception("Province Not Found!!");
                }
                Console.WriteLine(ProvinceList.ToJSON());
            }

        }
        public ProvinceEntity Find(string provinceCode)
        {
           

            Console.WriteLine(" Search :" + provinceCode);
            if (ProvinceList.ContainsKey(provinceCode))
            {
                var provinceRow = ((Dictionary<string, dynamic>)ProvinceList[provinceCode]);
                return new ProvinceEntity
                {
                    Id = provinceRow["Id"],
                    ProvinceCode = provinceRow["ProvinceCode"],
                    ProvinceName = provinceRow["ProvinceName"],
                    SortIndex = provinceRow["SortIndex"]
                };
                
            }
            else
            {
                return new ProvinceEntity
                {
                    Id = new Guid(),
                    ProvinceCode = "00",
                    ProvinceName = $"ไม่พบข้อมูล ({provinceCode})"
                };
                Console.WriteLine("Not found"+ provinceCode);
                return null;
            }
            

        }
       
    }
    public class ProvinceEntity
    {
        public Guid Id { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceNameEng { get; set; }
        public int SortIndex { get; set; }

    }

}