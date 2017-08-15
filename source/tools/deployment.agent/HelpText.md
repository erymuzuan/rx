# RX Developer data deployment tool
Usage :
deployment.agent <path-to-entity-definition-source>|/e:<entity-definition-name>|/e:entity-definition-id>|/nes

## Skipping elasticsearch
`/nes`
 No elasticsearch migration, specify this switch when you want to skip Elasticsearch

##  EntityDefinition with Treat data as source:
/truncate   will truncate the existing table if exist, and load the data from source files
the default option is to migrate the existing data and append any new source from your source files

## Cleaning
Removing queue for Elasticsearch mapping and SQL table
Exisiting RX Developer implementation comes with a subscriber which automatically rebuild SQL table and a subscriber
to build Elasticsearch mapping every time an EntityDefinition is ""Publish"". This auto rebuild might not be necessary if there's
no change in schema, though these subscribers do check for changes

/clean


## Query deployment history
Specify the EntityDefinition and `/q` switch


## Batch size when reading SQL server table
`/batch-size|batch|size` to specify your batch size when reading SQL Server table. Default values is 1000 rows. This tool will read the SQL server data in
the specified batch size, so even if there's millions of rows, it will not read everything thus making it easy on SQL Server


## CVS
Deployment tool requires commit id and commit comment of the currently deployed EntityDefinition, you can specify one in the .config file

```xml
    <object name="CvsProvider" type="Bespoke.Sph.Mangements.GitCvsProvider, deployment.agent" />
```

All you need to do is implement 2 public methods

```csharp
public string GetCommitId()
{
    return "my-commit-id";
}

public string GetCommitComment(
{
    return "my commit comment";
}
```

## GUI
To start this tool in UI interactive mode .. 
`/gui|ui|i`

![GUI window](https://i.imgur.com/VlZfmxb.png)

1. Your environment target SQL Server
2. Your environment target Elasticsearch
3. Select EntityDefinition to deploy by checking this checkbox
4. Option to skip Elasticsearch, see `/nes` switch
5. Option to truncate data, see `/truncate` switch
6. The last time your EntityDefinition was changed
7. The compiled dll CreationDateTime in your ouput folder
8. The date when the EntityDefinition was last deployed to the target environment

