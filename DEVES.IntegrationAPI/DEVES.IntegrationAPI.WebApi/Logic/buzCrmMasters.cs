using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DEVES.IntegrationAPI.Model.CRM;
using System.Configuration;
//using Microsoft.Crm.Sdk.Messages;
//using Microsoft.Xrm.Sdk;
//using Microsoft.Xrm.Sdk.Client;
//using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace DEVES.IntegrationAPI.WebApi.Logic
{
    public sealed class buzMasterCountry
    {
        List<CRM_MasterCountry> _lstCountry;
        private static readonly buzMasterCountry instantBuzCountry = new buzMasterCountry();

        static buzMasterCountry()
        { }

        private buzMasterCountry()
        {
            using (var connection = new CrmServiceClient(CommonConstant.CRMConnectionStr))
            {
                using (var serviceProxy = connection.OrganizationServiceProxy)
                {
                    serviceProxy.EnableProxyTypes();
                    ServiceContext sc = new ServiceContext(serviceProxy);

                    var countries = (from ctry in sc.pfc_master_countriesSet
                                     where ctry.statecode == pfc_master_countriesState.Active
                                     orderby ctry.pfc_master_countries_code descending
                                     select new CRM_MasterCountry
                                     {
                                         Id = ctry.Id
                                     ,
                                         Code = ctry.pfc_master_countries_code
                                     ,
                                         Name = ctry.pfc_master_countries_name
                                     ,
                                         ctryPolisy = ctry.pfc_ref_polisy_descpf_t3645
                                     ,
                                         ctrySAP = ctry.pfc_ref_sap
                                     }).ToList<CRM_MasterCountry>();
                    _lstCountry = countries;
                }
            }
        }

        public static buzMasterCountry Instant
        {
            get {
                return instantBuzCountry;
            }
        }

        public List<CRM_MasterCountry> CountryList
        {
            get {
                return _lstCountry;
            }
        }
    }


    public sealed class buzMasterSalutation
    {
        List<CRM_MasterSalutation> _lstSalutation;
        private static readonly buzMasterSalutation _instant = new buzMasterSalutation();

        static buzMasterSalutation()
        { }

        private buzMasterSalutation()
        {
            using (var connection = new CrmServiceClient(CommonConstant.CRMConnectionStr))
            {
                using (var serviceProxy = connection.OrganizationServiceProxy)
                {
                    serviceProxy.EnableProxyTypes();
                    ServiceContext sc = new ServiceContext(serviceProxy);

                    var saluatations = (from title in sc.pfc_master_title_personalSet
                                     where title.statecode == pfc_master_title_personalState.Active
                                     select new CRM_MasterSalutation
                                     {
                                         Id = title.Id
                                     ,
                                         Code = title.pfc_master_title_personal_code
                                     ,
                                         Name = title.pfc_master_title_personal_name
                                     ,
                                         titlePolisy = title.pfc_ref_polisy_descpf_t3583
                                     ,
                                         titleSAP = title.pfc_master_title_personal_name
                                     }).ToList<CRM_MasterSalutation>();
                    _lstSalutation = saluatations;
                }
            }
        }

        public static buzMasterSalutation Instant
        {
            get {
                return _instant;
            }
        }
        
        public List<CRM_MasterSalutation> SalutationList
        {
            get {
                return _lstSalutation;
            }
        }
    }
}