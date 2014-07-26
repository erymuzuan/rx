<Query Kind="Program">
  <Reference Relative="..\source\web\web.sph\bin\domain.sph.dll">C:\project\work\sph\source\web\web.sph\bin\domain.sph.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\Newtonsoft.Json.dll">C:\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
</Query>

void Main()
{
	var text = @"{
	""one"" : 1,
	""two"" :""two"",
	""three"" : 3,
	""four"" : false,
	""today"" : ""2014-07-26"",
	""list"" :[
		{
			""five"" : 5
		},
		
		{
			""five"" : 6
		}
	],
	""arrays"" : [1,2],
	""address"" : {
		""street"" : ""Jalan 1"",
		""postcode"" : ""3444"",
		""position"" : {
			""lat"": 0.34343,
			""lng"" : 102.3434
		}
	}
	
	
	}";
	
	
	// --
	
	var json = JObject.Parse(text);
	var ed = new EntityDefinition();
	
	foreach (JProperty p in json.Children())
	{
		var member = PrintJToken(p, null);
		ed.MemberCollection.Add(member);
	}
	Console.WriteLine (ed.MemberCollection);
}

// Define other methods and classes here
public static Member PrintJToken(JToken jt, Member parent, string level= "")
{
	var m = new Member();
	var p = jt as JProperty;
	if(null == p) 
	{
		
		//Console.WriteLine ("--------"+jt.Type);
		// array of objects
		if(jt.Type == JTokenType.Object)
		{
			foreach (var c in jt.Children())
			{
				PrintJToken( c,m, level + "    ");
			}
		}
		return m;
	}
	
	
	//Console.WriteLine ("{2}{0}({1})",p.Name, p.Value.Type, level);
	
	if(p.Value.Type == JTokenType.Object|| p.Value.Type == JTokenType.Array)
	{
		//Console.WriteLine ("--- object --");
		foreach (var c in p.Value.Children())
		{
			PrintJToken( c,m, level + "    ");
		}
	}
	
	return m;
}
