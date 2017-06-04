using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEVES.IntegrationAPI.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEVES.IntegrationAPI.WebApiTests1
{
    [TestClass]
    class SetupAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Initalization code goes here
            Console.WriteLine("AssemblyInitialize");
            AppConfig.Instance.StartupUnitTest();
        }
    }
}
