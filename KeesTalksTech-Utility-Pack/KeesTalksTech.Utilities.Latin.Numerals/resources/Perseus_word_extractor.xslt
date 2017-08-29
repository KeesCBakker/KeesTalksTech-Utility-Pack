<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyz'" />
  <xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />

  <xsl:template match="/">
    <ws>
      <xsl:for-each select="//entry">
        <xsl:variable name="word" select="translate(@key, $smallcase, $uppercase)"/>
        <xsl:variable name="word-end" select="substring($word, string-length($word), 1)" />

        <xsl:choose>
          <xsl:when test="$word-end = '1'">
            <w>
              <!-- remove the trailing 1 -->
              <xsl:value-of select="substring($word, 0, string-length($word))"/>
            </w>
          </xsl:when>
          <xsl:when test="
                    $word-end = '2' or 
                    $word-end = '3' or
                    $word-end = '4' or
                    $word-end = '5' or 
                    $word-end = '6' or 
                    $word-end = '7' or 
                    $word-end = '8' or 
                    $word-end = '9'"><!-- do nothing --></xsl:when>
          <xsl:otherwise>
            <w>
              <xsl:value-of select="$word"/>
            </w>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ws>
  </xsl:template>

</xsl:stylesheet>
