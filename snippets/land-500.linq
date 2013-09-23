<Query Kind="Statements">
  <Connection>
    <ID>84e06ebb-98ea-4fa0-a47c-8535465a77e6</ID>
    <Persist>true</Persist>
    <Server>.\KATMAI</Server>
    <Database>Sph</Database>
  </Connection>
  <Output>DataGrids</Output>
  <Reference Relative="..\bin\Debug\domain.sph.dll">C:\project\work\sph\bin\Debug\domain.sph.dll</Reference>
  <Reference Relative="..\bin\Debug\System.Spatial.dll">C:\project\work\sph\bin\Debug\System.Spatial.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
</Query>

var l = new Bespoke.Sph.Domain.Land{
	Location = "Tanah Merah",
	Address = new Address{
		City = "Tanah Merah",
		Country = "Malaysia",
		Postcode = "5600",
		State = "Kelantan"	
	},
	ApprovedBy = "Me",
	ApprovedPlanNo = "234",
	ApprovedDateTime = DateTime.Today,
	CreatedDate = DateTime.Today,
	CurrentMarketValue = 90000m,
	Lot = "W09",
	LandOffice = "Tanah Merah",
	Owner= new Owner{
		Email = "me@bespoke.com.my",
		Name = "me"
		},
	Size = 45,
	Status = "OK",
	Title = "WA09",
	Usage = "Perdagangan"
};
var locations = new []{"Jeli", "Tanah Merah", "Pasir Mas","Pasir Puteh", "Pasir Hor", "Kota Bharu", "Gua Musang", "Bukit Bunga"};
var status = new []{"OK", "NA", "IN"};
for (int i = 1; i < 901; i++)
{
		var loc = locations.OrderBy(f => Guid.NewGuid()).First();
		var land = l.Clone();
		land.Title = "Tanah " + i;
		land.Location = loc;
		land.Status = status.OrderBy(f => Guid.NewGuid()).First();
		land.Size = 84 + (i % 10);
		land.Address.City = loc;
		
		var item = new LINQPad.User.Land{
		Data = land.ToXElement(),
		Status = land.Status,
		ChangedBy = "LinqPad",
		ChangedDate = DateTime.Now,
		CreatedBy = "LinqPad",
		CreatedDate = DateTime.Now,
		EncodedWkt = l.EncodedWkt,
		Location = land.Location,
		Lot = land.Lot,
		//Path = l.Path,
		SheetNo = land.SheetNo,
		Size = land.Size + (i % 10),
		Title = land.Title,
		Wkt = land.Wkt
	};	
	Lands.InsertOnSubmit(item);	
}
SubmitChanges();