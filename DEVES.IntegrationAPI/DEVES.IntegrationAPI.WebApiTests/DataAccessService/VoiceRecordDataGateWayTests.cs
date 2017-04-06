using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Services.DataGateWay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.CTI;

namespace DEVES.IntegrationAPI.WebApi.Services.DataGateWay.Tests
{
    [TestClass()]
    public class VoiceRecordDataGateWayTests
    {
        [TestMethod()]
        public void CreateTest()
        {
            VoiceRecordDataGateWay db = new VoiceRecordDataGateWay();
            VoiceRecordRequestModel model = new VoiceRecordRequestModel();
            var reuslt = db.Create("693B360D-C5A4-E611-80CA-0050568D1874", model);
            Assert.IsNotNull(reuslt);
        }
    }
}