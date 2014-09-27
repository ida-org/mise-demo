<h1 class="with-tabs">Interfacing with the Security Services</h1>

<p>All interactions to publish and consume data within the MISE are secured
	interactions over SSL between trusted systems.  As a prerequisite to
	understanding the security implementation examples in this section, it is
	highly recommend you first read the following documents:</p>

<ol><li><a href="https://mise.mda.gov/drupal/node/77">National MDA Architecture Plan</a> for an overview of the MISE security approach.</li>
<li><a href="security-spec.md">MISE Interface Security Specification</a> for the details of how trusted systems securely connect to the ISI.</li>
<li><a href="https://mise.mda.gov/drupal/node/103">MISE Attribute Specification</a> for an explanation of the common attributes used for entitlement management.</li>
</ol><h1>Step 1: Obtain X.509 Certificates</h1>
<p>Numerous tools and processes are available for creating key pairs and X.509 certificates. The exact process chosen by a trusted system will vary depending on the platform the trusted system implementation is based upon, agency procedures, and the chosen root CA.  In some cases a trusted system may need to generate a keypair and a certificate signing request (CSR) internally using a tool such as OpenSSL  or Java’s keytool, and submit the CSR to a root CA for signing following instructions provided by the root CA.  General instructions are provided at <a href="https://mise.mda.gov/drupal/node/39">generating the private key and public Certificate Signing Request (CSR).</a></p>
<h1>Step 2: Register Trusted System in Trust Fabric</h1>
<p>The MISE trust fabric is an xml document cryptographically signed by the MISE Certificate Authority (CA) that describes each trusted system using the <a href="http://docs.oasis-open.org/security/saml/v2.0/saml-metadata-2.0-os.pdf">SAML 2.0</a> metadata format.  To successfully connect to the ISI, the public key for your X.509 certificate and additional entity metadata for your trusted system must be added to the trust fabric so the ISI can validate requests.  Work with MISE Management to provide the necessary information for registration in the trust fabric.</p>
<p>You will have an entry in the trust fabric for each role for which your system is authorized, i.e. information provider and/or information consumer.</p>
<p>Information you will provide to MISE Management:</p>
<ul><li> <code>entityID</code> - unique identifier for this entity agreed upon between the entity and the MISE Management.   Recommend using FQDN of trusted system endpoint, i.e. <a href="https://mise.agencythree.gov/">https://mise.agencythree.gov/</a>. As an example, the MISE uses <a href="https://services.mda.gov">https://services.mda.gov</a></li>
<li><code>ContactPerson</code> - Point of Contact info, i.e. Company Name, First Name, Last Name, Email, Phone</li>
<li><code>X509Certificate</code> -
<td> Base 64 Encoded Certificate containing trusted system’s public key

<table></table>
Following is an example entry for a provider trusted system:
<pre class="brush: xml">
&lt;md:EntityDescriptor entityID="https://mise.agencythree.gov/"&gt;
	&lt;md:RoleDescriptor protocolSupportEnumeration="urn:oasis:names:tc:SAML:2.0:protocol"
		xsi:type="mise:MISEProviderDescriptorType"&gt;
		&lt;md:KeyDescriptor use="signing"&gt;
			&lt;ds:KeyInfo xmlns:ds="http://www.w3.org/2000/09/xmldsig#"&gt;
				&lt;ds:X509Data&gt;
					&lt;ds:X509Certificate&gt;
						&lt;!-- Base 64 encoded certificate embedded here
						This is the client certificate which the trusted
						system will present during SSL connection handshake.
						The private key matching this certificate will also
						be used by this trusted system for signing SAML
						assertions.
						--&gt;
					&lt;/ds:X509Certificate&gt;
				&lt;/ds:X509Data&gt;
			&lt;/ds:KeyInfo&gt;
		&lt;/md:KeyDescriptor&gt;
	&lt;/md:RoleDescriptor&gt;
	&lt;md:ContactPerson contactType="technical"&gt;
		&lt;md:Company&gt;AgencyThree, Inc.&lt;/md:Company&gt;
		&lt;md:GivenName&gt;Thomas&lt;/md:GivenName&gt;
		&lt;md:SurName&gt;Jones&lt;/md:SurName&gt;
		&lt;md:EmailAddress&gt;tjones@agencythree.gov &lt;/md:EmailAddress&gt;
		&lt;md:TelephoneNumber&gt;703-555-4321&lt;/md:TelephoneNumber&gt;
	&lt;/md:ContactPerson&gt;
&lt;/md:EntityDescriptor&gt;
</pre><p>
Now an example entry for a consumer trusted system.  Notice in the consuming system entry in the trust fabric, the trusted system is assigned the appropriate indicator attributes used to make authorization decisions on queries.</p>
<pre class="brush: xml">
&lt;md:EntityDescriptor entityID="https://mise.agencythree.gov/"&gt;
	&lt;md:Extensions&gt;
		&lt;gfipm:EntityAttribute FriendlyName="COIIndicator"
			Name="mise:1.4:entity:COIIndicator" NameFormat="urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified"&gt;
			&lt;gfipm:EntityAttributeValue xsi:type="xs:string"&gt;True&lt;/gfipm:EntityAttributeValue&gt;
		&lt;/gfipm:EntityAttribute&gt;
		&lt;gfipm:EntityAttribute FriendlyName="LawEnforcementIndicator"
			Name="mise:1.4:entity:LawEnforcementIndicator" NameFormat="urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified"&gt;
			&lt;gfipm:EntityAttributeValue xsi:type="xs:string"&gt;True&lt;/gfipm:EntityAttributeValue&gt;
		&lt;/gfipm:EntityAttribute&gt;
		&lt;gfipm:EntityAttribute FriendlyName="PrivacyProtectedIndicator"
			Name="mise:1.4:entity:PrivacyProtectedIndicator" NameFormat="urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified"&gt;
			&lt;gfipm:EntityAttributeValue xsi:type="xs:string"&gt;True&lt;/gfipm:EntityAttributeValue&gt;
		&lt;/gfipm:EntityAttribute&gt;
		&lt;gfipm:EntityAttribute FriendlyName="OwnerAgencyCountryCode"
			Name="mise:1.4:entity:OwnerAgencyCountryCode" NameFormat="urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified"&gt;
			&lt;gfipm:EntityAttributeValue xsi:type="xs:string"&gt;USA&lt;/gfipm:EntityAttributeValue&gt;
		&lt;/gfipm:EntityAttribute&gt;
	&lt;/md:Extensions&gt;
	&lt;md:RoleDescriptor protocolSupportEnumeration="urn:oasis:names:tc:SAML:2.0:protocol"
		xsi:type="mise:MISEConsumerDescriptorType"&gt;
		&lt;md:KeyDescriptor use="signing"&gt;
			&lt;ds:KeyInfo xmlns:ds="http://www.w3.org/2000/09/xmldsig#"&gt;
				&lt;ds:X509Data&gt;
					&lt;ds:X509Certificate&gt;
						&lt;!-- Base 64 encoded certificate embedded here
                       			 This is the client certificate which the trusted
                        		system will present during SSL connection handshake.
                        		The private key matching this certificate will also
                        		be used by this trusted system for signing SAML
                        		assertions.
                        		--&gt;
					&lt;/ds:X509Certificate&gt;
				&lt;/ds:X509Data&gt;
			&lt;/ds:KeyInfo&gt;
		&lt;/md:KeyDescriptor&gt;
	&lt;/md:RoleDescriptor&gt;
	&lt;md:Organization&gt;
		&lt;md:OrganizationName xml:lang="en"&gt;Agency Three&lt;/md:OrganizationName&gt;
		&lt;md:OrganizationDisplayName xml:lang="en"&gt;Agency
			Three&lt;/md:OrganizationDisplayName&gt;
		&lt;md:OrganizationURL xml:lang="en"&gt;http://www.agencythree.gov/&lt;/md:OrganizationURL&gt;
	&lt;/md:Organization&gt;
	&lt;md:ContactPerson contactType="technical"&gt;
		&lt;md:Company&gt;AgencyThree, Inc.&lt;/md:Company&gt;
		&lt;md:GivenName&gt;Thomas&lt;/md:GivenName&gt;
		&lt;md:SurName&gt;Jones&lt;/md:SurName&gt;
		&lt;md:EmailAddress&gt;tjones@agencythree.gov &lt;/md:EmailAddress&gt;
		&lt;md:TelephoneNumber&gt;703-555-4321&lt;/md:TelephoneNumber&gt;
	&lt;/md:ContactPerson&gt;
&lt;/md:EntityDescriptor&gt;
</pre><h1>Step 3: Download the Trust Fabric Document</h1>

<p>At this point you can verify the trust fabric now has entity metadata so
	your trusted system can authenticate to the ISI by downloading the trust
	fabric.  As discussed in the <a href="security-spec.md">MISE Interface
	Security Specification</a>, the trust fabric contains public keys for all
	trusted systems participating in the MISE, as well as the ISI certificates
	you will need to trust to interact with the MISE services. </p>

<p>The trust fabric endpoint requires HTTPS but does not require a client certificate or any other method of authentication.<br />
Retrieve the trust fabric document by any standard means, including viewing in any browser, at the MISE server at <a href="https://services.mda.gov/miseresources/TrustFabric.xml">https://services.mda.gov/miseresources/TrustFabric.xml</a>.  </p>
<p>The trust fabric document for the MISE test environment is available at:<br /><a href="https://107.23.66.168:9443/miseresources/TrustFabric.xml">https://107.23.66.168:9443/miseresources/TrustFabric.xml</a></p>
<h2>Validate the Trust Fabric Signature Programmatically</h2>
<p>Since the trust fabric document is a SAML metadata file, this sample code is able to leverage the open source OpenSAML project to simplify implementation. Trusted system implementations not written in Java, or which already include other SAML implementations, may also be able to simplify implementation by relying on existing SAML metadata implementations.</p>
<p>The following code snippet shows how the trust fabric document may be loaded into a DOM object so the signing certificate can be parsed and the signature on the document validated.  It relies on the JAR files included in the <a href="https://mise.mda.gov/tools/MDAClient-CodeOnly.zip">MDA Client Toolkit</a>.</p>
<pre class="brush:java">
	    	// read in the trust fabric from a local file location
	    	FileInputStream fis = new FileInputStream("/local/path/TrustFabric.xml");
	    	m_domFactory = DocumentBuilderFactory.newInstance();
	    	m_domFactory.setNamespaceAware(true);
		Element domElement = m_domFactory.newDocumentBuilder().parse(fis).getDocumentElement();
			
		// cryptographic validation of signature
		X509Certificate signedByCert = verifyXMLSignature(domElement);
		System.out.println(String.format("Signature validation %s", signedByCert == null ? "FAILED" : "SUCCEEDED"));
</pre><p>
The following snippet takes the trust fabric as a DOM object and returns the signing certificate if it is valid.</p>
<pre class="brush:java">
	public static X509Certificate verifyXMLSignature(Element target) throws Exception {
		// Validate the signature -- i.e. SAML object is pristine:
		NodeList nl = target.getElementsByTagNameNS(XMLSignature.XMLNS, "Signature");
		if (nl.getLength() == 0)
			return null;

		KeyValueKeySelector kvs = new KeyValueKeySelector();
		DOMValidateContext context = new DOMValidateContext(kvs, nl.item(0));

		// Create a DOM XMLSignatureFactory that will be used to unmarshal the
		// document containing the XMLSignature
		String providerName = System.getProperty("jsr105Provider", "org.jcp.xml.dsig.internal.dom.XMLDSigRI");
		XMLSignatureFactory fac = XMLSignatureFactory.getInstance("DOM", (Provider) Class.forName(providerName).newInstance());

		DOMXMLSignature signature = (DOMXMLSignature) fac.unmarshalXMLSignature(context);
		if (!signature.validate(context))
			return null;

		return kvs.getUsedCertificate();
	}
</pre><h1>Step 4: Implement MISE Security Attributes</h1>
<p>As detailed in the <a href="https://mise.mda.gov/drupal/node/103">MISE Attribute Specification</a>, entitlement management within the MISE relies on the use of a common set of entity, user, and data attributes to make run-time authorization decisions as to whether a trusted system and requesting user are authorized to access a requested information resource.</p>
<p>There are three categories for attributes defined for the National MDA Architecture:</p>
<ol><li>Entity Attributes: Attributes that pertain to a trusted system within the MISE.</li>
<li>User Attributes: Attributes that pertain to a human user.</li>
<li>Data Attributes: Attributes that pertain to data/information.</li>
</ol><p>Currently information is grouped by LE sensitive (LEI), privacy protected (PPI) and the rest of the community (COI).  These groups map to the security indicators, i.e. Law Enforcement Indicator, Privacy Protected Indicator, and COI Indicator, defined in the attribute specification.  There is a one-to-one relationship between the security indicators assigned to information resources (data attributes) and the indicators assigned to information consumer trusted systems (entity attributes) and users (user attributes) to convey their permissions.  In order for trusted systems to access information on behalf of a user, both the trusted system’s and user’s permissions must match the security tags placed on the information.</p>
<h2>Information Provider use of Security Attributes</h2>
<p>Access controls for the information you publish are conveyed by adding security attribute tags on the root element of each message (record) published.  These security attributes applied within the message make up the information access policy that will be used by the ISI to determine which consumer requests can access your information. The following high level steps may be used as a guide to understand how you as the information provider define your information access policy.</p>

<ol>
<strong><li>First you must determine the information you will share.</li>
<p></p></strong>
The types of information that can currently be shared via the ISI include
vessel position reports, vessel of interest/alert information, and vessel
arrival information. Download the corresponding NIEM-M based information
exchange package documentation which defines the message format. Links to the
IEPD artifacts are available at the top of the 
<a href="data-mapping.md"> Data Mapping</a> section.

<p><strong>
<li>Identify the legislative or policy constraints on the information. </li>
<p></p></strong>As the information provider you must work with your policy and/or legal department to identify which elements within a type of message can be shared and which have restrictions.  This will involve an element by element evaluation of the message format defined within the IEPD. </p>
<p>In some cases there may be more value to the community to publish a record with the sensitive information omitted and less restriction on which trusted systems/users can access the information.  Consider for example the US Coast Guard UNCLASSIFIED Notice of Arrival information containing complete crew and passenger information (which is privacy protected). Sharing the Notice of Arrival information without the PII elements would still support being able to ask questions such as “Which vessels are inbound to port of Baltimore?” or “What are the pending notices of arrival for the Jennie Kay?”. To answer questions requiring more sensitive persons information, consumers will still go to the source system, i.e. SANS.</p>
<p><strong>
<li>Specify your information access policy using the MISE security attributes.</li>
<p></p></strong>Currently information is grouped by LE sensitive (LEI), privacy protected (PPI) and the rest of the community (COI).  Additionally you can limit based on country and specify releasability of the information.</p>
<p>The following table highlights the primary attributes used to specify information access policy.  For the full description of all attributes, see the <a href="https://mise.mda.gov/drupal/node/103">MISE Attribute Specification.</a></p>
<table><tr><td> Attribute Name</td>
<td> Possible Values</td>
<td> Description</td>
</tr><tr><td> securityIndicatorText</td>
<td> “LEI” | ”PPI” | “COI”</td>
<td> Indicates the level of access required to access the data. LEI for Law Enforcement sensitive information, PPI for privacy protected information, or COI for the rest of the community.</td>
</tr><tr><td> releasableIndicator</td>
<td> “true” | “false”</td>
<td> Marks data as releasable to the public domain under the restrictions of the associated security indicator.</td>
</tr><tr><td> releasableNationsCode</td>
<td> Space-delimited list of 3-letter country codes, ex. “CAN USA FRA”</td>
<td> Indicates data can only be released to those nations identified by the country codes.  Default value is “USA”.</td>
</tr></table><p>For example to publish a vessel position message that is law enforcement sensitive, not publically releasable, and shareable with only US, these fields will be added to the exchange element of the publish message as depicted in the example below.</p>
<pre class="brush: xml">
&lt;posex:Message xmlns:posex="http://niem.gov/niem/domains/maritime/2.1/position/exchange/3.2"
                mda:securityIndicatorText="LEI" mda:releasableNationsCode="USA" mda:releasableIndicator="false"&gt;
</pre><p>
<strong>
<li>(Optionally) Specify Scope using the MISE Security Attributes.</li>
<p></p></strong></p>
<p>Scope provides tags to associate data with a specific incident or operation. Generally, you would use these tags to relax or override the IAP for normal operations so trusted systems and users are able to access data they would normally be restricted from, but only within the scope of their involvement in the specified event or operation. Scope may be a planned event such as the Olympics or specific response operation in the wake of a specific disaster such as Hurricane Katrina.</p>
<p>The following scope modifiers can be used in conjunction with the three primary attributes:</p>
<table><tr><td> Attribute Name</td>
<td> Possible Values</td>
<td> Description</td>
</tr><tr><td> scopeText</td>
<td> "Superstorm Sandy”</td>
<td> Name of the scope. This will indicate which event or operation within which the scope is in context.</td>
</tr><tr><td> scopeIndicatorText</td>
<td> “LEI” | ”PPI” | “COI”</td>
<td> Minimum indicator required for access within the context of this scope. If this data is normally PPI data, you might want to provide it to all COI consumers within the context of this scope.</td>
</tr></table><p>For example for the same vessel position message that is law enforcement sensitive, not publically releasable, and shareable with only US, you may want it to be accessible to the entire community for disaster release efforts related to Superstorm Sandy as depicted below.</p>
<pre class="brush: xml">
&lt;posex:Message xmlns:posex="http://niem.gov/niem/domains/maritime/2.1/position/exchange/3.2"
                mda:securityIndicatorText="LEI" mda:releasableNationsCode="USA" mda:releasableIndicator="false"
                mda:scopeText="Superstorm Sandy" mda:scopeIndicatorText="COI"&gt;
</pre></ol><h2>Supplying User Attributes for Search and Retrieve</h2>
<h3>Map Local User Privileges to MISE User Attributes</h3>
<p>Use the MISE security attributes as defined in the <a href="https://mise.mda.gov/drupal/node/103">MISE Attribute Specification</a> to assert citizenship and the access level for the user associated with a query.<br />
Citizenship is conveyed using the CitizenshipCode attribute, <code> mise:1.4:user:CitizenshipCode </code>, with a value equal to the ISO 3-letter country code.</p>
<p>The access level is conveyed using one or more attributes defined as indicators.</p>
<table width="100%"><tr><td> Indicator</td>
<td> Attribute Name </td>
<td> Description</td>
</tr><tr><td> Law Enforcement Indicator</td>
<td> mise:1.4:user:LawEnforcementIndicator </td>
<td> User requires and qualifies for access to law enforcement information in accordance with all appropriate statutes and legislation.</td>
</tr><tr><td> Privacy Protected Indicator </td>
<td> mise:1.4:user:PrivacyProtectedIndicator </td>
<td> User requires and qualifies for access to privacy protected information in accordance with all appropriate statutes and legislation.</td>
</tr><tr><td> Community of Interest Indicator </td>
<td> mise:1.4:user:COIIndicator </td>
<td> Minimum access level assigned to user that requires access to information shared by the MISE community.</td>
</tr></table><h1>Forming the SAML User Assertion</h1>
<p>The following code snippet provides an example of building the user assertions and adding them to the context of the request.  The full example is shown in the section on <a href="https://mise.mda.gov/drupal/node/30">Interfacing with the Search Services</a>.</p>
<pre class="brush:java">
//Form the user assertion
String assertingPartyID = "test.client";
AssertionBuilder builder = new AssertionBuilder(assertingPartyID);
builder.addStandardConditions(Constants.MISE_AUDIENCE_RESTRICTION, 10*60);  // valid for 10 minutes
builder.addAttribute("ElectronicIdentityId", "gfipm:2.0:user:ElectronicIdentityId", "<a href="mailto:testuser@testsystem.gov">testuser@testsystem.gov</a>");
builder.addAttribute("FullName", "gfipm:2.0:user:FullName", "Test T. User");

Attribute attr = builder.addAttribute("CitizenshipCode", "mise:1.4:user:CitizenshipCode", "USA");
builder.addAttribute("LawEnforcementIndicator", "mise:1.4:user:LawEnforcementIndicator", "true");
builder.addAttribute("PrivacyProtectedIndicator", "mise:1.4:user:PrivacyProtectedIndicator", "true");
builder.addAttribute("COIIndicator", "mise:1.4:user:COIIndicator", "true");        
builder.signUsingPkcs12(assertingPartyID, FilenameUtils.separatorsToSystem(keystorePath), keystorePass);
Assertion assertion = builder.getAssertion();

HttpResponse response = m_client.post("/MDAUserSessionService/login", null, SAMLUtils.asXMLString(assertion), ContentType.APPLICATION_XML);
</pre><h2>Security Attributes on Summary Records from Search</h2>
<p>The following XML snippet provides an example of the security attributes applied on the records to convey data protection requirements.  </p>
<pre class="brush:xml">
&lt;?xml version="1.0" encoding="UTF-8"?&gt;
&lt;mise-recordset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
 xsi:noNamespaceSchemaLocation="../../../../position-3.2.iepd/XMLschemas/exchange/3.2/mise-recordset.xsd"
 pageElements="250"
 query="https://services.mda.gov/search/noa/?start=2013-08-27T22:20:56&amp;end=2013-08-28T22:22:56"
 nextQuery="https://services.mda.gov/search/noa/?start=2013-08-27T22:20:56&amp;nextTime=2013-08-27T22:20:00:001"&gt;
  &lt;posex:Message xmlns:posex="http://niem.gov/niem/domains/maritime/2.1/position/exchange/3.2"
                  mda:securityIndicatorText="LEI" mda:releasableNationsCode="USA" mda:releasableIndicator="false"&gt;
    ...snip...&lt;/posex:Message&gt;
  &lt;posex:Message xmlns:posex="http://niem.gov/niem/domains/maritime/2.1/position/exchange/3.2"
                  mda:securityIndicatorText="PPI" mda:releasableNationsCode="USA" mda:releasableIndicator="false"&gt;
    ...snip...&lt;/posex:Message&gt;
  [... next 248 records]
&lt;/mise-recordset&gt;
</pre></td></li></ul>