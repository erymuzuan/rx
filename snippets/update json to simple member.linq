<Query Kind="Program">
  <Reference Relative="..\bin\subscribers.host\Newtonsoft.Json.dll">C:\project\work\sph\bin\subscribers.host\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>


void Main()
{
	var file = @"C:\Users\erymuzuan\Downloads\Patient.json";
	var jo = JObject.Parse(File.ReadAllText(file));
	var members = jo.SelectToken("$.MemberCollection.$values");
	foreach (var m in members)
	{
		var m1 = (JObject)m;
		UpdateMember(m1);
		
	}
	Console.WriteLine(jo.ToString());
}

// Define other methods and classes here
private static void UpdateMember(JObject m1)
{
	var typeProp = m1.Property("$type");
	var typeNameProp = m1.Property("TypeName");

	var type = typeProp.Value.Value<string>();
	var typeName = typeNameProp.Value.Value<string>();

	if (type == "Bespoke.Sph.Domain.Member, domain.sph")
	{
		m1["$type"] = "Bespoke.Sph.Domain.SimpleMember, domain.sph";
		m1.Remove("FullName");
		if (typeName == "System.Object, mscorlib")
		{
			m1["$type"] = "Bespoke.Sph.Domain.ComplexMember, domain.sph";
			m1.Add(new JProperty("AllowMultiple", false));
			m1.Remove("IsNullable");
			m1.Remove("IsNotIndexed");
			m1.Remove("IsAnalyzed");
			m1.Remove("IsFilterable");
			m1.Remove("Boost");


		}
		if (typeName == "System.Array, mscorlib")
		{
			m1["$type"] = "Bespoke.Sph.Domain.ComplexMember, domain.sph";
			m1.Add(new JProperty("AllowMultiple", true));
			m1.Remove("IsNullable");
			m1.Remove("IsNotIndexed");
			m1.Remove("IsAnalyzed");
			m1.Remove("IsFilterable");
			m1.Remove("Boost");


		}
	}
	type = typeProp.Value.Value<string>();

	var childMembers = m1.SelectToken("MemberCollection.$values");
	foreach (var c1 in childMembers)
	{
		UpdateMember((JObject)c1);
	}
}
