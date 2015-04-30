Param(
       [switch]$KeepPackages = $false
     )
#switch to v1
$pwd = pwd
$sw = [Diagnostics.Stopwatch]::StartNew()

Write-Host " ==================================================================== "
Write-Host "     This script will clean all your outputs, and rebuild everything,"
Write-Host "     this include all the dll, pdb, err files" 
Write-Host "     You should run the  .\SetUpV1Environment.ps1 before running this script"
Write-Host " ==================================================================== "


#remove all V2 dll, and pdb from subscribers, web.sph etc
Write-Host "Cleaning up....." -ForegroundColor Magenta
ls -Filter *.err | Remove-Item
ls -Path .\source -Filter *.pdb -Recurse | Remove-Item
ls -Path .\source -Filter *.dll -Recurse | Remove-Item
& git checkout source/unit.test/mapping.transformation.test/rsc.Driver.dll
& git checkout source/unit.test/mapping.transformation.test/rsc.RilekWeb.dll
& git checkout .\source\unit.test\assembly.test\DevV1.Customer.dll
& git checkout .\source\unit.test\mapping.transformation.test\DevV1.Customer.dll
& git checkout .\source\unit.test\mapping.transformation.test\DevV1.Patient.dll
& git checkout .\source\unit.test\mapping.transformation.test\DevV1.District.dll
& git checkout .\source\unit.test\mapping.transformation.test\DevV1.State.dll


if($KeepPackages -eq $false )
{
    Write-Host ""
    Write-Host "  Removing packages folder is recommended for truely clean build,"
    Write-Host "  but it might take long time to restore if your internet connection is slow "
    # run restore Nuget packages
    if(Test-Path .\source\web\web.sph\obj){
        rmdir .\source\web\web.sph\obj -Recurse -Force
    }
    if(Test-Path .\packages){
        rmdir -Recurse -Force -WarningAction Continue .\packages
    }
    mkdir .\packages
}


if(Test-Path .\bin\output){
    rmdir -Recurse -Force -WarningAction Continue .\bin\output
}
mkdir .\bin\output


if(Test-Path .\bin\subscribers){
    rmdir -Recurse -Force -WarningAction Continue .\bin\subscribers
}
mkdir .\bin\subscribers

if(Test-Path .\source\web\web.durandal){
    rmdir -Recurse -Force .\source\web\web.durandal -WarningAction Continue
}
ls -Path .\source -Filter Debug -Recurse | Remove-Item -Force -Recurse -WarningAction Continue

ls -Path .\bin\subscribers -Filter *.* -Recurse | Remove-Item -Recurse -Force -WarningAction Continue
ls -Path .\bin\subscribers.host -Filter *.* -Recurse | Remove-Item -Recurse -Force -WarningAction Continue
ls -Path .\bin\tools -Filter *.* -Recurse | Remove-Item -Recurse -Force -WarningAction Continue
ls -Path .\bin\schedulers -Filter *.* -Recurse | Remove-Item -Recurse -Force -WarningAction Continue

#restore the NuGet packages
.\restore-package.ps1



$outputs = @(".\source\web\web.sph\bin", ".\bin\subscribers", ".\bin\subscribers.host", ".\bin\schedulers", ".\bin\tools")


function Parallel-Build {
  param(
    [Parameter(Mandatory=$true)]
    [string]
    $Name,
    [Parameter(Mandatory=$true)]
    [string]
    $folder,
    [Parameter(Mandatory=$true)]
    [string[]]
    $projects,
    [Switch]$CopyToOutput

  )
    Write-Host ""
    Write-Host "Building $Name" -ForegroundColor Magenta
    Write-Host "---------------------------------------"

  #  $projects | `
  #  %{
  #      Write-Host "$_  ..." -ForegroundColor DarkGray
        $project = ".\source\$folder\$_\$_.csproj"
        # $arg = @("$project" ,"/p:SolutionDir=$pwd","/p:Configuration=Debug", "/nologo", "/noconsolelogger", "/fileLogger", "/flp2:errorsonly;logfile=$_.err")
        #Start-Process msbuild -WorkingDirectory "." -WindowStyle Hidden -ArgumentList $arg > output.txt
  

  #  }

  #  $msbuilds = gps msbuild* | measure
  #  while($msbuilds.Count -gt 0){
  #      Start-Sleep -Milliseconds 500
  #      Write-Host "." -NoNewline    
  #      $msbuilds = gps msbuild* | measure
  #  }

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
    Write-Host "...done" -NoNewline
    Write-Host ""

}


$vs = gps devenv* | measure
if($vs.Count -gt 0){
    Write-Host -ForegroundColor Yellow -BackgroundColor Blue " Please close any running instance of Visual Studio `r`n [ENTER] to continue"
    
    Read-Host
}

# kill all the msbuild instance and rebuild all
gps msbuild* | kill
msbuild .\sph.all.sln /m /p:Configuration=Debug /p:Platform="Any CPU"

#build  dependencies
$domains = @("domain.sph", "trigger.action.messaging")
Parallel-Build -Name "domain" -folder "domain" -projects $domains

$dependencies = @("elasticsearch.logger","email.service", "rabbitmq.changepublisher","razor.template"`
,"report.sqldatasource","roslyn.scriptengine","sql.repository","sqlmembership.directoryservices","windows.taskschedulers","word.document.generator")

Parallel-Build -Name "dependencies" -folder "dependencies" -projects $dependencies -CopyToOutput


#build subscribers
$subscribers = @("subscriber.deletedelay","subscriber.elasticsearch.indexer","subscriber.entities",`
"subscriber.entities.dev","subscriber.infrastructure","subscriber.workflowscheduler","subscriber.workflow",`
"subscriber.persistence","subscriber.watcher","subscriber.version.control","subscriber.trigger","subscriber.report.delivery")

Parallel-Build -Name "Subscribers" -folder "subscribers" -projects $subscribers
$subscribers | %{
    $output = ".\source\subscribers\" + $_ + "\bin\Debug"   
    if(Test-Path $output){
        ls -Path $output -Filter *.* | Copy-Item -Force -Destination .\bin\subscribers  
    }
}

Parallel-Build -Name "workers" -folder "subscribers" -projects @("workers.console.runner","workers.windowsservice.runner")


#adapters
$adapters = @("http.adapter","mysql.adapter","oracle.adapter","sqlserver.adapter")
Parallel-Build -Name "adapters" -folder "adapters" -projects $adapters -CopyToOutput


#schedulers
Parallel-Build -Name "schedulers" -folder "scheduler" -projects @("scheduler.delayactivity","scheduler.report.delivery","scheduler.workflow.trigger")


#tools
$tools = @("csproj.gen","control.center","dead.letter.viewer","offline.generator","sph.builder")
Parallel-Build -Name "tools" -folder "tools" -projects $tools
$tools | %{
    $output = ".\source\tools\" + $_ + "\bin\Debug"   
    if(Test-Path $output){
        ls -Path $output -Filter *.* | Copy-Item -Force -Destination .\bin\tools  
    }
}

#build core.sph and web.sph
Parallel-Build -Name "web" -folder "web" -projects @("core.sph")
& msbuild .\source\web\web.sph\web.sph.csproj /p:SolutionDir=$pwd /p:Configuration="Debug" /p:Platform="x64" /nologo /noconsolelogger /fileLogger /flp2:"errorsonly;logfile=web.sph.v1.err"


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
$Microsoft_Owin = ls .\packages\Microsoft.Owin.3.0.0\lib\net45\Microsoft.Owin.dll
$Microsoft_Owin_Security = ls .\packages\Microsoft.Owin.Security.3.0.0\lib\net45\Microsoft.Owin.Security.dll
$RabbitmMq_Client = ls .\packages\RabbitMQ.Client.3.4.0\lib\net35\RabbitMQ.Client.dll
$Microsoft_Compostition = ls .\packages\Microsoft.Composition.1.0.27\lib\portable-net45+win8+wp8+wpa81\*.dll
$Microsoft_Code_Analysis = ls .\packages -Filter Microsoft.CodeAnalysis.*.dll -Recurse
$odp_managed_data_provider = ls .\packages\odp.net.managed.121.1.2\lib\net40\Oracle.ManagedDataAccess.dll
$log4net = ls .\packages\log4net.2.0.3\lib\net40-full\log4net.dll
$user_dll = ls -Path .\bin\output -Filter DevV1.*
$web_console_logger = ls .\source\dependencies\web.console.logger\bin\Debug\web.console.logger.dll
$web_bin = ".\source\web\web.sph\bin"
$tools_bin = ".\bin\tools"
$subscribers_bin = ".\bin\subscribers"
$subscribers_host_bin = ".\bin\subscribers.host"
$schedulers_bin = ".\bin\schedulers"
$control_center_bin = ".\bin\control.center"

$user_dll

$Microsoft_Owin_Security | Copy-Item -Destination $web_bin
$Microsoft_Owin | Copy-Item -Destination $web_bin
$RabbitmMq_Client | Copy-Item -Destination $web_bin

$Microsoft_Compostition | Copy-Item -Destination $tools_bin
$Microsoft_Code_Analysis | Copy-Item -Destination $tools_bin

$Microsoft_Compostition | Copy-Item -Destination $subscribers_bin
$Microsoft_Code_Analysis | Copy-Item -Destination $subscribers_bin


$Microsoft_Compostition| Copy-Item -Destination .\source\unit.test\sqlserver.adapter.test\bin\Debug
$Microsoft_Code_Analysis | Copy-Item -Destination .\source\unit.test\sqlserver.adapter.test\bin\Debug
$odp_managed_data_provider| Copy-Item -Destination .\source\web\web.sph\bin


$Microsoft_Compostition | Copy-Item -Destination .\source\unit.test\mapping.transformation.test\bin\Debug
$Microsoft_Code_Analysis | Copy-Item -Destination .\source\unit.test\mapping.transformation.test\bin\Debug

$log4net | Copy-Item -Destination $subscribers_host_bin
$log4net | Copy-Item -Destination $control_center_bin

$user_dll | Copy-Item -Destination $subscribers_bin
$user_dll | Copy-Item -Destination $schedulers_bin
$user_dll | Copy-Item -Destination $web_bin



$web_console_logger | Copy-Item -Destination $subscribers_bin
$web_console_logger | Copy-Item -Destination $subscribers_host_bin
$web_console_logger | Copy-Item -Destination $schedulers_bin
$web_console_logger | Copy-Item -Destination $web_bin


gps msbuild* | kill


#starts elastic search
.\bin\elasticsearch\bin\elasticsearch.bat

#compiles test assets
.\bin\tools\sph.builder.exe .\bin\sources\EntityDefinition\Patient.json
.\bin\tools\sph.builder.exe .\bin\sources\EntityDefinition\Customer.json
.\bin\tools\sph.builder.exe .\bin\sources\EntityDefinition\State.json
.\bin\tools\sph.builder.exe .\bin\sources\EntityDefinition\District.json
.\bin\tools\sph.builder.exe .\bin\sources\EntityDefinition\Appointment.json



if($success){
    Write-Host "Successfully building V1, Now starts web.sph on 4436" -ForegroundColor Cyan
}

$sw.Stop()
Write-Host $sw.Elapsed.Minutes  " minutes and "  $sw.Elapsed.Seconds  " seconds"