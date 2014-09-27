<h1 class="with-tabs">User Stories for Search</h1>

<p>Prior to reading this section, read <a href="process-flows.md">Process
	Flows for Security, Update, Delete, Search, and Retrieve</a> for a basic
	understanding of how information flows between the provider and
	consumer.</p>

<p>This set of user stories calls out specific examples for search and
	retrieve for the information products provided by the MISE. Examples for
	publish and delete are contained in the sections that discuss those
	operations, as they are simple, one-time operations.</p>

<p>As noted in the <a href="search-retrieve-spec.md">MISE Search/Retrieve
	Interface Specification</a>, these operations return the Atom summary
	feeds of each of the information products. The full message for any of the
	summaries would be accessed via a retrieve operation.</p>

<p>In each of the examples below, the URL is relative to the
	<code>services.mda.gov/services/MDAService</code> base path for MISE
	service access. The placeholder $value is used in the place of query
	values.</p>

<h2>Position</h2>

<p>Note that the Position instances use a different semantic for search than
	the LOA, NOA, IAN, and VINFO instances. Because Position instances are
	typically updated on a regular basis with new position reports, they are
	stored and indexed in a slightly different manner on the MISE.</p>

<table width="100%"><tr><td width="50%"> Return vessel position messages based on a geospatial area and time window, to see positions of all vessels within that geospatial area that within the last 24 hours (24 hour default time frame). The positions for each vessel returned will be in reverse chronological order within the start, end time range.</td>
<td> /search/positSearch?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value&amp;start=$value&amp;end=$value</td>
</tr><tr><td width="50%">Retrieve a vessel position message with all positions on a specific vessel (referred to as a track). To specify the time range for the track, include the start and end parameters.</td>
<td> /search/positSearch?entityid=$eid&amp;recordid=$posid</td>
</tr><tr><td width="50%">Retrieve the most recent vessel position message for each vessel that has updated in the last 30 seconds in the geographic area of interest. </td>
<td> /search/positSearch?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value &amp;start=$value&amp;end=$value            start, end should be the last 30 seconds </td>
</tr></table><h1>Indicators and Notifications</h1>
<table width="100%"><tr><td width="500px">Retrieve all IANs about all vessels in a specific geospatial area. If start and end are not provided, the time range is 24 hours.</td>
<td> /search/ian?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value &amp;start=$value&amp;end=$value </td>
</tr></table><h1>Notice of Arrival</h1>
<table width="100%"><tr><td width="50%">Search for all vessels inbound to a specified port in the specified time range. Returns NOA instances. </td>
<td> /search/noa?PortCodeText=$value&amp;start=$value&amp;end=$value </td>
</tr><tr><td width="50%">Retrieve all pending notices of arrival for a specified vessel. Returns NOA instances.</td>
<td> /search/noa?VesselName=$value&amp;VesselMMSIText=$value &amp;VesselIMONumberText=$value </td>
</tr><tr><td width="50%">Retrieve the IANs of all vessels in a specific geospatial area that are carrying certain dangerous cargo (CDC). </td>
<td> /search/ian?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value &amp;start=$value&amp;end=$value &amp;VesselCDCCargoOnboardIndicator=true </td>
</tr><tr><td width="50%">Retrieve NOA message for a specified vessel and port for which any data element has been updated in the last 10 minutes. </td>
<td> /search/noa?start=$value&amp;end=$value&amp;PortCodeText=$value &amp;VesselName=$value &amp;VesselMMSIText=$value &amp;VesselIMONumberText=$value start, end should be the last 10 minutes </td>
</tr></table><p>*Note that any combination of MMSI, IMO, and Name can be used, all three are not required.</p>
<h1>Levels Of Awareness</h1>
<table width="100%"><tr><td width="50%">Retrieve LOA messages for all vessels in a specified geospatial area. If start and end are not specified, defaults to 24 hours.</td>
<td> /search/loa?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value &amp;start=$value&amp;end=$value </td>
</tr><tr><td width="50%">Retrieve LOA messages for vessels with a specific threat level within a geospatial area. Note that $threat in the following example must align with one of the threat values available in the LOA schema. </td>
<td> /search/loa?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value &amp;$threat=$value </td>
</tr><tr><td width="50%"> Retrieve LOA message for a specific vessel in a geospatial area. </td>
<td> /search/loa?ulat=$value&amp;ulng=$value&amp;llat=$value&amp;llng=$value &amp;VesselName=$value &amp;VesselMMSIText=$value &amp;VesselIMONumberText=$value </td>
</tr></table><p>*Note that any combination of MMSI, IMO, and Name can be used, all three are not required.</p>
<h1>Vessel Information</h1>
<table width="100%"><tr><td width="50%"> Retrieve VInfo message for a specific vessel updated within a specified time range.</td>
<td>/search/vinfo?VesselName=$value &amp;VesselMMSIText=$value &amp;VesselIMONumberText=$value &amp;start=$value&amp;end=$value</td>
</tr></table>

<p>* Note that any combination of MMSI, IMO, and Name can be used, all three
	are not required.</p>
