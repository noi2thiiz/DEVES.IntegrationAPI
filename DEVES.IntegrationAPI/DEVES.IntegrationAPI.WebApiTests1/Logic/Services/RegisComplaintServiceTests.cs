using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.RegComplaint;
using DEVES.IntegrationAPI.Model.RequestSurveyor;
using DEVES.IntegrationAPI.WebApi.Templates;
using DEVES.IntegrationAPI.WebApi.Templates.Exceptions;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class RegisComplaintServiceTests
    {

        [TestMethod()]
        public void Execute_RegisComplaintService_It_should_return_fail_when_give_valid_json_input()
        {
            /* service นี้จะ return null ถ้าไม่ใส่ input  
            {
                  "comp_id": "155",
                  "case_no": "CAS201709-02784",
                  "errorMessage": null,
                  "data": null,
                  "code": null,   --code ไม่ควรเป็นค่า null
                  "message": null,
                  "description": null,
                  "transactionId": null,
                  "transactionDateTime": "0001-01-01 00:00:00"
                }
                             */
            try
            {
                var service =
                    new RegisComplaintService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new Request_RegComplaintModel
                {
                    compResolve = "",
                    compIdcard = "",
                    compRegno = "nullnull",
                    compEmail = "",
                    compClaim = "PL005601",
                    compPolicy = "PL012163",
                    chanInform = "",
                    compCustcompany = "",
                    empNo = "54002",
                    compCustname = "คุณสมนึก  วารีเจริญ",
                    compDetail = "คุณสมนึก  วารีเจริญ  คู่กรณี เข้าร้องเรียนผ่านสำนักงาน คปภ.เขตจตุจักร  เรียกร้องให้บริษัท เทเวศประกันภัย จำกัด ชดใช้ค่าสินไหมขาดประโยชน์จากการใช้และค่าเสื่อมสภาพให้กับผู้ร้อง อ้างอิงเลขเคลม PL005601  (รายละเอียดตามแนบ) นัดหมายชี้แจงวันพฤหัสบดีที่ 14 กันยายน 2560 เวลา 13.00 น. ณ สำนักงาน คปภ.เขตจตุจักร จึงเรียนมาเพื่อโปรดดำเนินการ ",
                    contrChanel = "1",
                    compMobile = "",
                    compAddr = "",
                    dtCompDate  = DateTime.Now,
                    compType = "002",
                    compCusttype = "2",
                    compFax = "",
                    cntType = "OTH",
                    caseNo = "CAS201709-02784",
                    dtkpvDate = DateTime.Now,
                    compPhone = ""
                });

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                Assert.IsTrue(false==string.IsNullOrEmpty(result.comp_id));
                Assert.IsTrue(false == string.IsNullOrEmpty(result.case_no));



                // Assert.AreEqual("400", result.code);
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Assert.Fail(e.Message);
            }
        }

        [TestMethod()]
        public void Execute_RegisComplaintService_It_should_return_fail_when_give_empty_json_input()
        {
            /* service นี้จะ return null ถ้าไม่ใส่ input
             {
                  "comp_id": null,
                  "case_no": null,
                  "errorMessage": null,
                  "data": null,
                  "code": null,
                  "message": null,
                  "description": null,
                  "transactionId": null,
                  "transactionDateTime": "0001-01-01 00:00:00"
                }
                             */
            try
            {
                var service =
                    new RegisComplaintService(Guid.NewGuid().ToString(), "UnitTest");
                var result = service.Execute(new Request_RegComplaintModel());

                Console.WriteLine("==================result======================");

                Console.WriteLine(result.ToJson());
                Assert.IsNotNull(result);
                Assert.IsTrue(string.IsNullOrEmpty(result.code));



                // Assert.AreEqual("400", result.code);
            }
            catch (BuzErrorException be)
            {
                Console.WriteLine("==================result BuzErrorException======================");
                Console.WriteLine(be.GetOutputModel().ToJson());
                Assert.Fail(be.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("==================result Exception======================");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Assert.Fail(e.Message);
            }
        }
    }
}