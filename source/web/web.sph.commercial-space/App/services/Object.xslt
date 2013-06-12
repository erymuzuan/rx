<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:bspk="http://www.bespoke.com.my/"
        xmlns:bs="http://www.bespoke.com.my/xsd/"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:bspk2="http://www.bespoke.com.my/"
	xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xsl:output method="text" />
  <xsl:template match="xs:schema">
    /// &lt;reference path="../../scripts/knockout-2.2.1.debug.js" /&gt;


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


          return {
          <!-- attribute-->
          <xsl:for-each select="xs:complexType/xs:attribute">
            <xsl:choose>
              <xsl:when test="@type">
                <xsl:value-of select="@name"/> : ko.observable(),
              </xsl:when>
              <xsl:otherwise>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
          <!-- Element -->
          <xsl:apply-templates select="xs:complexType/xs:all/xs:element"/>isBusy : ko.observable(false),
          WebId : ko.observable(webId)
          };
          };

        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    <!-- COMPLEX TYPE -->
    <xsl:for-each select="xs:complexType">
      [XmlType("<xsl:value-of select="@name"/>",  Namespace=Strings.DEFAULT_NAMESPACE)]
      public partial class <xsl:value-of select="@name"/>
      {

      <!-- attribute-->
      <xsl:for-each select="xs:attribute">
        <!--[DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/> m_<xsl:value-of select="bspk:CamelCase(@name)"/>;
        public const string PropertyName<xsl:value-of select="@name"/> = "<xsl:value-of select="@name"/>";-->
      </xsl:for-each>
      <!-- Element -->
      <xsl:for-each select="xs:all/xs:element[@type != '']">
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/> m_<xsl:value-of select="bspk:CamelCase(@name)"/>
        <xsl:if test="@bs:new='true'">
          =  new <xsl:value-of select="@type"/>()
        </xsl:if>;
        public const string PropertyName<xsl:value-of select="@name"/> = "<xsl:value-of select="@name"/>";
      </xsl:for-each>
      <xsl:apply-templates select="xs:all/xs:element"/>

      // public properties members
      <xsl:for-each select="xs:attribute">


        [XmlAttribute]
        public <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/>
        <xsl:value-of select="@name"/>
        {
        set
        {
        if(m_<xsl:value-of select="bspk:CamelCase(@name)"/>== value) return;
        var arg = new PropertyChangingEventArgs(PropertyName<xsl:value-of select="@name"/>, value);
        OnPropertyChanging(arg);
        if( !arg.Cancel)
        {
        m_<xsl:value-of select="bspk:CamelCase(@name)"/>= value;
        OnPropertyChanged(PropertyName<xsl:value-of select="@name"/>);
        }
        }
        get
        {
        return m_<xsl:value-of select="bspk:CamelCase(@name)"/>;}
        }
      </xsl:for-each>
      <xsl:for-each select="xs:all/xs:element[@type != '']">

        ///&lt;summary&gt;
        /// <xsl:value-of select="xs:annotation/xs:documentation"/>
        ///&lt;/summary&gt;
        public <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/>
        <xsl:value-of select="@name"/>
        {
        set
        {
        if(m_<xsl:value-of select="bspk:CamelCase(@name)"/>== value) return;
        var arg = new PropertyChangingEventArgs(PropertyName<xsl:value-of select="@name"/>, value);
        OnPropertyChanging(arg);
        if(! arg.Cancel)
        {
        m_<xsl:value-of select="bspk:CamelCase(@name)"/>= value;
        OnPropertyChanged(PropertyName<xsl:value-of select="@name"/>);
        }
        }
        get { return m_<xsl:value-of select="bspk:CamelCase(@name)"/>;}
        }
      </xsl:for-each>


      }

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
	  var firts = true;
      foreach(var c in field)
      {
        if(char.IsUpper(c)){
			  if(firts)
			  {
				  text.Append(c.ToString().ToUpper());
				  firts = false;
			    }else
			    {
			    text.Append("_" + c.ToString().ToUpper() );
				
			    }
            }else
		    {
			    text.Append(c.ToString().ToUpper());
		    }
      }
      
      return text.ToString();
		}	
	  ]]>

  </msxsl:script>
</xsl:stylesheet>
