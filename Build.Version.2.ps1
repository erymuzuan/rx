Param(
       [switch]$KeepPackages = $false
     )
#switch to v2

$outputs = @(".\source\web\web.sph\bin", ".\bin\subscribers", ".\bin\subscribers.host", ".\bin\schedulers", ".\bin\tools")
$pwd = pwd
$applicationName = "Dev"


Write-Host " ==================================================================== "
Write-Host "     This script will clean all your outputs, and rebuild everything,"
Write-Host "     this include all the dll, pdb, err files" 
Write-Host "     You should run the  .\SetUpV2Environment.ps1 before running this script"
Write-Host " ==================================================================== "

$msbuild = gps msbuild* | measure 
if($msbuild.Count -gt 0){
    $count = $msbuild.Count
    Write-Host "  You still have $count MsBuild running do you want to kill them, or let me kill them for you? [y/n] : " -NoNewline -BackgroundColor DarkBlue -ForegroundColor White
    $killMsBuild = Read-Host
    if($killMsBuild -eq "y"){
        gps msbuild* | kill
    }
    
}

$sw = [Diagnostics.Stopwatch]::StartNew()


#remove all V2 dll, and pdb from subscribers, web.sph etc
Write-Host "Cleaning up....." -ForegroundColor Magenta
ls -Filter *.err | Remove-Item
ls -Path .\source -Filter *.pdb -Recurse | Remove-Item -Force -WarningAction Continue
ls -Path .\source -Filter *.dll -Recurse | Remove-Item -Force -WarningAction Continue
ls -Path .\source\ -Filter obj -Recurse | ? {$_.PSIsContainer} |  Remove-Item -Force -Recurse -WarningAction Continue

& git checkout source/unit.test/mapping.transformation.test/rsc.Driver.dll
& git checkout source/unit.test/mapping.transformation.test/rsc.RilekWeb.dll
& git checkout source/unit.test/durandaljs.compiler.test/web/bin/System.Runtime.dll

#clean
$outputs | %{
    ls -Path $_ -Filter *.* -Recurse | Remove-Item -Recurse -WarningAction Continue
}

if($KeepPackages -eq $false)
{

    Write-Host "  Removing packages folder is recommended for truely clean build,"
    Write-Host "  but it might take long time to restore if your internet connection is slow "
    # run restore Nuget packages
    if(Test-Path .\source\web\web.sph\obj){
        rmdir .\source\web\web.sph\obj -Recurse -Force
    }
    if(Test-Path .\packages){
        ls -Path .\packages -Recurse | Remove-Item -Force -Recurse -WarningAction Continue
        #Invoke-Command -ScriptBlock { rmdir /S /Q packages }
        Remove-Item .\packages -Recurse -Force -WarningAction Continue
    }
    mkdir .\packages
}


#restore the NuGet packages
.\restore-package.ps1


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
    Write-Host "Copying $name" -ForegroundColor Magenta
    Write-Host "---------------------------------------"
    
    if($CopyToOutput){
        $projects | `
        %{
            Write-Host "$_" -NoNewline
            $output = ".\source\$folder\" + $_ + "\bin\Debug"
            if(Test-Path $output){       
                $outputs | %{
                    Write-Host "." -NoNewline
                    ls -Path $output -Filter *.dll | Copy-Item -Force -Destination $_
                    ls -Path $output -Filter *.pdb | Copy-Item -Force -Destination $_
                }
            }else{
                Write-Host "Cannot find output folder for $_" -ForegroundColor Red
            }
            Write-Host ""
        }
    }
    Write-Host ""
    Write-Host "Done building $name"

}

#build the solution
& msbuild .\sph.all.sln /m

$dependencies = @("elasticsearch.logger","email.service", "rabbitmq.changepublisher","razor.template"`
,"report.sqldatasource","roslyn.scriptengine","sql.repository","sqlmembership.directoryservices","windows.taskschedulers",`
"word.document.generator","durandaljs.form.compiler")

Parallel-Build -name "dependencies" -folder "dependencies" -projects $dependencies -CopyToOutput


#build subscribers
$subscribers = @("subscriber.deletedelay","subscriber.elasticsearch.indexer","subscriber.entities",`
"subscriber.entities.dev","subscriber.infrastructure","subscriber.workflowscheduler","subscriber.workflow",`
"subscriber.persistence","subscriber.watcher","subscriber.version.control","subscriber.trigger","subscriber.report.delivery")

Parallel-Build -name "Subscribers" -folder "subscribers" -projects $subscribers
$subscribers | %{
    $output = ".\source\subscribers\" + $_ + "\bin\Debug"   
    if(Test-Path $output){
        ls -Path $output -Filter *.* | Copy-Item -Force -Destination .\bin\subscribers  
    }
}

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

Write-Host "Removing dll.config and xml files from output folders"
$outputs | % {
    ls -Path $_ -Filter *.appcache | Remove-Item -Recurse
    ls -Path $_ -Filter *.dll.config | Remove-Item -Recurse
    ls -Path $_ -Filter *.xml | Remove-Item -Recurse
    ls -Path $_ -Filter *.manifest | Remove-Item -Recurse
    ls -Path $_ -Filter *.vshost.exe | Remove-Item -Recurse
    ls -Path $_ -Filter *.vshost.exe.config | Remove-Item -Recurse
    ls -Path $_ -Filter *.dll.config | Remove-Item -Recurse
    ls -Path $_ -Filter Common.Logging.pdb | Remove-Item -Recurse
    ls -Path $_ | ?{$_.PSIsContainer} | Remove-Item -Recurse -WarningAction Continue
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
#TODO : thes -Filter should get it from the application name
$filter = "$applicationName.*";
$user_dll = ls -Path .\bin\output -Filter $filter | ? {$_.Extension -eq '.dll' -or $_.Extension -eq '.pdb'}
$user_dll


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

$user_dll | Copy-Item -Destination .\bin\subscribers
$user_dll | Copy-Item -Destination .\bin\schedulers
$user_dll | Copy-Item -Destination .\source\web\web.sph\bin



if($success){
    Write-Host "Successfully building V2, Now starts web.sph on 8081, core.sph on 8080 and web.durandal on 8082" -ForegroundColor Cyan
}

$sw.Stop()
Write-Host $sw.Elapsed.Minutes  " minutes and "  $sw.Elapsed.Seconds  " seconds"