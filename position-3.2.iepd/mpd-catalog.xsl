<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0"
    xmlns="http://www.w3.org/1999/xhtml"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:ca="http://reference.niem.gov/niem/resource/mpd/catalog/1.0/">

<xsl:output method="html" indent="yes"/>

<xsl:template name="put-row">
  <xsl:param name="item"/>
		<xsl:variable name="p" select="$item/@ca:purposeURI"/>
		<xsl:variable name="n" select="$item/@ca:natureURI"/>
      <tr>
	<td>
		<xsl:choose>
			<xsl:when test="contains($p,'changelog')">Change Log</xsl:when>
			<xsl:when test="contains($p,'master-document')">Master Document</xsl:when>
			<xsl:when test="contains($n,'exchange-schema')">Exchange Schema</xsl:when>
			<xsl:when test="contains($n,'extension-schema')">Extension Schema</xsl:when>
			<xsl:when test="contains($n,'subset-schema')">Subset Schema</xsl:when>
			<xsl:when test="contains($p,'subset-schema-set')">Subset Schema Set</xsl:when>
			<xsl:when test="contains($p,'catalog')">Catalog</xsl:when>
			<xsl:when test="contains($p,'display')">Display</xsl:when>
			<xsl:when test="contains($p,'transformation')">Transformation</xsl:when>
			<xsl:when test="contains($p,'mapping')">Mapping</xsl:when>
			<xsl:when test="contains($p,'sample-instance')">Sample Instance</xsl:when>
			<xsl:when test="contains($n,'wantlist')">Subset Wantlist</xsl:when>
			<xsl:when test="contains($p,'tool-specific-file')">Tool Specific File</xsl:when>
		</xsl:choose>
	</td>
	<td>
	    <a>
		<xsl:attribute name="href">
		      <xsl:value-of select="$item/@ca:relativePathName"/>
		</xsl:attribute>
		<xsl:value-of select="$item/@ca:relativePathName"/>
	    </a>
	</td>
	<!--<td><xsl:value-of select="$item/@ca:descriptionText"/><xsl:if test="contains($p,'catalog')"> (this document)</xsl:if></td>-->
    </tr>
</xsl:template>

<xsl:template match="/">
    <html>
    	<meta><title><xsl:value-of select="/*/@ca:descriptionText" /></title></meta>
	<body><xsl:apply-templates/></body>
    </html>
</xsl:template>

<xsl:template match="/ca:Catalog">
    <h1><xsl:value-of select="@ca:descriptionText" /></h1>
    <table width="100%" border="1" style="border-collapse:collapse">
	<thead style="background-color:maroon; color:white; ">
	    <tr>
		<th>Attribute</th>
		<th>Value</th>
	    </tr>
	</thead>
	<tr>
	    <td>URI</td>
	    <td><xsl:value-of select="@ca:mpdURI" /></td>
	</tr>
	<tr>
	    <td>Class</td>
	    <td><xsl:value-of select="@ca:mpdClassCode" /></td>
	</tr>
	<tr>
	    <td>Name</td>
	    <td><xsl:value-of select="@ca:mpdName" /></td>
	</tr>
	<tr>
	    <td>Version</td>
	    <td><xsl:value-of select="@ca:mpdVersionID" /></td>
	</tr>
	<tr>
	    <td>Description</td>
	    <td><xsl:value-of select="@ca:descriptionText" /></td>
	</tr>
    </table>

    <h2>IEPD Artifacts</h2>

    <table width="100%" border="1" style="border-collapse:collapse">
	<thead style="background-color:maroon; color:white; ">
	    <tr>
		<th>Artifact Type</th>
		<th>Path and File Name</th>
		<!--<th>Description</th>-->
	    </tr>
	</thead>
     
	<xsl:for-each select="//ca:File">
        <xsl:call-template name="put-row">
          <xsl:with-param name="item" select="."/>
        </xsl:call-template>
    </xsl:for-each>

</table>


    <h2>Metadata</h2>

    <table width="100%" border="1" style="border-collapse:collapse">
      <thead style="background-color:maroon; color:white; ">
			<tr>
				<th>Attribute</th>
				<th>Value</th>
			</tr>
      </thead>
      <tr>
			<td>Security Marking</td>
			<td><xsl:value-of select="ca:Metadata/ca:SecurityMarkingText"/></td>
      </tr>
      <tr>
			<td>Creation Date</td>
			<td><xsl:value-of select="ca:Metadata/ca:CreationDate"/></td>
      </tr>
		<tr>
			 <td>Keywords</td>
			 <td>
				 <xsl:for-each select="ca:Metadata/ca:KeywordText">
					<xsl:value-of select="."/>
					<xsl:if test="position() != last()">, </xsl:if>
				 </xsl:for-each>
			 </td>
		</tr>
      <xsl:for-each select="ca:Metadata/ca:DomainText">
			<tr>
				 <td>Domain</td>
				 <td><xsl:value-of select="."/></td>
			</tr>
      </xsl:for-each>
      <tr>
		  <td>Purpose</td>
		  <td>    
				<xsl:value-of select="ca:Metadata/ca:PurposeText"/>
		  </td>
      </tr>
		<tr>
			 <td>Exchange Pattern</td>
			 <td>
				 <xsl:for-each select="ca:Metadata/ca:ExchangePatternText">
					<xsl:value-of select="."/>
					<xsl:if test="position() != last()">, </xsl:if>
				 </xsl:for-each>
			 </td>
		</tr>
    </table>

    <h2>Authoritative Source</h2> 

    <table width="100%" border="1" style="border-collapse:collapse">
	<thead style="background-color:maroon; color:white; ">
	    <tr>
		<th>Attribute</th>
		<th>Value</th>
	    </tr>
	</thead>
	<tr>
	    <td>Name</td>
	    <td>
		<xsl:value-of select="ca:Metadata/ca:AuthoritativeSource/ca:ASName"/>
	    </td>
	</tr>
	<!--<tr>
	    <td>Address</td>
		<td>
		  <xsl:value-of select="ca:Metadata/ca:AuthoritativeSource/ca:ASAddressText"/>
		</td>
	</tr>
	<tr>
	    <td>Web Site</td>
	    <td>
	    	<a href="{ca:Metadata/ca:AuthoritativeSource/ca:ASWebSiteURL}">
				<xsl:value-of select="ca:Metadata/ca:AuthoritativeSource/ca:ASWebSiteURL"/>
			</a>
	    </td>
	</tr>-->
	<xsl:for-each select="ca:Metadata/ca:AuthoritativeSource/ca:POC">
	    <tr>
		<td>Point of Contact</td>
		<td>
		    <xsl:value-of select="ca:POCName"/>, 
		    <xsl:value-of select="ca:POCEmail"/>, 
		    <xsl:value-of select="ca:POCTelephone"/>
		</td>
	    </tr>
	</xsl:for-each>
    </table>

</xsl:template>
</xsl:stylesheet>

