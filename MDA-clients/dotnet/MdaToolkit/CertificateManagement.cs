using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace MdaToolkit
{
    public class CertificateManagement
    {
        public X509Certificate2 GetTrustedSystemPrivateCert(string pfxCertPath, string password)
        {
            if (File.Exists(pfxCertPath))
            {
                X509Certificate2 cert = new X509Certificate2(pfxCertPath, password, X509KeyStorageFlags.DefaultKeySet);
                return cert;
            }

            return null;
        }
    }
}
