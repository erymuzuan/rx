 $extensions = "css","js", "map", "png", "gif", "html", "ico", "woff", "txt", "otf", "eot", "less", "xml"
foreach($ext in $extensions){

 Start-Process -WorkingDirectory ".\" -ArgumentList " a -tzip $ext.zip -r .\source\web\core.sph\**\*.$ext" -FilePath "C:\Program Files\7-Zip\7z.exe"
} 