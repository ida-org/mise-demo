using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace MdaToolkit
{
    public class ServiceHandler
    {
        private static CookieContainer _Cookies;
        private HttpWebRequest request { get; set; }
        private XDocument xmlResponse { get; set; }

        /// <summary>
        /// This method is used to build the SAML Assertion and send the HTTPWebRequest to get back the
        /// cookie sent by the ISI.  This cookie is not stored (can be), and is passed back to send along 
        /// with the follow up GET request to conduct a search or retrieve operation.
        /// </summary>
        /// <returns>Cookie Container used for the follow up GET request.</returns>
        public CookieContainer SendSamlRequest(X509Certificate2 cert, AttributeHolder attr)
        {
            SamlBuilder sb = new SamlBuilder(cert);
            String samlAssertion = sb.BuildSamlAssertion(attr);
            GetMiseCookieFromSamlAssertion(cert, samlAssertion); //This loads the _Cookies object

            return _Cookies;
        }

        /// <summary>
        /// This creates the initial request for publish operation, and is called after the SAML POST request
        /// has been sent for search or retrieve operations.
        /// </summary>
        /// <param name="serviceEndPoint">the service end point provided by the ServiceEndPointManager class</param>
        /// <param name="requestMethod">GET, POST, etc.  Allows this method to be reused accross all operations.</param>
        /// <param name="cert">The certificate of your system to pass with the request to establish a trusted connection.</param>
        /// <returns></returns>
        public HttpWebRequest CreateHttpWebRequest(string serviceEndPoint, string requestMethod, X509Certificate2 cert )
        {
            request = (HttpWebRequest)WebRequest.Create(serviceEndPoint);
            CookieContainer cookiejar = new CookieContainer();
            try
            {
                request.Method = requestMethod.ToUpper();
                request.CookieContainer = cookiejar;
                request.ContentType = "application/xml";
                request.Accept = "application/xml";
                request.ClientCertificates.Add(cert);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return request;
        }


        /// <summary>
        /// This is the method that sends the SAML POST request and gets back the cookie.
        /// It is called in this class by the SendSamlRequest() method.
        /// </summary>
        /// <param name="saml">the string of xml representing the SAML Assertion to POST to the ISI</param>
        /// <returns></returns>
        private CookieContainer GetMiseCookieFromSamlAssertion(X509Certificate2 cert, string saml)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings() { CloseInput = true, IgnoreProcessingInstructions = true, DtdProcessing = System.Xml.DtdProcessing.Ignore, ValidationType = ValidationType.None };
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                HttpWebRequest request = CreateHttpWebRequest("https://107.23.66.168:9443/services/MDAUserSessionService/login", "POST", cert);
                if (!String.IsNullOrEmpty(saml))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(saml.ToString());
                    request.ContentLength = bytes.Length;
                    using (Stream putStream = request.GetRequestStream())
                    {
                        putStream.Write(bytes, 0, bytes.Length);
                    }
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    _Cookies = new CookieContainer();
                    if (response.Cookies.Count >= 1)
                        _Cookies.Add(response.Cookies);
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }

            return _Cookies;
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// This method is used to send a query request for all operations.  It can return eitehr XML or KML.
        /// It is called after the SAML Post has been performed.
        /// </summary>
        /// <param name="miseCookies">The cookie returned from the SAML Post operation</param>
        /// <param name="url">The url with query string included.</param>
        /// <param name="contentType">either "application/xml" or "application/kml"</param>
        /// <returns></returns>
        public XDocument SendQueryRequest(X509Certificate2 cert, CookieContainer miseCookies, string url, string contentType)
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.CookieContainer = miseCookies;
            request.ContentType = contentType;
            request.Accept = "application/xml";
            request.ClientCertificates.Add(cert);

            using (HttpWebResponse getResponse = (HttpWebResponse)request.GetResponse())
            {
                if (getResponse.StatusCode == HttpStatusCode.OK)
                {
                    if (getResponse.ContentType.Contains("application/xml"))
                    {
                        using (Stream reader = getResponse.GetResponseStream())
                        {
                            StreamReader sreader = new StreamReader(reader, Encoding.UTF8);
                            String result = sreader.ReadToEnd();
                            xmlResponse = XDocument.Parse(result);
                            sreader.Close();
                        }
                    }
                }
            }

            return xmlResponse;
        }
    }
}
