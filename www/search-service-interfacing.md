<h1 class="with-tabs">Interfacing with the Search and Retrieve Service</h1>

<p>The search service provides the interface to search for information
	products on the MISE. The complete interface description for search is in
	the <a href="search-retrieve-spec.md">MISE Search/Retrieve Interface
	Specification</a>. See the <a href="code-overview.md">Code Overview</a>
	page for the code download and library details. The ClientTest project
	containing these code examples is also available on that page.</p>

<p>This example walks through a query for Position reports based on the first
	Position <a href="/drupal/node/26">User Story</a>, a search bounded by
	geospatial area and time. This example assumes the searching system can
	interface with the MISE
	<a href="security-services-interfacing.md">security services</a>,
	including registering the system as a trusted system in the Trust
	Fabric.</p>

<p>Unlike the previous two examples which only require the SSL handshake with
	the MISE, the search and retrieve operations require that the consumer
	system pass the entitlement attributes of the user. The meanings of the
	attributes are discussed in detail in the <a href="attribute-spec.md">MISE
	Attribute Specification</a>. These entitlement attributes are provided to
	the MISE via a SAML assertion. When the SAML assertion is validated, the
	MISE returns a session cookie to the client system, which must be provided
	for all subsequent search and retrieve operations.</p>

```java
package test;

import gov.mda.Constants;
import gov.mda.saml.AssertionBuilder;
import gov.mda.saml.SAMLUtils;
import gov.mda.trustfabric.TrustFabric;
import gov.mda.util.RestServiceClient;

import java.io.FileInputStream;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;

import org.apache.commons.io.FilenameUtils;
import org.apache.http.HttpResponse;
import org.apache.http.entity.ContentType;
import org.apache.http.util.EntityUtils;
import org.opensaml.saml2.core.Assertion;
import org.opensaml.saml2.core.Attribute;

public class TestSearchClient {

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
			
			//Form the user assertion
			String assertingPartyID = "test.client";
			AssertionBuilder builder = new AssertionBuilder(assertingPartyID);
			builder.addStandardConditions(Constants.MISE_AUDIENCE_RESTRICTION, 10*60);	// valid for 10 minutes
			builder.addAttribute("ElectronicIdentityId", "gfipm:2.0:user:ElectronicIdentityId", "<a href="mailto:testuser@testsystem.gov">testuser@testsystem.gov</a>");
			builder.addAttribute("FullName", "gfipm:2.0:user:FullName", "Test T. User");
			
                        Attribute attr = builder.addAttribute("CitizenshipCode", "mise:1.4:user:CitizenshipCode", "USA");
			builder.addAttribute("LawEnforcementIndicator", "mise:1.4:user:LawEnforcementIndicator", "true");
			builder.addAttribute("PrivacyProtectedIndicator", "mise:1.4:user:PrivacyProtectedIndicator", "true");
			
			builder.signUsingPkcs12(assertingPartyID, FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
			Assertion assertion = builder.getAssertion();
			
			//Important to not use SAMLUtils.asPrettyXMLString(object) as it will cause the signature validation to fail  
			HttpResponse response = m_client.post("/MDAUserSessionService/login", null, SAMLUtils.asXMLString(assertion), ContentType.APPLICATION_XML);
			EntityUtils.consumeQuietly(response.getEntity());
			response = m_client.get("/MDAService/positSearch?ulat=3.75&amp;ulng=-2.0&amp;llat=-2.75&amp;llng=3.0&amp;start=2012-06-10T12:10:00&amp;end=2013-012-25T12:30:00", null, "");
			
			//do something with the response
			
		} catch(Exception e) {
			e.printStackTrace();
		}
	}
}
```

<p>Note again that all the configuration parameters should not be hardcoded as
	strings in production code, but should be loaded dynamically from a
	configuration file or configuration database.</p>

<p>Examining the code in detail, the following 4 lines load the SSL
	certificate for the MISE from the filesystem, and initialize the Trust
	Fabric. The Trust Fabric code attempts to load the MISE-hosted Trust
	Fabric from the MISE endpoint first, and can fall back to a local copy if 
	he MISE endpoint cannot be accessed.</p>

```java
FileInputStream isCert = new FileInputStream(FilenameUtils.separatorsToSystem(miseCert));
CertificateFactory certFactory = CertificateFactory.getInstance("X.509");
X509Certificate cert = (X509Certificate) certFactory.generateCertificate(isCert);

TrustFabric.initializeFromURL(trustFabricUrl, trustFabricBackupPath, cert);
```

<p>The next section of the code creates the MISE Rest Client and initializes
	it with the client keystore. The client keystore must contain the private
	key corresponding to the certificate registered for the publishing system
	in the Trust Fabric.</p>

```java
m_client = new RestServiceClient(serverScheme, serverHost, Integer.valueOf(serverPort), serverBasePath);
		
m_client.setClientCert(FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
```

<p>
The next section of the code deals with the creation of the SAML assertion
with the user's attributes. The AssertionBuilder is a utility provided by the
MDA toolkit to aid in creating the SAML. The required attributes are defined
in the attribute specification. Every assertion must provide the
ElectronicIdentityID, FullName, CitizenshipCode, and entitlement attributes.
Note that the example below shows how to explicitly set the expiration time
for the assertion. If not included, the sessions formed by each assertion are
timed-out automatically.</p>

```brush:java
String assertingPartyID = "test.client";
AssertionBuilder builder = new AssertionBuilder(assertingPartyID);
builder.addStandardConditions(Constants.MISE_AUDIENCE_RESTRICTION, 10*60);	// valid for 10 minutes
builder.addAttribute("ElectronicIdentityId", "gfipm:2.0:user:ElectronicIdentityId", "<a href="mailto:testuser@testsystem.gov">testuser@testsystem.gov</a>");
builder.addAttribute("FullName", "gfipm:2.0:user:FullName", "Test T. User");
```

<p>The following code shows how to set the citizenship and entitlement
	information. These attributes are mapped from the consumer system's
	internal user database.</p>

```java
Attribute attr = builder.addAttribute("CitizenshipCode", "mise:1.4:user:CitizenshipCode", "USA");
builder.addAttribute("LawEnforcementIndicator", "mise:1.4:user:LawEnforcementIndicator", "true");
builder.addAttribute("PrivacyProtectedIndicator", "mise:1.4:user:PrivacyProtectedIndicator", "true");
```

<p>As the final step in the process to create the SAML assertion, the
	assertion must be signed using the private key of the consumer system.
	This is the same private key/keystore used to establish the SSL
	connection. Note that this signing operation explicitly includes the
	assertingPartyID, which should match the entity ID of the consumer system
	registered with the Trust Fabric.</p>

```java
builder.signUsingPkcs12(assertingPartyID, FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
Assertion assertion = builder.getAssertion();
```

<p>Once the assertion has been created, the HTTP request for the session and
	the search can be performed. Prior to the actual request, the consuming
	system must establish a session with the MISE with the entitlements for
	the requesting user. This code makes that request using the assertion that
	was just created. The RestClient internally stores the session cookie
	provided back by the MISE to use in future requests.</p>

```java
//Important to not use SAMLUtils.asPrettyXMLString(object) as it will cause the signature validation to fail  
HttpResponse response = m_client.post("/MDAUserSessionService/login", null, SAMLUtils.asXMLString(assertion), ContentType.APPLICATION_XML);
EntityUtils.consumeQuietly(response.getEntity());
```

<p>Finally, once the assertion has been created and the session established,
	the search request can be made. The search request shown requests all
	Position instances in the specified bounding box, published in the
	specified time period. This will return an Atom feed containing summaries
	of all matching records in the MISE. The retrieve operation for each of
	the individual records in the Atom feed is discussed in the next
	section.</p>

```java
response = m_client.get("/MDAService/positSearch?ulat=3.75&amp;ulng=-2.0&amp;llat=-2.75&amp;llng=3.0&amp;start=2012-06-10T12:10:00&amp;end=2013-012-25T12:30:00", null, "");
```

<p>In production, the searching response-handling, and error-handling code
	must be more robust, to deal with the various error codes that might be
	returned by the MISE depending on the interaction with the security
	services and the outcome of the search operation.</p>

<p>Please note that the current base URL for the MISE is
	<code>/services/MDAService/</code>, followed by the URLs described in this
	guide for publish and search/retrieve operations.</p>