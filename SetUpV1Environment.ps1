#REM Create V1 environment

$applicationName = "DevV1"
#create database
& sqlcmd -E -S "(localdb)\Projects" -Q "CREATE DATABASE [$applicationName]"
& sqlcmd -E -S "(localdb)\Projects" -d "$applicationName" -Q "CREATE SCHEMA [$applicationName] AUTHORIZATION [dbo]"
& sqlcmd -E -S "(localdb)\Projects" -d "$applicationName" -Q "CREATE SCHEMA [Sph] AUTHORIZATION [dbo]"

ls -Filter *.sql -Path .\source\database\Table `
| %{
    $sqlFileName = $_.FullName    
    & sqlcmd -E -S "(localdb)\Projects" -d "$applicationName" -i "$sqlFileName"
}


#asp.net memberships
Write-Host "Executing Aspnet membership provider"
Start-Process -RedirectStandardOutput "v1.log" -WindowStyle Hidden -FilePath "C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regsql.exe" `
-ArgumentList  @("-E","-S",'"(localdb)\Projects"',"-d " + $applicationName,"-A mr")


Write-Host "Aspnet membership has been added"
Write-Host "Please wait....."
#roles
& mru -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c ".\source\web\web.sph\web.config"
& mru -u admin -p 123456 -e admin@bespoke.com.my -r administrators -r developers -c ".\source\web\web.sph\web.config"


$applicationNameLower = $applicationName.ToLower()
#elastic search indices
curl -Method PUT ("http://localhost:9200/" + $applicationNameLower)
curl -Method PUT ("http://localhost:9200/" + $applicationNameLower + "_sys")

Get-ChildItem -Filter *.json -Path .\source\elasticsearch\mapping `
| %{
    $mappingUri = "http://localhost:9200/" + $applicationNameLower + "_sys/" + $_.Name.ToLowerInvariant().Replace(".json", "") + "/_mapping"
    Write-Host "Creating elastic search mapping for $mappingUri"
    Invoke-WebRequest -Method PUT -Uri $mappingUri -InFile $_.FullName -ContentType "application/javascript"
}



#rabbitmq
.\sph.packages\rabbitmq_server\sbin\rabbitmqctl.bat add_vhost $applicationName
.\sph.packages\rabbitmq_server\sbin\rabbitmqctl.bat set_permissions -p $applicationName guest ".*" ".*" ".*"
 

# run restore Nuget packages
.\restore-package.ps1


Write-Host "Now manually create a new IISExpress site to add web.sph.v1 and run on port 4436" -ForegroundColor Yellow
Write-Host "Done....."