Param(
       [switch]$RemovePackages = $false
     )
#switch to v2
$pwd = pwd
$sw = [Diagnostics.Stopwatch]::StartNew()
Write-Host " ==================================================================== "
Write-Host "     This script will clean all your outputs, and rebuild everything,"
Write-Host "     this include all the dll, pdb, err files" 
Write-Host "     You should run the  .\SetUpV1Environment.ps1 before running this script"
Write-Host " ==================================================================== "

#remove all V1 dll, and pdb from subscribers, web.sph etc
Write-Host "Cleaning up....." -ForegroundColor Magenta
ls -Filter *.err | Remove-Item
ls -Path .\source -Filter *.pdb -Recurse | Remove-Item
ls -Path .\source -Filter *.dll -Recurse | Remove-Item
& git checkout source/unit.test/mapping.transformation.test/rsc.Driver.dll
& git checkout source/unit.test/mapping.transformation.test/rsc.RilekWeb.dll

Write-Host "  Removing packages folder is recommended for truely clean build,"
Write-Host "  but it might take long time to restore if your internet connection is slow "

if($RemovePackages)
{
    # run restore Nuget packages
    if(Test-Path .\source\web\web.sph\obj){
        rmdir .\source\web\web.sph\obj -Recurse -Force
    }
    if(Test-Path .\packages){
        rmdir .\packages -Recurse -Force
    }
    mkdir .\packages
}


ls -Path .\bin\subscribers -Filter *.* -Recurse | Remove-Item -Recurse
ls -Path .\bin\subscribers.host -Filter *.* -Recurse | Remove-Item -Recurse
ls -Path .\bin\tools -Filter *.* -Recurse | Remove-Item -Recurse
ls -Path .\bin\schedulers -Filter *.* -Recurse | Remove-Item -Recurse

#restore the NuGet packages
.\restore-package.ps1

#build  dependencies
& msbuild .\source\domain\domain.sph\domain.sph.csproj /p:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=domain.sph.err"

$outputs = @(".\source\web\web.sph\bin", ".\bin\subscribers", ".\bin\subscribers.host", ".\bin\schedulers", ".\bin\tools")


function Parallel-Build {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $name,
    [Parameter(Mandatory=$true)]
    [string]
    $folder,
    [Parameter(Mandatory=$true)]
    [string[]]
    $projects,
    [Switch]$CopyToOutput

  )
    Write-Host ""
    Write-Host "Building $name" -ForegroundColor Magenta
    Write-Host "---------------------------------------"

    $projects | `
    %{
        Write-Host "Building $_" -ForegroundColor White
        $project = ".\source\$folder\$_\$_.csproj"

        $arg = @("$project" ,"/property:SolutionDir=$pwd", "/nologo", "/noconsolelogger", "/fileLogger", "/flp2:errorsonly;logfile=$_.err")
        Start-Process msbuild -WorkingDirectory "." -WindowStyle Hidden -ArgumentList $arg > output.txt
  

    }

    $msbuilds = gps msbuild* | measure
    while($msbuilds.Count -gt 0){
        Start-Sleep -Milliseconds 500
        Write-Host "." -NoNewline    
        $msbuilds = gps msbuild* | measure
    }

    if($CopyToOutput){
        $projects | `
        %{
            $output = ".\source\$folder\" + $_ + "\bin\Debug"       
            $outputs | %{
                ls -Path $output -Filter *.* | Copy-Item -Force -Destination $_
            }
        }
    }
    Write-Host ""
    Write-Host "Done building $name"

}
$dependencies = @("elasticsearch.logger","email.service", "rabbitmq.changepublisher","rabbitmq.persistence","razor.template"`
,"report.sqldatasource","roslyn.scriptengine","sql.repository","sqlmembership.directoryservices","windows.taskschedulers",`
"word.document.generator","durandaljs.form.compiler")

Parallel-Build -name "dependencies" -folder "dependencies" -projects $dependencies -CopyToOutput


#build subscribers
$subscribers = @("subscriber.deletedelay","subscriber.elasticsearch.indexer","subscriber.entities",`
"subscriber.entities.dev","subscriber.infrastructure","subscriber.workflowscheduler","subscriber.workflow",`
"subscriber.persistence","subscriber.watcher","subscriber.version.control","subscriber.trigger","subscriber.report.delivery")

Parallel-Build -name "Subscribers" -folder "subscribers" -projects $subscribers -CopyToOutput
Parallel-Build -name "workers" -folder "subscribers" -projects @("workers.console.runner","workers.windowsservice.runner")


#adapters
$adapters = @("http.adapter","mysql.adapter","oracle.adapter","sqlserver.adapter")
Parallel-Build -name "adapters" -folder "adapters" -projects $adapters -CopyToOutput


#schedulers
Parallel-Build -name "schedulers" -folder "scheduler" -projects @("scheduler.delayactivity","scheduler.report.delivery","scheduler.workflow.trigger")


#tools
$tools = @("csproj.gen","control.center","dead.letter.viewer","offline.generator","sph.builder")
Parallel-Build -name "tools" -folder "tools" -projects $tools
$tools | %{
    $output = ".\source\tools\" + $_ + "\bin\Debug"   
    if(Test-Path $output){
        ls -Path $output -Filter *.* | Copy-Item -Force -Destination .\bin\tools  
    }
}

#build core.sph and web.sph
Parallel-Build -name "web" -folder "web" -projects @("core.sph", "web.sph")



Write-Host "Removing dll.config and xml files from output folders"
$outputs | % {
    ls -Path $_ -Filter *.dll.config | Remove-Item -Recurse
    ls -Path $_ -Filter *.xml | Remove-Item -Recurse
}

#check the build errors
$success = $true
ls -Filter *.err | %{
    if($_.Length -gt 0){
        Write-Host "See the build error in $_" -ForegroundColor Red
        $success = $false
    }else{
        Remove-Item $_.FullName
    }
}

Write-Host "Copying some packages files to output"
$Microsoft_Composition =ls .\packages\Microsoft.Composition.1.0.30\lib\portable-net45+win8+wp8+wpa81\*.dll
$RabbitMq_Client = ls .\packages\RabbitMQ.Client.3.4.3\lib\net35\RabbitMQ.Client.dll
$Microsoft_Owin_Security = ls .\packages\Microsoft.Owin.Security.3.0.0\lib\net45\Microsoft.Owin.Security.dll
$Microsoft_Code_Analysis = ls .\packages -Filter Microsoft.CodeAnalysis.*.dll -Recurse
$odp_net_managed = ls .\packages\odp.net.managed.121.1.2\lib\net40\Oracle.ManagedDataAccess.dll


## copy all the packages to the run directory
$Microsoft_Owin_Security | Copy-Item -Destination .\source\web\web.sph\bin
$RabbitMq_Client | Copy-Item -Destination .\source\web\web.sph\bin

$Microsoft_Composition | Copy-Item -Destination .\bin\tools
$Microsoft_Code_Analysis | Copy-Item -Destination .\bin\tools

$Microsoft_Composition | Copy-Item -Destination .\bin\subscribers
$Microsoft_Code_Analysis | Copy-Item -Destination .\bin\subscribers


$Microsoft_Composition | Copy-Item -Destination .\source\unit.test\sqlserver.adapter.test\bin\Debug
$Microsoft_Code_Analysis | Copy-Item -Destination .\source\unit.test\sqlserver.adapter.test\bin\Debug

$Microsoft_Composition | Copy-Item -Destination .\source\unit.test\durandaljs.compiler.test\bin\Debug
$Microsoft_Code_Analysis | Copy-Item -Destination .\source\unit.test\durandaljs.compiler.test\bin\Debug

$odp_net_managed | Copy-Item -Destination  .\source\web\web.sph\bin



if($success){
    Write-Host "Successfully building V2, Now starts web.sph on 8081, core.sph on 8080 and web.durandal on 8082" -ForegroundColor Cyan
}

$sw.Stop()
Write-Host $sw.Elapsed.Minutes  " minutes and "  $sw.Elapsed.Seconds  " seconds"