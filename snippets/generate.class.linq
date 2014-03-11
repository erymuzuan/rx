<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var domain = Assembly.LoadFile(@"C:\project\work\sph\source\web\web.sph\bin\domain.sph.dll");
var types = domain.GetTypes()
.Where (d => !d.IsGenericType)
.Where (d => d.IsPublic);


foreach (var type in types)
{
	var md = new StringBuilder("#" + type.Name);
	md.AppendLine();
	md.AppendLine("##Overview");
	md.AppendLine();	
	md.AppendLine();	
	md.AppendLine();
	md.AppendLine("##Properties");
	md.AppendLine(@"<table class=""table table-condensed table-bordered"">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>");
	foreach (var prop in type.GetProperties(BindingFlags.Public|BindingFlags.Instance |BindingFlags.DeclaredOnly).OrderBy (t => t.Name))
	{
		
		md.AppendLine("<tr><td>" + prop.Name + "</td><td> - </td></tr>" );
	}
	md.AppendLine("</tbody></table>");
	Console.WriteLine (md.ToString());
	
		md.AppendLine();
		md.AppendLine();
		md.AppendLine();
				
		md.AppendLine("## See also");
		md.AppendLine();
		try{
			if(null == type.BaseType)continue;
			if(type.BaseType.Name == "DomainObject")continue;
			if(type.BaseType.Name == "Entity")continue;
			if(type.BaseType.Name == "Object")continue;
			if(type.IsInterface) continue;
			var sameLevels = types.Where (t => type.BaseType.IsAssignableFrom(t));
			foreach (var f in sameLevels)
			{			
				md.AppendLine();
				md.AppendFormat("[{0}](/docs/#{0}.html)",f.Name);
			}
		}
		finally
		{
			File.WriteAllText(@"C:\project\work\sph\source\web\web.sph\docs\" + type.Name + ".md",md.ToString());
		}
	
}
