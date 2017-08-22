Param(
       [switch]$RxSource = $false
       
     )
$Project = "$PWD\bin\sources"
$VsCode = "D:\Program Files (x86)\Microsoft VS Code\Code.exe"
if($RxSource.IsPresent){
    $Project =  "$PWD\source"
}

if((Test-Path($VsCode)) -eq $false){
    $VsCode = $env:RX_DevV1_VsCodePath
}
Write-Host "opening visual studio code with $Project"
Start-Process -ArgumentList "$Project" -FilePath $VsCode  -WindowStyle Maximized