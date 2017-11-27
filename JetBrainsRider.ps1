[CmdLetBinding()]
Param(
    [string]$Solution = ".\rx.sql-readonly.sln",
    [string]$Environment="dev.local"
  )
  .\env.devv1.ps1 -Environment $Environment
  & "D:\Program Files\JetBrains\Rider 2017.2\bin\rider64.exe" $Solution