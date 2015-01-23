#switch to v1
$pwd = pwd
$sw = [Diagnostics.Stopwatch]::StartNew()

Write-Host "This script will clean all your outputs, and rebuild everything, this include all the dll, pdb, err files" -ForegroundColor Yellow
Write-Host "You should run the  .\SetUpV1Environment.ps1 before running this script" -ForegroundColor Yellow

#remove all V2 dll, and pdb from subscribers, web.sph etc
Write-Host "Cleaning up....." -ForegroundColor Magenta
ls -Filter *.err | Remove-Item
ls -Path .\source -Filter *.pdb -Recurse | Remove-Item
ls -Path .\source -Filter *.dll -Recurse | Remove-Item
& git checkout source/unit.test/mapping.transformation.test/rsc.Driver.dll
& git checkout source/unit.test/mapping.transformation.test/rsc.RilekWeb.dll

Write-Host "Removing packages folder is recommended for truely clean build, but it might take long time to restore if your internet connection is slow" -ForegroundColor Yellow
Write-Host "Do you want to clean the packages folder ? (y/n) : " -ForegroundColor Yellow -NoNewline
$cleanPackage = Read-Host
if($cleanPackage -eq "y")
{
    # run restore Nuget packages
    if(Test-Path .\source\web\web.sph\obj){
        rmdir .\source\web\web.sph\obj -Recurse -Force
    }
    if(Test-Path .\packages){
        rmdir .\packages -Recurse -Force
    }
    mkdir .\packages
    .\restore-package.ps1
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


Write-Host "Building dependencies" -ForegroundColor Magenta

$dependencies = @("elasticsearch.logger","email.service", "rabbitmq.changepublisher","rabbitmq.persistence","razor.template"`
,"report.sqldatasource","roslyn.scriptengine","sql.repository","sqlmembership.directoryservices","windows.taskschedulers","word.document.generator")


$dependencies | `
%{
    $project = ".\source\dependencies\" + $_ + "\" + $_ + ".csproj"
    $output = ".\source\dependencies\" + $_ + "\bin\Debug"
    Write-Host "Building $_" -ForegroundColor White
    & msbuild $project  /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=$_.err"

    $outputs | %{
        ls -Path $output -Filter *.* | Copy-Item -Force -Destination $_
    }


}


#build subscribers
Write-Host "Building subscribers" -ForegroundColor Magenta
$subscribers = @("subscriber.deletedelay","subscriber.elasticsearch.indexer","subscriber.entities",`
"subscriber.entities.dev","subscriber.infrastructure","subscriber.workflowscheduler","subscriber.workflow",`
"subscriber.persistence","subscriber.watcher","subscriber.version.control","subscriber.trigger","subscriber.report.delivery")
$subscribers | `
%{
    $project = ".\source\subscribers\" + $_ + "\" + $_ + ".csproj"
    $output = ".\source\subscribers\" + $_ + "\bin\Debug"
    Write-Host "Building $_" -ForegroundColor Gray -NoNewline
    Write-Host "....."
    & msbuild $project  /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=$_.err"

    if(Test-Path $output){
        ls -Path $output -Filter *.* | Copy-Item -Force -Destination .\bin\subscribers  
    }
}

& msbuild .\source\subscribers\workers.console.runner\workers.console.runner.csproj /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=workers.console.runner.err"
& msbuild .\source\subscribers\workers.windowsservice.runner\workers.windowsservice.runner.csproj /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=workers.windowsservice.runner.err"


#adapters
Write-Host "Building adapters" -ForegroundColor Magenta
$adapters = @("http.adapter","mysql.adapter","oracle.adapter","sqlserver.adapter")
$adapters | %{
    $project = ".\source\adapters\" + $_ + "\" + $_ + ".csproj"
    $output = ".\source\adapters\" + $_ + "\bin\Debug"
    Write-Host "Building $_" -ForegroundColor Gray
    & msbuild $project  /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=$_.err"


}



#schedulers
Write-Host "Building scheduler" -ForegroundColor Magenta
$adapters = @("scheduler.delayactivity","scheduler.report.delivery","scheduler.workflow.trigger")
$adapters | %{
    $project = ".\source\scheduler\" + $_ + "\" + $_ + ".csproj"
    $output = ".\source\scheduler\" + $_ + "\bin\Debug"
    Write-Host "Building $_" -ForegroundColor Gray
    & msbuild $project  /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=$_.err"


}

#tools
Write-Host "Building tools" -ForegroundColor Magenta
$tools = @("csproj.gen","control.center","dead.letter.viewer","offline.generator","sph.builder")
$tools | %{
    $project = ".\source\tools\" + $_ + "\" + $_ + ".csproj"
    $output = ".\source\tools\" + $_ + "\bin\Debug"
    Write-Host "Building $_" -ForegroundColor Gray
    Write-Host "..."
    & msbuild $project  /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=$_.err"

    if(Test-Path $output){
        ls -Path $output -Filter *.* | Copy-Item -Force -Destination .\bin\tools  
    }
}

Write-Host "Building web.sph and core.sph...." -ForegroundColor Magenta
#build core.sph and web.sph
& msbuild .\source\web\core.sph\core.sph.csproj /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=core.sph.err"
& msbuild .\source\web\web.sph\web.sph.v1.csproj /property:SolutionDir=$pwd /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=web.sph.err"

Write-Host "Succesfully built core.sph and web.sph" -ForegroundColor DarkGray


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
copy .\packages\Microsoft.Owin.Security.3.0.0\lib\net45\Microsoft.Owin.Security.dll .\source\web\web.sph\bin
copy .\packages\RabbitMQ.Client.3.4.0\lib\net35\RabbitMQ.Client.dll .\source\web\web.sph\bin
ls .\packages\Microsoft.Composition.1.0.27\lib\portable-net45+win8+wp8+wpa81\*.dll | Copy-Item -Destination .\bin\tools
ls .\packages -Filter Microsoft.CodeAnalysis.*.dll -Recurse | Copy-Item -Destination .\bin\tools

ls .\packages\Microsoft.Composition.1.0.27\lib\portable-net45+win8+wp8+wpa81\*.dll | Copy-Item -Destination .\bin\subscribers
ls .\packages -Filter Microsoft.CodeAnalysis.*.dll -Recurse | Copy-Item -Destination .\bin\subscribers


ls .\packages\Microsoft.Composition.1.0.27\lib\portable-net45+win8+wp8+wpa81\*.dll | Copy-Item -Destination .\source\unit.test\sqlserver.adapter.test\bin\Debug
ls .\packages -Filter Microsoft.CodeAnalysis.*.dll -Recurse | Copy-Item -Destination .\source\unit.test\sqlserver.adapter.test\bin\Debug
copy .\packages\odp.net.managed.121.1.2\lib\net40\Oracle.ManagedDataAccess.dll .\source\web\web.sph\bin



if($success){
    Write-Host "Successfully building V1, Now starts web.sph on 4436" -ForegroundColor Cyan
}

$sw.Stop()
$sw.Elapsed
Write-Host $sw.Elapsed