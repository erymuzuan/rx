#MySql Adapter Procedure
This page allows you to customize you procedure request and response object


## Procedure with result set(reader)
If your procedure returns a result set, you must specify this in the response object using `<ResultSet>Collection` member with the type `Collection` and this result set need to contains the same member as your column name

```
CREATE DEFINER=`root`@`localhost` PROCEDURE `getEmployeesByEmpNo`(IN no INT)
BEGIN 
    
   SELECT * FROM employees
     WHERE emp_no = no;
  END

```

In this example, the procedure will return a result set with the following columns

![alt](http://i.imgur.com/FXoEqi2.png)

The you will need to create an exact response object just like this

![alt](http://i.imgur.com/q87Gamf.png)

This will generate an adapter with code like this one
```C#

       public async Task<GetEmployeesByEmpNoResponse> GetEmployeesByEmpNoAsync(GetEmployeesByEmpNoRequest request)
       {
           var sql ="CALL `employees`.`getEmployeesByEmpNo`(@no);";
           using(var conn = new MySqlConnection(this.ConnectionString))
           using(var cmd = new MySqlCommand(sql, conn))
           {
               cmd.Parameters.AddWithValue("@no", request.@no);
               await conn.OpenAsync();
               var response = new GetEmployeesByEmpNoResponse();
               var row = await cmd.ExecuteNonQueryAsync();
               using(var reader = await cmd.ExecuteReaderAsync())
               {
                   while(await reader.ReadAsync())
                   {
                       var item = new Staff();
                       item.emp_no = (int)reader["emp_no"];
                       item.birth_date = (DateTime)reader["birth_date"];
                       item.first_name = (string)reader["first_name"];
                       item.last_name = (string)reader["last_name"];
                       item.gender = (string)reader["gender"];
                       item.hire_date = (DateTime)reader["hire_date"];
                       response.StaffCollection.Add(item);
                   }
               }
               return response;
           }
       }



```