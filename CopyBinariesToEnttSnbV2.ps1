ls -Filter *.dll -Path .\source -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose

 ls -Filter *.pdb -Path .\source -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose



 
 ls -Filter *.pdb -Path .\bin\subscribers.host -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose

 
 ls -Filter *.exe -Path .\bin\subscribers.host -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose
 
 
 ls -Filter *.dll -Path .\bin\subscribers.host -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose


 # tools pdb
 ls -Filter *.pdb -Path .\bin\tools -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose

 
 # tools exe
 ls -Filter *.exe -Path .\bin\tools -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose
 
 
 # tools dll
 ls -Filter *.dll -Path .\bin\tools -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose


 
 # schedulers pdb
 ls -Filter *.pdb -Path .\bin\schedulers -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose

 
 # schedulers exe
 ls -Filter *.exe -Path .\bin\schedulers -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose
 
 
 # schedulers dll
 ls -Filter *.dll -Path .\bin\schedulers -Recurse | `
 ? {$_.FullName.Contains('obj') -eq $false -and $_.Name.EndsWith('resources.dll') -eq $false -and `
 $_.FullName.Contains('roslyn') -eq $false `
 -and $_.Name.StartsWith('DevV1') -eq $false `
 -and $_.FullName.Contains('unit.test\') -eq $false `
 -and $_.PSIsContainer -eq $false `
 } | Copy-Item -Destination ..\entt.snb.v2\libs\rx -Verbose

 
 
ls .\source\dependencies\roslyn.scriptengine\bin\Debug\roslyn.scriptengine.pdb | copy -Destination ..\entt.snb.v2\libs\rx
ls .\source\dependencies\roslyn.scriptengine\bin\Debug\roslyn.scriptengine.dll | copy -Destination ..\entt.snb.v2\libs\rx