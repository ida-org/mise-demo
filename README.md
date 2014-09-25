mise-demo
=========

This Maritime Information Sharing Environment (MISE) GitHub site is an
unofficial 3rd party demo. See the [MISE website](https://mise.mda.gov/)
for the official source material.

## General Description

The MISE defines a low cost, implementable solution for maritime information 
sharing while providing mechanisms to mitigate associated legal and policy 
concerns. As a participant in the MISE, data providers and consumers manage 
and share maritime information through common data definitions and security 
attributes, resulting in an internet accessible, unclassified information 
sharing capability.

## Common Profile

The MISE may be understood in terms of a 
"[common profile](http://pi2.ida.org/common-profile)" whereby a single
reference view is expanded on by technical guidance views, each of which may
have sub-views, either providing further technical guidance or details about
specific implementations:

![MISE Common Profile Overview](www/MISE.png)

The [Security](www/security-services-interfacing.md) and Data Technical 
Guidance views, along with their various subviews, define the exchange data 
formats as well as the security requirements for successful data exchange. For
example, [SAML](https://en.wikipedia.org/wiki/Security_Assertion_Markup_Language)
is to be used to exchange authentication and authorization information, and 
[SSL](https://en.wikipedia.org/wiki/Transport_Layer_Security) is to be used to
provide confidentiality as well as machine-to-machine authentication.

This GitHub project shares some of the implementation-level code. The 
"Implementation View Plumbing" refers to code that implements TG view
requirements like the ones mentioned above. The "Client Sample Code" shows how
to access the [publish service](www/publish-spec.md) (including deleting
position records) and the [search and retrieve services](www/search-retrieve-spec.md).

| Implementation | Implementation View Plumbing | Client Sample Code|
|----------------|------------------------------|-------------------|
| [Java](MDA-clients/java) | source and binary jar files - <code>MDA-clients/java/MDAUtils-1.0-*.jar</code>† | [MDA-clients/java/src/test](MDA-clients/java/src/test) |
| [.Net (C#)](MDA-clients/dotnet) | [MDA-clients/dotnet/MdaToolkit](MDA-clients/dotnet/MdaToolkit) | [MDA-clients/dotnet/ClientTest/ClientTest.cs](MDA-clients/dotnet/ClientTest/ClientTest.cs) |

† The jar files must be accessed by downloading the zip file below, or by
cloning this repository.

## Selected Implementation Instance Views

### Data:Exchange:Position

[Position IEPD V3.2 Master Document](position-3.2.iepd/master-document.docx?raw=true) - 
This IEPD is a set of NIEM 2.1-conformant exchange artifacts. It is conformant
to the NIEM MPD (Model Package Description) Specification, version 1.1, and
uses the file structure recommended by that document.

### Services:Publish:Position

[MISE Publish Specification](www/publish-spec.md) - 
Provides the interface details for publishing, updating, and
deleting recordsets within the MISE. The MISE follows the Representational
State Transfer (REST) style. The MISE defines a URI endpoint for publication,
and information-provider systems send HTTP requests and receive responses to
URI paths beneath this URI endpoint.

[TestPublishClient.java](MDA-clients/java/src/test/TestPublishClient.java) - 
Shows how to utilize the MDA Java client libraries to issue a publish request,
e.g., of a vessel position record.

[TestDeleteClient.java](MDA-clients/java/src/test/TestDeleteClient.java) - 
Shows how to utilize the MDA Java client libraries to issue a DELETE request
against a previously published resource, such as a vessel position record.

[ClientTest.cs](MDA-clients/dotnet/ClientTest/ClientTest.cs) - 
The `publish()` and `delete()` methods show how to utilize the MDA .Net client
libraries to publish and DELETE resources, such as vessel position records.

### Services:Search/Receive:Position

[MISE Search/Retrieve Specification](www/search-retrieve-spec.md) - Describes
the data-consumer facing representational state transfer (REST) architecture
providing search and retrieve (SR) functionality for the MISE. By conforming
to this interface, the MISE provides data consumers with the ability to find
and retrieve the right information at the right time, based on the needs,
rights, and authorities of the user and the organizations requesting the
information. 

[TestSearchClient.java](MDA-clients/java/src/test/TestSearchClient.java) - 
Shows how to utilize the MDA Java client libraries to issue a search request
against shared resources, such as vessel position records.

[TestRetrieveClient.java](MDA-clients/java/src/test/TestRetrieveClient.java) - 
Shows how to utilize the MDA Java client libraries to retrieve shared
resources, such as vessel position records.

[ClientTest.cs](MDA-clients/dotnet/ClientTest/ClientTest.cs) - 
The `search()` and `retrieve()` methods show how to utilize the MDA .Net
client to search for and retrieve shared resources, such as vessel position 
records.

## Downloads

* [GitHub ZIP](archive/master.zip)
* [Official Packages](https://mise.mda.gov/drupal/tools)
