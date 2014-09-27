<h1 class="with-tabs">Code Overview</h1>

<p>The MISE implementation team provides a pair of client toolkits for
  interface to the MISE REST services for Publish, Update, Delete, Search, and
  Retrieve, one in Java, one in .NET. Both can be found on the
  <a href="/drupal/tools">tools</a> page. All operations are simply REST
  operations to the correct endpoint. <a href="process-flows.md">Process Flows
  for Security, Publish/Update, Delete, Search, and Retrieve</a> are covered
  in the next section.</p>

<p>The following examples use the Java client toolkit, which is primarily
  designed to make interfacing with the security services easier. The client
  toolkit is implemented in accordance with the following specifications. More
  information about each of the specifications can be found in the links
  below.</p>

<ul><li><a href="attribute-spec.md">MISE Attribute Specification</a>
</li><li><a href="security-spec.md">MISE Interface Security Specification</a>
</li><li><a href="publish-spec.md">MISE Publish Interface Specification</a>
</li><li><a href="search-retrieve-spec.md">MISE Search/Retrieve Interface
Specification</a>
</li></ul>

<p>The client toolkit can be downloaded from the MDA Architecture on the
<a href="/drupal/tools">tools</a> page. The tools contain a simple Java
project that demonstrates how to connect and make a GET request against the
ISI services. Two JAR files are included. The first is the MDAUtils JAR, which
contains the MDA Architecture security implementation, the client REST
toolkit, and all the necessary dependencies. Additionally, the project also
requires the included commons-io JAR, which provides file handling utilities
for reading and writing files.</p>

<p>The Java client toolkit is used in all of the following examples:</p>

<ol><li><a href="security-services-interfacing.md">Interfacing with the
  Security Services</a>
</li><li><a href="/drupal/node/28">Interfacing with the Publication
Service</a>
</li><li><a href="/drupal/node/29">Interfacing with the Delete Service</a>
</li><li><a href="/drupal/node/30">Interfacing with the Search and Retrieve
Service</a>
</li></ol>

<p>Please note that the current base URL for the MISE is 
  /services/MDAService/, followed by publish, search, or retrieve, as
  described in this guide.</p>

<h2>Code-Only Java Toolkit</h2>

<p>The toolkit discussed above is a JAR file that includes all the necessary
  dependencies for making connections to the MISE, including things like the
  Servlet API and OpenSAML. However, if the client application is going to run
  in Tomcat or a similar container, or the developer wants to directly manage
  the dependencies, the developer will need to remove any overlapping
  dependencies from the JAR. This is required for Tomcat and other web
  containers, as WAR applications deployed in the container cannot contain a
  Servlet API that overrides the base Tomcat API. Other containers may have
  similar dependencies.</p>

<h2>.NET Toolkit</h2>

<p>See the <a href="/drupal/dotnet">.NET Client Toolkit</a> page for .NET
  examples.</p>