using DEVES.IntegrationAPI.WebApi.DataAccessService.DataGateway;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.Helper
{
    public class EnvironmentDataService
    {
        const string CASE_CATEGORY_FOR_RVP = "0201";
        const string CASE_SUB_CATEGORY_FOR_RVP = "0201-013";
        const string INFORMER_CORPORATE_POLISY_CLIENT_ID_FOR_RVP = "10077508";//  pfc_polisy_client_id = 10077508 ''  

       

        private static EnvironmentDataService instance;
        private EnvironmentDataService() { }
        public static EnvironmentDataService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnvironmentDataService();
                   
                }
                return instance;
            }
        }

        protected CaseCategoryEntity CaseCategoryForRVP { get; set; }
        public CaseCategoryEntity GetCaseCategoryForRVP()
        {


            if (null == CaseCategoryForRVP)
            {
                CaseCategoryForRVP = new CaseCategoryEntity();


                if (MemoryManager.Memory.ContainsKey("RVP_CaseCategoryForRVP"))
                {
                    CaseCategoryForRVP = (CaseCategoryEntity)MemoryManager.Memory.GetItem("RVP_CaseCategoryForRVP");
                }
                else
                {
                    PfcCategoryDataGateWay dg = new PfcCategoryDataGateWay();

                    var result = dg.FindByCode(CASE_CATEGORY_FOR_RVP);

                    if (null != result)
                    {
                        CaseCategoryForRVP = result;
                    }
                    else
                    {
                        throw new Exception("Case Category For RVP Not Found !!");
                    }

                }

            }
            return CaseCategoryForRVP;

           
        }


        protected CaseSubCategoryEntity CaseSubCategoryForRVP { get; set; }
        public CaseSubCategoryEntity GetCaseSubCategoryForRVP()
        {

            if (null == CaseSubCategoryForRVP)
            {
                CaseSubCategoryForRVP = new CaseSubCategoryEntity();


                if (MemoryManager.Memory.ContainsKey("RVP_CaseSubCategoryForRVP"))
                {
                    CaseSubCategoryForRVP = (CaseSubCategoryEntity)MemoryManager.Memory.GetItem("RVP_CaseSubCategoryForRVP");
                }
                else
                {
                    PfcSubCategoryDataGateWay dg = new PfcSubCategoryDataGateWay();

                    var result = dg.FindByCode(CASE_SUB_CATEGORY_FOR_RVP);

                    if (null != result)
                    {
                        CaseSubCategoryForRVP = result;
                    }
                    else
                    {
                        throw new Exception("Case Sub Category For RVP Not Found !!");
                    }

                }

            }
            return CaseSubCategoryForRVP;

        }

        public object ConectionString()
        {
            throw new NotImplementedException();
        }

        protected CorperateEntity InformerForRVP { get; set; } 
        public CorperateEntity GetInformerForRVP()
        {



            if (null ==  InformerForRVP) {

                InformerForRVP = new CorperateEntity();

                if (MemoryManager.Memory.ContainsKey("RVP_InformerForRVP"))
                {
                    InformerForRVP = (CorperateEntity)MemoryManager.Memory.GetItem("RVP_InformerForRVP");
                }
                else
                {
                    CorperateDataGateWay db = new CorperateDataGateWay();
                    CorperateEntity result = db.FindByPolisyClientId(INFORMER_CORPORATE_POLISY_CLIENT_ID_FOR_RVP);



                    if (null != result)
                    {
                        InformerForRVP = result;
                    }
                    else
                    {
                        throw new Exception("Case Sub Category For RVP Not Found !!");
                    }

                }
            }
            return InformerForRVP;

        }

       public string GetXrmConnectionString()
        {
            return ((string)CrmConfigurationSettings.AppConfig.Get("CRMSDK"));
        }

    }
}