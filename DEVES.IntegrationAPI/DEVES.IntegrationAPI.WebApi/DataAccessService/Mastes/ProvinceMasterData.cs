﻿using System;

namespace DEVES.IntegrationAPI.WebApi.DataAccessService.MasterData
{
    public class ProvinceMasterData:InMemoryDataStorageBase<ProvinceEntity, ProvinceEntityFields>
    {

        private static ProvinceMasterData _instance;
        private ProvinceMasterData() { }

        public static ProvinceMasterData Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new ProvinceMasterData();
                _instance.Load("sp_Query_Province", "ProvinceCode");
                return _instance;
            }
        }

        public string GetSubDistrictPrefix(string code)
        {
            return code == "10" ? "แขวง" : "ตำบล";
         }

        public string GetDistrictPrefix(string code)
        {
            return code == "10" ? "เขต" : "อำเภอ";
        }
    }
    public class ProvinceEntity
    {
        public Guid Id { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceNameEng { get; set; }
        public int SortIndex { get; set; }

    }

    public enum ProvinceEntityFields
    {
        Id, ProvinceCode, ProvinceName, ProvinceNameEng, SortIndex
    }

}