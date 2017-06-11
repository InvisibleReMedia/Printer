<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" encoding="utf-8"/>

  <xsl:template match="/Program">
    <xsl:apply-templates select="*" mode="code">
      <xsl:with-param name="offset">
        <xsl:value-of select="0"/>
      </xsl:with-param>
      <xsl:with-param name="align">
        <xsl:value-of select="0"/>
      </xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="conf" mode="code">
    <xsl:text><![CDATA[conf
]]></xsl:text>
    <xsl:apply-templates select="*" mode="code"/>
  </xsl:template>

  <xsl:template match="itemConf" mode="code">
    <xsl:variable name="name">
      <xsl:value-of select="@name"/>
    </xsl:variable>
    <xsl:variable name="val">
      <xsl:value-of select="text()"/>
    </xsl:variable>
    <xsl:value-of select="$name"/>
    <xsl:text><![CDATA[ = "]]></xsl:text>
    <xsl:value-of select="$val"/>
    <xsl:text><![CDATA["
]]></xsl:text>
  </xsl:template>

  <xsl:template match="vars" mode="code">
    <xsl:param name="offset"/>
    <xsl:param name="align"/>
    <xsl:text><![CDATA[vars
]]></xsl:text>
    <xsl:apply-templates select="*" mode="code">
      <xsl:with-param name="offset">
        <xsl:value-of select="$offset"/>
      </xsl:with-param>
      <xsl:with-param name="align">
        <xsl:value-of select="0"/>
      </xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="set" mode="code">
    <xsl:param name="offset"/>
    <xsl:param name="align"/>
    <xsl:variable name="name">
      <xsl:value-of select="@name"/>
    </xsl:variable>
    <xsl:variable name="include">
      <xsl:value-of select="@include"/>
    </xsl:variable>
    <xsl:variable name="indent">
      <xsl:if test="@indented">
        <xsl:value-of select="'true'"/>
      </xsl:if>
      <xsl:if test="@non-indented">
        <xsl:value-of select="'false'"/>
      </xsl:if>
    </xsl:variable>
    <xsl:if test="$include='true'">
      <xsl:call-template name="indent">
        <xsl:with-param name="offset">
          <xsl:value-of select="$offset"/>
        </xsl:with-param>
      </xsl:call-template>
      <xsl:text><![CDATA[var ]]></xsl:text>
      <xsl:if test="$indent='true'">
        <xsl:text><![CDATA[indented ]]></xsl:text>
      </xsl:if>
      <xsl:text><![CDATA[include ]]></xsl:text>
      <xsl:value-of select="@name"/>
      <xsl:text><![CDATA[ = "]]></xsl:text>
      <xsl:value-of select="@file"/>
      <xsl:text><![CDATA[" {
]]></xsl:text>
      <xsl:apply-templates select="vars/*" mode="code">
        <xsl:with-param name="offset">
          <xsl:value-of select="$offset + 1"/>
        </xsl:with-param>
        <xsl:with-param name="align">
          <xsl:value-of select="0"/>
        </xsl:with-param>
      </xsl:apply-templates>
      <xsl:text><![CDATA[}
]]></xsl:text>
    </xsl:if>
    <xsl:if test="$include='false'">
      <xsl:call-template name="indent">
        <xsl:with-param name="offset">
          <xsl:value-of select="$offset"/>
        </xsl:with-param>
      </xsl:call-template>
      <xsl:text><![CDATA[var ]]></xsl:text>
      <xsl:if test="$indent='true'">
        <xsl:text><![CDATA[indented ]]></xsl:text>
      </xsl:if>
      <xsl:value-of select="$name"/>
      <xsl:text><![CDATA[ = "]]></xsl:text>
      <xsl:value-of select="text()"/>
      <xsl:text><![CDATA["
]]></xsl:text>
    </xsl:if>
  </xsl:template>

  <xsl:template match="text" mode="code">
    <xsl:text><![CDATA[text = ]]></xsl:text>
    <xsl:apply-templates select="*" mode="code"/>
  </xsl:template>

  <xsl:template match="var" mode="code">
    <xsl:text><![CDATA[[]]></xsl:text>
    <xsl:value-of select="text()"/>
    <xsl:text><![CDATA[]]]></xsl:text>
  </xsl:template>

  <xsl:template match="const" mode="code">
    <xsl:value-of select="text()"/>
  </xsl:template>

  <xsl:template name="indent">
    <xsl:param name="offset"/>
    <xsl:call-template name="indentbcl">
      <xsl:with-param name="offset">
        <xsl:value-of select="$offset"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="indentbcl">
    <xsl:param name="offset"/>
    <xsl:if test="$offset &gt; 0">
      <xsl:text><![CDATA[	]]></xsl:text>
      <xsl:call-template name="indentbcl">
        <xsl:with-param name="offset">
          <xsl:value-of select="$offset - 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="move">
    <xsl:param name="length"/>
    <xsl:if test="$length &gt; 0">
      <xsl:text> </xsl:text>
      <xsl:call-template name="move">
        <xsl:with-param name="length">
          <xsl:value-of select="$length - 1"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>

