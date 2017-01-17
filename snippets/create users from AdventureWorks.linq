<Query Kind="Statements">
  <Connection>
    <ID>64e2a62c-d7cb-4a38-a34d-c2bc7e79bb7f</ID>
    <Persist>true</Persist>
    <Server>S301\DEV2016</Server>
    <Database>AdventureWorks</Database>
  </Connection>
  <Reference Relative="..\bin\subscribers.host\domain.sph.dll">C:\project\work\sph\bin\subscribers.host\domain.sph.dll</Reference>
  <Reference Relative="..\bin\subscribers.host\Newtonsoft.Json.dll">C:\project\work\sph\bin\subscribers.host\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>System.Net.Http</Namespace>
  <AppConfig>
    <Content>
      <configuration>
        <system.net>
          <defaultProxy>
            <proxy usesystemdefault="true" proxyaddress="http://127.0.0.1:8888" bypassonlocal="false" />
          </defaultProxy>
        </system.net>
      </configuration>
    </Content>
  </AppConfig>
</Query>

var client = new HttpClient { BaseAddress = new Uri("http://localhost:4436") };
client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImRldmVsb3BlcnMiXSwiZW1haWwiOiJhZG1pbkBiZXNwb2tlLmNvbS5teSIsInN1YiI6IjYzNjIwMjM3MTIyMDQ3ODM1NmY5MTU2MmJmIiwibmJmIjoxNTAwMjQ5OTIyLCJpYXQiOjE0ODQ2MTE1MjIsImV4cCI6MTQ4NjE2NjQwMCwiYXVkIjoiRGV2VjEifQ.Eq1Y7pv1fjGXnwNizGIdXF9WwNFoTz9QJpH74UyVFXQ");

foreach (var person in Persons)
{
	var userName = (person.FirstName + person.MiddleName + person.LastName).Replace(" ", "");
	var email = EmailAddresses.SingleOrDefault(x => x.BusinessEntityID == person.BusinessEntityID);

	var profile = new Bespoke.Sph.Domain.Profile
	{
		UserName = userName,
		Password = "123456",
		ConfirmPassword = "123456",
		Department = "Finance",
		Designation = "Pegawai Pendaftaran",
		FullName = $"{person.FirstName} {person.MiddleName} {person.LastName}",
		Email = userName + "@bespoke.com.my",
		Mobile = (person.PersonPhones.FirstOrDefault() ?? new PersonPhone { PhoneNumber = "012-3889200" }).PhoneNumber,
		Status = "New",
		Telephone = (person.PersonPhones.LastOrDefault() ?? new PersonPhone { PhoneNumber = "03-77294424" }).PhoneNumber,
		WebId = Guid.NewGuid().ToString(),
		Roles = new string[] { "PendaftaranAdmin" }
	};
	Console.Write(userName);

	var json = @"{""profile"":" +  Bespoke.Sph.Domain.JsonSerializerService.ToJsonString(profile) + "}";
	var content = new StringContent(json);
	content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
	var response = await client.PostAsync("sph/Admin/AddUser", content);
	Console.WriteLine(":" + (int)response.StatusCode);

	if (!response.IsSuccessStatusCode)
	{
		var rc = response.Content as StreamContent;
		var text = await rc.ReadAsStringAsync();
		Console.WriteLine(json);
		Console.WriteLine(text);
	}

}
