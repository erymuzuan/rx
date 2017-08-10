# RX Developer data deployment tool
Usage :
deployment.agent <path-to-entity-definition-source>|/e:<entity-definition-name>|/e:entity-definition-id>|/nes

/nes No elasticsearch migration, specify this switch when you want to skip Elasticsearch

For EntityDefinition with Treat data as source:
/truncate   will truncate the existing table if exist, and load the data from source files
the default option is to migrate the existing data and append any new source from your source files

Removing queue for Elasticsearch mapping and SQL table
Exisiting RX Developer implementation comes with a subscriber which automatically rebuild SQL table and a subscriber
to build Elasticsearch mapping every time an EntityDefinition is ""Publish"". This auto rebuild might not be necessary if there's
no change in schema, though these subscribers do check for changes

/clean


To start this tool in UI interactive mode .. coming soon
/gui
