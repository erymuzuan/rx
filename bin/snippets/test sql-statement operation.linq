<Query Kind="Statements">
  <Reference Relative="..\output\DevV1.AdventureWorks.dll">C:\project\work\sph\bin\output\DevV1.AdventureWorks.dll</Reference>
  <Reference Relative="..\subscribers\domain.sph.dll">C:\project\work\sph\bin\subscribers\domain.sph.dll</Reference>
  <Reference Relative="..\subscribers\Newtonsoft.Json.dll">C:\project\work\sph\bin\subscribers\Newtonsoft.Json.dll</Reference>
</Query>

var request = new Bespoke.DevV1.Adapters.AdventureWorks.UpdatePersonNameRequest();
request.BusinessEntityID = 1;
request.FirstName = "Ken 2";

var adapter = new Bespoke.DevV1.Adapters.AdventureWorks.AdventureWorks();
var response = await adapter.UpdatePersonNameAsync(request);
response.Dump();