using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData.Tests
{
    [TestClass()]
    public class InMemoryDataStorageBaseTests
    {
        [TestMethod()]
        public void CountryMasterDataFindByCodeTest()
        {
            var result = CountryMasterData.Instance.FindByCode("00002");
            Assert.AreEqual("00002", result.CountryCode);
        }
        [TestMethod()]
        public void CountryMasterDataFindByFieldTest()
        {
            var result = CountryMasterData.Instance.FindByField("Code", "00002");
            Assert.AreEqual("00002", result.CountryCode);
        }
        [TestMethod()]
        public void CountryMasterDataFindByEnumFieldTest()
        {
            var result = CountryMasterData.Instance.FindByField(CountryEntityFields.CountryCode, "00002");
            Assert.AreEqual("00002", result.CountryCode);
        }


        [TestMethod()]
        public void CountryMasterDataFindByCode_It_Shoud_Return_Null_Test()
        {
            var result = CountryMasterData.Instance.FindByCode("78943");
            Assert.IsNull(result);
          //  Assert.IsNull(result.CountryCode);
        }
        [TestMethod()]
        public void DistricMasterDataFindByCodeTest()
        {
            var result = DistricMasterData.Instance.FindByCode("1001");
            Assert.AreEqual("1001", result.DistrictCode);
        }
        [TestMethod()]
        public void NationalityMasterDataFindByCodeTest()
        {
            var result = NationalityMasterData.Instance.FindByCode("00002");
            Assert.AreEqual("00002", result.Code);
        }
        [TestMethod()]
        public void OccupationMasterDataFindByCodeTest()
        {
            var result = OccupationMasterData.Instance.FindByCode("00003");
            Assert.AreEqual("00003", result.Code);
        }
        [TestMethod()]
        public void ProvinceMasterDataFindByCodeTest()
        {
            var result = ProvinceMasterData.Instance.FindByCode("12");
            Assert.AreEqual("12", result.ProvinceCode);
        }
        [TestMethod()]
        public void SubDistrictMasterDataFindByCodeTest()
        {
            var result = SubDistrictMasterData.Instance.FindByCode("100104");
            Assert.AreEqual("100104", result.SubDistrictCode);
        }

        [TestMethod()]
        public void AddressTypeMasterDataFindByCodeTest()
        {
            var result = AddressTypeMasterData.Instance.FindByCode("01");
            Assert.AreEqual("01", result.Code);
        }


        

    }
}