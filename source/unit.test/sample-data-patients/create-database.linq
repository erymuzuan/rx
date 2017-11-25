<Query Kind="Program">
  <Reference Relative="..\..\..\..\rx.pos-entt\source\unit.test\elasticsearch.repository.test\bin\Debug\elasticsearc.repository.test.dll">F:\project\work\rx.pos-entt\source\unit.test\elasticsearch.repository.test\bin\Debug\elasticsearc.repository.test.dll</Reference>
  <Reference Relative="..\elasticsearch.repository.test\bin\Debug\elasticsearch.repository.dll">F:\project\work\rx.v1\source\unit.test\elasticsearch.repository.test\bin\Debug\elasticsearch.repository.dll</Reference>
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
	const string DATABASE = "rx_test_database";
	const string TABLE = "Patient";
	string CONNECTION_STRING = $@"Data Source=.\DEV2016;Initial Catalog={DATABASE};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

	using (var conn = new SqlConnection(CONNECTION_STRING))
	{


		await conn.OpenAsync();
		using (var truncate = new SqlCommand("TRUNCATE TABLE [DevV1].[Patient]", conn))
		{
			await truncate.ExecuteNonQueryAsync();
		}

		foreach (var file in Directory.GetFiles($".\\source\\unit.test\\sample-data-patients\\", "*.json"))
		{
			var id = Path.GetFileNameWithoutExtension(file);
			var text = File.ReadAllText(file);
			var json = JObject.Parse(text);
			using (var insert = new SqlCommand($@"INSERT INTO [DevV1].[Patient]
           ([Id]
           ,[Mrn]
           ,[FullName]
           ,[Gender]
           ,[Religion]
           ,[Race]
           ,[Status]
           ,[MaritalStatus]
           ,[Json]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           ('{id}'
           ,'{json["Mrn"]}'
           ,@FullName
           ,'{json["Gender"]}'
           ,'{json["Religion"]}'
           ,'{json["Race"]}'
           ,'{json["Status"]}'
           ,'{json["MaritalStatus"]}'
           ,@Json
           ,'{json["CreatedDate"]:s}'
           ,'{json["CreatedBy"]}'
           ,'{json["ChangedDate"]:s}'
           ,'{json["ChangedBy"]}')", conn))
			{
				insert.Parameters.AddWithValue("@FullName", $"{json["FullName"]}");
				insert.Parameters.AddWithValue("@Json", text);
				try
				{

					await insert.ExecuteNonQueryAsync();
				}
				catch (SqlException ex)
				{
					ex.Message.Dump("SQL Syntax error");
					text.Dump();
					throw;
				}
			}
		}

	}

}

