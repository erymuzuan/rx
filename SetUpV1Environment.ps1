#REM Create V1 environment

$applicationName = "DevV1"
$outputLog = ".\checkdatabase.log"

#prerequisites

Write-Host "Checking sqllocaldb" -ForegroundColor Cyan
Start-Process sqllocaldb -ArgumentList @("i") -Wait -RedirectStandardOutput $outputLog -NoNewWindow
if(Test-Path $outputLog){
    $selectDatabase =  (Get-Content $outputLog) | Out-String
    if($selectDatabase.Contains("Projects") -eq $false)
    {
        Start-Process sqllocaldb -ArgumentList @("c", "Projects") -Wait -NoNewWindow
    }
   
    Remove-Item $outputLog
}

Write-Host "....done"
Write-Host ""

## database

Write-Host "Checking database" -ForegroundColor Cyan
$checkDatabase = Start-Process sqlcmd -ArgumentList @("-E","-S `"(localdb)\Projects`"", "-d master", "-Q `"SELECT [Name] FROM sysdatabases WHERE [Name]=`'$applicationName`'`"") -Wait -RedirectStandardOutput $outputLog -NoNewWindow
if(Test-Path $outputLog){
    $selectDatabase =  (Get-Content $outputLog) | Out-String
    if($selectDatabase.Contains($applicationName))
    {
        Write-Host " $applicationName database already exist, Do you want to delete it [y/n]"
        $deleteDatabase = Read-Host
        if($deleteDatabase -eq "y"){            
            & sqlcmd -E -S "(localdb)\Projects" -Q "DROP DATABASE [$applicationName]"
        }
    }
   
    Remove-Item $outputLog
}
Write-Host "....done"
Write-Host ""


#check rabbitmq
Write-Host "Checking RabbitMQ" -ForegroundColor Cyan
$erl = Get-Process "erl*" | measure
if($erl.Count -eq 0){
    Start-Process .\sph.packages\rabbitmq_server\sbin\rabbitmq-server.bat
    Write-Host "Starting rabbit mq server, waiting for 15 seconds"
    Start-Sleep -Seconds 15
}

& .\sph.packages\rabbitmq_server\sbin\rabbitmqctl.bat list_vhosts > $outputLog
if(Test-Path $outputLog){
    $vhosts =  (Get-Content $outputLog) | Out-String

    if($vhosts.Contains($applicationName) -eq $false){        
        & .\sph.packages\rabbitmq_server\sbin\rabbitmqctl.bat add_vhost $applicationName
        Write-Host "setting permission for guest"
        & Start-Process -FilePath ".\sph.packages\rabbitmq_server\sbin\rabbitmqctl.bat" -ArgumentList @("set_permissions", "-p", $applicationName,"guest", "`".*`" `".*`" `".*`"") -NoNewWindow
    }
   
    Remove-Item $outputLog
}
Write-Host "....done"
Write-Host ""

#verify that elastic search 
Write-Host "Checking RabbitMQ" -ForegroundColor Cyan
$es = "http://localhost:9200/"
$esExist = $false;
$indexName = $applicationName.ToLowerInvariant();
Try
{
    $cat_indices = curl -Method GET "$es`_cat/indices" | Out-String
    $esExist = ($cat_indices.Contains("yellow $indexName"))
}
Catch{
    [System.Net.WebException]
    Start-Process .\StartElasticSearch.bat
    Write-Host "Starting elasticsearch, waiting for 15 seconds"
    Start-Sleep -Seconds 16
    
    $cat_indices = curl -Method GET "$es`_cat/indices" | Out-String
    $esExist = ($cat_indices.Contains("yellow $indexName"))
}

if($esExist){
    Write-Host " The index already exist, Do you want to recreate it [y/n]"
    $createIndex = Read-Host
    if($createIndex -eq "y"){
        curl -Method Delete "$es$indexName"
    }
    #curl -Method PUT "$es$indexName"
}

Write-Host "....done"
Write-Host ""



Write-Host "Creating database" -ForegroundColor Cyan

& sqlcmd -E -S "(localdb)\Projects" -Q "CREATE DATABASE [$applicationName]"
& sqlcmd -E -S "(localdb)\Projects" -d "$applicationName" -Q "CREATE SCHEMA [$applicationName] AUTHORIZATION [dbo]"
& sqlcmd -E -S "(localdb)\Projects" -d "$applicationName" -Q "CREATE SCHEMA [Sph] AUTHORIZATION [dbo]"

ls -Filter *.sql -Path .\source\database\Table `
| %{
    $sqlFileName = $_.FullName    
    & sqlcmd -E -S "(localdb)\Projects" -d "$applicationName" -i "$sqlFileName"
}
Write-Host "....done"
Write-Host ""



#asp.net memberships
Write-Host "Executing Aspnet membership provider" -ForegroundColor Cyan
Start-Process -RedirectStandardOutput "v1.log" -WindowStyle Hidden -FilePath "C:\Windows\Microsoft.NET\Framework\v4.0.30319\aspnet_regsql.exe" `
-ArgumentList  @("-E","-S",'"(localdb)\Projects"',"-d " + $applicationName,"-A mr")


Write-Host "Aspnet membership has been added"
Write-Host "Please wait....."
#roles
& mru -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c ".\source\web\web.sph\web.config"
& mru -u admin -p 123456 -e admin@bespoke.com.my -r administrators -r developers -c ".\source\web\web.sph\web.config"

Write-Host "....done"
Write-Host ""




Write-Host "Populating elastic search" -ForegroundColor Cyan
$applicationNameLower = $applicationName.ToLower()
#elastic search indices
$response = curl -Method PUT ("http://localhost:9200/" + $applicationNameLower)
Write-Host "Create $applicationNameLower : " + $response.StatusCode
$response = curl -Method PUT ("http://localhost:9200/" + $applicationNameLower + "_sys")
Write-Host "Create $applicationNameLower`_sys : " + $response.StatusCode

Get-ChildItem -Filter *.json -Path .\source\elasticsearch\mapping `
| %{
    $mappingUri = "http://localhost:9200/" + $applicationNameLower + "_sys/" + $_.Name.ToLowerInvariant().Replace(".json", "") + "/_mapping"
    #Write-Host "Creating elastic search mapping for $mappingUri"
    $response = Invoke-WebRequest -Method PUT -Uri $mappingUri -InFile $_.FullName -ContentType "application/javascript"
    Write-Host  $_.Name.ToLowerInvariant().Replace(".json","") " : " $response.StatusCode
}

Write-Host "....done"
Write-Host ""

Write-Host "Now run Build.Version.1.ps1 and create a new IISExpress site to add web.sph.v1 and run on port 4436" -ForegroundColor Yellow
Write-Host "Finish"