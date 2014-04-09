#update for 10279.ps1
Write-Host "Welcome to update 10729!"
Write-Host "Your application name"
$name = Read-Host
Write-Host "Your IIS Port No"
$port = Read-Host


Write-Host "Press [ENTER] to exit"
Read-Host

#download the the 10278.json to version
$version = Get-Content .\version.json -Raw | ConvertFrom-Json
$baseUri = "http://www.bespoke.com.my/download/10279";

Invoke-WebRequest -Uri "$baseUri/version.10279.json" -Method GET -OutFile .\version.json


$files = @("domain.sph.dll",
"Common.Logging.Core.dll",
"Common.Logging.dll",
"Dev.Account.dll",
"Dev.Building.dll",
"Dev.Customer.dll",
"Dev.Patient.dll",
"domain.sph.dll",
"elasticsearch.logger.dll",
"email.service.dll",
"EPPlus.dll",
"Humanizer.dll",
"Invoke.Docx.dll",
"ledger.msxl.dll",
"log4net.dll",
"Microsoft.Win32.TaskScheduler.dll",
"Monads.NET.dll",
"NCrontab.dll",
"Newtonsoft.Json.dll",
"rabbitmq.changepublisher.dll",
"RabbitMQ.Client.dll",
"razor.template.dll",
"RazorEngine.dll",
"report.sqldatasource.dll",
"Roslyn.Compilers.CSharp.dll",
"Roslyn.Compilers.dll",
"Roslyn.Compilers.VisualBasic.dll",
"roslyn.scriptengine.dll",
"Roslyn.Services.CSharp.dll",
"Roslyn.Services.dll",
"Roslyn.Services.VisualBasic.dll",
"Roslyn.Utilities.dll",
"Spring.Core.dll",
"sql.repository.dll",
"sqlmembership.directoryservices.dll",
"SQLSpatialTools.dll",
"subscriber.elasticsearch.indexer.dll",
"subscriber.entities.dll",
"subscriber.infrastructure.dll",
"subscriber.report.delivery.dll",
"subscriber.trigger.1.dll",
"subscriber.trigger.dll",
"subscriber.version.control.dll",
"subscriber.watcher.dll",
"subscriber.workflow.dll",
"subscriber.workflowscheduler.dll",
"SuperSocket.Common.dll",
"SuperSocket.SocketBase.dll",
"SuperSocket.SocketEngine.dll",
"SuperSocket.WebSocket.dll",
"System.Spatial.dll",
"System.Web.Mvc.dll",
"System.Web.Razor.dll",
"System.Web.WebPages.dll",
"System.Web.WebPages.Razor.dll",
"windows.taskschedulers.dll",
"word.document.generator.dll",
"workflows.10002.0.dll",
"workflows.11004.0.dll",
"workflows.12003.0.dll",
"workflows.2002.33.dll",
"workflows.5002.5.dll",
"workflows.7002.0.dll",
"workflows.7002.1.dll",
"workflows.9002.1.dll"

)

#subscribers
foreach($f in $files){
    Write-Host "Downloading $f ..."
    Invoke-WebRequest -Uri "$baseUri/subscribers/$f" -Method GET -OutFile .\subscribers\$f	
}


