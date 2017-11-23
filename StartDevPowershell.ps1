Param(
    [string]$Environment = "dev.local"
  )
  
  $envScript = "env.devv1.ps1";
  if($Environment -ne "dev.local"){
      $envScript = "env.devv1.$Environment.ps1"
  }

  if((Test-Path($envScript)) -eq $false){
      Write-Warning  "There's no environment called `"$Environment`" registered for Snb";
      return;
  }
  $env:RX_DEVV1_HOME="$PWD\bin"
  $env:RX_DEVV1_WebPath="$PWD\web\web.sph"

  C:\Windows\system32\WindowsPowerShell\v1.0\powershell.exe -NoExit -Command "Set-Variable -Name 'RxApplicationName' -Value 'DevV1'; .\$envScript -CallFromStartSnbPowershell"