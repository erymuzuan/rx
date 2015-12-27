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
------------------------------

| Key                 | Default value                         | Description                                                     |
|---------------------|:--------------------------------------|:----------------------------------------------------------------|
| Home               | No default                            | Your project base directory, since there's no default you have to specify one|
| BaseUrl             | http://localhost:4436/                | The url for your RX Developer web application                   |
| SqlConnectionString |Data Source=(localdb)\\Projects;Initial Catalog={ApplicationName};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False | Connection string for your Rx Developer database                        |
| SqlPersistenceDelay | 15000                                 | The wait time for the next retry if your Sql Server cannot be reach in miliseconds |
| SqlPersistenceMaxTry | 3                                    | The number of retry to execute your Sql Command, if the number is exceeeded an Exception will be thrown and your message will go to the dead-letter-queue |
| ApplicationFullName | Reactive Developer platform showcase  | The text used for the title of your application                 |
| FromEmailAddress    | admin@rxdeveloper.com                 | The email address used by S`mtpClient` for the `From` field     |
| StaticFileCache     | 120                                   | The number of days all static file cache expires when debug=false|
| WorkflowDebuggerPort| 50518                                 | Port number used by Workflow debugger                           |
| EnableOfflineForm   | false                                 | Turn on your offline capability for your web app                |








```csharp
public static class ConfigurationManager
   {
       public static string ApplicationNameToUpper = ApplicationName.ToUpper();
       public static string ApplicationName => AppSettings["sph:ApplicationName"] ?? "YOUR_APP";

       public static string ApplicationFullName => GetEnvironmentVariable("ApplicationFullName") ?? "Reactive Developer platform showcase";
       public static string FromEmailAddress => GetEnvironmentVariable("FromEmailAddress") ?? "admin@rxdeveloper.com";
       public static int StaticFileCache => GetEnvironmentVariableInt32("StaticFileCache", 120);
       public static int WorkflowDebuggerPort => GetEnvironmentVariableInt32("WorkflowDebuggerPort", 50518);
       public static long EsIndexingDelay => GetEnvironmentVariableInt32("EsIndexingDelay", 15000);
       public static int EsIndexingMaxTry => GetEnvironmentVariableInt32("EsIndexingMaxTry", 3);
       public static long SqlPersistenceDelay => GetEnvironmentVariableInt32("SqlPersistenceDelay", 15000);
       public static int SqlPersistenceMaxTry => GetEnvironmentVariableInt32("SqlPersistenceMaxTry", 3);
       public static bool EnableOfflineForm => GetEnvironmentVariableBoolean("EnableOfflineForm");
       public static string BaseUrl => GetEnvironmentVariable("BaseUrl") ?? "http://localhost:4436";
       public static string Home => GetEnvironmentVariable("HOME");
       public static string CompilerOutputPath => GetPath("CompilerOutputPath", "output");
       /// <summary>
       /// Ad directory where all the sph and systems source code like the *.json file for each asset definitions
       /// </summary>
       public static string SphSourceDirectory => GetPath("SourceDirectory", "sources");
       /// <summary>
       /// A directory where all the users source codes are
       /// </summary>
       public static string GeneratedSourceDirectory => GetPath("GeneratedSourceDirectory", @"sources\_generated\");
       public static string SqlConnectionString => GetEnvironmentVariable("SqlConnectionString") ?? $"Data Source=(localdb)\\Projects;Initial Catalog={ApplicationName};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";

       public static string RabbitMqUserName => GetEnvironmentVariable("RabbitMqUserName") ?? "guest";
       public static string RabbitMqPassword => GetEnvironmentVariable("RabbitMqPassword") ?? "guest";
       public static string RabbitMqHost => GetEnvironmentVariable("RabbitMqHost") ?? "localhost";
       public static string RabbitMqManagementScheme => GetEnvironmentVariable("RabbitMqManagementScheme") ?? "http";
       public static int RabbitMqPort => GetEnvironmentVariableInt32("RabbitMqPort", 5672);
       public static int RabbitMqManagementPort => GetEnvironmentVariableInt32("RabbitMqManagementPort", 15672);
       public static string RabbitMqVirtualHost => GetEnvironmentVariable("RabbitMqVirtualHost") ?? ApplicationName;

       public static string ElasticSearchHost => GetEnvironmentVariable("ElasticSearchHost") ?? "http://localhost:9200";
       public static string ElasticSearchIndex => GetEnvironmentVariable("ElasticSearchIndex") ?? ApplicationName.ToLowerInvariant();
       public static string ReportDeliveryExecutable => GetPath("ReportDeliveryExecutable", @"schedulers\scheduler.report.delivery.exe");
       public static string ScheduledTriggerActivityExecutable => GetPath("ScheduledTriggerActivityExecutable", @"schedulers\scheduler.workflow.trigger.exe");
       public static bool EnableWorkflowGetCacheDependency => GetEnvironmentVariableBoolean("EnableWorkflowGetCacheDependency");

       public static ConnectionStringSettingsCollection ConnectionStrings => System.Configuration.ConfigurationManager.ConnectionStrings;

       public static System.Collections.Specialized.NameValueCollection AppSettings => System.Configuration.ConfigurationManager.AppSettings;
       public static int JpegMaxWitdh => GetEnvironmentVariableInt32("jpg.max.width", 400);

       public static string SchedulerPath => GetPath("SchedulerPath", "schedulers");
       public static string SubscriberPath => GetPath("SubscriberPath", "subscribers");
       public static string ToolsPath => GetPath("ToolsPath", "tools");
       public static string WebPath => GetPath("WebPath", "web");
       public static string DelayActivityExecutable => GetPath("DelayActivityExecutable", @"schedulers\scheduler.delayactivity.exe");

```
