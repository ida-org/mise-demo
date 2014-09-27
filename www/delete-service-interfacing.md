<h1 class="with-tabs">Interfacing with the Delete Service</h1>

<p>The delete interface on the MISE is an extension of the publish interface.
	To delete a previously published information product, the publishing
	system need only issue a DELETE HTTP request with the same parameters as
	the original publish message. The complete interface description for
	delete is in the <a href="publish-spec.md">MISE Publish Interface
	Specification</a>. See the <a href="code-overview.md">Code Overview</a>
	page for the code download and library details. The ClientTest project
	containing these code examples is also available on that page.</p>

<p>The following code example is structurally identical to the publish
	example, save for the actual HTTP operation on line 47, which is a DELETE,
	instead of a PUT with XML content.</p>

```java
package test;

import gov.mda.trustfabric.TrustFabric;
import gov.mda.util.RestServiceClient;

import java.io.FileInputStream;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;

import org.apache.commons.io.FilenameUtils;
import org.apache.http.HttpResponse;

public class TestDeleteClient {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		RestServiceClient m_client; 

		
		/* Strongly recommend that these be loaded from a configuration file dynamically in production code */
		
		String miseCert = "C:\\Users\\user\\Documents\\ca\\ca.crt"; //public certificate for the MISE
		String trustFabricUrl = "<a href="https://mise.mda.gov/miseresources/TrustFabric.xml">https://mise.mda.gov/miseresources/TrustFabric.xml</a>"; //trust fabric URL on the MISE server
		String trustFabricBackupPath = "C:\\Users\\user\\Documents\\TrustFabricBackup.xml"; //backup local file location for a cached version of the trust fabric
		String serverScheme = "https";
		String serverHost = "mise.mda.gov";
		String serverPort = "9443";
		String serverBasePath = "/services";
		String keystorePath = "C:\\Users\\user\\Documents\\server.p12"; //keystore which contains the certificate and private key for this trusted system
		String keystorePass = "password";
		
		
		
		try {
			FileInputStream isCert = new FileInputStream(FilenameUtils.separatorsToSystem(miseCert));
			CertificateFactory certFactory = CertificateFactory.getInstance("X.509");
			X509Certificate cert = (X509Certificate) certFactory.generateCertificate(isCert);
			
	    	TrustFabric.initializeFromURL(trustFabricUrl, trustFabricBackupPath, cert);
		
			m_client = new RestServiceClient(serverScheme, serverHost,	Integer.valueOf(serverPort), serverBasePath);
		
			m_client.setClientCert(FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
			
			HttpResponse response = m_client.delete("/MDAService/publish/pos/id", "", null, null);
		} catch(Exception e) {
			e.printStackTrace();
		}
		

	}

}
```

<p>For the DELETE operation, the <code>/id</code> on the publish URL must
	actually be the ID under which the information product was originally
	published.</p>

```java
HttpResponse response = m_client.delete("/MDAService/publish/pos/id", "", null, null);
```

<p>In production, the delete, response-handling, and error-handling code must
	be more robust, to deal with the various error codes that might be
	returned by the MISE depending on the interaction with the security
	services and the outcome of the delete operation.</p>

<p>Please note that the current base URL for the MSIE is
	<code>/services/MDAService/</code>, followed by publish, search, or
	retrieve, as described in this guide.</p>