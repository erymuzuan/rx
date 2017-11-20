﻿[CmdLetBinding()]
Param(
       [string]$Version = '2.1',
       [string]$AssemblyVersion="1.0.2.1007",
       [string[]] $Destinations = @(".\source")
     )

if((Test-Path(".\source\assembly.versions")) -eq $false)
{ 
    mkdir .\source\assembly.versions
}

$y2012 = [System.DateTime]::Parse("2012-01-01")

foreach($folder in $Destinations){
    ls $folder -Filter *.csproj -Recurse | % { 

            $project = $_.DirectoryName
            $commitId = git log -n 1 --pretty=%h $project
            $rev = git rev-list --all --count $project
            $logDate = git log -n 1 --pretty=%aI $project
            $date = [System.DateTime]::Parse($logDate)
            $days = [System.Convert]::ToInt32( ($date - $y2012).TotalDays)
            Write-Host   " $days $commitId $_"

            $cs = $_.DirectoryName.Replace("$PWD\source\","").Replace("\", "-") +  ".cs"

            $content = "using System.Reflection;`r`n" + `
        "using System.Runtime.InteropServices;`r`n" + `
        "`r`n[assembly: AssemblyCompany(`"Bespoke Technology Sdn. Bhd.`")]" + `
        "`r`n[assembly: AssemblyProduct(`"Rx Developer`")]" + `
        "`r`n[assembly: AssemblyCopyright(`"Copyright (c) Bespoke Technology Sdn. Bhd. 2017`")]" + `
        "`r`n[assembly: AssemblyTrademark(`"Bespoke Technology Sdn. Bhd.`")]" + `
        "`r`n[assembly: AssemblyCulture(`"`")]" + `
        "`r`n[assembly: ComVisible(false)]" + `
        "`r`n[assembly: AssemblyVersion(`"1.0.2.1007`")]" + `
        "`r`n[assembly: AssemblyInformationalVersion(`"$Version.$days.$rev-$commitId`")]" + `
        "`r`n[assembly: AssemblyFileVersion(`"$Version.$days.$rev`")]"
            $content | Out-File -FilePath .\source\assembly.versions\$cs

        }


}
