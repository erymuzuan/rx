if((Test-Path .\packages) -eq $false){
        mkdir .\packages
}
.\.nuget\NuGet.exe Restore .\sph.all.sln
.\.nuget\NuGet.exe Restore .\sph.core.sln
