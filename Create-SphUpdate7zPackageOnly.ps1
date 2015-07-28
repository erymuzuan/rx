Param(
       [Parameter(Position=0)]
       [int]$Build
     )




if($Build -eq 0)
{
    Write-Host "Please specify the Build Number"
    exit;
}


#compress
& 7za a -t7z ".\$Build.7z" ".\bin\build\*"
