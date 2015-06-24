<Query Kind="Program">
  <Reference Relative="..\..\..\rep.generic.psikometrik\web\bin\Newtonsoft.Json.dll">C:\project\rep.generic.psikometrik\web\bin\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

async Task Main()
{
	const string entity = "soalan";
	var text1  = File.ReadAllText(@"C:\project\rep.generic.psikometrik\sources\EntityDefinition\" + entity +".mapping");
	var json1 = JObject.Parse(text1);
	
	JObject json2;
	
	using (var client = new HttpClient())
	{
		var text2 = await client.GetStringAsync("http://localhost:9200/epsikologi/_mapping/" + entity);
		json2 = JObject.Parse(text2);
	}
	JToken.DeepEquals(json1,json2).Dump("Deep Equal");
	JToken fields = json1[entity]["properties"];
	foreach (var field in fields)
	{
		Console.WriteLine ();
		Console.WriteLine ();;
		Console.WriteLine ("==============");
		Console.WriteLine (field.Path);
		var map = field.First;
		
		Console.WriteLine ("*********************");
		var es = json2.SelectToken("epsikologi.mappings."+field.Path);
		
		var type = map["type"].MapEquals<string>(es["type"]);
		if(map["type"].Value<string>() == "object")
		{
			// TODO : recursely checking the inner object - complex/collection
			Console.WriteLine ("$$$$$$$$$$$$$$$$$$$$$$$$$");
			type = null != es;
		}
		
		Console.WriteLine ("Type : " + type);
		
		if(map["type"].Value<string>() != "boolean")
		{
		
			var index = map["index"].MapEquals<string>(es["index"]);
			Console.WriteLine ("index : " + index);
			
			var include_in_all = map["include_in_all"].MapEquals<bool>(es["include_in_all"]);
			Console.WriteLine ("include_in_all : " + include_in_all);
			
			var boost = map["boost"].MapEquals<int>(es["boost"]);
			Console.WriteLine ("boost : " + index);
		}
		
	}
	
}


// Define other methods and classes here
public static class Helper
{
	public static bool MapEquals<T>(this JToken source, JToken es)
	{
		if(null == source && null == es) return true;		
		if(null == source) return false;
		if(null == es) return false;
		
			
		if(!source.HasValues && !es.HasValues) return true;
		if(!source.HasValues ) return false;
		if(!es.HasValues) return false;
		
		var sourceValue = source.Value<T>();
		var esValue = es.Value<T>();
		return esValue.Equals(sourceValue);
	}
}
