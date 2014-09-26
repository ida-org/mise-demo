<h1 class="with-tabs">MISE Search/Retrieve Specification</h1>

<h2>Introduction</h2>
<p>This page describes the data-consumer facing representational state transfer (REST) architecture providing search and retrieve (SR) functionality for the MISE. By conforming to this interface, the MISE provides data consumers with the ability to find and retrieve the right information at the right time, based on the needs, rights, and authorities of the user and the organizations requesting the information. </p>
<table cellpadding="10"><tr><th>Operation</th>
<th>URI</th>
<th>Description</th>
</tr><td width="20%"><a href="#positSearch">Position Search</a></td>
<td width="30%"><code>GET positSearch/?=</code></td>
<td>Returns set of all Position instances that match the temporal and geospatial parameters.</td>

<tr><td width="20%"><a href="#positSearchFull">Position Search (Full)</a></td>
<td><code>GET positSearch/?=</code></td>
<td>Returns the Position instance for the specified record based on RecordID/EntityID and temporal parameters.<br />If the start and end parameters are not specified, the default time window will be the previous 24 hours.<br />Specifying the full previous 30 day window will return all mda:Position elements for this NIEM-M POS instance.</td>
</tr><tr><td width="20%"><a href="#search">Search</a></td>
<td><code>GET search/&lt;iepd&gt;/?=</code></td>
<td>Returns set of records of the specified IEPD type based on the query
	parameters.	<br />
	A search cannot combine geospatial and term searches. If both are
	specified, the MISE will use the geospatial parameters only.	<br />
	If the start and end parameters are not specified, the default time window
	will be the previous 24 hours.
</td>
</tr>
</table>

<p>This interface provides a paginated search interface, which breaks large
	query results sets into "pages" based on time windows, which will be
	explained in the next sections.</p>

<p>All interactions with the SR interface are secured as described by the
	<a href="security-spec.md">MISE Interface Security Specification</a>. For
	search and retrieve operations, both the Trusted System Authentication and
	the User Attribute Conveyance portions of the interface security
	specification apply.  All records available on the MISE are formatted
	using NIEM-Maritime, as described in the
	<a href="/drupal/node/42">National MDA Architecture Information Exchange
		Package Documents (IE</p>

<h2>Protocol, Sessions, and Security</h2>
<p>All interactions with the MISE are done over Secure Socket Layer/Transport
	Layer Security (SSL/TLS) connections. SSL/TLS are enforced to protect
	information in transit and session cookies. Each client querying against
	the search interfaces is authenticated by the MISE and is provided with a
	limited-duration session cookie, which must be supplied in the header of
	following requests. The method for authentication is discussed the
	<a href="security-spec.md">MISE Interface Security Specification</a>.</p>

<h2> Header Information</h2>
<p>The MISE search interface uses the following HTTP headers for specific purposes.  All other HTTP headers should be interpreted according to the HTTP 1.1 standard.</p>
<ul><li>Accept: By default, if no Accept is specified, the summary feed will be returned in the Atom format as specified above. However, the following Accept headers may be supplied, resulting in translated feeds:
<ul><li>application/kml – returns the &lt;mise-recordset&gt; feed translated into a KML overlay.
</li></ul></li></ul><h2>Position Search Interface</h2>
<h3><a id="positSearch">Position Set Search Interface</a></h3>
<p>The MISE provides a position search service that allows the consumer to search for updated positions on the MISE based on temporal and geospatial parameters. This provides back a set of all Position instances that match the search parameters. </p>
<table><tr><th colspan="3">Position Set Search Service</th>
</tr><tr><th width="20%">URI</th>
<td width="30%">BaseURI/positSearch/?=</td>
<td>Example: <a href="">https://services.mda.gov/positSearch/? start=2013-08-28T22:20:56&amp;end=2013-08-28T22:22:56 &amp;ulat=39.636756&amp;ulng=-77.148293 &amp;llng=-75.793459&amp;llat=38.528532</a></td>
</tr><tr><th>Method</th>
<td>GET</td>
<td></td>
	</tr><tr><th>Request Headers</th>
<td>Authorization</td>
<td>
			As described in the <a href="security-spec.md">MISE Interface Security Specification.</a>
		</td>
</tr><tr><th>Request Content Type</th>
<td>Empty</td>
<td></td>
	</tr><tr><th rowspan="5">Status Codes</th>
<td>200 (Success)</td>
<td>Returns the results described below.</td>
</tr><tr><td>400 (Bad Request)</td>
<td>Request was not formatted correctly.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header.</td>
</tr><tr><td>403 (Forbidden)</td>
<td>Invalid authentication information.</td>
</tr><tr><td>5xx (Error)</td>
<td>Error in position search. The body content will provide error details.</td>
</tr><tr><th width="20%">Response Headers</th>
<td>Empty</td>
<td></td>
	</tr><tr><th>Response Content Type</th>
<td>&lt;mise-recordset&gt; containing results</td>
<td>See example below.</td>
</tr></table><p>The position search service requires all six parameters to be specified:</p>
<table><tr><th colspan="3">Position Set Search Parameters</th>
</tr><tr><th width="20%">start</th>
<td width="30%">ISO8601 DateTime</td>
<td>start of the query time window in GMT</td>
</tr><tr><th>end</th>
<td>ISO8601 DateTime</td>
<td>end of the query time window in GMT</td>
</tr><tr><th>ulat</th>
<td>WGS84 </td>
<td>upper left latitude of a bounding box</td>
</tr><tr><th>ulng</th>
<td>WGS84 </td>
<td>upper left longitude of a bounding box</td>
</tr><tr><th>llat</th>
<td>WGS84 </td>
<td>lower right latitude of a bounding box</td>
</tr><tr><th>llng</th>
<td>WGS84 </td>
<td>lower right longitude of a bounding box</td>
</tr></table><p>The position set search returns an &lt;mise-recordset&gt; element containing the results of the search. For full details on the &lt;mise-recordset&gt; and pagination, see the description in the General Batch Search Interface section. The &lt;mise-recordset&gt; contains the Position instances matching the query parameters. Each instance contains the Vessel information and one or more associated &lt;mda:position&gt;  elements, where the position datetime of each &lt;mda:position&gt;  element is within the time window specified by the start and end parameters, and where the position falls within the geospatial box defined by the other four parameters. The &lt;mda:position&gt;  elements are time-ordered from newest to oldest. The full Position instance for any result can be easily queried using the retrieval interface described below.</p>
<h3><a id="positSearchFull">Full Position Instance Retrieval (Full Track)</a></h3>
<p>Given the entity ID and record ID of a NIEM-M POS record published to the MISE, which has presumably been updated with further positions, the track retrieval interface provides the ability to query for some or all positions for a specific record. </p>
<table><tr><th colspan="3">Full Position Instance Retrieve Service</th>
</tr><tr><th width="20%">URI</th>
<td width="30%">BaseURI/positSearch/?=</td>
<td>Example: <a href="">https://services.mda.gov/positSearch/? start=2013-07-28T22:20:56&amp;end=2013-08-28T22:22:56 &amp;recordid=12345678&amp;entityid=agencyone.gov</a></td>
</tr><tr><th>Method</th>
<td>GET</td>
<td></td>
	</tr><tr><th>Request Headers</th>
<td>Authorization</td>
<td>
			As described in the <a href="security-spec.md">MISE Interface Security Specification.</a>
		</td>
</tr><tr><th>Request Content Type</th>
<td>Empty</td>
<td></td>
	</tr><tr><th rowspan="5">Status Codes</th>
<td>200 (Success)</td>
<td>Returns the NIEM-M POS instance corresponding to the query.</td>
</tr><tr><td>400 (Bad Request)</td>
<td>Request was not formatted correctly.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header.</td>
</tr><tr><td>403 (Forbidden)</td>
<td>Invalid authentication information.</td>
</tr><tr><td>5xx (Error)</td>
<td>Error in position search. The body content will provide error details.</td>
</tr><tr><th width="20%">Response Headers</th>
<td>Empty</td>
<td></td>
	</tr><tr><th>Response Content Type</th>
<td>Single NIEM-M POS instance</td>
<td>See example below.</td>
</tr></table><table><tr><th colspan="3">Full Position Instance Retrieve Parameters</th>
</tr><tr><th width="20%">start</th>
<td width="30%">ISO8601 DateTime</td>
<td>start of the query time window in GMT</td>
</tr><tr><th>end</th>
<td>ISO8601 DateTime</td>
<td>end of the query time window in GMT</td>
</tr><tr><th>recordID</th>
<td>String</td>
<td>ID of the POS record, content of the RecordIDURI element</td>
</tr><tr><th>entityID</th>
<td>String</td>
<td>Entity ID of the publisher of this record</td>
</tr></table><p>The full position instance retrieve service requires only that the record ID and entity ID parameters be specified. If the start and end parameters are not specified, the MISE will query over the last 24 hours. The number of &lt;mda:position&gt;  elements provided back in the response is based on the time range specified in the query. Specifying the full previous 30 day window will return all &lt;mda:position&gt;  elements for this NIEM-M POS instance. If the start and end parameters are not specified, the default time window will be the previous 24 hours.</p>
<p>The returned XML will be a NIEM-M POS instance with one or more &lt;mda:position&gt;  elements that fall within the specified time range. See the example file <a href="/drupal/sites/default/files/examplePositionRetrieve.xml">here</a>. The &lt;mda:position&gt;  elements will be ordered from newest to oldest in time.</p>
<h2><a id="search">General Batch Search Interface</a></h2>
<h3>URL Structure</h3>
<p>For the purposes of this document, the MISE is assumed to be accessible at the global uniform resource identifier (URI) https://services.mda.gov/services/MDAService, provided as an example base URL. SR provides a series of URL endpoints that provide global query functionality and focus-area-specific (or IEPD-specific) queries. The query URLs have the following form: https://services.mda.gov/search/iepdname/?= </p>
<p>The iepdname takes the form of one of the message types provided by the MISE. Currently, these are:</p>
<ol><li>noa (Notice of Arrival)
</li><li>ian (Indicator and Notification)
</li><li>vinfo (Vessel Information)
</li><li>loa (Levels of Awareness)
</li><li>pos (Position) - this query only returns the base published Position instance, not the full Position instance with all updated positions. For that, use the previously described Position Retrieve search.
</li></ol><p>Each URI in association with an IEPD name queries a single record type. For example, https://services.mda.gov/search/noa?=  provides the notice-of-arrival specific query endpoint. Using this scheme, further endpoints can be added for specific queries as new record types are defined for new focus areas. </p>
<h3>Query and Query Parameters</h3>
<table><tr><th colspan="3">Batch Search Service</th>
</tr><tr><th width="20%">URI</th>
<td width="30%">BaseURI/IEPDName/?=</td>
<td>Example: <a href="">https://services.mda.gov/search/noa/?start=2013-08-27T22:20:56&amp;end=2013-08-28T22:22:56</a></td>
</tr><tr><th>Method</th>
<td>GET</td>
<td></td>
	</tr><tr><th>Request Headers</th>
<td>Authorization</td>
<td>
			As described in the <a href="security-spec.md">MISE Interface Security Specification.</a>
		</td>
</tr><tr><th>Request Content Type</th>
<td>Empty</td>
<td></td>
	</tr><tr><th rowspan="5">Status Codes</th>
<td>200 (Success)</td>
<td>Returns the body content.</td>
</tr><tr><td>400 (Bad Request)</td>
<td>Request was not formatted correctly.</td>
</tr><tr><td>401 (Unauthorized)</td>
<td>No Authorization header.</td>
</tr><tr><td>403 (Forbidden)</td>
<td>Invalid authentication information.</td>
</tr><tr><td>5xx (Error)</td>
<td>Error in position search. The body content will provide error details.</td>
</tr><tr><th width="20%">Response Headers</th>
<td>Empty</td>
<td></td>
	</tr><tr><th>Response Content Type</th>
<td>&lt;mise-recordset&gt; containing results</td>
<td>See example below.</td>
</tr></table><p>The query URL for each IEPD type can be combined with a set of query parameters to narrow the results returned. </p>
<table><tr><th width="20%"> Parameter Name	</th>
<th width="30%"> Value Type	</th>
<th> Comment	</th>
</tr><tr><th> start	</th>
<td> ISO8601 DateTime	</td>
<td> GMT	</td>
</tr><tr><th> end	</th>
<td> ISO8601 DateTime	</td>
<td> GMT	</td>
</tr><tr><th> ulat	</th>
<td> WGS84 	</td>
<td> upper left latitude of a bounding box	</td>
</tr><tr><th> ulng	</th>
<td> WGS84 	</td>
<td> upper left longitude of a bounding box	</td>
</tr><tr><th> llat	</th>
<td> WGS84 	</td>
<td> lower right latitude of a bounding box	</td>
</tr><tr><th> llng	</th>
<td> WGS84 	</td>
<td> lower right longitude of a bounding box	</td>
</tr><tr><th> VesselName	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselSCONUMText	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselMMSIText 	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselIMONumberText	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselNationalFlag	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselClassText	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselCallSignText	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselHullNumberText	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselOwnerName	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> VesselCategoryText	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> InterestCategoryCode	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> InterestLevel	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> PortNameText	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> InterestType	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> InterestCategory	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> InterestLevel	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> InterestStart	</th>
<td> ISO8601 DateTime	</td>
<td> GMT	</td>
</tr><tr><th> InterestEnd	</th>
<td> ISO8601 DateTime	</td>
<td> GMT	</td>
</tr><tr><th> ArrivalPort	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> ExpectedArrivalTime	</th>
<td> ISO8601 DateTime	</td>
<td> GMT	</td>
</tr><tr><th> eid	</th>
<td> String	</td>
<td> The eid parameter allows a consumer to specify a specific data<br />
			provider. This field must contain the entity ID of a provider system<br />
			from the Trust Fabric document. This is only used in addition to<br />
			other parameters	</td>
</tr><tr><th> NoticeType	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> NoticeTransactionType	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> CDCCargoDeclared	</th>
<td> Boolean	</td>
<td> T or F	</td>
</tr><tr><th> ActivityName	</th>
<td> String	</td>
<td>
		</td>
</tr><tr><th> ActivityStart	</th>
<td> ISO8601 DateTime	</td>
<td> GMT	</td>
</tr><tr><th> ActivityEnd	</th>
<td> ISO8601 DateTime	</td>
<td> GMT	</td>
</tr><tr><th> LevelOfAwarenessCode	</th>
<td> Integer	</td>
<td> This parameter is applied to queries specifically for the Levels<br />
			of Awareness IEPD type. This specifies the LOA level	</td>
</tr></table><p>A search <i>cannot</i> combine geospatial and term searches. If both are specified, the MISE will use the geospatial parameters only. The start and end parameters are implicitly required. If not specified, the MISE defaults to the last 24 hours.</p>
<h4>Scope</h4>
<p>For specific events and situations, the MISE provides an additional set of query parameters that can be used to query for messages within a specific scope. When applied by an information publisher, the scope parameters modify the entitlements for a record for the duration of the scope. This, in combination with the entitlements specified as part of the authentication process, may allow a consumer access to additional information.</p>
<table><tr><th width="20%"> Parameter Name	</th>
<th width="30%"> Value Type	</th>
<th> Comment	</th>
</tr><tr><th>Scope</th>
<td>String</td>
<td></td>
</tr></table><p>Table 7 – Scope Search Arguments</p>
<p>For example, consider a Search query for<br />
all notice of arrival messages inbound to the Port of Miami during<br />
Hurricane Katrina:</p>
<ul><li><a href="">https://services.mda.gov/search/noa?PortNameText=Miami&amp;scope=HurricaneKatrina</a>
</li></ul><p>Valid scope values can be found on the National MDA Architecture website.</p>
<h3>Query Results</h3>
<p>As an example, consider a query for all notice of arrival instances in a 24-hour period: <a href="">https://services.mda.gov/search/noa/?start=2013-08-27T22:20:56&amp;end=2013-08-28T22:22:56</a>a&gt;. The results are broken up in to sets or “paginated” based on time and the number of records in the result set. Each page of records includes a link for the next set of records, moving from the newest to the oldest, if there is more than one page of results.</p>
<p>The interface returns an &lt;mise-recordset&gt; element, containing up to 250 results in the following form. Both the Position Search and General Batch Search interfaces use this format.</p>
<pre class="brush: xml">
&lt;?xml version="1.0" encoding="UTF-8"?&gt;
&lt;mise-recordset xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
 xsi:noNamespaceSchemaLocation="../../../../position-3.2.iepd/XMLschemas/exchange/3.2/mise-recordset.xsd"
 pageElements="250"
 query="https://services.mda.gov/search/noa/?start=2013-08-27T22:20:56&amp;end=2013-08-28T22:22:56"
 nextQuery="https://services.mda.gov/search/noa/?start=2013-08-27T22:20:56&amp;nextTime=2013-08-27T22:20:00:001"&gt;
  &lt;posex:Message&gt; ...snip...&lt;/posex:Message&gt;
  &lt;posex:Message&gt; ...snip...&lt;/posex:Message&gt;
  [... next 248 records]
&lt;/mise-recordset&gt;
</pre><p>
Listing 1 – Example Search Result Content</p>
<p>This search would normally return a very large number of results. Rather than overwhelm the consuming system and the network with a single very large XML result set, the results are “paginated” into smaller sets of records, bounded by time. This pagination is based on the number of records returned, to prevent overwhelming the network and the consuming system. Figure 1 shows how this works:</p>
<p><img style="max-height:150px; max-width:1000px;" src="/drupal/sites/default/files/queryTimeperiod.png" /><br />
Figure 1– Pagination</p>
<p>The pages are not fixed in time. Rather, they are fixed based on the record limit. That is, if there are N records in 30 seconds, then that page is 30 seconds long. The pages move in time from the end datetime of the query to the start datetime of the query, from newest to oldest. Allowing each page to have a dynamic time period allows the MISE to split clustered results into fixed size result sets. </p>
<p>The returned XML carries a number of important attributes in the <mise-recordset> element that wraps the results.</mise-recordset></p>
<table><tr><th width="20%">Attribute
</th><th>Description
</th></tr><tr><th width="30%">query
</th><td>Provides the URL of the original query to the system.
</td></tr><tr><th>nextQuery
</th><td>Provides the next page of results. This is the link pointing to the next page of records. The next page starts at nextTime 2013-08-27T22:20:00.001 and will return the next set of records, up to the record limit. If there are more records, another nextQuery attribute will point to the next result page. This will continue until the nextQuery attribute does not appear in the result, which indicates there are no more records. The start argument is carried through each page so the MISE knows when to stop returning pages.<br />All queries to the batch search/retrieve interface should have the start and end parameters specified. If not specified, then the query defaults to the last 24 hours.<br />For highly clustered results, where many results occur in a short time period, the nextTime argument may be specified down to the millisecond, so the receiving system should handle the .NNN millisecond format for the ISO8601 DateTime.<br />nextQuery will not be included in the last page of the results.
</td></tr><tr><th>pageElements
</th><td>Provides the number of records in this result set. 
</td></tr><tr><th>pageStart
</th><td>The beginning of the time range of the records in this recordset.
</td></tr><tr><th>pageEnd
</th><td>The end of the time range of the records in this recordset.<br /></td></tr></table><p>Table 8 – &lt;mise-recordset&gt; Attributes</p>
<p>Finally, if other query parameters, such as geospatial or vessel identifiers are specified, they are carried through on the nextQuery links. For example, if the geospatial query is:<br /><a href="">https://services.mda.gov/search/noa/?content=full&amp;start=2013-08-27T22:20:56&amp;end=2013-08-28T22:22:56&amp;ulat=39.636756&amp;ulng=-77.148293&amp;llng=-75.793459&amp;llat=38.528532</a></p>
<p>The next page link would look like this:</p>
<p><a href="">https://services.mda.gov/search/noa?content=full&amp;start=2013-08-27T22:20:56&amp;nextTime=2013-08-28T20:22:56&amp;ulat=39.636756&amp;ulng=-77.148293&amp;llng=-75.793459&amp;llat=38.528532</a></p>
<p>If the search were for a specific Vessel Identifier:<br /><a href="">https://services.mda.gov/search/noa?content=full&amp;start=2013-08-27T22:20:56&amp;end=2013-08-28T22:22:5&amp;VesselMMSIText=123456789</a></p>
<p>The nextQuery link would be:<br /><a href="">https://services.mda.gov/search/noa?content=full&amp;start=2013-08-27T22:20:56&amp;nextTime=2013-08-28T20:22:56&amp;VesselMMSIText=123456789</a></p>
<p>The consuming system can iteratively follow the next page links until all records matching the query have been retrieved, when they receive a page with no nextQuery attribute.</p>
