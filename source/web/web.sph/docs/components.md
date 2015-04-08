#Components
##Message Broker
The messaging infrastructure for `Rx Developer`, RabbitMQ is the default broker used their to their Open Source nature and ease of use. It's highly scalable with erlang node. Clustering the broker is also very easy to do. It also runs on various platforms.

`Rx Developer` could also be made to run on Microsoft Azure Enterprise Service Bus for cloud messaging solution.


##Persistence Database
Microsoft SQL Server 2012 is the default persistence database for `Rx Developer`. Any other database management system are planned for the future release.


##Read/Search Repository
ElasticSearch is the default analytics and search repository. `Rx Developer` use `aggregations` feature in ElasticSearch thus the minimum required version is V1.0

##Web Server
Microsoft IIS 8 , with .net 4.5.1

##Workers
Subscriber workers is the core service running all the job for `Rx Developer` operations. This is configured as Windows Service for production envrironment and Console application for development and debugging purposes.

## Clients
You'll need somewhat latest browsers, such as Google Chrome, FireFox or Internet Explorer.

Please refer to [CanIUse](http://www.caniuse.com), basically IE 9 and below will give you very bad experience.

We use Chrome Canary for development purpose with knockoutjs extension installed.

