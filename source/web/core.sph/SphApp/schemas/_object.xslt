<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:bspk="http://www.bespoke.com.my/"
    xmlns:bs="http://www.bespoke.com.my/xsd/"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:bspk2="http://www.bespoke.com.my/"
    xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xsl:output method="text" />
  <xsl:template match="xs:schema">
    /// &lt;reference path="~/scripts/knockout-3.2.0.debug.js" /&gt;
    /// &lt;reference path="~/Scripts/underscore.js" /&gt;
    /// &lt;reference path="~/Scripts/moment.js" /&gt;

    var bespoke = bespoke || {};
    bespoke.sph = bespoke.sph  || {};
    bespoke.sph.domain = bespoke.sph.domain || {};
    <!-- ELEMENT -->
    <xsl:for-each select="xs:element">
      <xsl:choose>
        <!-- Complex TYPE -->
        <xsl:when test="@type">
        </xsl:when>
        <xsl:when test="xs:annotation/xs:documentation = 'Placeholder'">
          // placeholder for <xsl:value-of select="@name"/>
        </xsl:when>
        <!-- ELEMENT -->
        <xsl:otherwise>

          bespoke.sph.domain.<xsl:value-of select="@name"/> = function(optionOrWebid) {
          <xsl:choose>
            <xsl:when test="xs:complexType/xs:complexContent/xs:extension">
              var v = new bespoke.sph.domain.<xsl:value-of select="xs:complexType/xs:complexContent/xs:extension/@base"/>(optionOrWebid);
              <xsl:for-each select="xs:complexType/xs:complexContent/xs:extension/xs:attribute">
                <xsl:if test="@type">
                  v.<xsl:value-of select="@name"/> = ko.observable(<xsl:value-of select="bspk:GetJsDefaultValue(@type, @nillable)"/>);
                </xsl:if>
              </xsl:for-each>
              <xsl:apply-templates select="xs:complexType/xs:complexContent/xs:extension"/>

              if (optionOrWebid &amp;&amp; typeof optionOrWebid === "object") {
              for (var n in optionOrWebid) {
              if (typeof v[n] === "function") {
              v[n](optionOrWebid[n]);
              }
              }
              }
              if (optionOrWebid &amp;&amp; typeof optionOrWebid === "string") {
              v.WebId(optionOrWebid);
              }


              if(bespoke.sph.domain.<xsl:value-of select="@name"/>Partial){
              return _(v).extend(new bespoke.sph.domain.<xsl:value-of select="@name"/>Partial(v));
              }
              return v;
            </xsl:when>
            <xsl:otherwise>
              <!-- attribute-->
              var model =  {
              "$type" : "Bespoke.Sph.Domain.<xsl:value-of select="@name"/>, domain.sph",
              <xsl:if test="@bspk:entity">Id : ko.observable("0"),
              </xsl:if>
              <xsl:for-each select="xs:complexType/xs:attribute">
                <xsl:if test="@type">
                  <xsl:value-of select="@name"/> : ko.observable(<xsl:value-of select="bspk:GetJsDefaultValue(@type, @nillable)"/>),
                </xsl:if>
              </xsl:for-each>
              <!-- Element -->
              <xsl:apply-templates select="xs:complexType/xs:all/xs:element"/>isBusy : ko.observable(false),
              WebId : ko.observable()
              };
              if (optionOrWebid &amp;&amp; typeof optionOrWebid === "object") {
              for (var n in optionOrWebid) {
              if (typeof model[n] === "function") {
              model[n](optionOrWebid[n]);
              }
              }
              }
              if (optionOrWebid &amp;&amp; typeof optionOrWebid === "string") {
              model.WebId(optionOrWebid);
              }


              if(bespoke.sph.domain.<xsl:value-of select="@name"/>Partial){
              return _(model).extend(new bespoke.sph.domain.<xsl:value-of select="@name"/>Partial(model));
              }
              return model;
            </xsl:otherwise>
          </xsl:choose>					};

        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    <!-- COMPLEX TYPE -->
    <xsl:for-each select="xs:complexType">
      bespoke.sph.domain.<xsl:value-of select="@name"/> = function(optionOrWebid) {
      <!-- attribute-->
      var model =  {
      "$type" : "Bespoke.Sph.Domain.<xsl:value-of select="@name"/>, domain.sph",
      <xsl:for-each select="xs:attribute">
        <xsl:value-of select="@name"/> : ko.observable(<xsl:value-of select="bspk:GetJsDefaultValue(@type, @nillable)"/>),
      </xsl:for-each>
      <xsl:apply-templates select="xs:all/xs:element"/>isBusy : ko.observable(false),
      WebId : ko.observable()
      };
      if (optionOrWebid &amp;&amp; typeof optionOrWebid === "object") {
      for (var n in optionOrWebid) {
      if (typeof model[n] === "function") {
      model[n](optionOrWebid[n]);
      }
      }
      }
      if (optionOrWebid &amp;&amp; typeof optionOrWebid === "string") {
      model.WebId(optionOrWebid);
      }

      if(bespoke.sph.domain.<xsl:value-of select="@name"/>Partial){
      return _(model).extend(new bespoke.sph.domain.<xsl:value-of select="@name"/>Partial(model));
      }
      return model;
      };

    </xsl:for-each>
    <!-- enum -->
    <xsl:for-each select="xs:simpleType">
      <xsl:choose>
        <xsl:when test="xs:annotation/xs:documentation = 'Placeholder'">
          // placeholder for <xsl:value-of select="@name"/>enum
        </xsl:when>
        <xsl:otherwise>
          bespoke.sph.domain.<xsl:value-of select="@name"/> = function()
          {
          return {
          <xsl:for-each select="xs:restriction/xs:enumeration">
            <xsl:value-of select="bspk2:ToConstantUpper(@value)"/> : '<xsl:value-of select="@value"/>',
          </xsl:for-each>
          DO_NOT_SELECT : 'DONTDOTHIS'
          };
          }();

        </xsl:otherwise>

      </xsl:choose>

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
