<Query Kind="Statements">
  <Connection>
    <ID>b08d3c88-c2f1-42cb-b0f9-998136af42d9</ID>
    <Persist>true</Persist>
    <Server>(localdb)\Projects</Server>
    <Database>sph</Database>
  </Connection>
  <Reference Relative="..\source\web\core.sph\bin\Newtonsoft.Json.dll">C:\project\work\sph\source\web\core.sph\bin\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

const string index = "dev";


using (var client = new HttpClient{BaseAddress = new Uri("http://localhost:9200")})
{
	foreach (var ent in this.EntityDefinitions)
	{	
			var result = await client.GetStringAsync(index + "/_mapping/" + ent.Name.ToLowerInvariant());
			Console.WriteLine ("getting mapping for {0}", ent.Name);
			File.WriteAllText(@"c:\temp\dev.mapping." + ent.Name+".json", result);
				
			// data
			var content = new StringContent(@"{
    ""from"": 0,
    ""size"": 2000
    
}");
			var response = await  client.PostAsync(index + "/" + ent.Name.ToLowerInvariant() +"/_search", content);
			var data = response.Content as StreamContent;
			var json2 = await data.ReadAsStringAsync();
			
			if(ent.Name == "Customer")
			{
				var o = JObject.Parse(json2);
				o.SelectToken("$.hits.hits").ToString().Dump();
			}
				
			File.WriteAllText(@"c:\temp\" + ent.Name + ".json", json2.ToString());
		
	}
}

// DELETE index
// disable cluster
// create index
// create mapping
// PUT data