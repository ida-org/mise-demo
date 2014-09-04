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

public class TestRetrieveClient {

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
		
			m_client = new RestServiceClient(serverScheme, serverHost,	Integer.valueOf(serverPort), serverBasePath);
			m_client.setClientCert(FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
			
			//Form the user assertion
			String assertingPartyID = "test.client";
			AssertionBuilder builder = new AssertionBuilder(assertingPartyID);
			builder.addStandardConditions(Constants.MISE_AUDIENCE_RESTRICTION, 10*60);	// valid for 10 minutes
			builder.addAttribute("ElectronicIdentityId", "gfipm:2.0:user:ElectronicIdentityId", "testuser@testsystem.gov");
			builder.addAttribute("FullName", "gfipm:2.0:user:FullName", "Test T. User");
			
			Attribute attr = builder.addAttribute("CitizenshipCode", "mise:1.4:user:CitizenshipCode", "USA");
			builder.addAttribute("LawEnforcementIndicator", "mise:1.4:user:LawEnforcementIndicator", "true");
			builder.addAttribute("PrivacyProtectedIndicator", "mise:1.4:user:PrivacyProtectedIndicator", "true");
			
			builder.signUsingPkcs12(assertingPartyID, FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
			Assertion assertion = builder.getAssertion();
			
			//Important to not use SAMLUtils.asPrettyXMLString(object) as it will cause the signature validation to fail  
			HttpResponse response = m_client.post("/MDAUserSessionService/login", null, SAMLUtils.asXMLString(assertion), ContentType.APPLICATION_XML);

			EntityUtils.consumeQuietly(response.getEntity());
			
			String entityID = "https%3A%2F%2Fmise.agencyone.gov%2F";
			String uuid = "79869883848892520";
			response = m_client.get("/MDAService/retrieve/ian?entityid=" + entityID + "&recordid=" + uuid, null, "");
			
			//do something with the response
			
		} catch(Exception e) {
			e.printStackTrace();
		}
	}
}
