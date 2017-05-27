using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService
{
   
    public sealed class GlobalTransactionIdGenerator

    {
        private GlobalTransactionIdGenerator()
        {
            if (Environment.MachineName == AppConst.PRO1_SERVER_NAME)
            {
                appId = "01";
            }
            else if (Environment.MachineName == AppConst.PRO2_SERVER_NAME)
            {
                appId = "02";
            }
            else
            {
                appId = "00";
            }
            currentdate = DateTime.Now.ToString("yyMMdd");
            //หา ลำดับล่าสุดจาก ฐานข้อมูล

        }

        private static readonly object padlock = new object();

        private static GlobalTransactionIdGenerator instance = null;
        public static GlobalTransactionIdGenerator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new GlobalTransactionIdGenerator();
                    }
                    return instance;
                }
            }
        }

        private int logCount = 0;
        private string appId = "00";

        private string currentdate = "";
        private Random rnd = new Random();
        public string GetNewGuid()
        {
            var now = DateTime.Now.ToString("yyMMdd");
            var time = DateTime.Now.ToString("HHms");
            if (currentdate == now)
            {
                ++logCount;
            }
            else
            {
                logCount=0;
                currentdate = now;
            }


            int seed = rnd.Next(1, 999);
            var globalId = appId + "-" + now +  time +"-"+ (logCount.ToString()).PadLeft(7, '0');
            globalIdList.Add(globalId,0);
            return globalId;
        }
        Dictionary<string,int> globalIdList = new Dictionary<string, int>();
        public string GetNewGuid(string globalId)
        {
            if (globalIdList.ContainsKey(globalId))
            {
                globalIdList[globalId]++;

                var lastId = globalIdList[globalId];

              return globalId + "-" + (lastId.ToString()).PadLeft(3, '0');

            }
            else
            {
                return GetNewGuid();
            }
        }

        public void ClearGlobalId(string globalId)
        {
            if (globalIdList.ContainsKey(globalId))
            {
                globalIdList.Remove(globalId);
            }
        }


    }
}