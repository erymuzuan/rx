Param(
       [switch]$InMemory = $false,
       [switch]$Debug = $false,
       [string]$config = "dev"
       
     )

$WorkingDirectory = $PWD

$env:RX_DEVV1_HOME = "$PWD\bin\"
$env:RX_DEVV1_BaseUrl="http://localhost:4436"
$env:RX_DEVV1_SqlConnectionString = "Data Source=(LocalDb)\ProjectsV13;Initial Catalog=DevV1;Integrated Security=SSPI"
$env:RX_DEVV1_ApplicationFullName = "Engineering Team Development"
$env:RX_DEVV1_FromEmailAddress = "erymuzuan@bespoke.com.my"
$env:RX_DEVV1_LoggerWebSocketPort="50238"
$env:RX_DEVV1_RabbitMqBase="$PWD\bin\rabbitmq_base"
$env:RX_DEVV1_WebPath="$PWD\source\web\web.sph"
#$env:RABBITMQ_BASE="$PWD\bin\rabbitmq_base"
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
  & .\bin\subscribers.host\workers.console.runner.exe /log:console /config:$config /debug
}else{
   & .\bin\subscribers.host\workers.console.runner.exe /log:console /config:$config
}
    
