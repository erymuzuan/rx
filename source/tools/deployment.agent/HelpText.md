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
public string GetCommitId(string file)
{
    return "my-commit-id";
}

public string GetCommitComment(string file)
{
    return "my commit comment";
}
```

## View changes
`/diff|/changes` to view changes made to the `EntityDefinition` from the one deployed on the server. A migration plan file wiill
saved in your `source\MigrationPlan\<ed.Name>-<previous-commit>-<current-commit>`. Edit this file to indicate your Migration strategy for each file

```json

   {
      "WebId": "8cd9193-40eb-4555-8110-6f04c7d6209a",
      "Name": "Mrn",
      "Action": null,
      "NewPath": "$.Mrn",
      "OldPath": "$.Mrn",
      "OldType": null,
      "NewType": null,
      "IsEmpty": true,
      "MigrationStrategy": "Direct"
    },
```
*Sample migration plan for field*

`IsEmpty` when `true` will be ignored by the tool, there are 3 strategies available

1. Direct
2. Script
3. Ignore , the same as setting the `IsEmpty` field to `true`


### Direct
The value for new field will be copied over to the member, only happens when you change the field name without changing the data 
type and other properties like, `AllowMultiple` and 'IsNullable`

```json
    {
      "WebId": "d5f801b0-d07d-4bd3-b0a9-451ea8c50a2f",
      "Name": "Name",
      "Action": "NameChanged",
      "NewPath": "$.ContactPerson.Name",
      "OldPath": "$.NextOfKin.FullName",
      "OldType": null,
      "NewType": null,
      "IsEmpty": false,
      "MigrationStrategy": "Direct"
    }
```


in this example, the value of ` "$.NextOfKin.FullName"` from Json column will be copied to `ContactPerson.Name` field in your
entity.


### Script
Allow you to do almost anything, write a script that return an object that's compatible with your target data type
```json
    {
      "WebId": "1199b1ae-f418-4083-fa2f-1973a46a5155",
      "Name": "Ward",
      "Action": "TypeChanged",
      "NewPath": "$.Ward",
      "OldPath": "$.Ward",
      "OldType": "string",
      "NewType": "int",
      "IsEmpty": false,
      "Script": "<your script goes here>",
      "MigrationStrategy": "Script"
    }
    
    // member definition in entity definition
    {
        "$type": "Bespoke.Sph.Domain.SimpleMember, domain.sph",
        "TypeName": "System.Int32, mscorlib",
        "IsNullable": true,
        "IsNotIndexed": false,
        "Name": "Ward",
        // ... remove for brevity
      }
```



This example, the `Ward` fields data type was changed from `string` to `int`, this we have to write a script

```csharp
// return Nullable int since the new type
public int? GetValue(string source)
{
    var json = Newtonsoft.Json.Linq.JObject.Parse(source);
    var wardText = json.SelectToken("$.Ward").Value<string>();
    if(string.IsNullOrWhiteSpace(wardText))
        return default(int?);
    
    int ward;
    if(int.TryParse(wardText, out ward))
        return ward;
    return default(int?);
    
}

```



### Ignore
Nothing happen


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

