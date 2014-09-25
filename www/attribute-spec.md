<h1 class="with-tabs">MISE Attribute Specification</h1>
                                                                                          
<h2>Introduction</h2>

<p>Maritime security is a national priority that depends on the ability to efficiently, effectively, and appropriately share and safeguard information among trusted maritime partners within the Global Maritime Community of Interest (GMCOI).  The Maritime Information Sharing Environment (MISE) as defined in the National Maritime Architecture Plan enables secure, standardized sharing of unclassified maritime information among a wide variety of federal, state and local agencies as well as international participants. MISE employs attribute-based access control and a standardized set of security attributes for information access policy enforcement to facilitate information sharing with non-provisioned users in a dynamic environment.  This security model embraces the philosophy that user accounts are most effectively managed by a user’s parent organization and exploiting existing user account verification, management, and revocation processes already in place. Allowing access using an individual’s existing local user identity and password improves security by eliminating the requirement to create and maintain yet another user identity. The federated model also eliminates the need for changes to user account privileges across multiple systems; an individual need only advise their parent organization of changes in employment status or organizational role, and changes to account privileges at the local level are sufficient to ensure security across the MISE domain.  By federating the identity management to the trusted systems, the cost and burden of managing user accounts is greatly reduced for the MISE thus reducing overall sustainment costs.  A common set of security attributes for entities, users, and data is necessary to consistently share and protect information across the federation of systems in the MISE.</p>
<h3>Purpose</h3>
<p>This specification defines a common set of attributes used within the Maritime Information Sharing Environment (MISE) to communicate information about users and the trusted systems that connect to the MISE on the user’s behalf.<br />
There are three categories for attributes defined for the National MDA Architecture:</p>
<ol><li>Entity Attributes: Attributes that pertain to a trusted system within the MISE.
</li><li>User Attributes: Attributes that pertain to a human user.
</li><li>Data Attributes: Attributes that pertain to data.
</li></ol><p>The following tables summarize the entity, user, and data attributes defined for the MISE. </p>
<table><tr><th>Entity Attributes</th>
</tr><tr><td>Certificate</td>
</tr><tr><td>Entity ID</td>
</tr><tr><td>Entity Name</td>
</tr><tr><td>Entity Abbreviation</td>
</tr><tr><td>Administrative Point of Contact</td>
</tr><tr><td>Technical Point of Contact</td>
</tr><tr><td>Owner Agency Name</td>
</tr><tr><td>Owner Website URI</td>
</tr><tr><td>Owner Agency Country Code</td>
</tr><tr><td>Law Enforcement Indicator</td>
</tr><tr><td>Privacy Protected Indicator</td>
</tr><tr><td>Community of Interest Indicator</td>
</tr></table><table><tr><th>User Attributes</th>
</tr><tr><td>Electronic Identity ID</td>
</tr><tr><td>Full Name</td>
</tr><tr><td>Citizenship Code</td>
</tr><tr><td>Law Enforcement Indicator</td>
</tr><tr><td>Privacy Protected Indicator</td>
</tr><tr><td>Community of Interest Indicator</td>
</tr></table><table><tr><th>Data Attributes</th>
</tr><tr><td>Data Scope</td>
</tr><tr><td>Law Enforcement Indicator</td>
</tr><tr><td>Privacy Protected Indicator</td>
</tr><tr><td>Community of Interest Indicator</td>
</tr><tr><td>Releasable Indicator</td>
</tr><tr><td>Releasable Nations List</td>
</tr></table><p>Entity attributes are used to capture relevant information about the trusted system i.e. SSL certificate, administrative information, and website Uniform Resource Identifier (URI). The Owner Agency Country Code is used in protecting data based on nation. An entity is restricted from accessing information unless it is explicitly coded for release to their country code. The "Indicator" attributes are key to establishing what categories of information the trusted system can access.<br />
User attributes provide detailed information on the individual users of a trusted system, and serve to ensure that they are only able to access that subset of the available information to which they individually require and qualify for access. The user attributes are assigned and managed by the trusted system to which the user belongs. </p>
<p>Data attributes are fundamental to entitlement management and data management processes within the MISE.  The three data attributes defined in this specification as “indicators” support entitlement management within the MISE.  The “scope” attribute provides the flexibility to associate data with a specific incident or operation to provide context for data management.</p>
<h3>Indicators</h3>
<p>Entitlement management within the MISE involves run-time authorization decisions about whether a trusted system and requesting user are authorized to access a requested information resource.  Three primary security indicators are used by data providers to declare information access policy to set access restrictions on information they share with MISE.  These information access policies are the basis for entitlement decisions within the MISE.  A fourth indicator, the “Releasable Indicator”, is a Boolean used in conjunction with the three primary Security Indicators to mark data as releasable to the public domain under the restrictions of the associated indicator. The three primary security indicators are listed in the following table in order of decreasing degree of restriction.</p>
<table style="border: 1px solid black"><tr><th>Indicator</th>
<th>Abbreviation</th>
</tr><tr><td>Law Enforcement</td>
<td>LEI</td>
</tr><tr><td>Privacy Protected</td>
<td>PPI</td>
</tr><tr><td>Community of Interest </td>
<td>COI</td>
</tr></table><p>The following describes the three primary attributes and explains their intent as well as how the attribute might be used:</p>
<ul><li>Law Enforcement Indicator: This Indicator is the most restrictive, and is used to code data for release to entities such as federal, state and local law enforcement agencies. Only Entities assigned the Law Enforcement Indicator and the U.S. owner agency code can access this data. Within an Entity with the Law Enforcement Indicator, only Users who are U.S. Citizens and assigned the Law Enforcement Indicator by that Entity will be able to access the information.
</li><li>Privacy Protected Indicator: Personally Identifiable Information (PII) is information that can be used to uniquely identify, contact, or locate a single person or can be used with other sources to uniquely identify a single individual. State and Federal legislation, as well as the policy of many agencies such as DOD and DHS, impose strict limitations on the release of PII. The Privacy Protected Indicator is designed to restrict access to and distribution of PII across the MISE Domain. Application of the Privacy Protected Indicator means the information will only be released to Entities with the requisite country code, and then only to Individuals with that attribute and the requisite country code assigned to their account.
</li><li>Community of Interest Indicator: The community of interest indicator is the minimum access level assigned to trusted systems participating in the environment. By default, all trusted systems are granted the Community of Interest Indicator.<br />
The following two modifiers can be used in conjunction with any of the three primary security indicators:
</li><li>Releasable Indicator (default False): This attribute is a Boolean value to mark data as releasable to the public domain under the restrictions of the associated indicator.   This attribute is used to indicate data can be released in accordance with the consuming systems policies, business processes and as required to support their mission.
</li><li>Releasable Nations List (default USA): This provides a comma-separated list of nations who can access the associated data. This is a space-separated list of three-letter country codes, for example (CAN USA FRA).  This attribute is used to indicate data can be released to only those nations identified by country codes.
</li></ul><p>Scope provides tags to associate data with a specific incident or operation. These tags are used by data providers to relax or override their IAP for normal operations so trusted systems and users are able to access data they would normally be restricted from, but only within the scope of their involvement in the specified event or operation. Scope may be a planned event such as the Olympics or specific response operation in the wake of a specific disaster such as Hurricane Katrina. Each scope is named, and provides three modifiers to the data indicators discussed in the previous section:</p>
<ul><li>Scope: Name of the scope. This will indicate which event or operation within which the scope is in context, e.g. SuperstormSandy.
</li><li>ScopeDataIndicator: Minimum indicator required for access within the context of this scope. If this data is normally PPI data, the data provider might want to provide it to all COI consumers within the context of this scope.
</li><li>ScopeReleasable: This attribute is a Boolean value to mark data as releasable to the public domain under the restrictions of the associated indicator.   This attribute is used to indicate data is can be released in accordance with the consuming systems policies, business processes and as required to support their mission, within the context of the associated scope.
</li><li>ScopeReleaseableNations: This provides a comma-separated list of nations who can access the associated data within the context of the associated scope. This is a space-separated list of three-letter country codes, for example (CAN USA FRA).
</li></ul><p>As an example, the following table shows in the case of a trusted system providing position reports, the policy during routine operations is PPI required for access because the data contains US Persons information.  However, in support of disaster relief operations during Hurricane Sandy the data provider may decide they have a need to share with any trusted system within the context of Humanitarian Aid and Disaster Relief (HADR) operations so COI can be set as minimum requirement for access.  The data provider can modify the indicator, releasability, and releasable nations in the context of the Hurricane Sandy scope only. </p>
<table style="border: 1px black solid"><tr><th>Use Case (Positions) </th>
<th>Routine Operations</th>
<th>Indicator/Scope</th>
</tr><tr><td>HADR (US Persons)</td>
<td>PPI</td>
<td>COI / SuperstormSandy</td>
</tr></table><p>The four attributes described for scope are not new attributes, but simply modifiers on the existing data attributes defined in section 4 of this specification. </p>
<h2>Entity Attributes</h2>
<p>The following entity attributes are associated with a trusted system.</p>
<table><tr><th colspan="2">Administrative Point of Contact – Email Address Text
</th></tr><tr><td>Name
</td><td>AdministrativePointofContactEmailAddressText
</td></tr><tr><td>Description
</td><td>The electronic mailing address by which to contact the person who is the administrative point of contact
for the entity within the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification 
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:AdministrativePointofContactEmailAddressText
</td></tr><tr><td>Example Values
</td><td>“<a href="mailto:john.doe@company.com">john.doe@company.com</a>”
</td></tr><tr><th colspan="2">Administrative Point of Contact – Fax Number
</th></tr><tr><td>Name
</td><td>AdministrativePointofContactFaxNumber
</td></tr><tr><td>Description
</td><td>The telephone number for a facsimile device by which to contact the person who is the administrative point of contact for the entity within the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:AdministrativePointofContactFaxNumber
</td></tr><tr><td>Example Values
</td><td>“(555) 555-5555” 
</td></tr><tr><th colspan="2">Administrative Point of Contact – Full Name
</th></tr><tr><td>Name
</td><td>AdministrativePointofContactFullName
</td></tr><tr><td>Description
</td><td>The complete name of the person who is the administrative point of contact for the entity within the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:AdministrativePointofContactFullName
</td></tr><tr><td>Example Values
</td><td>“John Doe”
</td></tr><tr><th colspan="2">Administrative Point of Contact – Telephone Number
</th></tr><tr><td>Name
</td><td>AdministrativePointofContactTelephoneNumber
</td></tr><tr><td>Description
</td><td>The telephone number for a telecommunications device by which to contact the person who is the administrative point of contact for the entity within the organization or agency by which the entity is 
owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:AdministrativePointofContactTelephoneNumber
</td></tr><tr><td>Example Values
</td><td>“(555) 555-5555”
</td></tr><tr><th colspan="2">Certificate
</th></tr><tr><td>Name
</td><td>Certificate
</td></tr><tr><td>Description
</td><td>An electronic certificate used by the entity as a cryptographic trust anchor within a federation for the purposes of digital signatures and/or encryption. The certificate is represented in X.509 v3, base-64 encoded format.
</td></tr><tr><td>Data Type
</td><td>Base-64 Binary
</td></tr><tr><td>Typical Usage
</td><td>Authentication, Registration, Audit Logging
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:Certificate
</td></tr><tr><td>Example Values
</td><td>MIICJzCCAZCgAwIBAgIBGDANBgkqhkiG9w0BAQUFADBAMQswCQYDVQQGEwJVUzEMMAoGA1UE<br />
ChMDSVNDMSMwIQYDVQQDExpJU0MgQ0RLIFNhbXBsZSBDZXJ0aWZpY2F0ZTAeFw0wMzA3MT<br />
cwMDAwMDBaFw0wNDA3MTcwMDAwMDBaMEAxCzAJBgNVBAYTAlVTMQwwCgYDVQQKEwNJ<br />
U0MxIzAhBgNVBAMTGklTQyBDREsgU2FtcGxlIENlcnRpZmljYXRlMIGfMA0GCSqGSIb3DQEBAQU<br />
AA4GNADCBiQKBgQC9GQTkukn+153rATR8dh2Hm8ixF7f7Y7bI0VFJnJAQCKqta4/IhFwQIK5F2Gn<br />
8j9tITBiXCF7F6XSvaF8bivN10zR0pvI11NflEm2kwh7Yw0jZJB17Y3FHg183qYegmm/UwqX5zKUa4<br />
xw+cE8XSEqUuwjg0roBMGhAMzFEihHzLwIDAQABozEwLzAMBgNVHRMBAf8EAjAAMA4GA1UdD<br />
wEB/wQEAwIAYDAPBgNVHQ4ECHJzYS0xMDI0MA0GCSqGSIb3DQEBBQUAA4GBALWGxxo55S<br />
cpLfECnqEUixFwrzftQGD2ISda7EWp/d7k23fOXgHC7Za18OpvlBUZ3sC2Fg4finfRHd2J4mXONk5<br />
OEdjhJILd58GErcCECg4J2uJPz77/zk+giiXldQEPtG+YOaAbZC2SFbdfyYDKiSPhgzdy0/b4cElf4+VzegRM
</td></tr><tr><th colspan="2">Community of Interest Indicator
</th></tr><tr><td>Name
</td><td>COIIndicator
</td></tr><tr><td>Description
</td><td>True if the entity is allowed access to COI data, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:entity:COIIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Entity Abbreviation
</th></tr><tr><td>Name
</td><td>EntityAbbreviation
</td></tr><tr><td>Description
</td><td>An abbreviation or acronym for the name of a trusted system.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:EntityAbbreviation
</td></tr><tr><td>Example Values
</td><td>“MAGNET,” “MSSIS”
</td></tr><tr><th colspan="2">Entity ID
</th></tr><tr><td>Name
</td><td>EntityID
</td></tr><tr><td>Description
</td><td>The unique identifier by which the entity is denoted.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration, Audit Logging
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:EntityId
</td></tr><tr><td>Example Values
</td><td>“MSSIS:123,” ”MISE:TIB:MAGNET”
</td></tr><tr><th colspan="2">Entity Name
</th></tr><tr><td>Name
</td><td>EntityName
</td></tr><tr><td>Description
</td><td>The name of an entity.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:EntityName
</td></tr><tr><td>Example Values
</td><td>“Maritime Analysis Global Network,” “Maritime Safety and Security Information System”
</td></tr><tr><th colspan="2">Law Enforcement Indicator
</th></tr><tr><td>Name
</td><td>LawEnforcementIndicator
</td></tr><tr><td>Description
</td><td>True if the entity is allowed access to law enforcement protected data, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:entity:LawEnforcementIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Owner Agency Country Code
</th></tr><tr><td>Name
</td><td>OwnerAgencyCountryCode
</td></tr><tr><td>Description
</td><td>The country of the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Country Code ISO 3166-1
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:entity:OwnerAgencyCountryCode
</td></tr><tr><td>Example Values
</td><td>”USA,” “GBR,” “FRA”
</td></tr><tr><th colspan="2">Owner Agency Name
</th></tr><tr><td>Name
</td><td>OwnerAgencyName
</td></tr><tr><td>Description
</td><td>The name of the organization or agency by which the trusted system is owned.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:OwnerAgencyName
</td></tr><tr><td>Example Values
</td><td>”NORAD-USNORTHCOM”
</td></tr><tr><th colspan="2">Owner Agency Website URI
</th></tr><tr><td>Name
</td><td>OwnerAgencyWebSiteURI
</td></tr><tr><td>Description
</td><td>The Internet address or website of the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:OwnerAgencyWebSiteURI
</td></tr><tr><td>Example Values
</td><td>“<a href="http://website.company.com">http://website.company.com</a>”
</td></tr><tr><th colspan="2">Privacy Protected Indicator
</th></tr><tr><td>Name
</td><td>PrivacyProtectedIndicator
</td></tr><tr><td>Description
</td><td>True if the entity is allowed access to privacy-protected data, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:entity:PrivacyProtectedIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Technical Point of Contact – Email Address Text
</th></tr><tr><td>Name
</td><td>TechnicalPointofContactEmailAddressText
</td></tr><tr><td>Description
</td><td>The electronic mailing address by which to contact the person who is the technical or engineering point of contact for the entity within the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:TechnicalPointofContactEmailAddressText
</td></tr><tr><td>Example Values
</td><td>“<a href="mailto:john.doe@company.com">john.doe@company.com</a>”
</td></tr><tr><th colspan="2">Technical Point of Contact – Fax Number
</th></tr><tr><td>Name
</td><td>TechnicalPointofContactFaxNumber
</td></tr><tr><td>Description
</td><td>The telephone number for a facsimile device by which to contact the person who is the technical or engineering 
point of contact for the entity within the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:TechnicalPointofContactFaxNumber
</td></tr><tr><td>Example Values
</td><td>“(555) 555-5555”
</td></tr><tr><th colspan="2">Technical Point of Contact – Full Name
</th></tr><tr><td>Name
</td><td>TechnicalPointofContactFullName
</td></tr><tr><td>Description
</td><td>The complete name of the person who is the technical or engineering point of contact for the 
entity within the organization or agency by which the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:TechnicalPointofContactFullName
</td></tr><tr><td>Example Values
</td><td>“John Doe”
</td></tr><tr><th colspan="2">Technical Point of Contact – Telephone Number
</th></tr><tr><td>Name
</td><td>TechnicalPointofContactTelephoneNumber
</td></tr><tr><td>Description
</td><td>The telephone number for a telecommunication device by which to contact the person who is the 
technical or engineering point of contact for the entity within the organization or agency by which 
the entity is owned and operated.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Registration
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:entity:TechnicalPointofContactTelephoneNumber
</td></tr><tr><td>Example Values
</td><td>“(555) 555-5555”
</td></tr></table><h2>User Attributes</h2>
<p>The following user attributes are associated with an end user.</p>
<table><tr><th colspan="2">Citizenship Code
</th></tr><tr><td>Name
</td><td>CitizenshipCode
</td></tr><tr><td>Description
</td><td>The country that has assigned rights, duties, and privileges to the user because of the birth 
or naturalization of the user in that country.
</td></tr><tr><td>Data Type
</td><td>ISO 3166-1 Alpha-3 Country Code
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:user:CitizenshipCode
</td></tr><tr><td>Example Values
</td><td>”USA,” “GBR,” “FRA”
</td></tr><tr><th colspan="2">Community of Interest Indicator
</th></tr><tr><td>Name
</td><td>COIIndicator
</td></tr><tr><td>Description
</td><td>True if the user is allowed access to COI data, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:user:COIIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Electronic Identity Id
</th></tr><tr><td>Name
</td><td>ElectronicIdentityId
</td></tr><tr><td>Description
</td><td>The unique identifier that is associated with the electronic identity for the user within the user’s identity provider’s user base.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Audit Logging
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:user:ElectronicIdentityId
</td></tr><tr><td>Example Values
</td><td>“DOE.JOHN.A.2370295257”
</td></tr><tr><th colspan="2">Full Name
</th></tr><tr><td>Name
</td><td>FullName
</td></tr><tr><td>Description
</td><td>The complete name of the user.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Audit Logging
</td></tr><tr><td>References
</td><td>GFIPM 2.0 Metadata Specification
</td></tr><tr><td>Formal Name
</td><td>gfipm:2.0:user:FullName
</td></tr><tr><td>Example Values
</td><td>“John Doe,” “Jim Q. Public”
</td></tr><tr><th colspan="2">Law Enforcement Indicator
</th></tr><tr><td>Name
</td><td>LawEnforcementIndicator
</td></tr><tr><td>Description
</td><td>True if the user is allowed access to law enforcement protected data, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:user:LawEnforcementIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Privacy Protected Indicator
</th></tr><tr><td>Name
</td><td>PrivacyProtectedIndicator
</td></tr><tr><td>Description
</td><td>True if the user is allowed access to privacy-protected data, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:user:PrivacyProtectedIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”<br /></td></tr></table><p> </p>
<h2>Data Attributes</h2>
<p>The following data attributes are associated with information exchanged within the MISE.</p>
<table><tr><th colspan="2">Community of Interest Indicator
</th></tr><tr><td>Name
</td><td>COIIndicator
</td></tr><tr><td>Description
</td><td>True if the data is COI protected, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:data:CommunityOfInterestIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Data Scope
</th></tr><tr><td>Name
</td><td>Scope
</td></tr><tr><td>Description
</td><td>An indicator which denotes a scope, situation, or event of interest for which data is deemed relevant and/or accessible.
</td></tr><tr><td>Data Type
</td><td>Text
</td></tr><tr><td>Typical Usage
</td><td>Authorization
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:data:Scope
</td></tr><tr><td>Example Values
</td><td>“HurricaneKatrina,” “Baltimore1812Exercise,” “OperationTomadachi”
</td></tr><tr><th colspan="2">Law Enforcement Indicator
</th></tr><tr><td>Name
</td><td>LawEnforcementIndicator
</td></tr><tr><td>Description
</td><td>True if the data is law enforcement-protected, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:data:LawEnforcementIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Privacy Protected Indicator
</th></tr><tr><td>Name
</td><td>PrivacyProtectedIndicator
</td></tr><tr><td>Description
</td><td>True if the data is privacy-protected, false otherwise.
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:data:PrivacyProtectedIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Releasable Indicator
</th></tr><tr><td>Name
</td><td>Releasable Indicator
</td></tr><tr><td>Description
</td><td>True if the data is publicly releasable
</td></tr><tr><td>Data Type
</td><td>Boolean
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:data:ReleasableIndicator
</td></tr><tr><td>Example Values
</td><td>“True,” “False”
</td></tr><tr><th colspan="2">Releasable Nations
</th></tr><tr><td>Name
</td><td>Releasable Nations Code List
</td></tr><tr><td>Description
</td><td>A space separated list of countries with assigned rights, duties, and privileges to access the data. 
</td></tr><tr><td>Data Type
</td><td>ISO 3166-1 Alpha-3 Country Code
</td></tr><tr><td>Typical Usage
</td><td>Authorization, Audit Logging
</td></tr><tr><td>References
</td><td></td>
</tr><tr><td>Formal Name
</td><td>mise:1.4:data:ReleasableNationsCodeList
</td></tr><tr><td>Example Values
</td><td>”USA GBR FRA”<br /></td></tr></table>
