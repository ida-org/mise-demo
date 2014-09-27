<h1 class="with-tabs">Interfacing with the Publication Service</h1>

<p>The publication service provides the interface to publish information
	products to the MISE. The complete interface description for publish is in
	the <a href="publish-spec.md">Publish Specification</a>. See the
	<a href="code-overview.md">Code Overview</a> page for the code download
	and library details.</p>

<p>This example walks through publishing a single Position instance to the
	position interface, assuming that the publishing system can already
	interface with the <a href="security-services-interfacing.md">security
	services</a>, including registering the publishing system as a trusted
	system in the Trust Fabric. However,the same interface is used to publish
	batches of instances, in the same mise-recordset element set.</p>

<p>The instance file that might be published via this operation can be
	downloaded <a href="../sample-data/SamplePosition.xml">here</a>. This file
	contains a NIEM-M Position instance containing three position reports for
	the MV Example. <em>It is strongly recommended that the information
	provider validate the XML published to the MISE. The MISE will perform
	validation and return errors, but for fastest publication, validate before
	the publish operation.</em></p>

<p>The actual XML for a publish operation would normally be assembled from a
	database or some other storage location. The example below just reads the
	XML from a file.</p>

```java
package test;

import gov.mda.trustfabric.TrustFabric;
import gov.mda.util.RestServiceClient;

import java.io.File;
import java.io.FileInputStream;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;

import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.xpath.XPathFactory;

import org.apache.commons.io.FileUtils;
import org.apache.commons.io.FilenameUtils;
import org.apache.http.HttpResponse;
import org.apache.http.entity.ContentType;

public class TestPublishClient {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		RestServiceClient m_client; 

		
		/* Strongly recommend that these be loaded from a configuration file dynamically in production code */
		
		String miseCert = "C:\\Users\\user\\Documents\\ca\\ca.crt"; //public certificate for the MISE
		String trustFabricUrl = "https://mise.mda.gov/miseresources/TrustFabric.xml"; //trust fabric URL on the MISE server
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
		
			m_client = new RestServiceClient(serverScheme, serverHost, Integer.valueOf(serverPort), serverBasePath);
		
			m_client.setClientCert(FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
			
			String body = null;
			FileUtils.readFileToString(new File("SamplePosition.xml"), body);

                        //wrap the body in the mise-recordset element
			
			HttpResponse response = m_client.put("/MDAService/publish/pos", "", body, ContentType.APPLICATION_XML); //perform the request
		} catch(Exception e) {
			e.printStackTrace();
		}
		

	}

}
```

<p>
Note again that all the configuration parameters should not be hardcoded as
strings in production code, but should be loaded dynamically from a
configuration file or configuration database.</p>

<p>Examining the code in detail, the following 4 lines load the SSL
	certificate for the MISE from the filesystem, and initialize the Trust
	Fabric. The Trust Fabric code attempts to load the MISE-hosted Trust
	Fabric from the MISE endpoint first, and can fall back to a local copy if
	the MISE endpoint cannot be accessed.</p>

```java
FileInputStream isCert = new FileInputStream(FilenameUtils.separatorsToSystem(miseCert));
CertificateFactory certFactory = CertificateFactory.getInstance("X.509");
X509Certificate cert = (X509Certificate) certFactory.generateCertificate(isCert);

TrustFabric.initializeFromURL(trustFabricUrl, trustFabricBackupPath, cert);
```

<p>
The next section of the code creates the MISE Rest Client and initializes it
with the client keystore. The client keystore must contain the private key
corresponding to the certificate registered for the publishing system in the
Trust Fabric.</p>

```java
m_client = new RestServiceClient(serverScheme, serverHost, Integer.valueOf(serverPort), serverBasePath);
		
m_client.setClientCert(FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
```

<p>Finally, the last few lines read the XML from a file and publish it to the
	MISE. The /id on the publish URL must actually be a unique ID for this
	Position message in the publishing system.</p>

<p>The instance document to be published must be wrapped in the mise-recordset
	element to be published. This element carries carries one or more
	instances for publish:</p>

```xml
&lt;?xml version="1.0" encoding="UTF-8"?&gt;
&lt;mise-recordset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	xsi:noNamespaceSchemaLocation="../../../../position-3.2.iepd/XMLschemas/exchange/3.2/mise-recordset.xsd"
	pageElements="250"&gt;
	&lt;posex:Message&gt; ...snip...&lt;/posex:Message&gt;
	&lt;posex:Message&gt; ...snip...&lt;/posex:Message&gt;
	[... next 248 records]
&lt;/mise-recordset&gt;
```

```java
String body = null;
FileUtils.readFileToString(new File("SamplePosition.xml"), body);

//wrap the body in the mise-recordset element
			
HttpResponse response = m_client.put("/MDAService/publish/pos", "", body, ContentType.APPLICATION_XML); //perform the request
```
<p>In production, the publishing, response-handling, and error-handling code
	must be more robust, to deal with the various error codes that might be
	returned by the MISE depending on the interaction with the security
	services and the outcome of the publish operation. </p>

<p>Please note that the current base URL for the MISE is 
	<code>/services/MDAService/</code>, followed by publish or
	search/retrieve, as described in this guide.</p>