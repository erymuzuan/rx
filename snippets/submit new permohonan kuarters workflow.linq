<Query Kind="Statements">
  <NuGetReference>RestSharp</NuGetReference>
  <Namespace>RestSharp</Namespace>
</Query>

var seed = 6500;

for (int i = 0; i < 100; i++)
{
	var json = $@"{{
	    ""Nama"" : ""Nama {seed + i}"",
	    ""NoKp"" : ""{seed + i}"",
	    ""Umur"" : ""{i}"",
	    ""Alamat"":{{
	        ""Street1"" : ""No {seed + i}"",
	        ""Street2"":""Jalan {seed + i}/{seed + i}"",
	        ""Postcode"" : ""{seed + i}"",
	        ""State"" : ""Kelantan""
	    }}
	    
	}}";
	
	var client = new RestClient("http://localhost:4436/wf/permohonan-kuarters/v1/terima-permohonan-baru");
	var request = new RestRequest(Method.POST);
	request.AddHeader("postman-token", "8273e9b6-e309-29af-42bd-7a1b04faad84");
	request.AddHeader("cache-control", "no-cache");
	request.AddHeader("authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImRldmVsb3BlcnMiXSwiZW1haWwiOjE0NjI1NzkyMDAsInN1YiI6ImI2NDY4NjYxLWFmMDQtNDY1Ni1hY2RiLTliNTM4NzcwN2NkZSIsIm5iZiI6MTQ3NjM5OTg0OSwiaWF0IjoxNDYwNTg4NjQ5LCJleHAiOjE0NjI1NzkyMDAsImF1ZCI6IkRldlYxIn0.HkTucqK0Q84dCAkJn5PNaHrAYW0TZr9gEZwA_sFo0DA");
	request.AddHeader("content-type", "application/json");

	request.AddParameter("application/json",json, ParameterType.RequestBody);
	IRestResponse response = client.Execute(request);
	json.Dump();
}