Param(
       [switch]$InMemory = $false,
       [switch]$Debug = $false,
       [string]$config = "all",
       [ValidateSet("Debug","Verbose","Info")] 
       [string]$ConsoleLoggerSwitch="Info",
       [ValidateSet("Debug","Verbose","Info")] 
       [string]$FileLoggerSwitch="Info"
       
     )

$WorkingDirectory = $PWD
$env:Path=$env:Path + "$PWD\bin\tools"

& .\env.devv1.ps1

#copy some dependencies
copy .\source\web\web.sph\bin\Common.Logging.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.Mvc.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.Razor.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.Razor.dll .\bin\subscribers


copy .\source\web\web.sph\bin\Common.Logging.dll .\bin\schedulers
copy .\source\web\web.sph\bin\System.Web.Mvc.dll .\bin\schedulers
copy .\source\web\web.sph\bin\System.Web.Razor.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.dll .\bin\subscribers
copy .\source\web\web.sph\bin\System.Web.WebPages.Razor.dll .\bin\subscribers


copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\schedulers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\schedulers

copy source\web\web.sph\bin\System.Web.WebPages.Razor.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.WebPages.dll bin\subscribers
copy source\web\web.sph\bin\System.Web.Mvc.dll bin\subscribers

#delete all *.config in subscribers
ls -Filter *.config -Path .\bin\subscribers | Remove-Item
ls -Filter *.xml -Path .\bin\subscribers | Remove-Item

if($Debug -ne $false){
   Start-Process -FilePath .\bin\subscribers.host\workers.console.runner.exe -WorkingDirectory $PWD `
   -ArgumentList "/config:$config /debug  /out:C:\temp\logs\workers.console.log /outSize:100KB /outSwitch:$FileLoggerSwitch /switch:$ConsoleLoggerSwitch"
}else{
    Start-Process -FilePath .\bin\subscribers.host\workers.console.runner.exe -WorkingDirectory $PWD `
    -ArgumentList "/config:$config /out:C:\temp\logs\workers.console.log /outSize:100KB /outSwitch:$FileLoggerSwitch /switch:$ConsoleLoggerSwitch"
}
    
