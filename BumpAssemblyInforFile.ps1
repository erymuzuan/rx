$aggregate = ls -Recurse -Filter AssemblyInfo.cs -Path .\source | measure
$total = $aggregate.Count;
$done = 0;

ls -Recurse -Filter AssemblyInfo.cs -Path .\source |
%{
    $project = $_.Directory.Parent.Name
    Write-Progress -Activity "Bumping version" -Status "$project ($done/$total)" -PercentComplete ($done*100/$total)
    $cs = [System.IO.File]::ReadAllText($_.FullName) 
    [System.IO.File]::WriteAllText($_.FullName, $cs + "`r`n//")

    git add $_.FullName
    git commit -m "Bumping version"
    $done = $done + 1;
}

