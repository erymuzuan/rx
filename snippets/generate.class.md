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
	md.AppendLine("##Overview");
	
	md.AppendLine("##Properties");
	md.AppendLine(@"<table class=""table table-condensed table-bordered"">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>");
	foreach (var prop in type.GetProperties(BindingFlags.Public|BindingFlags.Instance |BindingFlags.DeclaredOnly))
	{
		
		md.AppendLine("<tr>td>" + prop.Name + "</td><td> - </td></tr>" );
	}
	md.AppendLine("</tbody></table>");
	Console.WriteLine (md.ToString());
	File.WriteAllText(@"c:\
}
