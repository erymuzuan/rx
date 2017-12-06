ls -Recurse -Filter AssemblyInfo.cs -Path .\source |
%{

    $cs = [System.IO.File]::ReadAllText($_.FullName) 
    [System.IO.File]::WriteAllText($_.FullName, $cs + "`r`n//")
}