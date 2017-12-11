<Query Kind="Program">
  <Reference Relative="..\source\web\web.sph\bin\DevV1.Customer.dll">F:\project\work\rx.v1\source\web\web.sph\bin\DevV1.Customer.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\domain.sph.dll">F:\project\work\rx.v1\source\web\web.sph\bin\domain.sph.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\Newtonsoft.Json.dll">F:\project\work\rx.v1\source\web\web.sph\bin\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>RestSharp</NuGetReference>
  <Namespace>Bespoke.DevV1.Customers.Domain</Namespace>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>RestSharp</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var files = Directory.GetFiles(@"E:\temp\lhdn\23", "*.txt", SearchOption.AllDirectories);
	foreach (var txt in files)
	{
		var reader = new CustomerReader();
		reader.StartAsync(txt).Wait();
	}
	Console.WriteLine("Done");



}

// Define other methods and classes here
public class CustomerReader
{
	const int BATCH_SIZE = 100;
	public async Task StartAsync(string lhdn)
	{
		var lines = File.ReadLines(lhdn);
		var customers = new List<Customer>();
		var count = 0;
		foreach (var l in lines.Take(100))
		{
			count++;
			var customer = Parse(l, count);
			if (customer.RegisteredDate != DateTime.MinValue)
				customers.Add(customer);

			if (customers.Count > BATCH_SIZE)
			{
				await RegisterAll(customers);
				customers.Clear();
			}

		}
		// the last bit
		/**/
		await RegisterAll(customers);
		customers.Clear();
		Console.WriteLine("Done with " + lhdn);
	}

	private async Task RegisterAll(List<Customer> customers)
	{
		var tasks = from p in customers
					select Register(p);
		await Task.WhenAll(tasks);
	}
	private Random m_random = new Random(5000);
	HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:8081") };
	private async Task Register(Customer customer)
	{
		if (string.IsNullOrWhiteSpace(customer.AccountNo)) return;
		await Task.Delay(10);
		// post it to
		customer.Id = customer.AccountNo;
		customer.CreatedBy = "erymuzuan";
		customer.ChangedBy = "azrol";

		var changed = customer.RegisteredDate.AddDays(m_random.Next(15000));
		if (changed >= DateTime.Today)
			changed = DateTime.Now.AddDays(-m_random.Next(9000));

		customer.ChangedDate = changed;
		customer.CreatedDate = customer.ChangedDate.AddDays(-m_random.Next(15000));

		var json = customer.ToJson();


		var request = new StringContent(json);
		client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImRldmVsb3BlcnMiXSwiZW1haWwiOiJhZG1pbkB5b3VyY29tcGFueS5jb20iLCJzdWIiOiI2MzYzNjczOTE0ODQwMTczMjY0MTdiMDFlYyIsIm5iZiI6MTUxNzAxMTE0OCwiaWF0IjoxNTAxMTEzNTQ4LCJleHAiOjE1MTQ3NjQ4MDAsImF1ZCI6IkRldlYxIn0.RpNvCVYHkkXOJvGrv8IGtbY2aBWvLJ9EEnvdmhDjbxs");
		request.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
		
		var response = await client.PostAsync("/api/customers/", request);
		response.EnsureSuccessStatusCode();

		$"Done with  {customer.FirstName}".Dump("API");

	}

	private Customer Parse(string l, int count)
	{

		var customer = new Customer
		{
			FirstName = l.Read(73, 134),
			AccountNo = l.Read(58, 72),
			Address = new Bespoke.DevV1.Customers.Domain.Address
			{
				Street1 = l.Read(226, 267),
				//Street2 = l.Read(266,307),
				District = l.Read(346, 377),
				State = l.Read(386, 417),
				Postcode = l.Read(376, 382)
			},
			WebId = Guid.NewGuid().ToString(),
			Gender = l.GuessGender()

		};
		DateTime dob;
		var culture = System.Globalization.CultureInfo.InvariantCulture;
		var style = System.Globalization.DateTimeStyles.None;
		//Console.WriteLine (l.Read(186,197));
		if (DateTime.TryParseExact(l.Read(186, 197), "yyyy-MM-dd", culture, style, out dob))
		{
			customer.RegisteredDate = dob;
			customer.Age = Convert.ToInt32((DateTime.Today - dob).TotalDays / 365);
			customer.Revenue = Convert.ToDecimal((DateTime.Today - dob).TotalDays);
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(customer.AccountNo) && DateTime.TryParseExact("19" + customer.AccountNo.Substring(0, 6), "yyyyMMdd", culture, style, out dob))
			{
				customer.RegisteredDate = dob;
				customer.Age = Convert.ToInt32((DateTime.Today - dob).TotalDays / 365);
				customer.Revenue = Convert.ToDecimal((DateTime.Today - dob).TotalDays);
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(customer.AccountNo))
					Console.WriteLine("[{1}] Cannot parse DOB : {0} - {2}", customer.AccountNo.Substring(0, 6), count, customer.AccountNo);
			}
		}
		DateTime rd;
		if (DateTime.TryParseExact(l.Read(205, 216), "yyyy-MM-dd", culture, style, out rd))
		{
			customer.RegisteredDate = rd;
		}
		else
		{
			customer.RegisteredDate = new DateTime(2000, 1, 1);
		}

		return customer;
	}

}

// Define other methods and classes here
public static class Strings
{
	public static string Read(this string line, int startIndex, int endIndex)
	{
		if (line.Length < endIndex - 1) return string.Empty;
		return line.Substring(startIndex, endIndex - 1 - startIndex).TrimEnd();
	}

	public static string GuessReligion(this string line)
	{
		var name = line.Read(73, 134);
		if (name.Contains(" BIN ")) return "Islam";
		if (name.Contains(" BINTI ")) return "Islam";
		if (name.Contains(" B. ")) return "Islam";
		if (name.Contains(" BT. ")) return "Islam";
		if (name.Contains(" B ")) return "Islam";
		if (name.Contains(" BT ")) return "Islam";
		if (name.Contains(" A/L ")) return "Hinduism";
		if (name.Contains(" A/P ")) return "Hinduism";


		return "Others";
	}


	public static string GuessGender(this string line)
	{
		var name = line.Read(73, 134);
		if (name.Contains(" BIN ")) return "Male";
		if (name.Contains(" BINTI ")) return "Female";
		if (name.Contains(" B. ")) return "Male";
		if (name.Contains(" BT. ")) return "Female";
		if (name.Contains(" B ")) return "Male";
		if (name.Contains(" BT ")) return "Female";
		if (name.Contains(" A/L ")) return "Male";
		if (name.Contains(" A/P ")) return "Female";

		var id = line.Read(138, 151).ToCharArray().LastOrDefault();
		int lastid;
		if (int.TryParse(string.Format("{0}", id), out lastid))
		{
			return lastid % 2 == 0 ? "Female" : "Male";
		}

		return "Others";
	}



	public static string GuessRace(this string line)
	{
		var name = line.Read(73, 134);
		if (name.Contains(" BIN ")) return "Malay";
		if (name.Contains(" BINTI ")) return "Malay";
		if (name.Contains(" B. ")) return "Malay";
		if (name.Contains(" BT. ")) return "Malay";
		if (name.Contains(" B ")) return "Malay";
		if (name.Contains(" BT ")) return "Malay";
		if (name.Contains(" A/L ")) return "Indian";
		if (name.Contains(" A/P ")) return "Indian";
		int count = name.Split(' ').Length;
		if (count == 3) return "Chinese";

		return "Others";
	}
}