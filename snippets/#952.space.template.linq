<Query Kind="Statements">
  <Connection>
    <ID>b3819c1f-fba2-4316-80ee-6e094a070d4a</ID>
    <Server>(localdb)\Projects</Server>
    <Database>sph</Database>
  </Connection>
  <Output>DataGrids</Output>
</Query>

Console.WriteLine ("NOTE : Rename SpaceLotsElement  to SpaceUnitElement in SpaceTemplate" );
foreach (var template in SpaceTemplates)
{
	var xml = template.Data.ToString().Replace("SpaceLotsElement", "SpaceUnitElement");
	template.Data = XElement.Parse(xml);
	
	SubmitChanges();
}
// now doing the building
foreach (var b in Buildings)
{
	var xml = b.Data.ToString()
	.Replace("LotCollection", "UnitCollection")
	.Replace("<Lot", "<Unit")
	.Replace("</Lot>", "</Unit>")
	;
	b.Data = XElement.Parse(xml);
	
	SubmitChanges();
}