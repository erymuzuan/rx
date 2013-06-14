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
    <!-- collection -->
    <xsl:for-each select="xs:all/xs:element/xs:complexType/xs:sequence/xs:element">
      v.<xsl:value-of select="../../../@name"/> = ko.observableArray();</xsl:for-each>

    <xsl:for-each select="xs:all/xs:element">
      <!-- 
      Just plain assocation 1 to 1 
    -->
      <xsl:if test="@ref">
        v.<xsl:value-of select="@ref"/> = ko.observable(new bespoke.sphcommercialspace.domain.<xsl:value-of select="@ref"/>());</xsl:if>
      <xsl:if test="@nillable">
        v.<xsl:value-of select="@name"/> = ko.observable();//nillable</xsl:if>
      <xsl:if test="@type and not(@nillable)">
        v.<xsl:value-of select="@name"/> = ko.observable();//type but not nillable</xsl:if>

      <!-- 
      Element with simple restriction rules- for string and integer
      -->
      <xsl:if test="xs:simpleType">
        v.<xsl:value-of select="@name"/>= ko.observable(); //enum
      </xsl:if>
    </xsl:for-each>


  </xsl:template>


  <xsl:template match="xs:element">
    <!-- Collection -->
    <xsl:for-each select="xs:complexType/xs:sequence/xs:element">
      <xsl:value-of select="../../../@name"/> : ko.observableArray(),
    </xsl:for-each>

    <!-- 
      Just plain assocation 1 to 1 
    -->
    <xsl:if test="@ref">
      <xsl:value-of select="@ref"/> : ko.observable(new bespoke.sphcommercialspace.domain.<xsl:value-of select="@ref"/>()),
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
		public string RemovePrefixMember(string value, string maxOccur)
		{
			int max;
			string suffix = string.Empty;
			if( int.TryParse(maxOccur, out max))
			{
				if( max > 1) suffix = "Collection";
			}else
			{
				if( maxOccur == "unbounded") suffix = "Collection";
			}
			
			int indexOfColon = value.IndexOf(":") + 1;
			return value.Substring(indexOfColon, value.Length - indexOfColon) + suffix;
		}	
		public string RemovePrefixDataType(string value, string maxOccur, string type)
		{
			int max ;
			string suffix = string.Empty;
			string prefix = string.Empty;
      string className = value;
      if(string.IsNullOrEmpty(value))
      {
        className = type;
      }
			if( int.TryParse(maxOccur, out max))
			{
				if( max > 1) 
				{	
					suffix = ">";
					prefix = "BindingList<";
				}
			}else
			{
				if( maxOccur == "unbounded")
				{	
					suffix = "> ";
					prefix = "ObjectCollection<";
				}
			}
			
			int indexOfColon = value.IndexOf(":") + 1;
			return prefix + className.Substring(indexOfColon, className.Length - indexOfColon) + suffix;
		}
		
		public string GetCLRDataType(string xsType, bool nillable)
		{
			string type = "object";
			switch(xsType)
			{
				case "xs:string":
					type = "string";
					break;
				case "xs:date":
				case "xs:dateTime":
					type = "DateTime";
					break;
				case "xs:int":
					type = "int";
					break;
				case "xs:long":
					type = "long";
					break;
				case "xs:boolean":
					type = "bool";
					break;
				case "xs:float":
					type="float";
					break;
				case "xs:double":
					type = "double";
					break;
				case "xs:decimal":
					type = "decimal";
					break;
				case "State":
					type = "State";
					break;
				default:
					type = xsType;
					break;
			}
			if( nillable) type += "?";
			return type += " ";
		}
    
    
    public string GetCLREqualitySymbol(string name, string xsType)
		{
			string first = name.Substring(0,1).ToLowerInvariant();
			string field = first + name.Substring(1, name.Length - 1);
			
			if( String.Equals("xs:string", xsType, StringComparison.Ordinal))
			{
				return "String.Equals( m_" + field + ", value, StringComparison.Ordinal)";
			}
			return "m_" + field + " == value";
		
    
		}
	  ]]>

  </msxsl:script>
</xsl:stylesheet>