# Rx Developer Configuration

With the release of RX Developer 1.7(build no 10321), we have entirely move the Configuration setting from `*.config` file to `Environment Variable`

Every process has an environment block that contains a set of environment variables and their values. There are 3 types of environment variables: current process environment variables, user environment variables (set for each user) and system environment variables (set for everyone).

By default, a child process inherits the environment variables of its parent process. Programs started by the command processor inherit the command processor's environment variables. So when start the ControlCenter, all your other components will inherit the environment variables from your ControlCenter, this including your

* IIS Express
* Console Worker
* RabbitMq
* Elasticsearch

Thus all your child process will inherits the same Configuration without changing individual .config files in your solution, making Configuration changes a much easier things to do.

The other aspects of Rx Developer Configuration is the fact that it read the values from an Environment Variable in the following order

1. Process
2. User
3. System

So let's say we take a Configuration for `BaseUrl` , Rx Developer will look for Environment Variable `RX_<YOUR_APPLICARION_NAME>_BaseUrl` value in your **Process** first, it can find it then it will return this value from the `Process` Environment Variable, but it cannot find it then it will traverse to **User** Environment Variable followed by **System**. If it cannot find the value in any of those Environment Variable then the default value is returned, in this case it's `http://localhost:4436/`

## Common configuration options

These are sets of shared and common configuration setting

| Key                 | Default value                         | Description                                                     |
|---------------------|:--------------------------------------|:----------------------------------------------------------------|
| Home               | No default                            | Your project base directory, since there's no default you have to specify one|
| BaseUrl             | http://localhost:4436/                | The url for your RX Developer web application                   |
| SqlConnectionString |Data Source=(localdb)\\Projects;Initial Catalog=<APPLICATION_NAME>;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False | Connection string for your Rx Developer database                        |
| SqlPersistenceDelay | 15000                                 | The wait time for the next retry if your Sql Server cannot be reach in miliseconds |
| SqlPersistenceMaxTry | 3                                    | The number of retry to execute your Sql Command, if the number is exceeded an Exception will be thrown and your message will go to the dead-letter-queue |
| ApplicationFullName | Reactive Developer platform showcase  | The text used for the title of your application                 |
| FromEmailAddress    | admin@rxdeveloper.com                 | The email address used by `SmtpClient` for the `From` field     |
| StaticFileCache     | 120                                   | The number of days all static file cache expires when debug=false|
| WorkflowDebuggerPort| 50518                                 | Port number used by Workflow debugger                           |
| EnableOfflineForm   | false                                 | Turn on your offline capability for your web app                |


## Path configuration
Path based configuration basically defines where the specified file or directory is, it takes a little different twist as the the path is depend on your `RX_<APPLICATION_NAME>_HOME` value if no root is specified.

For example if your `RX_<APPLICATION_NAME>_HOME` is `c:\apps\` and you have this scenario
1. You do not specify your `RX_<APPLICATION_NAME>_WebPath`, it will take the default value as `web` and the path is calculated as `c:\apps\web`
2. You specify your `RX_<APPLICATION_NAME>_WebPath` = `src-web` then your web application web folder is calculated as `c:\apps\src-web`
3. You specify your `RX_<APPLICATION_NAME>_WebPath` = `D:\web-app\app01`, then it will be calculated as `D:\web-app\app01`


| Key                     | Default value                         | Description                                                     |
|-------------------------|:--------------------------------------|:----------------------------------------------------------------|
| WebPath                 | web                                   | Your web application root directory                             |
| CompilerOutputPath      | output                                | directory where all you compiled output dll will be created     |
| SourceDirectory         | sources                               | The source directory for your system assets, things like your EntityDefinition, EntityForm, WorkflowDefinition etc, will be saved.                                                                                              |
| GeneratedSourceDirectory| sources/_generated                    | Where the compiler will save your C# generated source code, the source for your dll
| SubscriberPath          | subscribers                           | The directory where your subscribers, triggers dll are, it will used by your subscriber.host |
| ToolsPath               | tools                                 | the directory where your other utility tools are                |
| SchedulerPath           | schedulers                            | The directory for you schedulers
| DelayActivityExecutable | schedulers\scheduler.delayactivity.exe | Your WorkflowDefinition delay activity is fired by task scheduler, this the executable that kicks it off |
| ReportDeliveryExecutable| schedulers\scheduler.report.delivery.exe | Report Deliver scheduler path |
| ScheduledTriggerActivityExecutable| schedulers\scheduler.workflow.trigger.exe | For your scheduled workflow


## RabbitMq connection setting
We finally rid off `IBrokerConnection` object, and all the duplicates that you have to do in all your `*.config` files

| Key                     | Default value                         | Description                                                     |
|-------------------------|:--------------------------------------|:----------------------------------------------------------------|
| RabbitMqUserName        | guest                                 | The user name to connection to the broker                       |
| RabbitMqPassword        | guest                                 | The user password to connect to the broker                      |
| RabbitMqHost            | localhost                             | The broker host address                                         |
| RabbitMqManagementScheme | http                                 | If you have `rabbitmq_management` plugin enabled, this the scheme, it could https |
| RabbitMqPort            | 5672                                  | The port no used by the broker |
| RabbitMqManagementPort  | 15672                                 | If you have `rabbitmq_management` plugin enabled, this the port number |
| RabbitMqVirtualHost     | `<APPLICATION_NAME>`                 | vhost use to isolate you RabbitMq environment, the default value is equal to you APPLICATION_NAME|



## Elasticsearch connection setting

| Key                     | Default value                         | Description                                                     |
|-------------------------|:--------------------------------------|:----------------------------------------------------------------|
| ElasticSearchHost       | http://localhost:9200                 | The full URL to your Elasticsearch Server                       |
| ElasticSearchIndex      | lowered application name              |                        |
| EsIndexingDelay         | 1500                                  | The number in miliseconds to wait if the attempt to update you Elasticsearch fails
| EsIndexingMaxTry        | 3                                     | The number of retry to update your Elasticsearch data           |
