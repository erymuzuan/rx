<Query Kind="Program">
  <Reference Relative="..\bin\subscribers.host\domain.sph.dll">C:\project\work\sph\bin\subscribers.host\domain.sph.dll</Reference>
  <Reference Relative="..\bin\subscribers.host\Newtonsoft.Json.dll">C:\project\work\sph\bin\subscribers.host\Newtonsoft.Json.dll</Reference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Bespoke.Sph.Domain</Namespace>
</Query>

void Main()
{
	var file = @"C:\project\work\sph\bin\sources\EntityDefinition\Patient.json";
	var originalJson = File.ReadAllText(file);
	var jo = JObject.Parse(originalJson);
	var members = jo.SelectToken("$.MemberCollection.$values");
	foreach (var m in members)
	{
		var m1 = (JObject)m;
		UpdateMember(m1);

	}
	var ed = JsonSerializerService.DeserializeFromJson<EntityDefinition>(jo.ToString());
	Console.WriteLine(ed.Name);
	File.Copy(file, $"{file}-{DateTime.Now:yyyyMMdd-HHmmss}.backup", true);
	File.WriteAllText(file, jo.ToString());

}

// Define other methods and classes here

private static void RemoveObsoleteMembersFromComplexObject(JObject m1)
{
	m1.Remove("TypeName");
	m1.Remove("IsNullable");
	m1.Remove("IsNotIndexed");
	m1.Remove("IsAnalyzed");
	m1.Remove("IsFilterable");
	m1.Remove("IsExcludeInAll");
	m1.Remove("DefaultValue");
	m1.Remove("Boost");

}
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
			if (m1.Property("AllowMultiple") == null)
				m1.Add(new JProperty("AllowMultiple", false));
			RemoveObsoleteMembersFromComplexObject(m1);


		}
		if (typeName == "System.Array, mscorlib")
		{
			m1["$type"] = "Bespoke.Sph.Domain.ComplexMember, domain.sph";
			if (m1.Property("AllowMultiple") == null)
				m1.Add(new JProperty("AllowMultiple", true));
			RemoveObsoleteMembersFromComplexObject(m1);


		}
	}
	type = typeProp.Value.Value<string>();

	var childMembers = m1.SelectToken("MemberCollection.$values");
	foreach (var c1 in childMembers)
	{
		UpdateMember((JObject)c1);
	}
}