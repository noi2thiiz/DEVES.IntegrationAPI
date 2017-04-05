using System;
using System.Collections.Generic;
using System.Linq;

namespace DEVES.IntegrationAPI.WebApi
{
    public class CrmConfigurationSettings
    {
        private static readonly CrmConfigurationSettings Instant = new CrmConfigurationSettings();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CrmConfigurationSettings()
        {
        }

        public static CrmConfigurationSettings AppConfig
        {
            get { return Instant; }
        }
        public static CrmConfigurationSettings AppSettings
        {
            get { return Instant; }
        }



        public Dictionary<string, dynamic> config { get; set; }


        private CrmConfigurationSettings()
        {
            var proxy = "https://crmappdev.deves.co.th/proxy/xml.ashx?";

            var SMSServiceEnpoint = proxy + "https://192.168.3.194/ServiceProxy/Common/jsonservice/COMP_SendSMS";
            var CTIMakeCallEnpoint = proxy + "https://10.10.0.22:8443/webdialer/services/WebdialerSoapService";

            var LERTFinishEndpoint = "http://ilerturest.cloudapp.net/v1/thirdparty/lert/finish";
            this.config = new Dictionary<string, dynamic>();

            this.config.Add("SMSRequestDefault", new
            {
                username = "sysdynamic",
                password = "REZOJUNtN04",
                token = "",
                gid = "SendSMS",
                uid = "CRM_Claim",
                content = new
                {
                    message = "",
                    mobileNumber = "",
                }
            });




            this.config.Add("CTIMakeCallEnpoint", CTIMakeCallEnpoint);
            this.config.Add("SMSServiceEnpoint", SMSServiceEnpoint);

            this.config.Add("LERTFinishEndpoint", LERTFinishEndpoint);


            this.config.Add("CRMDB", "Data Source=DESKTOP-Q30CAGJ;Initial Catalog=CRMDEV_MSCRM;Persist Security Info=True;User ID=sa;Password=patiwat");

        }

        protected void _CopyValues<T>(T target, T source)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);
            foreach
            (var prop in
                properties)
            {
                var value = prop.GetValue(source, null);
                if (value != null)
                    prop.SetValue(target, value, null);
            }
        }

        public T getConfig<T>(string key, T source)
        {

            if (config.ContainsKey(key))
            {
                var target =  config[key];
                 _CopyValues<T>(target, source);
                return target;
            }
            else
            {
                return source;
            }

        }

        public dynamic Get(string key)
        {
            return config[key];
        }
    }
}