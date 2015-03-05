<Query Kind="Statements">
  <Connection>
    <ID>70e89200-dc96-4211-84c7-468b218a4dfe</ID>
    <Persist>true</Persist>
    <Server>(localdb)\Projects</Server>
    <ExcludeRoutines>true</ExcludeRoutines>
    <Database>DevV1</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference Relative="..\source\web\core.sph\bin\Newtonsoft.Json.dll">C:\project\work\sph\source\web\core.sph\bin\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

const string index = "devv1";
const string backupDirMapping = @"c:\temp\_rx_backup_mapping";
Directory.CreateDirectory(backupDirMapping);
const string backupDirMappingdata = @"c:\temp\_rx_backup_mapping_data";
Directory.CreateDirectory(backupDirMappingdata);

using (var client = new HttpClient{BaseAddress = new Uri("http://localhost:9200")})
{
	foreach (var ent in this.EntityDefinitions)
	{	
			Console.WriteLine ("ajax - " + index + "/_mapping/" + ent.Name.ToLowerInvariant());
			var result = await client.GetStringAsync(index + "/_mapping/" + ent.Name.ToLowerInvariant());
			Console.WriteLine ("getting mapping for {0}", ent.Name);
			File.WriteAllText(backupDirMapping + "\\dev.mapping." + ent.Name+".json", result);
				
			// data
			var content = new StringContent(@"{
    ""from"": 0,
    ""size"": 2000
    
}");
			Console.WriteLine("ajax - " + index + "/" + ent.Name.ToLowerInvariant() +"/_search", content);
			var response = await  client.PostAsync(index + "/" + ent.Name.ToLowerInvariant() +"/_search", content);
			var data = response.Content as StreamContent;
			var json2 = await data.ReadAsStringAsync();
			
			if(ent.Name == "Customer")
			{
				var o = JObject.Parse(json2);
				o.SelectToken("$.hits.hits").ToString().Dump();
			}
				
			File.WriteAllText(backupDirMappingdata + ent.Name + ".json", json2.ToString());
		
	}
}

// DELETE index
// disable cluster
// create index
// create mapping
// PUT data