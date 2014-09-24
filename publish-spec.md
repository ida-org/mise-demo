<html>
<head>
<title>MISE Publish Specification</title>
</head>
<body>
<h1>MISE Publish Specification</h1>
<h2>Introduction</h2>
<p>This page provides the interface details for publishing, updating, and deleting recordsets within the MISE.  The MISE follows the Representational State Transfer (REST) style. The MISE defines a URI endpoint for publication, and information-provider systems send HTTP requests and receive responses to URI paths beneath this URI endpoint.   </p>
<p>Each of these operations are summarized in the following table.  In each case a record published to the MISE is uniquely identified by the combination of RecordID and EntityID. Therefore, each RecordID must be unique on an information provider's system.  </p>
<table cellpadding="10"><tr><th>Operation</th>
<th>URI</th>
<th>Description</th>
</tr><td width="20%"><a href="#version_resource">Version Resource</a></td>
<td width="30%"><code>GET publish/version</code></td>
<td>Returns the version resource used to confirm the MISE interface version matches the version expected by the information provider implementation.</td>

<tr><td width="20%"><a href="#publish">Publish</a></td>
<td><code>PUT publish/&lt;iepd&gt;</code></td>
<td>Publishes set of XML records to the MISE.</td>
</tr><tr><td width="20%"><a href="#update">Update</a></td>
<td><code>PUT publish/&lt;iepd&gt;</code></td>
<td>Updates the existing record by replacing it entirely in the MISE.  Records are identified based on the combination of RecordID and EntityID.</td>
</tr><tr><td width="20%"><a href="#posUpdate">Partial Update<br /><b>POS only</b></a></td>
<td><code>PUT update/pos/?=</code></td>
<td><strong>Only available for the Vessel Position (POS) IEPD.</strong>  Updates the &lt;mda:Position&gt; elements in the specified record. Records are identified based on the combination of RecordID and EntityID.</td>
</tr><tr><td width="20%"><a href="#delete">Delete</a></td>
<td><code>DELETE publish/&lt;iepd&gt;/&lt;RecordID&gt;</code></td>
<td>Deletes the record uniquely identified by the RecordID and EntityID.</td>
</tr></table><p>All URIs are defined relative to BaseURI, which represents the HTTP endpoint of the publication interface at the MISE. The physical URI will be provided when the trusted system is integrated with the MISE. For example only, this specification uses https://services.mda.gov/ as the base URI. </p>
<p>All messages are authenticated and secured in the manner described in the <a href="https://mise.mda.gov/drupal/node/104">MISE Interface Security Specification</a>. For publication, the Trusted System Authentication portion of the interface security specification applies, but the User Attribute Conveyance portion does not apply since publication is not done on behalf of any individual user. Each published data set must have an associated Information Access Policy (IAP). For more information on the IAP, see the <a href="https://mise.mda.gov/drupal/node/103">MISE Attribute Specification</a> and the <a href="https://mise.mda.gov/drupal/node/42">NIEM Maritime Information Exchange Package Documents (IEPD)</a>. The IAP information is carried via attributes in the XML documents published to the MISE.</p>
<h2>Message Flow Patterns</h2>
<p>This section describes the sequence of HTTP request/response messages that is expected to occur during normal operation. These processes allow an information provider to keep the MISE up to date.</p>
<h3>Initial Full Publication</h3>
<p>Before publishing a data set, an information provider will work with MISE management to configure the trusted system and data set at the MISE. Once these steps are complete, the information provider should do an initial full publication of the data set to the MISE, meaning all records that the information provider wishes to share should be sent to the MISE cache using the batch publish interface. The first step is to HTTP GET the version resource. If the version resource cannot be read, or if the version number is not one that the information provider is implemented to support, no further interaction with the interface should be taken. After that, the information provider should HTTP PUT the records in batches of up to 250. During this process, if the information provider fails to connect to the MISE, or if the MISE returns a status code indicating a server-side error (5xx), the information provider should periodically retry the PUT until it succeeds and contact the MISE helpdesk if errors persist. If the information provider’s back-end data store changes during the initial full publication, the provider system must track that and ensure that all shareable records have been successfully PUT to the MISE before the initial full publication process is considered complete. Note that since the publication interface follows HTTP RESTful semantics, no harm is done if the information provider PUTs the same record more than once. The HTTP terminology for this characteristic is idempotence. Therefore, during any kind of error-recovery scenario, the provider is free to PUT the record if there is any question whether a previous PUT succeeded. </p>
<h3>Ongoing Updating</h3>
<p>After the initial full publication is complete, the information provider should update the MISE whenever any changes occur to the set of shareable records. As with initial full publication, the information provider should periodically HTTP GET the version resource to confirm that the version of the publication interface is compatible. This does not necessarily need to be done before each record update, but it should be checked frequently—at least daily. The version resource is defined to support If-Modified-Since to make the check highly efficient. Whenever any shared record is added or changed in the provider’s back-end data store, the full record should be sent to the MISE using HTTP PUT, via the batch publish interface. Whenever any shared record is deleted from the provider’s back-end data store, the provider should use HTTP DELETE to delete the record from the MISE. If the information provider fails to connect to the MISE, or if the MISE returns a status code indicating a server-side error (5xx), the information provider should periodically retry the PUT or DELETE until it succeeds and contact the MISE help desk if errors persist. Note that the HTTP DELETE method is also idempotent, so the provider is free to DELETE the record if there is any question whether a previous DELETE succeeded.</p>
<h2><a id="version_resource">Resource Reference</a></h2>
<p>This section presents details of resources defined as part of the publication interface and aspects of HTTP protocol usage that are important to interoperability.</p>
<h3>Metadata</h3>
<h4>Interface Version</h4>
<p>Before interacting with the MISE publication interface, an information provider should GET the version resource to confirm that the MISE interface version matches what is expected by the information provider implementation. The table below presents details of the HTTP GET request an information provider uses to retrieve the version resource and the possible responses, which may be received back from the MISE.</p>
<table><tr><th colspan="3">Version Resource</th>
</tr><tr><th width="20%">URI</th>
<td width="30%">BaseURI/version</td>
<td>Example: https://services.mda.gov/publish/version</td>
</tr><tr><th>Method</th>
<td>GET</td>
<td></td>
</tr><tr><th rowspan="2">Request Headers</th>
<td>Authorization</td>
<td>As described in the <a href="https://mise.mda.gov/drupal/node/104">MISE Interface Security Specification.</a></td>
</tr><tr><td>If-Modified-Since</td>
<td>Information provider system may send this header if it has previously read and cached this resource.</td>
</tr><tr><th>Request Content Type</th>
<td></td>
<td>Empty</td>
</tr><tr><th rowspan="3">Status Codes</th>
<td>200 (OK)</td>
<td>Successful. Response content as described below.</td>
</tr><tr><td>304 (Not Modified)</td>
<td>The version resource has not been modified since the time specified in the If-Modified-Since request header.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header or userid/password does not match any trusted system.</td>
</tr><tr><th width="20%">Response Headers</th>
<td>Last-Modified</td>
<td></td>
</tr><tr><th>Response Content Type</th>
<td>application/xml; charset=UTF-8</td>
<td>Response content only returned if status code is 200.</td>
</tr><tr><th>Response Content	</th>
<td colspan="2">
<pre class="brush: xml">
&lt;?xml version="1.0" encoding="UTF-8"?&gt;
&lt;MISEInterface&gt;
    &lt;Name&gt;Publication&lt;/Name&gt;
    &lt;MajorVersion&gt;1&lt;/MajorVersion&gt;
    &lt;MinorVersion&gt;0&lt;/MinorVersion&gt;
&lt;/MISEInterface&gt;
</pre></td>
</tr></table><h2><a id="publish">General Batch Publish Interface</a></h2>
<p>Batch publish operations to the MISE are PUT operations against the correct batch publish URL. The information provider may publish 1 or more records in the same recordset, up to the recordset maximum.  The batch publish interface allows the information provider to publish and update records in sets of up to 250 at a time. <i>It is strongly recommended that the information provider validate the XML published to the MISE. The MISE will perform validation and return errors, but for fastest publication, validate before the publish operation.</i></p>
<table><tr><th colspan="3">Batch Publish Service</th>
</tr><tr><th width="20%">URI</th>
<td width="30%">BaseURI/IEPDName/</td>
<td>Example: <a href="">https://services.mda.gov/publish/loa/</a></td>
</tr><tr><th>Method</th>
<td>PUT</td>
<td></td>
	</tr><tr><th>Request Headers</th>
<td>Authorization</td>
<td>
			As described in the <a href="https://mise.mda.gov/drupal/node/104">MISE Interface Security Specification.</a>
		</td>
</tr><tr><th>Request Content</th>
<td>Set of XML records</td>
<td>See example below.</td>
</tr><tr><th rowspan="5">Status Codes</th>
<td>201 (Success)</td>
<td>Successful Publish.</td>
</tr><tr><td>400 (Bad Request)</td>
<td>Request was not formatted correctly.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header.</td>
</tr><tr><td>403 (Forbidden)</td>
<td>Invalid authentication information.</td>
</tr><tr><td>500 (Error)</td>
<td>Error in publish. The body content will provide error details.</td>
</tr><tr><th width="20%">Response Headers</th>
<td>Empty</td>
<td></td>
	</tr><tr><th>Response Content Type</th>
<td>Empty</td>
<td></td>
	</tr></table><p>Listing 2 shows the XML structure that defines a recordset of records to be published to the MISE. The only attribute in the mise-recordset element is the pageElements attribute, which indicates to the MISE how many individual records are contained in this publish operation. This example is up to a set of 250 Position instances. </p>
<p><a id="listing2">Example Batch Publish Content</a></p>
<pre class="brush: xml">
&lt;?xml version="1.0" encoding="UTF-8"?&gt;
&lt;mise-recordset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	xsi:noNamespaceSchemaLocation="../../../../position-3.2.iepd/XMLschemas/exchange/3.2/mise-recordset.xsd"
	pageElements="250"&gt;
	&lt;posex:Message&gt; ...snip...&lt;/posex:Message&gt;
	&lt;posex:Message&gt; ...snip...&lt;/posex:Message&gt;
	[... next 248 records]
&lt;/mise-recordset&gt;
</pre><p>
To publish a single record, simply supply a recordset with only the one record. Each individual record in the recordset carries its own security indicator attributes, as defined in the MISE Attribute Specification. The MISE will publish new records and update existing records, based on the EntityID of the publishing system from the trust fabric and the RecordIDURI element in each individual record. As with all records on the MISE, the EntityID/RecordID pair is used to uniquely identify each record.</p>
<h2><a id="update">Updates</a></h2>
<p>All REST PUT publish operations against the MISE for record publication will either create the new record or update the record if it already exists. Records are identified based on the RecordID/EntityID pair. </p>
<h3><a id="posUpdate">Position Update</a></h3>
<p>The position update operation in the publish interface is designed to work only with NIEM-M position instances, allowing information providers to update an existing <a href="https://mise.mda.gov/drupal/node/42">NIEM-M Position (POS)</a> instance message with new &lt;mda:Position&gt; elements.  For instance, an information provider might publish new positions in the track for a vessel every 5 minutes. The position publish interface allows the information provider to constantly update the track for a specific vessel, and when a query is performed via the <a href="">search interface</a>, retrieve part or all of that track, as a NIEM-M Position XML instance.</p>
<p>Initial publish of a document to the MISE will performed in accordance with the general batch <a href="#publish">Publish</a> interface. When a document is published, it is available via a retrieve URL for the just-published document that will always point to that document as long as it is available on the MISE. For example if a POS record is published to the MISE, that URL might be:</p>
<p><a href="">https://services.mda.gov/retrieve/pos/?entityid=agencyone.gov&amp;recordid=12345678</a></p>
<p>This URL communicates the two critical parameters required for updating the positions on that record, the entity ID and the record ID. Both are required for all &lt;mda:Position&gt; element updates. The entity ID and record ID in combination will uniquely identify any record on the MISE. </p>
<table><tr><th colspan="3">Position Update</th>
</tr><tr><th width="20%">URI</th>
<td width="30%"><a href="">https://services.mda.gov/update/pos/?=</a></td>
<td>Example: <a href="">https://services.mda.gov/update/pos/?entityid=agencyone.gov&amp;recordid=12345678</a></td>
</tr><tr><th>Method</th>
<td>PUT</td>
<td></td>
	</tr><tr><th>Request Headers</th>
<td>Authorization</td>
<td>
			As described in the <a href="https://mise.mda.gov/drupal/node/104">MISE Interface Security Specification.</a>
		</td>
</tr><tr><th>Request Content Type</th>
<td>NIEM-M POS instance with one or more &lt;mda:Position&gt; elements</td>
<td>See example below.</td>
</tr><tr><th rowspan="5">Status Codes</th>
<td>204 (Success)</td>
<td>Returns the location header.</td>
</tr><tr><td>400 (Bad Request)</td>
<td>Request was not formatted correctly.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header.</td>
</tr><tr><td>403 (Forbidden)</td>
<td>Invalid authentication information.</td>
</tr><tr><td>404 (Not found)</td>
<td>The record does not exist on the MISE, cannot update.</td>
</tr><tr><th width="20%">Response Headers</th>
<td>Location</td>
<td>Position Search URL corresponding to the just-published position<br />
			or positions.</td>
</tr><tr><th>Response Content Type</th>
<td>Empty</td>
<td></td>
	</tr></table><p>The listing below shows a position update with a single &lt;mda:Position&gt; element.<br />
Example Position Update</p>
<pre class="brush: xml">
&lt;?xml version="1.0" encoding="UTF-8"?&gt;
&lt;posex:Message xsi:schemaLocation="http://niem.gov/niem/domains/maritime/2.1/position/exchange/3.2 ../XMLSchemas/exchange/3.2/position-exchange.xsd" xmlns:m="http://niem.gov/niem/domains/maritime/2.1" xmlns:mda="http://niem.gov/niem/domains/maritime/2.1/mda/3.2" xmlns:posex="http://niem.gov/niem/domains/maritime/2.1/position/exchange/3.2" xmlns:nc="http://niem.gov/niem/niem-core/2.0" xmlns:gml="http://www.opengis.net/gml/3.2" xmlns:ism="urn:us:gov:ic:ism" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"  mda:securityIndicatorText="LEI" mda:releasableNationsCode="USA" mda:releasableIndicator="true"&gt;
&lt;nc:DocumentExpirationDate&gt;
		&lt;nc:Date&gt;2012-01-01&lt;/nc:Date&gt;
	&lt;/nc:DocumentExpirationDate&gt;
	&lt;mda:Position&gt;
		&lt;m:LocationPoint&gt;
			&lt;gml:Point gml:id="tp1"&gt;
				&lt;gml:pos&gt;-1.0 -1.0&lt;/gml:pos&gt;
			&lt;/gml:Point&gt;
		&lt;/m:LocationPoint&gt;
		&lt;mda:PositionSpeedMeasure&gt;
			&lt;nc:MeasureText&gt;12&lt;/nc:MeasureText&gt;
			&lt;nc:SpeedUnitCode&gt;kt&lt;/nc:SpeedUnitCode&gt;
		&lt;/mda:PositionSpeedMeasure&gt;
		&lt;mda:PositionCourseMeasure&gt;
			&lt;nc:MeasureText&gt;180&lt;/nc:MeasureText&gt;
			&lt;m:AngleUnitText&gt;deg&lt;/m:AngleUnitText&gt;
		&lt;/mda:PositionCourseMeasure&gt;
		&lt;mda:PositionHeadingMeasure&gt;
			&lt;nc:MeasureText&gt;180&lt;/nc:MeasureText&gt;
			&lt;m:AngleUnitText&gt;deg&lt;/m:AngleUnitText&gt;
		&lt;/mda:PositionHeadingMeasure&gt;
		&lt;mda:PositionNavigationStatus&gt;
			&lt;nc:StatusText&gt;Under way using engines&lt;/nc:StatusText&gt;
		&lt;/mda:PositionNavigationStatus&gt;
		&lt;mda:PositionDateTime&gt;
			&lt;nc:DateTime&gt;2013-08-28T22:20:56Z&lt;/nc:DateTime&gt;
		&lt;/mda:PositionDateTime&gt;
	&lt;/mda:Position&gt;
&lt;/posex:Message&gt;
</pre><p>
Note that position updates MUST be valid instances of the NIEM-M Position/Track schema (POS). However, the update service will ignore any information provided in the mda:Vessel element, so that element need not be included. To update vessel information in a POS record, the standard update interface via the batch publish described below should be used.</p>
<p>The gml:Point element must have a gml:id attribute, and it is up to the information provider to ensure that it is a unique id. This must be unique to prevent conflicts when the POS instance for all positions corresponding to this vessel are returned in a query. </p>
<p>If the mda:PositionDateTime element is not provided, the MISE will use the current time at publish, in GMT as the position date/time.</p>
<p>The update service will extract and index all mda:Position elements in the provided XML instance, correlating them with the existing position information based on the provide entity ID and record ID.</p>
<p>The location header in the response on a successful update with a URL for the positSearch service, described below, which matches the position or positions just published. For example:</p>
<p><a href="">https://services.mda.gov/positSearch/?start=2013-08-28T22:20:56&amp;end=2013-08-28T22:22:57&amp;RecordID=12345678&amp;EntityID=agencyone.gov</a></p>
<h2><a id="delete">Record Delete</a></h2>
<p>The following table presents details of the HTTP DELETE request an information provider uses to delete a record and the possible responses, which may be received back from the MISE. Records must be deleted individually.</p>
<table><tr><th colspan="3">Deleting Records</th>
</tr><tr><th width="20%">URI</th>
<td width="30%">BaseURI/IEPDName/RecordID</td>
<td>Example: <a href="">https://services.mda.gov/publish/noa/12345</a></td>
</tr><tr><th>Method</th>
<td>DELETE</td>
<td></td>
	</tr><tr><th>Request Headers</th>
<td>Authorization</td>
<td>
			As described in the <a href="https://mise.mda.gov/drupal/node/104">MISE Interface Security Specification.</a>
		</td>
</tr><tr><th>Request Content Type</th>
<td>Empty</td>
<td></td>
	</tr><tr><th rowspan="3">Status Codes</th>
<td>204 (No Content)</td>
<td>Record was successfully deleted if it existed.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header.</td>
</tr><tr><td>403 (Forbidden)</td>
<td>Authenticated information provider is not configured at MISE for publication of this IEPD.</td>
</tr><tr><th>Response Content Type</th>
<td>Empty</td>
<td></td>
	</tr></table><h2>Record Expiration</h2>
<p>Records in the MISE will always be considered expired and be deleted after 30 days in the cache. Information providers can exercise more precise control over their records via the following means:</p>
<ol><li>
		The DELETE interface described above.
	</li>
<li>Each IEPD includes the DocumentExpirationDate element. When set to<br />
		a value less than 30 days, the record will be expired and deleted<br />
		from the MISE when that date is reached. If set to more than 30 days,<br />
		it will be ignored and the record is deleted at 30 days.
	</li>
</ol><h2><a id="publish_scope">Publishing with a Data Scope</a></h2>
<p>For specific events and situations, the MISE provides the means for an information provider to specify a different level of data access. For instance, an information provider might allow temporary access to vessel position data for a wider range of data consumers during a hurricane. To publish data in a specific scope, the information provider must provide the Scope and DataAttribute parameters. A new recordset is published with those parameters, or an existing set of records can be updated. The Scope and other parameters apply to all records in the mise-recordset published.</p>
<table><tr><th colspan="3">Adding or Updating a Record with Data Scope</th>
</tr><tr><th width="20%">URI</th>
<td width="30%">BaseURI/IEPDName/?Scope=XXX&amp;DataAttribute=YYYl &amp;Releasable=F&amp;Nation=NNN,MMM</td>
<td>Example: <a href="">https://services.mda.gov/publish/noa/? Scope=HurricaneKatrina&amp;DataAttribute=COI &amp;Releasable=F&amp;Nation=USA</a></td>
</tr><tr><th>Scope</th>
<td>String defining the scope in which this record has modified data access. These are defined by the MISE Board for specific events.</td>
<td>HurricaneKatrina</td>
</tr><tr><th>DataAttribute</th>
<td>This is the modified data access attribute for that scope, as defined in the MISE Attribute Specification.</td>
<td>COI</td>
</tr><tr><th>Releasable</th>
<td>Optional. Boolean. Indicates whether the data is releasable within the scope.</td>
<td>T or F</td>
</tr><tr><th>Nation</th>
<td>Optional. Comma-separated list of ISO 3-letter country codes. Indicates which nations to which the data can be provide in this scope.</td>
<td>USA,CAN</td>
</tr><tr><th>Method</th>
<td>PUT</td>
<td></td>
	</tr><tr><th>Request Headers</th>
<td>Authorization</td>
<td>
			As described in the <a href="https://mise.mda.gov/drupal/node/104">MISE Interface Security Specification.</a>
		</td>
</tr><tr><th>Request Content Type</th>
<td>application/xml; charset=UTF-8</td>
<td></td>
	</tr><tr><th>Request Content</th>
<td>Standard mise-recordset as defined in the batch publish section.</td>
<td><a href="#listing2">See Listing 2</a></td>
</tr><tr><th rowspan="5">Status Codes</th>
<td>201 (Success)</td>
<td>Successful Publish.</td>
</tr><tr><td>204 (No Content)</td>
<td>Record was successfully deleted if it existed.</td>
</tr><tr><td>400 (Bad Request)</td>
<td>Request content did not validate against the schema for IEPDName.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header.</td>
</tr><tr><td>403 (Forbidden)</td>
<td>Authenticated information provider is not configured at MISE for publication of this IEPD.</td>
</tr><tr><th>Response Headers</th>
<td>Empty</td>
<td></td>
	</tr><tr><th>Response Content Type</th>
<td>Empty</td>
</body>
</html>