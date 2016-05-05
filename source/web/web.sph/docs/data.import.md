# Data Import

Rx Developers allows you to import data from most common relational database using `Adapter` and `Mapping`

## Toolbar commands
![Toolbar](https://lh3.googleusercontent.com/-Ci4wTLiN76w/VyqQC-3IIuI/AAAAAAAA7yA/-9i88Wuft7wDdAR8l1dHGQqOoINYA9D-ACCo/s2048/%255BUNSET%255D)

#### Save
Allow you to persist your `Data Import` definition, so that you can re-open it latter. A file will be created in your `web\App_Data\data-imports` folder.

#### Open
Allow you to retrieved your saved `Data Import` definition

#### Delete
Remove the saved, currently opened `Data Import` definition

#### Preview
Lets you preview a subset of the data in tabular format, as defined by your SQL query

#### Starts
Run your data import command

#### Stop
Halt the execution of currently running data import process


#### Truncate all data
**Beware** This command will permanently delete all your data as defined by your data import `Entity` property. It will TRUNCATE sql table and completely DELETE your Elasticsearch contents for that particular type. It will also PURGE all your data import messages in your queues.


## General options

#### Name
Define a name for your data import to be easily identifiable

#### Adapter
Select an Adapter for your data source, the Adapter must have at least 1 table

#### Table
The table from the Adapter where your data source is

#### SQL Statement
You may not want every single row, you can define your sql statement here to filter your rows

#### Batch size
The number of rows to be imported in one `message`, the `message` which contains the rows will be delivered to SQL server and Elasticsearch queue.

#### Entity
The destination `Entity`

#### Mapping
Select a `map` to transform your source table to your `Entity`


## Advanced Options

#### Delay throttle
Set a delay to read between batch

#### Ignore Messaging
This should be enabled for pure data import scenario, where it will bypass all RX messaging infrastructure so things like any Trigger(Workflow, emails or actions), AuditTrail will not be executed.

#### SQL Retry
The number if retry should the workers make to INSERT the data, after this number exceeded, the batch will go to the dead letter queue(DLQ)

If leave blank, the default figure is 5 retries

#### SQL Wait
The number in milliseconds the worker will wait when the first attempt failed, the subsequent wait will increase exponentially

If leave blank, the default figure is 2500 milliseconds

#### Elasticsearch Retry
The number if retry should the workers make to INSERT the data, after this number exceeded, the batch will go to the dead letter queue(DLQ)

If leave blank, the default figure is 5 retries


#### Elasticsearch Wait
The number in milliseconds the worker will wait when the first attempt failed, the subsequent wait will increase exponentially

The wait will be `n * wait`, where n is number of attempts, so if you set the wait at 1500 milliseconds, when the first failure happened it will wait for 1500 milliseconds before attempting another retry, but if the second retry also failed, it will wait for 3000 milliseconds( 2 * 1500 milliseconds) before attempting another retry.. and so and so on

If leave blank, the default figure is 1000 milliseconds
