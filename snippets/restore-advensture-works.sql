RESTORE FILELISTONLY
FROM DISK = 'G:\temp\AdventureWorks2014.bak'

RESTORE DATABASE AdventureWorks
FROM DISK = 'G:\temp\AdventureWorks2014.bak'
WITH MOVE 'AdventureWorks2014_Data' TO 'G:\data\AdventureWorks2014_Data.mdf',
MOVE 'AdventureWorks2014_Log' TO 'G:\Data\AdventureWorks2014_Log.ldf'