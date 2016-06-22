# Microsoft SQL Server Adapter

List of icons
1.  <i class="fa fa-eye"></i> Readonly - Computed column
1.  <i class="fa fa-eye-slash"></i> Ignore - The column will be ignored when serializing or deserializing to JSON
1.  <i class="fa fa-binoculars"></i> Lookup - This column holds a key, and the value will be retrieved from another table
1.  <i class="fa fa-link"></i> Complex - This column will not be serialized or retrieved, it's configured another endpoint belong to the row

## Caching and versioning
The first thing you want to do to optimize you database access is provided some kind of caching, with Rx Developer `Adapter` REST API framework you are able to do so with simple `ROWVERSION` column or `ModifiedDate` column.

### ROWVERSION
`ROWVERSION` column , is a kind of special column in Microsoft SQL Server, where it's readonly and the value will be incremented each time the row is updated. So users don't have to do anything.

To use `ROWVERSION` column all you have to do is, select a `table` in your adapter designer, and on the property pane select your `ROWVERSION` column.

Now every row in your table will have a version number included in `GetOne` API request via `ETag` header. and if your client include this header every time it make a request, RX Developer will compare the value with the one in the database, and quickly return `304 Not Modified` if it's unchanged.


First request
```
GET /api/hospital-inforformation-system-2016/patient/2015-120 HTTP/1.1
Authorization: Bearer eyJ0eXAiOiJKV1QiL.....
Content-Type: application/json

```

Response
```
HTTP/1.1 200 OK
Content-Type: application/json
ETag: "0x0000000000000949"
Vary: Accept-Encoding
Server: Microsoft-IIS/10.0
X-AspNet-Version: 4.0.30319
X-Powered-By: ASP.NET

{
  "mrn": "2015-120",
  "firstName": "2015 - 120",
  "lastName": "Wan",
  "fullName": "2015 - 120 Wan",
  "IdCardMimeType": "image/jpeg",
  "civil-servant": false,
  "children": true,
  .....
}
```

Now for next request, include the `If-None-Match` header
```
GET /api/hospital-inforformation-system-2016/patient/2015-120 HTTP/1.1
Authorization: Bearer eyJ0eXAiOiJKV1QiL.....
Connection: keep-alive
Cache-Control: max-age=0
If-None-Match: "0x0000000000000949"
```

and if the row is unchanged between the first and the second request is made
```
HTTP/1.1 304 Not Modified
Cache-Control: private
Content-Length: 0
Content-Type: text/plain; charset=utf-8
ETag: "0x0000000000000949"

```
Learn more about HTTP with ETag  [Caching with Entity Tags](https://www.w3.org/2005/MWI/BPWG/techs/CachingWithETag.html)  


If your table doesn't have a `ROWVERSION` column yet, you can easily add one without any worry
``` sql
ALTER TABLE [schema].[table]
ADD [Version] ROWVERSION NOT NULL

```

You can find out more about `ROWVERSION` in [MSDN](https://msdn.microsoft.com/en-us/library/ms182776.aspx)

### ModifiedDate

Apart from `ROWVERSION`, RX Developer also support `ModifiedDate` column, although this is manual way of keeping update on the versioning and it's supported by HTTP with `Last-Modified` header in the response and the accompanying `If-Modified-Since` for the GET request and `If-Unmodified-Since` for others.

Your table would have to a column where with `DateTime` or equivalent data type, and everytime the row is updated, so does this column.

The way it works is, when your client issuing the first `GET` request, you'll include the `Last-Modified` value in your response header. Now your client can keep this value, if they are trying to issue another `GET` request to the same resource, they could include `If-Modified-Since` value in their request header. And if the server see this value, and the resource the `ModifiedDate` hasn't changed, then a `304` response code will be returned. If however that the value has changed, then a new `200` response with the new value for `Last-Modified` will be sent.


#### Updating a row with `ROWVERSION`/`ModifiedDate`
If your table defined a `ROWVERSION` or `ModifiedDate` or both, then the `UpdateAction`, i.e the `PUT` to the resource url will check against conflict. The way it works is just the opposite of the `GET` request, in `GET` request your client basically asking the server.

** I have this resource and the last time it changed was `2 hours ago`, and can I get it again**, now the server will reply, don't worry your copy is still valid(304 Not Modified).

With update, your client is basically saying I have got this resource and the lastime it was changed was `2 hours ago`, now please update something. and the server will response. No you can't somebody else has beat you(428 response status code, saying that your resource is out of date).


``` curl
PUT /api/hospital-inforformation-system-2016/patient/1
If-Unmodified-Since:<>

{
  "Gender": "M",
  "LastName":"Roseberg",
  .......
}

```

## Creating a lookup value
Most database tables were designed for OLTP, which normally means table are normalized for optimum update and insert. But the table is no longer presenting your actual your business `Logical` data.

Take for example a Sales

| SalesId    | SalesPersonId     | Amount |
| :------------- | :------------- |------|
| 1       | 1005       | 50.00 |
| 2       | 1852       | 100.00|

and there's another Sales person table

| SalesPersonId | FullName |
| :------------- | :------------- |
| 1005 | Lewis Hamilton |
| 185 | Cliff Burton |

Your logical sales data might look sometime like

| SalesId | SalesPerson | Amount
| :------------- | :------------- | ---|
| 1 | Lewis Hamilton | 50.00 |
| 2 | Cliff Burton | 100.00 |

With Rx Developer `Sql Server` adapter, you could make this possible with a simple `Enable Lookup Column` context menu, on any column.

Once you enable `LookupColumn` on a column, you'll be presented will a property pane that let you choose the followings fields

1. Lookup table - the other table where the actual value is stored, e.g. `SalesPerson` table
2. Name - the property name of the column, in the example above it's `SalesPerson`
3. Type - The property type
4. Key Column - the column in the lookup table where the corresponding column in the original table is stored
5. Value column - the column in the lookup table where the final value will be retrieved if matched with the value of the provided key column.



## Creating a complex column
There are times where you don't want to display the column value right inline or included is select statement, now imaging you have this table

``` sql
/* Sample Patient Table DDL */
CREATE TABLE [dbo].[Patient]
(
	 [Mrn] VARCHAR(255) NOT NULL
	,[FirstName] VARCHAR(50) NOT NULL
	,[LastName] VARCHAR(50) NOT NULL
	,[FullName] as [FirstName] + ' ' + [LastName] PERSISTED
	,[Gender] CHAR(1) NOT NULL
	,[Income] MONEY NOT NULL
	,[Dob] DATE NOT NULL
	,[Nationality.Code] TINYINT NOT NULL
	,[RaceCode] TINYINT NOT NULL
	,[ReligionCode] TINYINT NULL
	,[Age] TINYINT NULL
	,[Nrid] BIGINT NULL
	,[PassportNo] NVARCHAR(50) NULL
	,[BirthCert] NVARCHAR(50) NULL
	,[IdCardCopy] VARBINARY(MAX) NULL
	,[IdCardMimeType] VARCHAR(255) NULL
	,[Fee] SMALLMONEY NULL
	,[Weight] DECIMAL NULL
	,[Height] REAL NOT NULL
	,[AdditionalInfo] XML NULL
	,[Address] XML NOT NULL
	,[IsCivilServant] BIT NULL
	,[IsChildren] BIT NOT NULL
	,[RegisteredDate] DATETIME2 NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[ModifiedDate] DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
	,[Version] ROWVERSION NOT NULL
	,CONSTRAINT PK_Patient PRIMARY KEY CLUSTERED ([Mrn] ASC)
)
```
There's a column called `[IdCardCopy] VARBINARY(MAX)`, to select the field every time you would be very expensive and most of the time it's irrelevant . So does `[Address]` and `[AdditionalInfo]` column which might represent another data.

Now if we choose, this column and make it `Complex`, RX Developer will not select or serialize the column value anymore. See `Complex link below` for more information about how the generated API will be handled.


## Configuring API
### List
### Get one
### Insert
### Update
### Complex link
Once you had a column mark as `Complex`, the column will no longer serialized to JSON, and `ComplexLinkActionCode` generator will generate a new endpoint for this column.

On the example above, the route for `[IdCardCopy]` endpoint will be `/api/adapter-id/{mrn}/patients/IdCardCopy`, and link is provided on every item generated by `GetOne` action.

It's very important to specify the correct MIME type, for `Complex` column.
1. Literal string e.g. `application/xml`
2. Value from another column with `=IdCardMimeType`
3. Code expression with `{code expression}`

### Child relations
