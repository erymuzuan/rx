#REM Create V1 environment

#create database
& sqlcmd -E -S "(localdb)\Projects" -Q "CREATE DATABASE [SphV1]"
& sqlcmd -E -S "(localdb)\Projects" -d "SphV1" -Q "CREATE SCHEMA [DevV1] AUTHORIZATION [dbo]"
& sqlcmd -E -S "(localdb)\Projects" -d "SphV1" -Q "CREATE SCHEMA [Sph] AUTHORIZATION [dbo]"

ls -Filter *.sql -Path .\source\database\Table `
| %{
    $sqlFileName = $_.FullName    
    & sqlcmd -E -S "(localdb)\Projects" -d "SphV1" -i "$sqlFileName"
}


#asp.net memberships
Write-Host "Executing Aspnet membership provider"
Start-Process -RedirectStandardOutput  $true -WindowStyle Hidden -FilePath "C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regsql.exe" `
-ArgumentList  '-E','-S','"(localdb)\Projects"','-d "SphV1"','-A mr'


Write-Host "Aspnet membership has been added"
Write-Host "Please wait....."
#roles
& mru -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c ".\source\web\web.sph\web.config"
& mru -u admin -p 123456 -e admin@bespoke.com.my -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c ".\source\web\web.sph\web.config"



#elastic search indices
curl -Method PUT "http://localhost:9200/devv1"
curl -Method PUT "http://localhost:9200/devv1_sys"

Get-ChildItem -Filter *.json -Path .\source\elasticsearch\mapping `
| %{
    $mappingUri = "http://localhost:9200/devv1_sys/" + $_.Name.ToLowerInvariant().Replace(".json", "") + "/_mapping"
    Write-Host "Creating elastic search mapping for $mappingUri"
    Invoke-WebRequest -Method PUT -Uri $mappingUri -InFile $_.FullName -ContentType "application/javascript"
}



#rabbitmq
.\sph.packages\rabbitmq_server\sbin\rabbitmqctl.bat add_vhost DevV1
.\sph.packages\rabbitmq_server\sbin\rabbitmqctl.bat set_permissions -p DevV1 guest ".*" ".*" ".*"
 

# run restore Nuget packages
.\restore-package.ps1
