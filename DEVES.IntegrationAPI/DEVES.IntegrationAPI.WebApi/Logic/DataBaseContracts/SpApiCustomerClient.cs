using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using DEVES.IntegrationAPI.WebApi.Templates;
using Microsoft.Xrm.Client.Collections.Generic;

namespace DEVES.IntegrationAPI.WebApi.Logic.DataBaseContracts
{
    public class SpApiCustomerClient: BaseDataBaseContracts<CustomerClientEntity>
    {   
        private static SpApiCustomerClient _instance;

        public static SpApiCustomerClient Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new SpApiCustomerClient("sp_API_CustomerClient");

                return _instance;
            }
        }

        public List<string> SearchCrmContactClientId(string clientType,string clientId)
        {
            clientId = clientId.Trim().Replace(" ", "");
            var result = Excecute(new Dictionary<string, string> { { "clientType", clientType }, { "clientId", clientId } });
            Console.WriteLine(result.ToJson());
            var searchResult = new List<string>();
            if (true==result.Success)
            {
                if (result.Data.Any())
                {
                    foreach (var item in result.Data)
                    {
                        searchResult.Add(Tranform(item)?.CrmClientId);
                    }


                }
                return searchResult;
            }
            else
            {
                throw new Exception("SearchCrmContactClientId Error:" + result.Message);
            }
           
        }

        public string GetCrmContactClientId(string clientType, string clientId)
        {
            clientId = clientId.Trim().Replace(" ", "");
            var result = Excecute(new Dictionary<string, string> { { "clientType", clientType }, { "clientId", clientId } });
            
            var searchResult = "";
            if (result.Success)
            {
                if (result.Data.Any())
                {
                    return Tranform(result.Data[0])?.CrmClientId;


                }

            }
            else
            {
                throw new Exception("SearchCrmContactClientId Error:" + result.Message);
            }
            return searchResult;
        }


        public SpApiCustomerClient(string storeName) : base(storeName)
        {
        }

        public CustomerClientEntity Tranform(dynamic item)
        {
            var row = ((Dictionary<string, dynamic>)item);
            var clientType = "P";
            if (row.ContainsKey("AccountId"))
            {
                clientType = "C";
            }
         
            CustomerClientEntity cus;
            if (clientType == "P")
            {
                cus = new CustomerClientEntity
                {
                  
                    CrmClientId = "" + row["pfc_crm_person_id"],
                    Name1 = "" + row["FirstName"],
                    Name2 = "" + row["LastName"]


                };
            }
            else
            {
                cus = new CustomerClientEntity
                {
                  
                    CrmClientId = "" + row["AccountNumber"],
                    Name1 = "" + row["pfc_long_giving_name"],
                    Name2 = "" + row["pfc_long_surname"]

                };
            }

            return cus;
        }

      
    }

    public class CustomerClientEntity
    {
      
        public string CrmClientId { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }

    }
}
/*
 * DECLARE @clientType NVARCHAR(1) = 'P'
   DECLARE @clientId NVARCHAR(100) = 'CRM5555'
   EXEC [dbo].[sp_API_CustomerClient] @clientType,@clientId
 */
