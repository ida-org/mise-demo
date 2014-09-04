using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using MdaToolkit;

namespace ClientTest
{
    public class ClientTest
    {
        public static void Main(string[] args)
        {
            try
            {
                publish();
                delete();
                search();
                retrieve();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            Console.ReadLine();
        }

        private static string certPath = "certificate.pfx";
        private static string certPassword = "password";

        public static void publish()
        {
            CertificateManagement cm = new CertificateManagement();
            X509Certificate2 cert = cm.GetTrustedSystemPrivateCert(certPath, certPassword);
            HttpHandler httph = new HttpHandler(cert);
            XDocument iepdDocument = new XDocument();
            iepdDocument.Add(new XElement("test", "test")); //Load a valid IEPD instance here
            string iepdType = "IAN";
            int msgID = 123456789;
            string result = httph.GetHttpWebResponse("PUT", "Publish", iepdType, msgID, iepdDocument, null).ToString();
            Console.WriteLine(result);
        }

        public static void delete()
        {
            CertificateManagement cm = new CertificateManagement();
            X509Certificate2 cert = cm.GetTrustedSystemPrivateCert(certPath, certPassword);
            HttpHandler httph = new HttpHandler(cert);
            string iepdType = "IAN";
            int msgID = 123456789;
            string result = httph.GetHttpWebResponse("DELETE", "Delete", iepdType, msgID, null, null).ToString();
            Console.WriteLine(result);
        }

        public static void search()
        {
            CertificateManagement cm = new CertificateManagement();
            X509Certificate2 cert = cm.GetTrustedSystemPrivateCert(certPath, certPassword);

            //build the attributes
            AttributeHolder attr = new AttributeHolder();
            attr.Id = "123";
            attr.IssueInstant = DateTime.Now.ToUniversalTime();
            attr.ElectronicEntityId = "https://mise.agencyone.gov/";
            attr.FullName = "John Doe";
            attr.CitizenCodes = new List<string>() {"USA"};
            attr.LEI = true;
            attr.PPI = true;
            attr.COI = true;

            ServiceHandler sh = new ServiceHandler();

            //send the saml login - the cookie represents the session
            CookieContainer session = sh.SendSamlRequest(cert, attr);

            ServiceEndPointManager sepm = new ServiceEndPointManager();
            //set up a base search URL
            Uri url = new Uri(sepm.BuildServiceEndPoint("Search", "IAN", "")); 
            
            //Assemble the query arguments - e.g. lat/lng, etc
            Dictionary<string, string> qd = new Dictionary<string,string>();
            qd.Add("ulat", "10");
            qd.Add("ulng", "-10");
            qd.Add("llat", "-10");
            qd.Add("llng", "10");
            TimeSpan tStart = new TimeSpan(365, 0,0,0); //search the past years data
            TimeSpan tEnd = new TimeSpan(0, 0, 30); //cover search up to 30 seconds ago (data refresh rate determination)
            string start = DateTime.Now.Subtract(tStart).ToUniversalTime().ToString("o");
            string end = DateTime.Now.Subtract(tEnd).ToUniversalTime().ToString("o");
            qd.Add("start", start);
            qd.Add("end", end);

            url = HttpExtensions.AddQuery(url, qd);

            XDocument result = sh.SendQueryRequest(cert, session, url.ToString(), "application/xml");

            Console.WriteLine(result.ToString());
        }

        public static void retrieve()
        {
            CertificateManagement cm = new CertificateManagement();
            X509Certificate2 cert = cm.GetTrustedSystemPrivateCert(certPath, certPassword);

            //build the attributes
            AttributeHolder attr = new AttributeHolder();
            attr.Id = "123";
            attr.IssueInstant = DateTime.Now.ToUniversalTime();
            attr.ElectronicEntityId = "https://mise.agencyone.gov/";
            attr.FullName = "John Doe";
            attr.CitizenCodes = new List<string>() { "USA" };
            attr.LEI = true;
            attr.PPI = true;
            attr.COI = true;

            ServiceHandler sh = new ServiceHandler();

            //send the saml login - the cookie represents the session
            CookieContainer session = sh.SendSamlRequest(cert, attr);

            ServiceEndPointManager sepm = new ServiceEndPointManager();
            //set up a base retrieve URL
            Uri url = new Uri(sepm.BuildServiceEndPoint("Retrieve", "IAN", "")); 
            
            Dictionary<string, string> qd = new Dictionary<string, string>();
            qd.Add("entityid", "https://mise.agencyone.gov/");
            qd.Add("recordid", "123456789");

            url = HttpExtensions.AddQuery(url, qd);

            XDocument xd = sh.SendQueryRequest(cert, session, url.ToString(), "application/xml");

            Console.WriteLine(xd.ToString());
        }


    }
}
