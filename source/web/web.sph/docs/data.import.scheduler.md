# Data Import Scheduler

You can create data import scheduled task within your `DataTransferDefinition` editor


## How to invoke the scheduler manually
In your subscribers folder, you'll find `scheduler.data.import.exe` which takes the following parameters

1. `Id` - which is the id for your `DataTransferDefinition`, this parameter must be the first one to appear and its mandatory
2. `/notificationOnError` - Send an email, as configured in your `config` if errors happened when processing your data transfer
3. `/notificationOnSuccess` - Send an email when your data transfer process has been successfully executed
4. `/truncateData` - Truncate all data in your Elasticsearch and SQL Server database, before executing your data transfer.
5. `/debug` - The process will halt with a 'Press [ENTER] to continue' message, this will allow you to attach a debugger.

Your `scheduler.data.import.exe` is actually a `SignalR` client, that talks to your web server, since the data import endpoint required users in `administrators` or `developers` roles, you must supply them within your `config` file. So there's actually no difference between invoking your data transfer via web interface or the command line tools.

## Configuration options
There things that you can customize how your data import execution behave by modify various option is config file

1. `username` - Username used to connect to your web application
2. `password` - The password for the user.
3. `EmailFrom` - Email address where all the emails will be sent from.
4. `EmailTo` - Email addresses where all the emails will be sent to.

You should also modify the `system.net` setting in the `config` file to reflect your actual SMTP setting

```xml
<system.net>
<mailSettings>
  <smtp deliveryMethod="SpecifiedPickupDirectory">
    <specifiedPickupDirectory pickupDirectoryLocation="C:\temp\sphEmail" />
  </smtp>
</mailSettings>
</system.net>
```
You should change the deliveryMethod to `Network` and specify the appropriate SMTP

```xml
<configuration>
  <system.net>
    <mailSettings>
      <smtp deliveryMethod="network" deliveryFormat="SevenBit"  from="ben@contoso.com">
        <network
          host="localhost"
          port="25"
          defaultCredentials="true"
        />
      </smtp>
    </mailSettings>
  </system.net>
</configuration>
```
