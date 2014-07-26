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
	""collection"" :[
		{
			""five"" : 5,
			""six"" : ""whatever""
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
	ed.MemberCollection.Where (mc => mc.Type== typeof(Array)).Dump();
	//Console.WriteLine (JsonConvert.SerializeObject( ed.MemberCollection,Newtonsoft.Json.Formatting.Indented));
}

// Define other methods and classes here
public static Member PrintJToken(JToken jt, Member parent, string level= "")
{
	var m = new Member();
	var p = jt as JProperty;
	if(null == p) 
	{
	
		// array of objects
		if(jt.Type == JTokenType.Object)
		{
			foreach (var c in jt.Children())
			{
				var cm = PrintJToken( c,m, level + "    ");
				
				if(!parent.MemberCollection.Any(x => x.Name == cm.Name))
					parent.MemberCollection.Add(cm);
			}
		}
		return m;
	}
	
	m.Name = p.Name;
	var typeName = p.Value.Type;
	var type =Type.GetType(string.Format("System.{0}, mscorlib",p.Value.Type));
	if(typeName == JTokenType.Integer)
		type = typeof(int);//"
	if(null != type)
		m.Type = type;
	Console.WriteLine ("{2}{0}({1})",p.Name, p.Value.Type, level);
	
	if(p.Value.Type == JTokenType.Object|| p.Value.Type == JTokenType.Array)
	{
		//Console.WriteLine ("--- object --");
		foreach (var c in p.Value.Children())
		{
			PrintJToken( c,m, level + "    ");
		}
	}
	if(null != parent)
		parent.MemberCollection.Add(m);
	
	return m;
}
