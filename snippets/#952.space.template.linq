<Query Kind="Statements">
  <Connection>
    <ID>84e06ebb-98ea-4fa0-a47c-8535465a77e6</ID>
    <Persist>true</Persist>
    <Server>.\KATMAI</Server>
    <Database>Sph</Database>
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