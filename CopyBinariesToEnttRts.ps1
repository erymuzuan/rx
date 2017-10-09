
$toolsFolder = '..\entt.rts\tools'
$webBin = '..\entt.rts\web\bin'
$subscriberHost = '..\entt.rts\subscribers.host'
$schedulersFolder = '..\entt.rts\schedulers'



$domain = @('.\source\domain\domain.sph\bin\Debug\domain.sph.dll', '.\source\domain\domain.sph\bin\Debug\domain.sph.pdb')
$sphBuilder = @('.\bin\tools\sph.builder.exe', '.\bin\tools\sph.builder.pdb')
$loggers = ls -Path .\source\web\web.sph\bin -Filter *.logger.* | sort -Descending LastWriteTime | select -First 10
$rabbitMqChangePublisher = @('.\source\dependencies\rabbitmq.changepublisher\bin\Debug\rabbitmq.changepublisher.dll', '.\source\dependencies\rabbitmq.changepublisher\bin\Debug\rabbitmq.changepublisher.pdb')



copy $domain $toolsFolder -Verbose
copy $domain $webBin -Verbose
copy $domain $subscriberHost -Verbose
copy $rabbitMqChangePublisher $webBin -Verbose

copy $sphBuilder $toolsFolder -Verbose

$loggers | Copy -Destination $toolsFolder -Verbose 
$loggers | Copy -Destination $webBin -Verbose 
$loggers | Copy -Destination $subscriberHost -Verbose