<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:bspk="http://www.bespoke.com.my/"
    xmlns:bs="http://www.bespoke.com.my/xsd/"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:bspk2="http://www.bespoke.com.my/"
    xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xsl:output method="text" />
  <xsl:template match="xs:schema">
    /// &lt;reference path="~/scripts/knockout-2.3.0.debug.js" /&gt;
    /// &lt;reference path="~/Scripts/underscore.js" /&gt;
    /// &lt;reference path="~/Scripts/moment.js" /&gt;


    var bespoke = bespoke || {};
    bespoke.sphcommercialspace =  {};
    bespoke.sphcommercialspace.domain = {};

    <!-- ELEMENT -->
    <xsl:for-each select="xs:element">
      <xsl:choose>
        <!-- Complex TYPE -->
        <xsl:when test="@type">
        </xsl:when>
        <!-- ELEMENT -->
        <xsl:otherwise>

          bespoke.sphcommercialspace.domain.<xsl:value-of select="@name"/> = function(webId) {
          <xsl:choose>
            <xsl:when test="xs:complexType/xs:complexContent/xs:extension">
              var v = new bespoke.sphcommercialspace.domain.<xsl:value-of select="xs:complexType/xs:complexContent/xs:extension/@base"/>(webId);
              <xsl:for-each select="xs:complexType/xs:complexContent/xs:extension/xs:attribute">
                <xsl:if test="@type">
                  v.<xsl:value-of select="@name"/> = ko.observable(<xsl:value-of select="bspk:GetJsDefaultValue(@type, @nillable)"/>);</xsl:if>
              </xsl:for-each>
              <xsl:apply-templates select="xs:complexType/xs:complexContent/xs:extension"/>
              if(bespoke.sphcommercialspace.domain.<xsl:value-of select="@name"/>Partial){
              return _(v).extend(new bespoke.sphcommercialspace.domain.<xsl:value-of select="@name"/>Partial(v));
              }
              return v;
            </xsl:when>
            <xsl:otherwise>
              <!-- attribute-->
              var model =  {
				"$type" : "Bespoke.SphCommercialSpaces.Domain.<xsl:value-of select="@name"/>, domain.commercialspace",
              <xsl:for-each select="xs:complexType/xs:attribute">
                <xsl:if test="@type">
                  <xsl:value-of select="@name"/> : ko.observable(<xsl:value-of select="bspk:GetJsDefaultValue(@type, @nillable)"/>),
                </xsl:if>
              </xsl:for-each>
              <!-- Element -->
              <xsl:apply-templates select="xs:complexType/xs:all/xs:element"/>isBusy : ko.observable(false),
				WebId : ko.observable(webId)
              };
              if(bespoke.sphcommercialspace.domain.<xsl:value-of select="@name"/>Partial){
              return _(model).extend(new bespoke.sphcommercialspace.domain.<xsl:value-of select="@name"/>Partial(model));
              }
              return model;
            </xsl:otherwise>
          </xsl:choose>					};

        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    <!-- COMPLEX TYPE -->
    <xsl:for-each select="xs:complexType">
      bespoke.sphcommercialspace.domain.<xsl:value-of select="@name"/> = function(webId) {
      <!-- attribute-->
      return {
		"$type" : "Bespoke.SphCommercialSpaces.Domain.<xsl:value-of select="@name"/>, domain.commercialspace",
      <xsl:for-each select="xs:attribute">
        <xsl:value-of select="@name"/> : ko.observable(<xsl:value-of select="bspk:GetJsDefaultValue(@type, @nillable)"/>),
      </xsl:for-each>
      <xsl:apply-templates select="xs:all/xs:element"/>isBusy : ko.observable(false),
		WebId : ko.observable(webId)
		};
		};

	</xsl:for-each>
    <!-- enum -->
    <xsl:for-each select="xs:simpleType">
      bespoke.sphcommercialspace.domain.<xsl:value-of select="@name"/> = function()
      {
      return {
      <xsl:for-each select="xs:restriction/xs:enumeration">
        <xsl:value-of select="bspk2:ToConstantUpper(@value)"/> : '<xsl:value-of select="@value"/>',
      </xsl:for-each>
      DO_NOT_SELECT : 'DONTDOTHIS'
      };
      }();
    </xsl:for-each>
  </xsl:template>
  <xsl:include href="ReferenceObject.xslt"/>


  <msxsl:script language="C#" implements-prefix="bspk2">
    <![CDATA[
public string ToConstantUpper(string field)
{
      var text = new System.Text.StringBuilder();
	  var first = true;
      foreach(var c in field)
      {
            if(char.IsUpper(c))
            {
                  if(first)
                  {
                      first = false;
                  }else
                  {
                    text.Append("_");
                  }
            }

            text.Append(c.ToString().ToUpper());

      }

      return text.ToString();
}
	  ]]>

  </msxsl:script>
</xsl:stylesheet>
