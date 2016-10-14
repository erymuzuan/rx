<Query Kind="Program">
  <Reference Relative="..\bin\subscribers.host\domain.sph.dll">C:\project\work\sph\bin\subscribers.host\domain.sph.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Accessibility.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Deployment.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.Formatters.Soap.dll</Reference>
  <NuGetReference>Mono.Cecil</NuGetReference>
  <Namespace>Mono.Cecil</Namespace>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Windows.Forms</Namespace>
</Query>

void Main()
{
	var f = AssemblyDefinition.ReadAssembly(@"C:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll");
	var md = f.MainModule;
	var adapter = md.Types.Single(x => x.Name == "AssemblyClassToTest");
	var schema = adapter.GetJsonSchema();
	schema.Dump();
	Clipboard.SetText(schema);

}

// Define other methods and classes here



/// <summary>
/// Just helper for the Dictionary object
/// </summary>
public static class DictionaryExtensions
{

	public static string GetJsonSchema(this PropertyDefinition prop)
	{
		var elements = new Dictionary<string, string>();
		var typeBags = new Dictionary<string, string>
			{
				{typeof(string).FullName,"string"},
				{typeof(DateTime).FullName,"string"},
				{typeof(System.Xml.XmlDocument).FullName,"string"},
				{typeof(int).FullName,"integer"},
				{typeof(byte).FullName,"integer"},
				{typeof(short).FullName,"integer"},
				{typeof(long).FullName,"integer"},
				{typeof(decimal).FullName,"number"},
				{typeof(double).FullName,"integer"},
				{typeof(float).FullName,"number"},
				{typeof(bool).FullName,"boolean"},
			};
		var formatBags = new Dictionary<string, string>
			{
				{typeof(DateTime).FullName,"date-time"}
			};


		var type = prop.PropertyType;
		TypeDefinition td = type as TypeDefinition;
		//Console.WriteLine(type.GetType().FullName + ": " + prop.Name);
		var nullable = typeof(Nullable<>).FullName;
		var generic = type as GenericInstanceType;
		if (generic?.ElementType.FullName == nullable)
		{
			type = generic.GenericArguments.First();
			Console.WriteLine("Nullable of type " + type.FullName);
		}
		elements.AddIfNotExist("required", "true");
		elements.Add("type", string.Empty);
		if (typeBags.ContainsKey(type.FullName))
			elements["type"] = $@"""{typeBags[type.FullName]}""";
		if (formatBags.ContainsKey(type.FullName))
			elements.AddIfNotExist("format", $@"""{formatBags[type.FullName]}""");

		if (generic?.ElementType.FullName == nullable)
		{
			type = generic.GenericArguments.First();
			td = type as TypeDefinition;

			if (typeof(DateTime).IsOfType(type))
			{
				elements.AddIfNotExist("format", @"""date-time""");
			}
			if (null != td && td.BaseType.IsOfType(typeof(Enum)))
			{
				elements.AddIfNotExist("enum", "[" + td.Fields.Where(x => x.HasConstant).Select(x => x.Name).ToString(",", x => $@"""{x}""") + "]");
				elements["type"] = @"""string""";
			}

			elements["type"] = $@"[{elements["type"]}, ""null""]";
		}

		if (null != td && td.BaseType.IsOfType(typeof(Enum)))
		{
			elements.AddIfNotExist("enum", "[" + td.Fields.Where(x => x.HasConstant).Select(x => x.Name).ToString(",", x => $@"""{x}""") + "]");
		}

		// most likely IList, or so
		if (string.IsNullOrWhiteSpace(elements["type"]) && null != generic)
		{
			// TODO : Need a more robust way to check for IEnumerable<>
			var array = true;
			if (array)
			{
				var itemType = generic.GenericArguments.First();
				td = type as TypeDefinition;
				if (!itemType.IsOfType(typeof(string)) && !itemType.IsPrimitive/* and IsClass*/)
				{
					if (null == td && prop.DeclaringType.Scope == itemType.Scope)
					{
						td = prop.DeclaringType.Module.Types.Single(x => x.FullName == itemType.FullName);
					}
					if (null != td)
					{
						var children = from p in td.Properties
									   where p.DeclaringType.FullName != typeof(DomainObject).FullName
									   select p.GetJsonSchema();
						elements["type"] = @"[""array"", ""null""]";
						var items = $@"{{
					""type"":[""object"", ""null""],
					""properties"" : {{{ children.ToString(",\r\n", x => $"{x}")} }}
				}}";
						elements.Add("items", items);
					}
				}
				else
				{
					// TODO, now get back the type
					elements["type"] = @"[""array"", ""null""]";
					var items = $@"{{
					""type"":[""{typeBags[itemType.FullName]}"", ""null""]
				}}";
					elements.Add("items", items);

				}
			}
		}


		if (null != td && string.IsNullOrWhiteSpace(elements["type"]))
		{
			var children = from p in td.Properties
							   //where p.DeclaringType != typeof(DomainObject)
						   select p.GetJsonSchema();
			elements.AddIfNotExist("required", "true");
			elements["type"] = @"[""object"", ""null""]";
			elements.Add("properties", "{" + children.ToString(",\r\n", x => $"{x}") + "}");
		}




		var code = new StringBuilder();
		code.Append($@" ""{prop.Name}"": {{
");
		code.JoinAndAppendLine(elements.Keys, ",\r\n", x => $@"""{x}"" : {elements[x]}");
		code.Append("}");
		return code.ToString();
	}
	public static string GetJsonSchema(this TypeDefinition t)
	{
		var schema = new StringBuilder();
		var properties = from p in t.Properties
						 where p.DeclaringType.FullName != typeof(DomainObject).FullName
						 select p.GetJsonSchema();

		schema.Append($@"
{{
  ""type"": ""object"",
  ""properties"": {{
   {properties.ToString(",\r\n")}
  }}
}}");
		return schema.ToString();
	}

	public static StringBuilder JoinAndAppend<T>(this StringBuilder text,
		IEnumerable<T> list,
		string seperator = ",",
		Func<T, string> projection = null)
	{
		if (null == projection)
			projection = x => $"{x}";

		var line = string.Join(seperator, list.Select(projection));
		return text.Append(line);
	}
	public static StringBuilder JoinAndAppendLine<T>(this StringBuilder text,
		IEnumerable<T> list,
		string seperator = ",",
		Func<T, string> projection = null)
	{
		if (null == projection)
			projection = x => $"{x}";

		var line = string.Join(seperator, list.Select(projection));
		return text.AppendLine(line);
	}
	public static string ToString<T>(this IEnumerable<T> list, string seperator = ",", Func<T, string> projection = null)
	{
		if (null == projection)
			projection = x => $"{x}";
		return string.Join(seperator, list.Select(projection));
	}

	public static void AddOrReplace<T, T1>(this IDictionary<T, T1> dictionary, T key, T1 value)
	{
		if (dictionary.ContainsKey(key))
			dictionary[key] = value;
		else
			dictionary.Add(key, value);

	}
	/// <summary>
	/// Add a new key-value, if not exist else do nothing
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="T1"></typeparam>
	/// <param name="dictionary"></param>
	/// <param name="key"></param>
	/// <param name="value"></param>
	public static void AddIfNotExist<T, T1>(this IDictionary<T, T1> dictionary, T key, T1 value)
	{
		if (!dictionary.ContainsKey(key)) dictionary.Add(key, value);

	}
}

public static class TypeExtensions
{
	public static bool IsOfType(this TypeReference tr, Type type)
	{
		if (null == tr) return false;
		var tr0 = type.GetTypeReference();
		if (null == tr0) return false;
		return tr0.FullName == tr.FullName;
	}
	public static bool IsOfType(this Type type, TypeReference tr)
	{
		var tr0 = type.GetTypeReference();
		return tr0.FullName == tr.FullName;
	}
	public static TypeReference GetTypeReference(this Type type)
	{
		var dll = type.Assembly.Location;
		var tr = new TypeReference(type.Namespace, type.Name, ModuleDefinition.ReadModule(dll), null);
		return tr;
	}
}