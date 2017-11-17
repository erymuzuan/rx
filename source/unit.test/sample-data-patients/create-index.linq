<Query Kind="Program">
  <Reference Relative="..\elasticsearch.repository.test\bin\Debug\elasticsearc.repository.test.dll">F:\project\work\rx.pos-entt\source\unit.test\elasticsearch.repository.test\bin\Debug\elasticsearc.repository.test.dll</Reference>
  <Reference Relative="..\elasticsearch.repository.test\bin\Debug\elasticsearch.repository.dll">F:\project\work\rx.pos-entt\source\unit.test\elasticsearch.repository.test\bin\Debug\elasticsearch.repository.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference Version="8.0.2">Newtonsoft.Json</NuGetReference>
  <Namespace>Bespoke.Sph.ElasticsearchRepository</Namespace>
  <Namespace>Bespoke.Sph.ElasticsearchRepository.Extensions</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	const string INDEX = "test_index";
	const string TYPE = "patient";
	const string URL = "http://localhost:9200";
	
	var client = new HttpClient{BaseAddress = new Uri(URL)};
	var response = await client.GetAsync("test_index/patient/_count");
	if (response.IsSuccessStatusCode)
	{
		var json = await response.ReadContentAsJsonAsync();
		var count = json["count"].Value<int>();
		if (count == 100) return;
	}
	var delete = await client.DeleteAsync(INDEX);
	Console.WriteLine(delete.StatusCode);
	var index = await client.PostAsync(INDEX, new StringContent(""));
	index.EnsureSuccessStatusCode();
	
	var cm = new ByteArrayContent(File.ReadAllBytes(".\\source\\unit.test\\sample-data-patients\\patient.mapping"));
	var mapping = await client.PutAsync($"{INDEX}/_mapping/{TYPE}", cm);
	mapping.EnsureSuccessStatusCode();
	/* */
	var tasks = from file in Directory.GetFiles(
			$".\\source\\unit.test\\sample-data-patients\\", "*.json")
				let id = Path.GetFileNameWithoutExtension(file)
				let content = new StringContent(File.ReadAllText(file))
				select client.PostAsync($"/{INDEX}/{TYPE}/{id}", content);
	
	await Task.WhenAll(tasks);
}

// Define other methods and classes here
public static class HttpClientExtensions
{
	public static Task<string> ReadContentAsStringAsync(this HttpResponseMessage response, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
	{
		if (ensureSuccessStatusCode)
			response.EnsureSuccessStatusCode();
		if (!(response.Content is StreamContent content)) throw new Exception(exceptionMessage);
		return content.ReadAsStringAsync();
	}
	public static async Task<JObject> ReadContentAsJsonAsync(this HttpResponseMessage response, bool ensureSuccessStatusCode = true, string exceptionMessage = "Cannot execute query on es ")
	{
		var text = await response.ReadContentAsStringAsync(ensureSuccessStatusCode, exceptionMessage);
		return JObject.Parse(text);
	}


}
