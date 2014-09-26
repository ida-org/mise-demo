<h1 class="with-tabs">Introduction</h1>

<p>Maritime security is a national priority that depends on the ability to
	efficiently, effectively, and appropriately share and safeguard
	information among trusted maritime partners within the Global Maritime
	Community of Interest (GMCOI).  The Maritime Information Sharing
	Environment (MISE) as defined in the National Maritime Architecture Plan
	enables secure, standardized sharing of unclassified maritime information
	among a wide variety of federal, state and local agencies as well as
	international participants. MISE employs NIEM-M exchange models,
	representational state transfer (REST) services for publishing/consuming,
	and attribute-based access control to facilitate information sharing and
	safeguarding with non-provisioned users in a dynamic environment.</p>

<p>The purpose of this implementation guide is to provide practitioners with
	guidance and specific examples for interfacing with MISE.  Specifically,
	it shows how to create messages that conform to the
	<a href="http://niem.gov/">National Information Exchange Model (NIEM)</a>
	Maritime IEPD formats, how to implement security to successfully access
	the environment, and how to interface with the services to publish and
	consume messages from the environment.</p>

<p>For new practitioners it is recommended you start with
	<a href="code-overview.md">Code Overview</a> and proceed in order through
	the implementation guide.</p>

<h3>NIEM-M Exchange Models</h3>

<p>To learn more about using the NIEM-M exchange models, where they can be
	downloaded, and how to produce messages to adhere to these standards
	review the section on <a href="/drupal/node/24">Data Mapping</a>.</p>

<h3>Service Interfaces</h3>

<p>To learn more about the service interfaces to share information via the
	MISE start with the section titled <a href="code-overview.md">Code
	Overview</a>, and then visit the sections on
	<a href="process-flows.md">Process Flows for Security, Publish/Update,
		Delete, Search, and Retrieve</a>, <a href="drupal/node/28">Interfacing
		with the Publication Service</a>,
	<a href="/drupal/node/29">Interfacing with the Delete Service</a>, and
	<a href="/drupal/node/30">Interfacing with the Search and Retrieve
		Service</a>.</p>

<h3>Security Services</h3>

<p>To learn more about interfacing with the MISE security services see the
	sections on <a href="process-flows.md">Process Flows for Security,
	Publish/Update, Delete, Search, and Retrieve</a> and 
	<a href="security-services-interfacing.md">Interfacing with the Security
		Services</a>.</p>