<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" encoding="utf-8"/>

  <xsl:template match="/Program">
      <xsl:apply-templates select="*" mode="code"/>
  </xsl:template>

  <xsl:template match="vars" mode="code">
    <xsl:apply-templates select="*" mode="code"/>
  </xsl:template>

  <xsl:template match="set" mode="code">
    <xsl:variable name="name">
      <xsl:value-of select="@name"/>
    </xsl:variable>
    <xsl:text><![CDATA[var ]]></xsl:text>
    <xsl:value-of select="$name"/>
    <xsl:text><![CDATA[ = "]]></xsl:text>
    <xsl:value-of select="text()"/>
    <xsl:text><![CDATA["
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

</xsl:stylesheet>

