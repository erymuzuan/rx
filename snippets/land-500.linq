<Query Kind="Statements">
  <Connection>
    <ID>a60442a9-d977-4eac-a3f0-a8a7142bbe06</ID>
    <Server>(localdb)\Projects</Server>
    <Database>Sph</Database>
    <DisplayName>sph</DisplayName>
  </Connection>
  <Output>DataGrids</Output>
  <Reference Relative="..\source\web\web.sph.commercial-space\bin\domain.commercialspace.dll">C:\project\work\sph\source\web\web.sph.commercial-space\bin\domain.commercialspace.dll</Reference>
  <Reference Relative="..\source\web\web.sph.commercial-space\bin\System.Spatial.dll">C:\project\work\sph\source\web\web.sph.commercial-space\bin\System.Spatial.dll</Reference>
  <Namespace>Bespoke.SphCommercialSpaces.Domain</Namespace>
</Query>

var l = new Bespoke.SphCommercialSpaces.Domain.Land{
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
	Usage = "Perdaganga"
};
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
		Size = land.Size + (i % 10),
		Title = land.Title,
		Wkt = land.Wkt
	};	
	Lands.InsertOnSubmit(item);	
}
SubmitChanges();