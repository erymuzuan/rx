<Query Kind="Statements">
  <Connection>
    <ID>17105f55-c9fe-4392-bede-775484a8a8d3</ID>
    <Persist>true</Persist>
    <Server>(localdb)\Projects</Server>
    <Database>sph</Database>
  </Connection>
  <Output>DataGrids</Output>
  <Reference Relative="..\source\web\web.sph\bin\domain.sph.dll">C:\project\work\sph\source\web\web.sph\bin\domain.sph.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\Newtonsoft.Json.dll">C:\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll</Reference>
</Query>

foreach(var db in EntityViews)
{
	var wd = Bespoke.Sph.Domain.XmlSerializerService.Deserialize<Bespoke.Sph.Domain.EntityView>(db.Data);
	db.Json = Bespoke.Sph.Domain.JsonSerializerService.ToJsonString(wd, true);
	
}

SubmitChanges();