<Query Kind="Statements">
  <Connection>
    <ID>84e06ebb-98ea-4fa0-a47c-8535465a77e6</ID>
    <Persist>true</Persist>
    <Server>.\KATMAI</Server>
    <Database>Sph</Database>
  </Connection>
  <Output>DataGrids</Output>
  <Reference Relative="..\source\web\web.sph.commercial-space\bin\domain.commercialspace.dll">C:\project\work\sph\source\web\web.sph.commercial-space\bin\domain.commercialspace.dll</Reference>
  <Reference Relative="..\source\web\web.sph.commercial-space\bin\System.Spatial.dll">C:\project\work\sph\source\web\web.sph.commercial-space\bin\System.Spatial.dll</Reference>
  <Namespace>Bespoke.SphCommercialSpaces.Domain</Namespace>
</Query>

var l = XmlSerializerService.DeserializeFromXml<Bespoke.SphCommercialSpaces.Domain.Land>(Lands.First().Data.ToString());

for (int i = 0; i < 500; i++)
{
		var land = l.Clone();
		l.Title = "Tanah " + i;
		
		var item = new LINQPad.User.Land{
		Data = land.ToXElement(),
		Status = "OK",
		ChangedBy = "LinqPad",
		ChangedDate = DateTime.Now,
		CreatedBy = "LinqPad",
		CreatedDate = DateTime.Now,
		EncodedWkt = l.EncodedWkt,
		Location = l.Location,
		Lot = l.Lot,
		//Path = l.Path,
		SheetNo = l.SheetNo,
		Size = l.Size,
		Title = land.Title,
		Wkt = l.Wkt
	};
	
	Lands.InsertOnSubmit(item);
	
}
SubmitChanges();