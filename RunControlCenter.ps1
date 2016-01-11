Param(
       [switch]$InMemory = $false
     )

$WorkingDirectory = $PWD

$env:RX_DEVV1_HOME = "$PWD\bin\"
$env:RX_DEVV1_ApplicationFullName = "Engineering Team Development"
$env:RX_DEVV1_FromEmailAddress = "erymuzuan@bespoke.com.my"
$env:RX_DEVV1_LoggerWebSocketPort="50238"
$env:RX_DEVV1_RabbitMqBase="$PWD\bin\rabbitmq_base"
$env:RABBITMQ_BASE="$PWD\bin\rabbitmq_base"



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

$cc = ".\bin\control.center\controlcenter.exe"
IF(Test-Path $cc){

    Write-Host "Starting control center..."
    $args = "/log:console /debug"
    if($InMemory){
        $args = "/log:console /debug /in-memory-broker"
    }
    Start-Process -FilePath .\controlcenter.exe -ArgumentList $args -WorkingDirectory .\bin\control.center
    
}
ELSE{
    Write-Host "Please compile your console.runner"
}