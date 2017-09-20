using DEVES.IntegrationAPI.Model.ClaimRegistration;
using DEVES.IntegrationAPI.Model.QuerySQL;
using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI.WebApi.Controllers.Tests;
using DEVES.IntegrationAPI.WebApi.DataAccessService.QuerySQLAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApiTests1.Controllers.claim_service
{
    [TestClass]
    public class ClaimRegistrationControllerTests : BaseControllersTests
    {
        Guid g = new Guid();
        LocusClaimRegistrationInputModel inputTest = new LocusClaimRegistrationInputModel();

        [TestMethod]
        public void Post_ClaimRegistrationController_It_Should_Pass()
        {
            Guid guid = UnitTest();
            //input

            var jmodel = new {
                incidentId = guid,
                currentUserId = new Guid("50529F88-2BE9-E611-80D4-0050568D1874")
            };

            string json = JsonConvert.SerializeObject(jmodel);

            // Console.WriteLine(jmodel);

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<ClaimRegistrationController>(json, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.IsNotNull(outputJson["claimID"]?.ToString());
            Assert.IsNotNull(outputJson["claimNo"]?.ToString());

            //var checkVal = outputJson["claimID"]?.ToString();
            //Console.WriteLine(checkVal);
        }


        [TestMethod]
        public void Post_ClaimRegistrationController_It_Should_Fail_When_Give_Duplicate_Input_Test()
        {
            //input
            var jsonString = @"
                {  
                   ""incidentId"":""DC8C0ADA-4A91-E711-80DA-0050568D615F"",
                   ""currentUserId"":""50529F88-2BE9-E611-80D4-0050568D1874""
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<ClaimRegistrationController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            Assert.AreEqual("claimNotiNo is duplicated", outputJson["errorMessage"]?.ToString());
        }


        protected Guid UnitTest()
        {
            // Create Case
            var connection = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM_DEVES"].ConnectionString);
            OrganizationServiceProxy _serviceProxy = connection.OrganizationServiceProxy;
            ServiceContext svcContext = new ServiceContext(_serviceProxy);

            Incident crmCase = new Incident
            {
                CustomerId = new Microsoft.Xrm.Sdk.EntityReference(Contact.EntityLogicalName, new Guid("4F2858B8-C3CE-E611-80D1-0050568D1874")),
                OwnerId = new Microsoft.Xrm.Sdk.EntityReference(SystemUser.EntityLogicalName, new Guid("50529F88-2BE9-E611-80D4-0050568D1874"))
            };

            g = _serviceProxy.Create(crmCase);

            var queryCase = from c in svcContext.IncidentSet
                            where c.IncidentId == g
                            select c;

            // Retrieve Case
            Incident incident = queryCase.FirstOrDefault<Incident>();

            Incident retrievedIncident = (Incident)_serviceProxy.Retrieve(Incident.EntityLogicalName, g, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
            string ticketNo = retrievedIncident.TicketNumber;

            // Query claimNotiNo
            string SQL_ClaimNotiNo = @"DECLARE @ticketNo AS NVARCHAR(20) = '{0}'; DECLARE @uniqueID AS UNIQUEIDENTIFIER = '{1}'; DECLARE @requestBy AS NVARCHAR(250) = '{2}'; DECLARE @resultCode AS NVARCHAR(10) DECLARE @resultDesc AS NVARCHAR(1000) select @uniqueID = ISNULL(IncidentId, @uniqueID) from dbo.IncidentBase a with(nolock) where a.TicketNumber = @ticketNo EXEC[dbo].[sp_ReqMotorClaimNotiNo] @uniqueID, @requestBy, @resultCode OUTPUT, @resultDesc OUTPUT select @ticketNo as [@ticketNo],@uniqueID as [@uniqueID],@requestBy as [@requestBy],@resultCode as [@resultCode], @resultDesc as [@resultDesc]";
            string sqlCommand = string.Format(SQL_ClaimNotiNo, ticketNo, g, "50529F88-2BE9-E611-80D4-0050568D1874");

            QuerySqlService sql = QuerySqlService.Instance;
            QuerySQLOutputModel mappingOutput = new QuerySQLOutputModel();
            mappingOutput = sql.GetQuery("CRMQA_MSCRM", sqlCommand);

            // mock value to incident
            retrievedIncident.pfc_policy_number = "V0756004";
            retrievedIncident.pfc_customer_vip = false;
            retrievedIncident.pfc_customer_privilege = new OptionSetValue(100000001);
            retrievedIncident.pfc_high_loss_case_flag = false;
            retrievedIncident.pfc_legal_case_flag = false;
            retrievedIncident.pfc_case_of_claim_remark = "วันที่เกิดเหตุ : 28-8-2017 เวลา 08:00 น.\n การเกิดเหตุ : คู่กรณีชนท้ายประกัน";
            retrievedIncident.pfc_claim_type = new OptionSetValue(200000001);

            retrievedIncident.pfc_informer_client_number = "14514669";
            // retrievedIncident.pfc_informer_name = "ธวัชชัย จันทน์แดง";
            retrievedIncident.pfc_informer_mobile = "0863156332";
            retrievedIncident.pfc_driver_client_number = "14514669";
            // retrievedIncident.pfc_driver_nameName = "ธวัชชัย จันทน์แดง";
            retrievedIncident.pfc_driver_mobile = "0863156332";
            retrievedIncident.pfc_relation_cutomer_accident_party = new OptionSetValue(100000000);
            retrievedIncident.pfc_current_reg_num = "ษม7045";
            retrievedIncident.pfc_current_reg_num_prov = "กท";
            retrievedIncident.pfc_notification_date = Convert.ToDateTime("2017-08-30 17:36:00");
            retrievedIncident.pfc_accident_on = Convert.ToDateTime("2017-08-28 08:00:00");
            retrievedIncident.pfc_accident_desc_code = "201";
            retrievedIncident.pfc_accident_desc = "วันที่เกิดเหตุ : 28-8-2017 เวลา 08:00 น.\n การเกิดเหตุ : คู่กรณีชนท้ายประกัน";
            retrievedIncident.pfc_num_of_expect_injuries = new OptionSetValue(100000000);
            retrievedIncident.pfc_accident_place = "97 ถนน ดินสอ แขวง วัดบวรนิเวศ เขต พระนคร กรุงเทพมหานคร 10200 ประเทศไทย";
            retrievedIncident.pfc_accident_latitude = "13.757";
            retrievedIncident.pfc_accident_longitude = "100.502";
            retrievedIncident.pfc_send_out_surveyor = new OptionSetValue(100000001);

            retrievedIncident.pfc_surveyor_team = "TEAM0099";

            retrievedIncident.pfc_police_bail_flag = false;


            // mapping policyaddtional to case
            retrievedIncident.pfc_policy_additionalId = new EntityReference(retrievedIncident.LogicalName, new Guid("D78D5356-F4B6-E611-80CA-0050568D1874"));

            _serviceProxy.Update(retrievedIncident);

            return g;
        }

    }
}
