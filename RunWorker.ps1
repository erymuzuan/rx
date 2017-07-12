Param(
       [switch]$InMemory = $false,
       [switch]$Debug = $false,
       [string]$config = "dev"
       
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
   Start-Process -FilePath .\bin\subscribers.host\workers.console.runner.exe `
   -ArgumentList "/log:console /config:$config /u:$env:RX_DEVV1_RabbitMqUserName /p:$env:RX_DEVV1_RabbitMqPassword /h:$env:RX_DEVV1_RabbitMqHost /debug"
}else{
    Start-Process -FilePath .\bin\subscribers.host\workers.console.runner.exe `
    -ArgumentList " /log:console /u:$env:RX_DEVV1_RabbitMqUserName /p:$env:RX_DEVV1_RabbitMqPassword /h:$env:RX_DEVV1_RabbitMqHost /config:$config"
}
    
