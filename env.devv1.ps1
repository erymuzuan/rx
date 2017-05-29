﻿$RxHome = "$PWD\bin"
$machine = ($env:COMPUTERNAME).Replace("-","_")

[System.Environment]::SetEnvironmentVariable("RX_DEVV1_HOME","$RxHome", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_WebPath","$PWD\source\web\web.sph", "Process")

[System.Environment]::SetEnvironmentVariable("RABBITMQ_BASE","$RxHome\rabbitmq_base", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqUserName","devv1", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqPassword","slayer", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqBase","$RxHome\rabbitmq_base", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqDirectory","$RxHome\rabbitmq_server", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_RabbitMqManagementPort","15672", "Process")


[System.Environment]::SetEnvironmentVariable("RX_DEVV1_IisExpressExecutable","$RxHome\IIS Express\iisexpress.exe", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_WebsitePort","4436", "Process")

[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchIndexNumberOfShards","1", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchIndexNumberOfReplicas","0", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchHttpPort","9200", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticSearchJar","$RxHome\elasticsearch\lib\elasticsearch-1.7.5.jar", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchClusterName","cluster_$machine""_DevV1", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_ElasticsearchNodeName","node_$machine" + "_DevV1", "Process")

[System.Environment]::SetEnvironmentVariable("RX_DEVV1_LoggerWebSocketPort","50238", "Process")

[System.Environment]::SetEnvironmentVariable("RX_DEVV1_SqlLocalDbName","ProjectsV13", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_SqlConnectionString", "Data Source=.\DEV2016;Initial Catalog=DevV1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", "Process")


[System.Environment]::SetEnvironmentVariable("PATH","$env:Path;$RxHome\rabbitmq_server\sbin", "Process")

[System.Environment]::SetEnvironmentVariable("RX_DEVV1_BromConnectionString", "Data Source=S301\DEV2016;Initial Catalog=PittisNonCore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", "Process")
[System.Environment]::SetEnvironmentVariable("RX_DEVV1_SnbWebNewAccount_BaseAddress", "http://eryken2.asuscomm.com:8086", "Process")


[System.Environment]::SetEnvironmentVariable("JAVA_HOME","D:\Program Files\Java\jdk1.8.0_131\jre", "Process")
[System.Environment]::SetEnvironmentVariable("ERLANG_HOME","D:\Program Files\erl8.3", "Process")
