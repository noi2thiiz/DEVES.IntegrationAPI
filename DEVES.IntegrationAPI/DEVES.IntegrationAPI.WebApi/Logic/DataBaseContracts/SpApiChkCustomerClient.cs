using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.WebApi.Templates;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts
{
    public class SpApiChkCustomerClient : BaseDataBaseContracts<SpApiChkCustomerClient.ChkCustomerClient>
    {
        private static SpApiChkCustomerClient _instance;

        private SpApiChkCustomerClient(string storeName) : base(storeName)
        {
        }

        public static SpApiChkCustomerClient Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new SpApiChkCustomerClient("sp_API_ChkCustomerClient");

                return _instance;
            }
        }

        /// <summary>
        /// return true if cleansingId is existing
        /// </summary>
        /// <param name="cleansingId"></param>
        /// <returns></returns>
        public bool CheckByCleansingId(string cleansingId)
        {
            cleansingId = cleansingId.Trim().Replace(" ", "");
            var result =
                Excecute(new Dictionary<string, string> {{"searchType", "cleansingid"}, {"searchValue", cleansingId}});
            Console.WriteLine(result.ToJson());
            var searchResult = new List<string>();
            if (result.Success)
            {
                if (result.Data.Any())
                {
                    var model = (Tranform(result.Data[0]));
                    Console.WriteLine(model.ToJson());
                    return model.returnCheck == "Y";
                }
            }
            return false;
        }

        public class ChkCustomerClient
        {
            public string returnCheck { get; set; }
            public string returnObjTtpyeCode { get; set; }
        }
    }
}