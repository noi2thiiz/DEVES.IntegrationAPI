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
            var result = SendSmsService.Instance.SendMessage("ทดสอบส่งข้อความ", "0943481249");
            
            Assert.AreEqual(true, result.success);
        }
    }
}