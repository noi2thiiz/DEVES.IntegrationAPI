using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.Model.CRM;
using DEVES.IntegrationAPI.WebApi.Logic;

namespace DEVES.IntegrationAPI.WebApi.Controllers.Tests
{
    [TestClass()]
    public class RequestCleansingIdControllerTests
    {
        [TestMethod()]
        public void updateCleansingIdToCRMTest()
        {
            buzReqClsId ctrl = new buzReqClsId();

            CRMRequestCleansingIdDataInputModel input = new CRMRequestCleansingIdDataInputModel();
            string clsId = "C2017-7357";
            ctrl.Execute(input, clsId);
           
        }
    }
}