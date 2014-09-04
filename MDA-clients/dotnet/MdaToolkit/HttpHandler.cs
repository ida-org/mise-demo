using System;
using System.Web;
using System.Xml;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Text;

namespace MdaToolkit
{
    public class HttpHandler
    {
        private X509Certificate2 _certToPass;
        public CookieContainer _cookieContainer;

        public HttpHandler(X509Certificate2 certToPass)
        {
            this._certToPass = certToPass;
        }

        public Exception exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceEndPoint"></param>
        /// <param name="certToPass"></param>
        /// <param name="requestMethod"></param>
        /// <returns></returns>
        public HttpWebRequest CreateHttpWebRequest(string serviceEndPoint, string requestMethod)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceEndPoint);
            CookieContainer cookiejar = new CookieContainer();
            try
            {
                request.Method = requestMethod.ToUpper();
                request.CookieContainer = cookiejar;
                request.ContentType = "application/xml";
                request.Accept = "application/xml";
                request.ClientCertificates.Add(_certToPass);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="certToPass"></param>
        /// <param name="requestMethod"></param>
        /// <param name="service"></param>
        /// <param name="operation"></param>
        /// <param name="messageId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public XDocument GetHttpWebResponse(string requestMethod, string service, string operation, int messageId, XDocument message, string saml)
        {
            XDocument xmlResponseDoc = new XDocument();
            HttpStatusCode errorCode = new HttpStatusCode();
            ServiceEndPointManager sepm = new ServiceEndPointManager();
            XmlReaderSettings settings = new XmlReaderSettings() { CloseInput = true, IgnoreProcessingInstructions = true, DtdProcessing = System.Xml.DtdProcessing.Ignore, ValidationType = ValidationType.None };

            //Needed to force acceptance of the MISE SSL certificate if it isn't installed in TSL
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            
            HttpWebRequest request = CreateHttpWebRequest(sepm.BuildServiceEndPoint(service, operation, messageId.ToString()), requestMethod);

            if (message != null) //Performing a publish
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message.ToString());
                request.ContentLength = bytes.Length;
                using (Stream putStream = request.GetRequestStream())
                {
                    putStream.Write(bytes, 0, bytes.Length);
                }
            }
            if (!String.IsNullOrEmpty(saml)) //Sending SAML Assertion 
            {
                byte[] bytes = Encoding.UTF8.GetBytes(saml.ToString());
                request.ContentLength = bytes.Length;
                using (Stream putStream = request.GetRequestStream())
                {
                    putStream.Write(bytes, 0, bytes.Length);
                }
            }

            //Delete falls through with no body.

            _cookieContainer = new CookieContainer();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK) //Successful response from SAML or Search
                {
                    if (response.Cookies.Count >= 1)
                        _cookieContainer.Add(response.Cookies);
                }
                else if (response.StatusCode == HttpStatusCode.Created) //Successful response from Publish
                {
                    using (XmlReader reader = XmlReader.Create(response.GetResponseStream(), settings))
                    {
                        xmlResponseDoc = XDocument.Load(reader);
                        return xmlResponseDoc;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    //Successful delete, no return
                }
                else
                {
                    errorCode = response.StatusCode;
                    using (XmlReader reader = XmlReader.Create(response.GetResponseStream(), settings))
                    {
                        xmlResponseDoc = XDocument.Load(reader);
                        return xmlResponseDoc;
                    }
                    //Handle the various errors in accordance with preference
                }
            }
            return null;
        }

        /// <summary>
        /// This method is used for force the Trusted System to accept the SSL certificate coming back from the
        /// ISI when an HTTP Response is received.  This is only needed if the ISI certificate hasn't been
        /// added as a trusted cert within the environments certificate store.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certification"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
