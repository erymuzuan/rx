# SQL Server Stored Procedures and Functions
Rx Developer adapter for SQL Server supports native SQL Server

2. Scalar valued functions
4. Table valued functions
1. Stored procedures
3. Inline table valued functions

Once selected RX Developer will generate invocation code in the `Adapter` class that allows you use within your .Net project as well as REST API so that it could be used within environment.
For every operation, there will a set of `Request` and `Response` object. For most parts you don't have to worry about the `Request` object,as RX Developer will automatically take care of this for you.


## Scalar-Valued-Function
Takes this simple Scalar valued function as an example
``` sql

CREATE FUNCTION GetFullName
(
	@FirstName VARCHAR(50),
	@LastName VARCHAR(50)
)
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @FullName varchar(100)

	SELECT @FullName = @FirstName + ' ' + @LastName
	RETURN @FullName

END
```

RX Developer will automatically generates a `Request` object

``` javascript
{
  "@FirstName" : "string",
  "@LastName" : "string"
}
```

and the `Response` object

``` javascript

{
  "@FullName": "string"
}
```

![Scalar](https://lh3.googleusercontent.com/-6sYMPAY4Z0I/V2zSrtKYKpI/AAAAAAAA8jY/pm-GqS0lRkg2w3iD25dis9H-2Gga9hY5ACCo/s2048/%255BUNSET%255D)

1. The `Request` object, the class name will `GetFullNameRequest`
2. The `Response` object, the class name will be `GetFullNameResponse`
3. Sample Scalar valued function.

Then the adapter code will be generated
``` csharp
public async Task<GetFullNameResponse> GetFullNameAsync(GetFullNameRequest request)
{
    using (var conn = new SqlConnection(this.ConnectionString))
    using (var cmd = new SqlCommand("SELECT @FullName = [dbo].[GetFullName](@FirstName, @LastName)", conn))
    {
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@FirstName", request.@FirstName);
        cmd.Parameters.AddWithValue("@LastName", request.@LastName);
        var result = cmd.Parameters.Add("@FullName", SqlDbType.VarChar, 255);
        result.Direction = ParameterDirection.Output;
        await conn.OpenAsync();
        var row = await cmd.ExecuteNonQueryAsync();
        var response = result.Value.ReadNullableString();
        return new GetFullNameResponse { FullName = response };
    }

}
```

This is a very simple example, but it works almost similarly in every scenario for `Scalar-Valued-Function`.

A REST API endpoint will also be created.

```csharp
[HttpPost]
[Route("get-full-name")]
public async Task<IHttpActionResult> GetFullName([FromBody]GetFullNameRequest request)
{
    var adapter = new HospitalInforformationSystem2016();
    var response = await adapter.GetFullNameAsync(request);
    return Ok(response);

}
```
where it could be accessed via

``` http
POST /api/hospital-inforformation-system-2016/get-full-name HTTP/1.1
Host: localhost:4436
Authorization: Bearer eyJ0eXAiOiJKV1Qi...
Content-Type: application/json

{
    "FirstName": "Erymuzuan",
    "LastName": "Mustapa"
}
```
and will return this response
``` HTTP
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Vary: Accept-Encoding
Server: Microsoft-IIS/10.0
X-Powered-By: ASP.NET
Date: Fri, 24 Jun 2016 06:35:53 GMT
Content-Length: 32

{"FullName":"Erymuzuan Mustapa"}
```

## Table-Valued function
Things are almost similiar for Table-Valued function to it's Scalar-Valued-Function counter part, except the return value will normally be a table and it's wrapped around a `ComplexType` with `AllowedMultiple` property is set to true.

Take this one as example

``` sql

CREATE FUNCTION [dbo].[GetPatientsByCountry]
(
	@Country INT
)
RETURNS
@PatientTable TABLE
(
	Mrn VARCHAR(50),
	FullName VARCHAR(255)
)
AS
BEGIN

	INSERT INTO @PatientTable
	SELECT [Mrn], [FullName] FROM [dbo].[Patient]
	WHERE [Nationality.Code] = @Country

	RETURN
END
```

a corresponding `Request`(1) and `Response`(2) object will be populated accordingly
![Table valued](https://lh3.googleusercontent.com/-QcOUbjU8c-M/V2zW5qvYsNI/AAAAAAAA8jk/ImhEqNbLQCwpOEleSNl2HobMTEY4aXCegCCo/s2048/%255BUNSET%255D)

Except when invoking the endpoint the result is now in a array
```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Vary: Accept-Encoding
Server: Microsoft-IIS/10.0
X-Powered-By: ASP.NET
Date: Fri, 24 Jun 2016 06:46:58 GMT
Content-Length: 1530
{
  "GetPatientsByCountryResults": [
    {
      "Mrn": "2015-120",
      "FullName": "Mike Spane",
      "WebId": null
    },
    {
      "Mrn": "2015-13",
      "FullName": "Tom Hardi",
      "WebId": null
    },
    {
      "Mrn": "2015-132",
      "FullName": "Wan Ubi",
      "WebId": null
    },
    {
      "Mrn": "2015-20",
      "FullName": "Wan Kenobi",
      "WebId": null
    }
  ]
}
```

## Inline Table Valued Function
Things are little different with `Inline-Table-Valued-Function`, in the `Response` object, since RX has no way of extracting the exact returns type, columns, You will have to provide them accordingly.

``` sql

CREATE FUNCTION [dbo].[PatientsBornInYear]
(
	@Year INT
)
RETURNS TABLE
AS
RETURN
(
	SELECT [Mrn],[FullName], [Nrid] FROM [dbo].[Patient]
	WHERE YEAR([Dob]) = @Year
)

```

In this example, RX will automatically extract the `Request` object as expected, but for the returns  you have to create a `ComplexType` with `AllowedMultiple=true`

![Response](https://lh3.googleusercontent.com/-YMQE71o6hRA/V2zZGwF3ulI/AAAAAAAA8jw/M4bQEwMLOcYnAzu90-MtD6io7Befp8SSACCo/s2048/%255BUNSET%255D)

Right click on the `Response` node and clik on the `Add result set`. In this example , give it a Name = `GetPatientsByCountryResults` and the type name is `GetPatientsByCountryResultset`.
Now your response class will contains a property call `GetPatientsByCountryResults` which is an array of type `GetPatientsByCountryResultset`

Pseudo code
``` csharp

public class GetPatientsByCountryResponse
{
    public GetPatientsByCountryResultset[] GetPatientsByCountryResults { get; }
}
```
All you need to do now it to add the columns into your `GetPatientsByCountryResultset`.

![Record](https://lh3.googleusercontent.com/-UimwmSSgHT8/V2zaqIrgCpI/AAAAAAAA8j8/0RDg2F7RROgUhn-ALt5iuM6oNwHQQiWxwCCo/s2048/%255BUNSET%255D)
Now on the `GetPatientsByCountryResults` context menu, clik on the `Add record`, do this 3 times for each of the column

![Record](https://lh3.googleusercontent.com/-U7fWpkvI3xc/V2zbBtQT6yI/AAAAAAAA8kE/O7-ZJCPOqGsC6TZkRxzTSugk2WvQ4RrJwCCo/s2048/%255BUNSET%255D)
See how the record `Name` and `Type` match the one in the function text, so the `Mrn` record match with `[Mrn]` column in the create function code select statement `SELECT [Mrn],[FullName], [Nrid] FROM ...` , this applies for `FullName` and `Nrid` too.


## Stored procedures
Stored procedure is basically similar to `Inline-Table-Valued` function, but it differs in away that the returns object could be anything, consider this scenario

1. `IN` Parameters
2. `INOUT` parameters
3. `OUT` parameters
4. `SELECT` statement
5. More than 1 `SELECT` statement
6. and then the Stored procedure return value

So for the parameters, RX developer will automatically figure out for you,

1. `IN` goes to `Request`
2. `INOUT` goes to `Request` and `Response`
3. `OUT` goes to `Response`
4. `SELECT` you will have to provide the object, just like in the `Inline-Table-Valued-Function`
5. If more that one `SELECT` statements, repeat the result set.
6. The return value will goes to `Response.return_value` object of type int


## Use HTTP Get for REST endpoint
While by default, all the operation are considered Not safe and have side effect and thus it's `POST` HTTP method, but if you thing that your `Stored procedure` are safe and have no side effect that you could actually expose it as `HTTP GET`

With `Get` the parameters now goest to endpoint `Route`, so in this example you can execute it like this
```
GET /api/hospital-inforformation-system-2016/get-patients-by-country/1 HTTP/1.1
Host: localhost:4436
Authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbG..
Content-Type: application/json

```
