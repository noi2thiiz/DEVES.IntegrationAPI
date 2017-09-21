using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DEVES.IntegrationAPI.Core.Helper;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using DEVES.IntegrationAPI.WebApi.Logic;
using DEVES.IntegrationAPI.WebApi.TechnicalService;
using DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter;
using DEVES.IntegrationAPI.WebApi.Logic.Validator;

namespace DEVES.IntegrationAPI.WebApi
{
    public class AppBootstrap
    {
        #region Singleton

        private static AppBootstrap _instance;
        private AppBootstrap() { }
        public static AppBootstrap Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new AppBootstrap();
                return _instance;
            }
        }

        #endregion

        public void Start()
        {
           
            //start watch config change
            AppConfig.Instance.Startup();

            // load last id
            GlobalTransactionIdGenerator.Instance.Init();

            //start log job persis log
            LogJobHandle.Start();

           
            //load master data

            CountryMasterData.Instance.Init();
            NationalityMasterData.Instance.Init();
            PersonalTitleMasterData.Instance.Init();
            SubDistrictMasterData.Instance.Init();
            DistricMasterData.Instance.Init();
            AddressTypeMasterData.Instance.Init();
            OccupationMasterData.Instance.Init();
            ProvinceMasterData.Instance.Init();
            TypeOfLossMasterData.Instance.Init();

            // [[North]] set sql
            QuerySqlService.Instance.Start();


            // add custom data validator
            JsonHelper.AddJsonValidator(new CountryOriginValidator());




            //check connection 
            var crmCustomConnectionString = System.Configuration.ConfigurationManager.AppSettings["CRM_CUSTOMAPP_DB"].ToString();
            //TestConnection(crmCustomConnectionString);

            var crmdbConnectionString = System.Configuration.ConfigurationManager.AppSettings["CRMDB"].ToString();
            //TestConnection(crmdbConnectionString);

        }

        private void  TestConnection(string connectionString)
        {
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connectionString);
                conn.Open();
                SqlCommand comm = conn.CreateCommand();
                comm.CommandType = CommandType.Text;
                comm.CommandText = "SELECT 1 AS Test";
                comm.ExecuteReader(); //System.Data.CommandBehavior.CloseConnection
            }
            finally
            {
                conn?.Close();
          
            }
           
        }
    }

}