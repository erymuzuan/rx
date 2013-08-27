<Query Kind="Statements">
  <Connection>
    <ID>84e06ebb-98ea-4fa0-a47c-8535465a77e6</ID>
    <Server>.\KATMAI</Server>
    <Database>Sph</Database>
  </Connection>
  <Output>DataGrids</Output>
  <Reference Relative="..\source\web\web.sph.commercial-space\bin\domain.commercialspace.dll">C:\project\work\sph\source\web\web.sph.commercial-space\bin\domain.commercialspace.dll</Reference>
  <Reference Relative="..\source\web\web.sph.commercial-space\bin\System.Spatial.dll">C:\project\work\sph\source\web\web.sph.commercial-space\bin\System.Spatial.dll</Reference>
  <Namespace>Bespoke.SphCommercialSpaces.Domain</Namespace>
</Query>

var l = XmlSerializerService.DeserializeFromXml<Bespoke.SphCommercialSpaces.Domain.Land>(Lands.First().Data.ToString());
var locations = new []{"Jeli", "Tanah Merah", "Pasir mas", "Kota Bharu", "Gua Musang", "Bukit Bunga"};

for (int i = 0; i < 500; i++)
{
		var loc = locations.OrderBy(f => Guid.NewGuid()).First();
		var land = l.Clone();
		land.Title = "Tanah " + i;
		land.Location = loc;
		land.Size = 4000 + (i % 10);
		land.Address.City = loc;
		
		var item = new LINQPad.User.Land{
		Data = land.ToXElement(),
		Status = "OK",
		ChangedBy = "LinqPad",
		ChangedDate = DateTime.Now,
		CreatedBy = "LinqPad",
		CreatedDate = DateTime.Now,
		EncodedWkt = l.EncodedWkt,
		Location = land.Location,
		Lot = land.Lot,
		//Path = l.Path,
		SheetNo = land.SheetNo,
		Size = land.Size,
		Title = land.Title,
		Wkt = land.Wkt
	};
	
	Lands.InsertOnSubmit(item);
	
}
SubmitChanges();