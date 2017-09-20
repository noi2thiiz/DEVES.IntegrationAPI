using DEVES.IntegrationAPI.WebApi.Controllers;
using DEVES.IntegrationAPI.WebApi.Controllers.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApiTests1.Controllers.isurvey_service
{
    [TestClass]
    public class AccidentPrilimSurveyorReportControllerTests : BaseControllersTests
    {

        [TestMethod]
        public void Post_AccidentPrilimSurveyorReportController_It_Should_Success_When_Give_Valid_Input_Test()
        {
            //input
            var jsonString = @"
                {  
                   ""ticketNo"":""CAS201709-00014"",
                   ""claimNotiNo"":""1709-00011"",
                   ""eventId"":""313769"",
                   ""caseOwnerCode"":""201508"",
                   ""caseOwnerFullName"":""ณัฐวุฒิ  เจริญสุวรรณ"",
                   ""reportAccidentResultDate"":""2017-09-06 14:57:45"",
                   ""eventDetailInfo"":{  
                      ""accidentOn"":""2017-09-06 00:00:00"",
                      ""accidentLatitude"":""13.58"",
                      ""accidentLongitude"":""100.526"",
                      ""accidentPlace"":"" ถนนประชาอุทิศ-วัดคู่สร้าง  899/50  มบ.ศรีนครวิลล์"",
                      ""accidentNatureDesc"":""วันที่เกิดเหตุ : 6-9-2017 เวลา 12:00 น. การเกิดเหตุ : ประกันเบียดเสา สถานที่เกิดเหตุ :  ถนนประชาอุทิศ-วัดคู่สร้าง  899/50  มบ.ศรีนครวิลล์สรุป-รถ ป ประเภท 1 ฝ่ายประมาทเบียดเสา ตรวจสอบใบขับขี่+เลขตัวถังถูกต้อง ออกหลักฐานรับผิดชอบ ป"",
                      ""accidentRemark"":""ติดต่อ บมจ. เทเวศ คุมราคาก่อนจัดซ่อม"",
                      ""accidentLegalResult"":""4"",
                      ""policeStation"":""-"",
                      ""policeRecordId"":""-"",
                      ""policeRecordDate"":""2017-09-06 00:00:00"",
                      ""policeBailFlag"":""0"",
                      ""numOfTowTruck"":0,
                      ""numOfAccidentInjury"":0,
                      ""numOfDeath"":0,
                      ""deductibleFee"":0,
                      ""excessFee"":0,
                      ""totalEvent"":""1"",
                      ""iSurveyCreatedDate"":""2017-09-06 14:57:45"",
                      ""iSurveyModifiedDate"":""2017-09-06 14:58:48"",
                      ""iSurveyIsDeleted"":""0"",
                      ""iSurveyIsDeletedDate"":""1900-01-01 00:00:00"",
                      ""numOfAccidentParty"":0
                   },
                   ""partiesInfo"":[  

                   ],
                   ""claimDetailInfo"":[  
                      {  
                         ""claimDetailEventId"":""313769"",
                         ""claimDetailItem"":""1"",
                         ""claimDetailDetailid"":""1000"",
                         ""claimDetailDetail"":""กระบะซ้าย"",
                         ""claimDetailLevels"":""M"",
                         ""claimDetailIsRepair"":""1"",
                         ""claimDetailRemark"":""-"",
                         ""claimDetailCreatedDate"":""2017-09-06 14:51:44"",
                         ""claimDetailModifiedDate"":""2017-09-06 14:51:44"",
                         ""claimDetailIsDeleted"":"""",
                         ""claimDetailIsDeletedDate"":""1900-01-01 00:00:00""
                      }
                   ],
                   ""claimDetailPartiesInfo"":[  

                   ]
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<AccidentPrilimSurveyorReportController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.AreEqual("200", outputJson["code"]?.ToString());
        }

        [TestMethod]
        public void Post_AccidentPrilimSurveyorReportController_It_Should_Fail_When_Give_NotExisting_ClaimNotiNo_Test()
        {
            //input
            var jsonString = @"
                {  
                   ""ticketNo"":""CAS201709-00014"",
                   ""claimNotiNo"":""9999-99999"",
                   ""eventId"":""313769"",
                   ""caseOwnerCode"":""201508"",
                   ""caseOwnerFullName"":""ณัฐวุฒิ  เจริญสุวรรณ"",
                   ""reportAccidentResultDate"":""2017-09-06 14:57:45"",
                   ""eventDetailInfo"":{  
                      ""accidentOn"":""2017-09-06 00:00:00"",
                      ""accidentLatitude"":""13.58"",
                      ""accidentLongitude"":""100.526"",
                      ""accidentPlace"":"" ถนนประชาอุทิศ-วัดคู่สร้าง  899/50  มบ.ศรีนครวิลล์"",
                      ""accidentNatureDesc"":""วันที่เกิดเหตุ : 6-9-2017 เวลา 12:00 น. การเกิดเหตุ : ประกันเบียดเสา สถานที่เกิดเหตุ :  ถนนประชาอุทิศ-วัดคู่สร้าง  899/50  มบ.ศรีนครวิลล์สรุป-รถ ป ประเภท 1 ฝ่ายประมาทเบียดเสา ตรวจสอบใบขับขี่+เลขตัวถังถูกต้อง ออกหลักฐานรับผิดชอบ ป"",
                      ""accidentRemark"":""ติดต่อ บมจ. เทเวศ คุมราคาก่อนจัดซ่อม"",
                      ""accidentLegalResult"":""4"",
                      ""policeStation"":""-"",
                      ""policeRecordId"":""-"",
                      ""policeRecordDate"":""2017-09-06 00:00:00"",
                      ""policeBailFlag"":""0"",
                      ""numOfTowTruck"":0,
                      ""numOfAccidentInjury"":0,
                      ""numOfDeath"":0,
                      ""deductibleFee"":0,
                      ""excessFee"":0,
                      ""totalEvent"":""1"",
                      ""iSurveyCreatedDate"":""2017-09-06 14:57:45"",
                      ""iSurveyModifiedDate"":""2017-09-06 14:58:48"",
                      ""iSurveyIsDeleted"":""0"",
                      ""iSurveyIsDeletedDate"":""1900-01-01 00:00:00"",
                      ""numOfAccidentParty"":0
                   },
                   ""partiesInfo"":[  

                   ],
                   ""claimDetailInfo"":[  
                      {  
                         ""claimDetailEventId"":""313769"",
                         ""claimDetailItem"":""1"",
                         ""claimDetailDetailid"":""1000"",
                         ""claimDetailDetail"":""กระบะซ้าย"",
                         ""claimDetailLevels"":""M"",
                         ""claimDetailIsRepair"":""1"",
                         ""claimDetailRemark"":""-"",
                         ""claimDetailCreatedDate"":""2017-09-06 14:51:44"",
                         ""claimDetailModifiedDate"":""2017-09-06 14:51:44"",
                         ""claimDetailIsDeleted"":"""",
                         ""claimDetailIsDeletedDate"":""1900-01-01 00:00:00""
                      }
                   ],
                   ""claimDetailPartiesInfo"":[  

                   ]
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<AccidentPrilimSurveyorReportController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);
            // Assert Return code 200
            Assert.AreEqual("500", outputJson["code"]?.ToString());
            Assert.AreEqual("claimNotiNo ไม่มีในระบบ CRM", outputJson["description"]?.ToString());

        }

        [TestMethod]
        public void Post_AccidentPrilimSurveyorReportController_It_Should_Fail_When_Give_InValid_Input_Test()
        {

            //input
            var jsonString = @"
                {  
                   ""ticketNo"":""CAS201709-00014"",
                   ""caseOwnerCode"":""201508"",
                   ""caseOwnerFullName"":""ณัฐวุฒิ  เจริญสุวรรณ"",
                   ""reportAccidentResultDate"":""2017-09-06 14:57:45"",
                   ""eventDetailInfo"":{  
                      ""accidentOn"":""2017-09-06 00:00:00"",
                      ""accidentLatitude"":""13.58"",
                      ""accidentLongitude"":""100.526"",
                      ""accidentPlace"":"" ถนนประชาอุทิศ-วัดคู่สร้าง  899/50  มบ.ศรีนครวิลล์"",
                      ""accidentNatureDesc"":""วันที่เกิดเหตุ : 6-9-2017 เวลา 12:00 น. การเกิดเหตุ : ประกันเบียดเสา สถานที่เกิดเหตุ :  ถนนประชาอุทิศ-วัดคู่สร้าง  899/50  มบ.ศรีนครวิลล์สรุป-รถ ป ประเภท 1 ฝ่ายประมาทเบียดเสา ตรวจสอบใบขับขี่+เลขตัวถังถูกต้อง ออกหลักฐานรับผิดชอบ ป"",
                      ""accidentRemark"":""ติดต่อ บมจ. เทเวศ คุมราคาก่อนจัดซ่อม"",
                      ""accidentLegalResult"":""4"",
                      ""policeStation"":""-"",
                      ""policeRecordId"":""-"",
                      ""policeRecordDate"":""2017-09-06 00:00:00"",
                      ""policeBailFlag"":""0"",
                      ""numOfTowTruck"":0,
                      ""numOfAccidentInjury"":0,
                      ""numOfDeath"":0,
                      ""deductibleFee"":0,
                      ""excessFee"":0,
                      ""totalEvent"":""1"",
                      ""iSurveyCreatedDate"":""2017-09-06 14:57:45"",
                      ""iSurveyModifiedDate"":""2017-09-06 14:58:48"",
                      ""iSurveyIsDeleted"":""0"",
                      ""iSurveyIsDeletedDate"":""1900-01-01 00:00:00"",
                      ""numOfAccidentParty"":0
                   },
                   ""partiesInfo"":[  

                   ],
                   ""claimDetailInfo"":[  
                      {  
                         ""claimDetailEventId"":""313769"",
                         ""claimDetailItem"":""1"",
                         ""claimDetailDetailid"":""1000"",
                         ""claimDetailDetail"":""กระบะซ้าย"",
                         ""claimDetailLevels"":""M"",
                         ""claimDetailIsRepair"":""1"",
                         ""claimDetailRemark"":""-"",
                         ""claimDetailCreatedDate"":""2017-09-06 14:51:44"",
                         ""claimDetailModifiedDate"":""2017-09-06 14:51:44"",
                         ""claimDetailIsDeleted"":"""",
                         ""claimDetailIsDeletedDate"":""1900-01-01 00:00:00""
                      }
                   ],
                   ""claimDetailPartiesInfo"":[  

                   ]
                }
            ";

            // ระบุ  ที่ต้องการทดสอบ และ Method ที่ต้องการทดสอบ ในตัวอย่างนี้ต้องการ  ทดสอบ Method  Post  
            var response = ExcecuteControllers<AccidentPrilimSurveyorReportController>(jsonString, "Post");
            Console.WriteLine("==============output==================");
            Assert.IsNotNull(response?.Result);
            Console.WriteLine(response?.Result);

            //แปลง string เป็น JObject
            var outputJson = JObject.Parse(response?.Result);

            // Assert Return code 400
            Assert.AreEqual("400", outputJson["code"]?.ToString());
        }

    }
}
