RESTORE FILELISTONLY
FROM DISK = 'c:\temp\AdventureWorks2014.bak'

RESTORE DATABASE AdventureWorks
FROM DISK = 'c:\temp\AdventureWorks2014.bak'
WITH MOVE 'AdventureWorks2014_Data' TO 'c:\data\AdventureWorks2014_Data.mdf',
MOVE 'AdventureWorks2014_Log' TO 'c:\Data\AdventureWorks2014_Log.ldf'