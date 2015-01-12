SELECT * FROM sys.all_objects
WHERE [schema_id] = 5
AND [type] = 'U'

SELECT * FROM sys.schemas


SELECT * FROM sys.all_objects
WHERE [name] = 'uspUpdateEmployeeHireInfo'


select * from information_schema.PARAMETERS
where SPECIFIC_NAME = 'uspUpdateEmployeeHireInfo'
order by ORDINAL_POSITION



SELECT * FROM sys.all_objects
WHERE [schema_id] = 5
AND [type] IN ('U', 'F','FK')


Select
object_name(rkeyid) Parent_Table,
object_name(fkeyid) Child_Table,
object_name(constid) FKey_Name,
c1.name FKey_Col,
c2.name Ref_KeyCol
From
sys.sysforeignkeys s
Inner join sys.syscolumns c1
on ( s.fkeyid = c1.id And s.fkey = c1.colid )
Inner join syscolumns c2
on ( s.rkeyid = c2.id And s.rkey = c2.colid )
Order by Parent_Table,Child_Table