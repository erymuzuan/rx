<Query Kind="Program">
  <Connection>
    <ID>353b5cd5-b9db-4c86-bb0e-0003d679e40f</ID>
    <Persist>true</Persist>
    <Server>.\DEV2016</Server>
    <Database>His</Database>
    <DisplayName>dev2016-his</DisplayName>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
</Query>

void Main()
{
	
	var count = 0;
	foreach (var f in Directory.GetFiles(@"F:\temp\images", "*.*").Select(x => new FileInfo(x))
//	.Where(x => x.Length <= 1092712)
	.OrderBy(x => x.Length))
	{
		count ++;
		System.Data.Linq.Binary card = new Binary(File.ReadAllBytes(f.FullName));
	
		var patient = new Patient
		{
			Mrn = Path.GetFileNameWithoutExtension(f.Name).Replace(" ", ""),
			FirstName = Path.GetFileNameWithoutExtension(f.Name),
			LastName = "Wan",
			Gender = (new []{'M', 'F'}).OrderBy(g => Guid.NewGuid()).First(),
			Income = 2500.00m + (count + 120),
			Dob = (new DateTime(1980, 4, 6)).AddYears(count).AddDays(count).AddMonths(count),
			NationalityCode = Random((byte)1, (byte)2, (byte)3, (byte)1),
			RaceCode = Random(Enumerable.Range(1,5).Select(x => Convert.ToByte(x)).ToArray()),
			Age = Convert.ToByte(count + 36),
			Nrid = 800406034567 + count,
			PassportNo = "",
			BirthCert = "C45666",
			IdCardCopy = card,
			IdCardMimeType = System.Web.MimeMapping.GetMimeMapping(Path.GetExtension(f.FullName)),
			Fee = 50.05m + count,
			Weight = 48.00m + count,
			Height = 160f + (count * 0.1f),
			Address = XElement.Parse(@"<Address Street=""No 1"" State=""Kelantan""/>"),
			AdditionalInfo = count % 3 == 0 ? null : XElement.Parse($@"<AdditionalInfo Count=""{count}"" Nrid=""{800406034567 + count}""/>"),
			IsCivilServant = (new []{true, false}).OrderBy(g => Guid.NewGuid()).First(),
			IsChildren = (new []{true, false}).Random(),
			RegisteredDate = DateTime.Today,
			ModifiedDate = DateTime.Now			
		};
	
		Patients.InsertOnSubmit(patient);
		try
		{	        
			SubmitChanges();
		}
		catch (Exception ex)
		{
		Console.WriteLine(ex.Message);
		Console.WriteLine(f.Name);
		}
	}
	
}

// Define other methods and classes here

public static T Random<T>(params T[] list) {
	return RandomExtension.Random(list);
}
public static class RandomExtension
{
	public static T Random<T>(this IEnumerable<T> list)
	{
		return list.OrderBy(g => Guid.NewGuid()).FirstOrDefault();
	}
}