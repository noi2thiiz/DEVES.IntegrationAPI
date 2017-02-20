using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DEVES.IntegrationAPI.WebApi.Models.WebDialer
{
    public class WebDialerRequester: IServiceRequestAble<MakeCallRequestModel>
    {
        string URI = "";
        string Proxy = "https://crmappdev.deves.co.th/proxy/xml.ashx";
        public WebDialerRequester(string strURI)
        {
            URI = Proxy+"?"+strURI + "/webdialer/services/WebdialerSoapService";
        }

        public IServiceResult Execute(MakeCallRequestModel req)
        {
      
           string data = postXMLData(URI, req);
            var m = parsXML(data);
            m.Data = req;

            return m;
        }

        public MakeCallResultModel parsXML(string stringXml)
        {
            var document = XDocument.Parse(stringXml); ;

            var namespaceManager = new XmlNamespaceManager(new NameTable());

            namespaceManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            namespaceManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
            namespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");


            XmlDocument xdoc = new XmlDocument();



            try
            {

                var Code = "";
                var CodeRef = document.XPathSelectElement("/soapenv:Envelope/soapenv:Body/multiRef[1]/responseCode", namespaceManager).Attribute("href").Value;
                if (CodeRef != "")
                {
                    IEnumerable<XElement> list = document.XPathSelectElements("/soapenv:Envelope/soapenv:Body/multiRef", namespaceManager);
                    foreach (XElement el in list)
                    {

                        if ("#" + el.Attribute("id").Value == CodeRef)
                        {
                            Code = el.Value;
                        }
                    }

                }
                var Description = document.XPathSelectElement("/soapenv:Envelope/soapenv:Body/multiRef[1]/responseDescription", namespaceManager).Value;
                MakeCallResultModel m = new MakeCallResultModel();
                m.CodeRef = CodeRef;
                m.Code = Code.ToString();
                m.Description = Description.ToString();


                return m;
            }
            catch (Exception e)
            {
                var Description = e.ToString();
                MakeCallResultModel m = new MakeCallResultModel();

                m.Code = "500";
                m.Description = Description;
                m.Content = stringXml;

                return m;
            }

        }


        public string postXMLData(string destinationUrl, MakeCallRequestModel req)
        {
            HttpWebRequest webRequest = (System.Net.HttpWebRequest)WebRequest.Create(destinationUrl);
            webRequest.Headers.Add("SOAP:Action");
            webRequest.ContentType = "text/xml";//charset=\"utf-8\"
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";

            string requestXml = "<soapenv:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:urn='urn:WD70'>"
+ "<soapenv:Header/><soapenv:Body><urn:makeCallSoap soapenv:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>"
 + "<in0 xsi:type='urn:Credential'><userID xsi:type='xsd:string'>webdialeradmin</userID><password xsi:type='xsd:string'>P@ssw0rdcrm</password></in0><in1 xsi:type='soapenc:string' xmlns:soapenc='http://schemas.xmlsoap.org/soap/encoding/'>" + req.DestinationNumber + "</in1>"
 + "<in2 xsi:type='urn:UserProfile'><user xsi:type='xsd:string'>" + req.UserName + "</user><lineNumber xsi:type='xsd:string'>1</lineNumber><supportEM xsi:type='xsd:boolean'>true</supportEM>"
 + "</in2></urn:makeCallSoap></soapenv:Body></soapenv:Envelope>";


            byte[] buf = Encoding.UTF8.GetBytes(requestXml);


            webRequest.ContentLength = buf.Length;
            webRequest.GetRequestStream().Write(buf, 0, buf.Length);

            //  var HttpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            WebHeaderCollection header = response.Headers;

            var encoding = ASCIIEncoding.ASCII;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
            {
                string responseText = reader.ReadToEnd();
                return responseText;
            }



        }




    }
}
