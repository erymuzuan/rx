@echo off
SET RX_DEVV1_HOME=%~dp0%bin
SET RX_DEVV1_RabbitMqPassword=guest
SET RX_DEVV1_RabbitMqBase=%RX_DEVV1_HOME%\rabbitmq_base
SET RX_DEVV1_ElasticsearchIndexNumberOfShards=1
SET RX_DEVV1_IisExpressExecutable=%RX_DEVV1_HOME%\IIS Express\iisexpress.exe
SET RX_DEVV1_ElasticsearchHttpPort=9200
SET RX_DEVV1_ElasticSearchJar=%RX_DEVV1_HOME%\elasticsearch\lib\elasticsearch-1.7.5.jar
SET RX_DEVV1_ElasticsearchClusterName=cluster_WS28_DevV1
SET RX_DEVV1_RabbitMqManagementPort=15672
SET RX_DEVV1_ElasticsearchIndexNumberOfReplicas=0
SET RX_DEVV1_WebsitePort=4436
SET RX_DEVV1_RabbitMqDirectory=%RX_DEVV1_HOME%\rabbitmq_server
SET RX_DEVV1_LoggerWebSocketPort=50238
SET RX_DEVV1_SqlLocalDbName=ProjectsV13
SET RX_DEVV1_SqlConnectionString="Data Source=(localdb)\ProjectsV13;Initial Catalog=DevV1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
SET RX_DEVV1_RabbitMqUserName=guest
SET RX_DEVV1_ElasticsearchNodeName=node_WS28_DevV1
SET RABBITMQ_BASE=%RX_DEVV1_HOME%\rabbitmq_base
SET PATH=%PATH%;%RX_DEVV1_HOME%\rabbitmq_server\bin;
@echo on
echo Your environment has been set to %RX_DEVV1_HOME%