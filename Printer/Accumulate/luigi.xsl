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

  <xsl:template match="defs" mode="code">
    <xsl:param name="offset"/>
    <xsl:param name="align"/>
    <xsl:text><![CDATA[defs
]]></xsl:text>
    <xsl:apply-templates select="*" mode="code"/>
  </xsl:template>

  <xsl:template match="accu" mode="code">
    <xsl:variable name="typeName">
      <xsl:value-of select="@typeName"/>
    </xsl:variable>
    <xsl:variable name="fileName">
      <xsl:value-of select="@fileName"/>
    </xsl:variable>
    <xsl:value-of select="$typeName"/>
    <xsl:text><![CDATA[ = "]]></xsl:text>
    <xsl:value-of select="$fileName"/>
    <xsl:text><![CDATA["
]]></xsl:text>
  </xsl:template>

  <xsl:template match="vars" mode="code">
    <xsl:param name="offset"/>
    <xsl:param name="align"/>
    <xsl:text><![CDATA[vars
]]></xsl:text>
    <xsl:apply-templates select="*" mode="codevar"/>
  </xsl:template>

  <xsl:template match="accu" mode="codevar">
    <xsl:variable name="varName">
      <xsl:value-of select="@typeName"/>
    </xsl:variable>
    <xsl:variable name="refName">
      <xsl:value-of select="@fileName"/>
    </xsl:variable>
    <xsl:value-of select="$varName"/>
    <xsl:text><![CDATA[ = ]]></xsl:text>
    <xsl:value-of select="$refName"/>
    <xsl:text><![CDATA[
]]></xsl:text>
  </xsl:template>

  <xsl:template match="copy" mode="code">
    <xsl:param name="offset"/>
    <xsl:param name="align"/>
    <xsl:text><![CDATA[copy
]]></xsl:text>
    <xsl:apply-templates select="*" mode="code"/>
  </xsl:template>

  <xsl:template match="keyval" mode="code">
    <xsl:variable name="varName">
      <xsl:value-of select="@varName"/>
    </xsl:variable>
    <xsl:variable name="refPointer">
      <xsl:value-of select="@refPointer"/>
    </xsl:variable>
    <xsl:variable name="varRef">
      <xsl:value-of select="@varRef"/>
    </xsl:variable>
    <xsl:variable name="refValue">
      <xsl:value-of select="@refValue"/>
    </xsl:variable>
    <xsl:text><![CDATA[$]]></xsl:text>
    <xsl:value-of select="$varName"/>
    <xsl:value-of select="$refPointer"/>
    <xsl:text><![CDATA[ = $]]></xsl:text>
    <xsl:value-of select="$varRef"/>
    <xsl:value-of select="$refValue"/>
    <xsl:text><![CDATA[
]]></xsl:text>
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

