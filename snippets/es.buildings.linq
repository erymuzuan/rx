<Query Kind="Program">
  <Connection>
    <ID>84e06ebb-98ea-4fa0-a47c-8535465a77e6</ID>
    <Persist>true</Persist>
    <Server>.\KATMAI</Server>
    <Database>Sph</Database>
  </Connection>
  <Reference Relative="..\bin\Debug\domain.sph.dll">C:\project\work\sph\bin\Debug\domain.sph.dll</Reference>
  <Reference Relative="..\packages\Newtonsoft.Json.5.0.6\lib\net45\Newtonsoft.Json.dll">C:\project\work\sph\packages\Newtonsoft.Json.5.0.6\lib\net45\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference Relative="..\packages\Microsoft.Net.Http.2.2.13\lib\net45\System.Net.Http.Extensions.dll">C:\project\work\sph\packages\Microsoft.Net.Http.2.2.13\lib\net45\System.Net.Http.Extensions.dll</Reference>
  <Reference Relative="..\packages\Microsoft.Net.Http.2.2.13\lib\net45\System.Net.Http.Primitives.dll">C:\project\work\sph\packages\Microsoft.Net.Http.2.2.13\lib\net45\System.Net.Http.Primitives.dll</Reference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

void Main()
{
	var list = (from d in Buildings.Take(1000)
				let item = Bespoke.Sph.Domain.XmlSerializerService.Deserialize<Bespoke.Sph.Domain.Building>(d.Data)
				select new {item, id = d.BuildingId}).ToList();
				
	Console.WriteLine (list.Count);			
				
	var setting = new JsonSerializerSettings();
	setting.TypeNameHandling = TypeNameHandling.All;
	
	foreach (var x in list)
	{
		var id = x.id;
		x.item.BuildingId = id;
		var json = JsonConvert.SerializeObject(x.item, setting);
		var content = new StringContent(json);
		
		HttpClient client = new HttpClient();
		client.PutAsync("http://localhost:9200/sph/building/" + id,content)
		.ContinueWith(_ =>{
			var result = _.Result;
			Console.WriteLine (result.Content.ReadAsStringAsync());
		})
		.Wait();
		/*
		File.WriteAllText(@"C:\project\work\sph\snippets\building." + id + ".json", json);
		*/
		
	}
}