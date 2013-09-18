<Query Kind="Program">
  <Connection>
    <ID>08de3e8b-14a7-4fb8-830a-907362486f0a</ID>
    <Persist>true</Persist>
    <Server>(localdb)\Projects</Server>
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
	var list = from d in Buildings.Take(1000)
				let item = Bespoke.Sph.Domain.XmlSerializerService.Deserialize<Bespoke.Sph.Domain.Building>(d.Data)
				let id = SetId(item, d.BuildingId)
				select new {item, id};
	
	foreach (var x in list)
	{
		var id = x.id;
		HttpClient client = new HttpClient();
		var json = JsonConvert.SerializeObject(x.item);
		var content = new StringContent(json);
		client.PutAsync("http://localhost:9200/sph/building/" + id,content)
		.ContinueWith(_ =>{
			var result = _.Result;
			Console.WriteLine (result.Content.ReadAsStringAsync());
		})
		.Wait();
		//Console.WriteLine (json);
		
	}
}

// Define other methods and classes here
public int SetId(Bespoke.Sph.Domain.Building building, int id)
{
	 Console.WriteLine ("Id"  + id);
	 building.BuildingId = id;
	 return id;
}