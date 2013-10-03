<Query Kind="Statements">
  <Connection>
    <ID>b3819c1f-fba2-4316-80ee-6e094a070d4a</ID>
    <Persist>true</Persist>
    <Server>(localdb)\Projects</Server>
    <Database>sph</Database>
  </Connection>
  <Output>DataGrids</Output>
  <Reference Relative="..\bin\Debug\domain.sph.dll">C:\project\work\sph\bin\Debug\domain.sph.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
</Query>

/* updates all spaces to make it available for applications */

var apps = ApplicationTemplates.Select (at => at.ApplicationTemplateId).ToArray();

foreach (var item in Spaces)
{
 	var space = Bespoke.Sph.Domain.XmlSerializerService.Deserialize<Bespoke.Sph.Domain.Space>(item.Data);
 	space.SpaceId = item.SpaceId;
	space.IsAvailable = true;
	space.IsOnline = true; 
 	space.ApplicationTemplateOptions = apps;
	
	item.Data = space.ToXElement();
	item.IsAvailable = true;
	item.IsOnline = true;
	item.RentalRate = space.RentalRate;
	
	SubmitChanges();
}