#MySql Adapter

MySql adapter allows you to connect to MySql database and your MariaDB databases.


##Connections
![connection](http://i.imgur.com/S1yU2cf.png)

You will need to fill the connection details to your MySql Server, click connect button on the toolbar the select your database and schema, this should be the same item.

#Table adapters
Table Adapters allow you to create a direct table that will provide the following features

* Insert item
* Update item via primary key(s)
* Remove item via primary key(s)
* Select an item via primary key(s)
* Select items based on query  with paging

For each table selected an Adapter class witht the name `<table>Adapter` and a domain object with the same name as the table will be created. The domain object will contains all the columns in the table. While the Adapter class will contains method to access that particular table in your database.


Selecting child tables for your table adapter will create a WebApi that allows you to access child items in `/api/<database>/<schema>/<parent>/{id}/<child>`  for example in `employees` sample database(you can download this from MySql.com). Selecting `empoyees` and the corresponding child table `title` will create a new API endpoint

`api/employees/employees/{emp_no:int}/titles`.

this is the code produced by the Adapter
```c#
[Route("{emp_no:int}/titles")]
public async Task<object> GettitlesByemployees(int emp_no, int page = 1, int size = 40, bool includeTotal = false)       {
	// method body
            
}
```


##Procedure Adapters

Procdure adapters allows you to create a method to invoke MySql Stored Procedure, for each procedure selected 2 classes will be created
* Request class with the name `<ProcedureName>Request`
* Response class with the name `<ProcedureName>Response`

and a method in your `<AdapterName>` class with the name `<ProcedureName>Async`

This is a sample procedure created in the employees database

```SQL
CREATE DEFINER=`root`@`localhost` PROCEDURE `getStaffCountByTitle`(IN title VARCHAR(255), OUT count INT)
BEGIN 
  
    SELECT COUNT(*) INTO count FROM employees.titles
      WHERE 'title' = title;
  
  END
```


and the following code will be generated
```C#
public async Task<GetStaffCountByTitleResponse> GetStaffCountByTitleAsync(GetStaffCountByTitleRequest request)
{
    const string SPROC = "getStaffCountByTitle";

    var sql ="CALL `employees`.`getStaffCountByTitle`(@title,@count);";
    sql +="SELECT CAST(@count AS SIGNED);";
    using(var conn = new MySqlConnection(this.ConnectionString))
    using(var cmd = new MySqlCommand(sql, conn))
    {
        cmd.Parameters.AddWithValue("@title", request.@title);
        await conn.OpenAsync();
        var response = new GetStaffCountByTitleResponse();
        using(var reader = await cmd.ExecuteReaderAsync())
        {
            if(await reader.ReadAsync())
            {
                response.@count = (long)reader[0];
            }
        }
        return response;
    }
}


```


Note : you will need to add `Allow User Variables=true;` in your connection string for any procedure with `OUT` parameter