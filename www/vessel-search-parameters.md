<h1 class="with-tabs">Search Parameters for Vessel Positions</h1>

<h2>Position Request Logical Model</h1>
<p>As depicted in the logical diagram below you can search for position reports within a geospatial area.<br /><img src="/drupal/sites/default/files/Position_Request.png" width="100%" /></p>
<p>In the examples below, the URL is relative to the mise.mda.gov base path for MISE service access. The placeholder $value is used in the place of query values.</p>
<table width="100%"><tr><td width="50%"> Search for vessel position by geospatial area</td>
<td width="50%"> /search/pos?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value</td>
</tr></table><p>You can also search by any of the vessel identifiers.</p>
<table width="100%"><tr><td width="50%">Search for vessel position by Nation Flag</td>
<td width="50%">/search/pos?VesselNationalFlagISO3166Alpha3Code=$value</td>
</tr><tr><td>Search for vessel position by Vessel Name</td>
<td>/search/pos?VesselName=$value</td>
</tr><tr><td>Search for vessel position by Vessel Hull Number</td>
<td>/search/pos?VesselHullNumberText=$value</td>
</tr><tr><td>Search for vessel position by Vessel MMSI</td>
<td>/search/pos?VesselMMSIText=$value</td>
</tr><tr><td>Search for vessel position by Vessel IMO Number</td>
<td>/search/pos?VesselIMONumberText=$value</td>
</tr><tr><td>Search for vessel position by Vessel SCONUM</td>
<td>/search/pos?VesselSCONUMText=$value</td>
</tr><tr><td>Search for vessel position by Vessel Official Coast Guard Number</td>
<td>/search/pos?VesselOfficialCoastGuardNumberText=$value</td>
</tr></table><p>As always you can append bounding box coordinates and/or a date range for a more focused query.  These parameters are listed in table below:</p>
<table width="100%"><tr><td width="50%"> GML Envelope</td>
<td width="50%"> ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value</td>
</tr><tr><td width="50%"> Date Range</td>
<td width="50%"> start=$value&amp;end=$value</td>
</tr></table><p>See <a href="/drupal/node/26">User Stories for Search</a> for examples of combining search parameters.</p>

<h2> Filter by Record Metadata </h2>
<p>Record metadata is included in a record to provide additional information for handling and/or managing the record.  In the request logical model above, the record metadata depicted on the left side of the graphic includes such information as record status, creation date, and exercise name.  In some cases it may be useful to further filter the result set on the client side by record metadata.  For example, filter for only updated records, only records created in the last 24 hours, or only records associated with a specific exercise.</p>
<p>Record metadata elements  and their XML paths are listed in the table below.  </p>
<table width="100%"><tr><td width="50%"><b>Common Name</b></td>
<td width="50%"><b>XML Path</b></td>
</tr><tr><td width="50%">Creation Date</td>
<td width="50%">nc:DocumentCreationDate/nc:Date</td>
</tr><tr><td width="50%">Creator &gt; Organization Name</td>
<td width="50%">
				nc:DocumentCreator/nc:EntityOrganization/nc:OrganizationName</td>
</tr><tr><td width="50%">Exercise Name</td>
<td width="50%">mda:MessageExerciseName</td>
</tr><tr><td width="50%">Expiration Date</td>
<td width="50%">nc:DocumentExpirationDate/nc:Date</td>
</tr><tr><td width="50%">Source System</td>
<td width="50%">mda:MessageSourceSystemName</td>
</tr><tr><td width="50%">Status</td>
<td width="50%">mda:MessageStatusCode</td>
</tr><tr><td width="50%">Record ID</td>
<td width="50%">mda:RecordIDURI</td>
</tr></table><table width="100%"><tr><td width="50%">ISM Marking &gt; Classification</td>
<td width="50%">mda:ICISMMarkings/@classification</td>
</tr><tr><td width="50%">ISM Marking &gt; Dissemination Controls</td>
<td width="50%">mda:ICISMMarkings/@disseminationControls</td>
</tr><tr><td width="50%">ISM Marking &gt; Owner Producer</td>
<td width="50%">mda:ICISMMarkings/@ownerProducer</td>
</tr></table><table width="100%"><tr><td width="50%">Security Indicator</td>
<td width="50%">@securityIndicatorText</td>
</tr><tr><td width="50%">Scope</td>
<td width="50%">@scopeText</td>
</tr><tr><td width="50%">Scope Indicator</td>
<td width="50%">@scopeIndicatorText</td>
</tr><tr><td width="50%">Releasable</td>
<td width="50%">@releasableIndicator</td>
</tr><tr><td width="50%">Releasable Nations</td>
<td width="50%">@releasableNationsCode</td>
</tr></table>