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


## Progress
![](https://lh3.googleusercontent.com/-gbNnfs9mEV4/Vzk7F6MzsHI/AAAAAAAA8ME/jyrLXJMJvMQgKMmwENM8FBKx2bvTgvtFQCCo/s2048/%255BUNSET%255D)

* Imported rows - the number of rows read from the datasource and successfully submitted to messaging broker, the number in red is the row count where there error when mapping couldn't be carried out successfully. Please refer to the Error tab to see the individual rows.
* Sql Server Queue - The first part is the number of messages still in the queue. For every message there will be `BatchSize` of rows, so if your have 20 messages in your SQL Server queue and your BatchSize is 50, then there are still 1000 rows still yet to be inserted into the SQL Server. The second part is the number of messages processed in one second.
* Elasticsearch Queue - The first part is the number of messages still in the queue. For every message there will be `BatchSize` of rows, so if your have 20 messages in your Elasticsearch queue and your BatchSize is 50, then there are still 1000 rows still yet to be inserted into the Elasticsearch. The second part is the number of messages processed in one second.
* Number of rows available for each SQL Server and Elasticsearch is shown next to the queues. Bear in mind that , if you choose to starts from empty database, i.e. new or you do truncate all data command, these numbers should be the same with total imported rows once the whole process completed.

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


## Error handling

When you are importing lots of data, chance is, there might be some data which cannot be transformed correctly and the final output doesn't fit the SQL schema defined in your `EntityDefinition`.

Rx Developer allow you to edit and continue your import process, with a few steps.

The first kind of error that could happen is the mapping error, where your source could not be transformed successfully to the your `Entity` type using the `TransformDefinition` supplied.  There are various reason why this could happened for example
* Your `TransformDefinition` do not expecting the kind of input format you had designed.
* Your source field could be null
* Your source field could be different type than the one defined in your `TransformDefinition`

When a mapping error occured, the data will be rejected and it will be written to disk in your `/web/App_Data/data-imports/<data-import-definition-id>` folder. There will 2 file types

* .data extension, is the raw data in json format
* .error is the actual error details that happened during transformation, Clik on error details, and the `Exception` will be logged to your console.(use F12 to open your browser's developers tool)

If you could view the `.error` file to view the error detail, that can help you to make decision on what to do next. You have 2 options to fix these errors

1. Change your `TransformDefinition` and recompile
2. Change the data, and re submit. To change the data, click on **Data** column, your raw json will be presented, click `Save and close` will resubmit your data.


or you could ignore the data by choosing **ignore this row** button. This will delete those files and your data will not be part of your import anymore.

## History and resume

All data import execution will be logged and stored in a file located in your `\web\App_Data\data-imports\history` folder. The file format will always be `<data-import-id>-<timestamp>.log`

You can always resume your data import execution if there are more unread rows in the datasource.
