using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.DataRepository
{
    public class DistrictRepository
    {
        private static DistrictRepository instance;
        private DistrictRepository() { }
        public static DistrictRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DistrictRepository();
                    instance.Load();
                }
                return instance;
            }
        }

        protected Dictionary<string, dynamic> DistrictList { get; set; }
        protected void Load()
        {
            if (null == DistrictList)
            {
                DistrictList = new Dictionary<string, dynamic>();


                SpQueryDistrictDataGateWay dg = new SpQueryDistrictDataGateWay();
                var result = dg.FetchAll();
                if (result.Count > 0)
                {

                    foreach (Dictionary<string, dynamic> item in result.Data)
                    {
                        // Console.WriteLine(item.ToJSON());
                        var DistrictCode = (string)item["DistrictCode"];
                        if (!string.IsNullOrEmpty(DistrictCode))
                        {
                            DistrictList.Add((string)item["DistrictCode"], item);
                        }


                    }
                }
                else
                {
                        throw new Exception("District Not Found!!");
                }
                //Console.WriteLine(DistrictList.ToJSON());
            }

        }
        public DistrictEntity Find(string DistrictCode)
        {
            

            Console.WriteLine(" Search :" + DistrictCode);
            if (DistrictList.ContainsKey(DistrictCode))
            {
                var DistrictRow = ((Dictionary<string, dynamic>)DistrictList[DistrictCode]);
                return new DistrictEntity
                {
                    Id = DistrictRow["Id"],
                    DistrictCode = DistrictRow["DistrictCode"],
                    DistrictName = DistrictRow["DistrictName"]
                   
                };

            }
            else
            {
                Console.WriteLine("Not found" + DistrictCode);

                return new DistrictEntity
                {

                    DistrictCode = "0000",
                    DistrictName = $"ไม่พบข้อมูล ({DistrictCode}) "

                };
               // return null;
            }


        }
    }

    public class DistrictEntity
    {
        public Guid Id { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEng { get; set; }
      

    }
}