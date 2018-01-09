[CmdLetBinding()]
Param(
        [Parameter(Position=0)]
       [string]$Solution = ".\sph.all.sln"
     )
& .\env.devv1.ps1
& "$env:JETBRAINS_RIDER_PATH\bin\rider64.exe" $Solution
