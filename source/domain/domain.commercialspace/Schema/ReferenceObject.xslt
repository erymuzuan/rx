<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xs="http://www.w3.org/2001/XMLSchema"
  xmlns:ms="urn:schemas-microsoft-com:xslt"
  xmlns:bspk="http://www.bespoke.com.my/"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:bs="http://www.bespoke.com.my/xsd/">
	<xsl:template match="xs:element">
		<!-- Collection -->
		<xsl:for-each select="xs:complexType/xs:sequence/xs:element">
			private readonly <xsl:value-of select="bspk:RemovePrefixDataType(@ref, @maxOccurs, @type)"/> m_<xsl:value-of select="../../../@name"/> = new <xsl:value-of select="bspk:RemovePrefixDataType(@ref, @maxOccurs, @type)"/>();

			///&lt;summary&gt;
			/// <xsl:value-of select="../../../xs:annotation/xs:documentation"/>
			///&lt;/summary&gt;
			[XmlArrayItem("<xsl:value-of select="@ref"/>", IsNullable = false)]
			public <xsl:value-of select="bspk:RemovePrefixDataType(@ref, @maxOccurs, @type)"/> <xsl:value-of select="../../../@name"/>
			{
			get{ return m_<xsl:value-of select="../../../@name"/>;}
			}
		</xsl:for-each>

		<!-- 
      Just plain assocation 1 to 1 
    -->
		<xsl:if test="@ref">
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private <xsl:value-of select="@ref"/> m_<xsl:value-of select="bspk:CamelCase(@ref)"/>
			<xsl:choose>
				<!-- when new is missing, always new them -->
				<xsl:when test="@bs:new='false'">;   </xsl:when>
				<xsl:otherwise>
					=  new <xsl:value-of select="@ref"/>();
				</xsl:otherwise>
			</xsl:choose>
			public const string PropertyName<xsl:value-of select="@ref"/> = "<xsl:value-of select="@ref"/>";
			[DebuggerHidden]

			public <xsl:value-of select="@ref"/><xsl:text> </xsl:text> <xsl:value-of select="@ref"/>
			{
			get{ return m_<xsl:value-of select="bspk:CamelCase(@ref)"/>;}
			set
			{
			m_<xsl:value-of select="bspk:CamelCase(@ref)"/> = value;
			OnPropertyChanged();
			}
			}
		</xsl:if>

		<!-- 
      Element with simple restriction rules- for string and integer
      -->
		<xsl:if test="xs:simpleType">
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private <xsl:value-of select="bspk:GetCLRDataType(xs:simpleType/xs:restriction/@base, xs:simpleType/xs:restriction/@nillable)"/> m_<xsl:value-of select="@name"/>;
			public const string PropertyName<xsl:value-of select="@name"/> = "<xsl:value-of select="@name"/>";

			public <xsl:value-of select="bspk:GetCLRDataType(xs:simpleType/xs:restriction/@base, xs:simpleType/xs:restriction/@nillable)"/> <xsl:value-of select="@name"/>
			{
			get { return m_<xsl:value-of select="@name"/>; }

			set
			{
			<xsl:if test="xs:simpleType/xs:restriction/@base = 'xs:string'">
				if(value.Length &lt; <xsl:value-of select="xs:simpleType/xs:restriction/xs:minLength/@value"/>)
				{
				SetColumnError( PropertyName<xsl:value-of select="@name"/>, "The length is less than <xsl:value-of select="xs:simpleType/xs:restriction/xs:minLength/@value"/>");
				return;
				}

				if( value.Length &gt; <xsl:value-of select="xs:simpleType/xs:restriction/xs:maxLength/@value"/>)
				{
				SetColumnError( PropertyName<xsl:value-of select="@name"/>, "The length is greater than <xsl:value-of select="xs:simpleType/xs:restriction/xs:maxLength/@value"/>");
				return;
				}
			</xsl:if>
			<xsl:if test="xs:simpleType/xs:restriction/@base = 'xs:int'">
				if(value &lt; <xsl:value-of select="xs:simpleType/xs:restriction/xs:minInclusive/@value"/>)
				{
				SetColumnError( PropertyName<xsl:value-of select="@name"/>,"The length is less than <xsl:value-of select="xs:simpleType/xs:restriction/xs:minInclusive/@value"/>");
				return;
				}
				if( value &gt; <xsl:value-of select="xs:simpleType/xs:restriction/xs:maxInclusive/@value"/>)
				{
				SetColumnError( PropertyName<xsl:value-of select="@name"/>,"The value is greater than <xsl:value-of select="xs:simpleType/xs:restriction/xs:maxInclusive/@value"/>");
				return;
				}
			</xsl:if>

			if(m_<xsl:value-of select="@name"/>== value) return;

			m_<xsl:value-of select="@name"/> = value;
			ClearColumnError(PropertyName<xsl:value-of select="@name"/>);
			OnPropertyChanged();
			}
			}
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
		
		if(string.IsNullOrEmpty(value))
		{
			
			if(type == "xs:string") return " ObjectCollection<string> ";
			if(type == "xs:int") return "int ";
		}
		
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
        case "xs:anySimpleType":
          type = "object";
          break;
				default:
					type = xsType;
					break;
			}
			if( nillable) type += "?";
			return type += " ";
		}
    
    
    public string GetClrEqualitySymbol(string name, string xsType, bool nillable)
		{
			string first = name.Substring(0,1).ToLowerInvariant();
			string field = first + name.Substring(1, name.Length - 1);
			
			if( String.Equals("xs:string", xsType, StringComparison.Ordinal))
			{
				return "String.Equals( m_" + field + ", value, StringComparison.Ordinal)";
			}
			if( String.Equals("xs:double", xsType, StringComparison.Ordinal))
			{
        if(nillable)return "Math.Abs(m_" + field +"  ?? 0d - value ?? 0d ) < 0.00001d";
				return "Math.Abs(m_" + field +" - value) < 0.00001d";
			}
			return "m_" + field + " == value";
		
    
		}
	  ]]>

	</msxsl:script>
</xsl:stylesheet>