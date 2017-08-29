$xslt	= "Perseus_word_extractor.xslt"
$in		= "Perseus_text_1999.04.0060.xml"
$out	= "Perseus_words.xml"

$transformer = New-Object System.Xml.Xsl.XslCompiledTransform
$transformer.Load($xslt)
$transformer.Transform($in, $out)