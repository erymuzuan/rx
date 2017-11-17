
# mkdir .\source\assembly.versions
<#
     var y2012 = new DateTime(2012, 1, 1);
            var commit = logs.ItemCollection.FirstOrDefault() ?? new CommitLog
            {
                CommitId = "NA",
                DateTime = fileInfo.LastWriteTime,
                Commiter = "NA",
                Comment = "NA"
            };using System.Runtime.InteropServices;
            var version = $"{ConfigurationManager.MajorVersion}.{ConfigurationManager.MinorVersion}.{Convert.ToInt32((commit.DateTime - y2012).TotalDays)}.{logs.TotalRows}";

#>
$y2012 = [System.DateTime]::Parse("2012-01-01")

ls .\source -Filter *.csproj -Recurse | % { 

    $project = $_.DirectoryName
    $commitId = git log -n 1 --pretty=%h $project
    $rev = git rev-list --all --count $project
    $logDate = git log -n 1 --pretty=%aI $project
    $date = [System.DateTime]::Parse($logDate)
    $days = [System.Convert]::ToInt32( ($date - $y2012).TotalDays)
    Write-Host   "$project  $days"

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
"`r`n[assembly: AssemblyInformationalVersion(`"1.0.$days.$rev-$commitId`")]" + `
"`r`n[assembly: AssemblyFileVersion(`"1.0.$days.$rev`")]"
    $content | Out-File -FilePath .\source\assembly.versions\$cs

}
