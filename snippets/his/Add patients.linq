<Query Kind="Statements">
  <Connection>
    <ID>116d4209-bdea-481e-8626-0da5620bb091</ID>
    <Persist>true</Persist>
    <Server>(localdb)\ProjectsV13</Server>
    <Database>His</Database>
  </Connection>
</Query>


var count = 0;
foreach (var f in Directory.GetFiles(@"E:\OneDrive\Pictures\Italia2015", "*.jpg").Select(x => new FileInfo(x))
.Where(x => x.Length <= 1092712 && x.Length > 4572)
.OrderBy(x => x.Length))
{
	count ++;
	System.Data.Linq.Binary card = new Binary(File.ReadAllBytes(f.FullName));

	var patient = new Patient
	{
		Mrn = Path.GetFileNameWithoutExtension(f.Name),
		Name = "Wan " + Path.GetFileNameWithoutExtension(f.Name),
		Gender = (new []{'M', 'F'}).OrderBy(g => Guid.NewGuid()).First()
		Income = 2500.00m + (count + 120),
		Dob = (new DateTime(1980, 4, 6)).AddYears(count),
		Nationality = "Malaysian",
		Race = "Islam",
		Age = Convert.ToByte(count + 36),
		Nrid = 800406034567 + count,
		PassportNo = "",
		BirthCert = "C45666",
		IdCardCopy = card,
		Fee = 50.05m + count,
		Weight = 48.00m + count,
		Height = 160f + (count * 0.1f),
		Address = XElement.Parse(@"<Address Street=""No 1"" State=""Kelantan""/>"),
		AdditionalInfo = count % 3 == 0 ? null : XElement.Parse($@"<AdditionalInfo Count=""{count}"" Nrid=""{800406034567 + count}""/>")
		IsCivilServant = true,
		IsChildren = false,
		RegisteredDate = DateTime.Today,
		ModifiedDate = DateTime.Now
	};

	Patients.InsertOnSubmit(patient);
	SubmitChanges();
}
