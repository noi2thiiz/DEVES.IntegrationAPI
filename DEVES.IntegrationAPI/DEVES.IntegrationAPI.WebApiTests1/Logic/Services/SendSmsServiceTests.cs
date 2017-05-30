using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Logic.Services.Tests
{
    [TestClass()]
    public class SendSmsServiceTests
    {
        [TestMethod()]
        public void SendMessageTest()
        {
            AppConfig.Instance.StartupForUnitTest();
            var result = SendSmsService.Instance.SendMessage("บริษัท เทเวศประกันภัย จำกัด (มหาชน) ขอขอบพระคุณอย่างสูงที่ท่านมอบความไว้วางใจในการใช้บริการ เพื่อการปรับปรุงบริการที่ดีขึ้น บริษัทขอความกรุณาท่านโปรดตอบแบบสอบถามตามลิงก์ด้านล่างจักเป็นพระคุณยิ่ง https://crmappqa.deves.co.th/survey/?ref=A1qrMHAEQy  ", "0818102771");
            
            Assert.AreEqual(true, result.success);
        }
    }
}