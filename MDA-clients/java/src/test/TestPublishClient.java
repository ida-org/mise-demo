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
		
			m_client = new RestServiceClient(serverScheme, serverHost,	Integer.valueOf(serverPort), serverBasePath);
		
			m_client.setClientCert(FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
			
			String body = null;
			FileUtils.readFileToString(new File("SamplePosition.xml"), body);
			
			HttpResponse response = m_client.put("/MDAService/publish/pos/id", "", body, ContentType.APPLICATION_XML); //perform the request
		} catch(Exception e) {
			e.printStackTrace();
		}
		

	}

}
