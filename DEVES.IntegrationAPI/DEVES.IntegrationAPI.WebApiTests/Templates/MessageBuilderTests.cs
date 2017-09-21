using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.Templates.Tests
{
    [TestClass()]
    public class MessageBuilderTests
    {
        [TestMethod()]
        public void ConvertMessageSapToCrmTest()
        {
            Assert.AreEqual("Please fill tax id (เลขจดทะเบียนนิติบุคคล/เลขประจำตัวผู้เสียภาษี)", MessageBuilder.Instance.ConvertMessageSapToCrm("Please fill tax number 3."));
        }
    }
}