<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:bspk="http://www.bespoke.com.my/"
        xmlns:bs="http://www.bespoke.com.my/xsd/"
	xmlns:xs="http://www.w3.org/2001/XMLSchema">
  <xsl:output method="text" />
  <xsl:template match="xs:schema">
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.ComponentModel.DataAnnotations;

    namespace Bespoke.SphCommercialSpaces.Domain
    {
    <!-- ELEMENT -->
    <xsl:for-each select="xs:element">
      <xsl:choose>
        <!-- Complex TYPE -->
        <xsl:when test="@type">
        </xsl:when>
        <!-- ELEMENT -->
        <xsl:otherwise>
          ///&lt;summary&gt;
          /// <xsl:value-of select="xs:annotation/xs:documentation"/>
          ///&lt;/summary&gt;
          [DataObject(true)]
          [Serializable]
          [XmlType("<xsl:value-of select="@name"/>",  Namespace=Strings.DefaultNamespace)]
          public <xsl:value-of select="@bs:inheritance"/> partial class <xsl:value-of select="@name"/>
          {

          <!-- attribute-->
          <xsl:for-each select="xs:complexType/xs:attribute">
            <xsl:choose>
              <xsl:when test="@type">
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/> m_<xsl:value-of select="bspk:CamelCase(@name)"/>;
                public const string PropertyName<xsl:value-of select="@name"/> = "<xsl:value-of select="@name"/>";

              </xsl:when>
              <xsl:otherwise>
                [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private  <xsl:value-of select="bspk:GetCLRDataType(xs:simpleType/xs:restriction/@base, @nillable)"/> m_<xsl:value-of select="bspk:CamelCase(@name)"/>;
                public const string PropertyName<xsl:value-of select="@name"/> = "<xsl:value-of select="@name"/>";

              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>

          <!-- Element -->
          <xsl:for-each select="xs:complexType/xs:all/xs:element[@type != '']">
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
                private <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/> m_<xsl:value-of select="bspk:CamelCase(@name)"/>
                <xsl:if test="@bs:new='true'">
                  =  new <xsl:value-of select="@type"/>()
                </xsl:if>;
                public const string PropertyName<xsl:value-of select="@name"/> = "<xsl:value-of select="@name"/>";

         
          </xsl:for-each>
          <xsl:apply-templates select="xs:complexType/xs:all/xs:element"/>
          <xsl:for-each select="xs:complexType/xs:attribute">
            ///&lt;summary&gt;
            /// <xsl:value-of select="xs:annotation/xs:documentation"/>
            ///&lt;/summary&gt;
            [XmlAttribute]
            <xsl:if test="@use='required'">
              [Required]
            </xsl:if>
            [DebuggerHidden]
            <xsl:choose>
              <xsl:when test="@type">
                public <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/>
                <xsl:value-of select="@name"/>
                {
                set
                {
                if( <xsl:value-of select="bspk:GetCLREqualitySymbol(@name, @type)"/>) return;
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

              </xsl:when>
              <xsl:otherwise>
                <!-- string length-->
                <xsl:if test="xs:simpleType/xs:restriction/xs:minLength/@value">
                  [StringLength(<xsl:value-of select="xs:simpleType/xs:restriction/xs:maxLength/@value"/>, MinimumLength = <xsl:value-of select="xs:simpleType/xs:restriction/xs:minLength/@value"/>)]
                </xsl:if>
                <!-- Range -->
                <xsl:if test="xs:simpleType/xs:restriction/xs:minInclusive/@value">
                  [Range(<xsl:value-of select="xs:simpleType/xs:restriction/xs:minInclusive/@value"/>,<xsl:value-of select="xs:simpleType/xs:restriction/xs:maxInclusive/@value"/>)]
                </xsl:if>
                public <xsl:value-of select="bspk:GetCLRDataType(xs:simpleType/xs:restriction/@base, @nillable)"/>
                <xsl:value-of select="@name"/>
                {
                set
                {
                if( <xsl:value-of select="bspk:GetCLREqualitySymbol(@name, xs:simpleType/xs:restriction/@base)"/>) return;
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

              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
          <xsl:for-each select="xs:complexType/xs:all/xs:element[@type != '']">

            ///&lt;summary&gt;
            /// <xsl:value-of select="xs:annotation/xs:documentation"/>
            ///&lt;/summary&gt;
            [DebuggerHidden]

            public <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/>
            <xsl:value-of select="@name"/>
            {
            set
            {
            if(<xsl:value-of select="bspk:GetCLREqualitySymbol(@name, @type)"/>) return;
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
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    <!-- COMPLEX TYPE -->
    <xsl:for-each select="xs:complexType">
      [XmlType("<xsl:value-of select="@name"/>",  Namespace=Strings.DefaultNamespace)]
      public partial class <xsl:value-of select="@name"/>
      {

      <!-- attribute-->
      <xsl:for-each select="xs:attribute">
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        private  <xsl:value-of select="bspk:GetCLRDataType(@type, @nillable)"/> m_<xsl:value-of select="bspk:CamelCase(@name)"/>;
        public const string PropertyName<xsl:value-of select="@name"/> = "<xsl:value-of select="@name"/>";
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
      public enum <xsl:value-of select="@name"/>
      {
      <xsl:for-each select="xs:restriction/xs:enumeration">
        <xsl:value-of select="@value"/>,
      </xsl:for-each>}
    </xsl:for-each>
    }

  </xsl:template>
  <xsl:include href="ReferenceObject.xslt"/>
</xsl:stylesheet>
