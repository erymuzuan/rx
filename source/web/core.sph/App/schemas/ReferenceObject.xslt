<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  xmlns:ms="urn:schemas-microsoft-com:xslt"
  xmlns:bspk="http://www.bespoke.com.my/"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:bs="http://www.bespoke.com.my/xsd/">

  <!-- inheritance with extension -->
  <xsl:template match="xs:extension">
	  v["$type"] = "Bespoke.Sph.Domain.<xsl:value-of select="../../../@name"/>, domain.sph";
    <!-- collection -->
    <xsl:for-each select="xs:all/xs:element/xs:complexType/xs:sequence/xs:element">
      v.<xsl:value-of select="../../../@name"/> = ko.observableArray([]);</xsl:for-each>

    <xsl:for-each select="xs:all/xs:element">
      <!-- 
      Just plain assocation 1 to 1 
    -->
      <xsl:if test="@ref">
        v.<xsl:value-of select="@ref"/> = ko.observable(new bespoke.sph.domain.<xsl:value-of select="@ref"/>());</xsl:if>
      <xsl:if test="@nillable">
        v.<xsl:value-of select="@name"/> = ko.observable();//nillable</xsl:if>
      <xsl:if test="@type and not(@nillable)">
        v.<xsl:value-of select="@name"/> = ko.observable();//type but not nillable</xsl:if>

      <!-- 
      Element with simple restriction rules- for string and integer
      -->
      <xsl:if test="xs:simpleType">v.<xsl:value-of select="@name"/>= ko.observable(); //enum
      </xsl:if>
    </xsl:for-each>

  </xsl:template>


  <xsl:template match="xs:element">
    <!-- Collection -->
    <xsl:for-each select="xs:complexType/xs:sequence/xs:element">
      <xsl:value-of select="../../../@name"/> : ko.observableArray([]),
    </xsl:for-each>

    <!-- 
      Just plain assocation 1 to 1 
    -->
    <xsl:if test="@ref">
      <xsl:value-of select="@ref"/> : ko.observable(new bespoke.sph.domain.<xsl:value-of select="@ref"/>()),
    </xsl:if>
    <xsl:if test="@nillable">
      <xsl:value-of select="@name"/> : ko.observable(),
    </xsl:if>
    <xsl:if test="@type and not(@nillable)">
      <xsl:value-of select="@name"/> : ko.observable(),
    </xsl:if>

    <!-- 
      Element with simple restriction rules- for string and integer
      -->
    <xsl:if test="xs:simpleType">
      <xsl:value-of select="@name"/>: ko.observable(), //enum
    </xsl:if>
  </xsl:template>
  <msxsl:script language="C#" implements-prefix="bspk">
    <![CDATA[
		public string CamelCase(string field)
		{
			string first = field.Substring(0,1).ToLowerInvariant();
			return first + field.Substring(1, field.Length - 1);
		}	
		public string GetJsDefaultValue(string xsType, bool nillable)
		{
			switch(xsType)
			{
				case "xs:string":return "''";
				case "xs:date":
				case "xs:dateTime": return "moment().format('DD/MM/YYYY')";
				case "xs:int":
				case "xs:long":return "0";
				case "xs:boolean":return "false";
				case "xs:float":return "0.0";
				case "xs:double":return "0.00";
				case "xs:decimal":return "0.00";
				default : return "'" + xsType + "'";
			}
			return String.Empty;
		}
    
	  ]]>

  </msxsl:script>
</xsl:stylesheet>