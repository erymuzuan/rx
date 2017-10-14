<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="..\bin\output\DevV1.Patient.dll">F:\project\work\rx.pos-entt\bin\output\DevV1.Patient.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\domain.sph.dll">F:\project\work\rx.pos-entt\source\web\web.sph\bin\domain.sph.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\Newtonsoft.Json.dll">F:\project\work\rx.pos-entt\source\web\web.sph\bin\Newtonsoft.Json.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>Bespoke.Sph.Domain</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Bespoke.DevV1.Patients.Domain</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
</Query>

void Main()
{
	var files = Directory.GetFiles(@"E:\temp\lhdn\22", "*.txt", SearchOption.AllDirectories);
	foreach (var txt in files)
	{
		var reader = new PatientReader();
		reader.StartAsync(txt).Wait();
	}
	Console.WriteLine("Done");

}

public class PatientReader
{
	const int BATCH_SIZE = 100;
	const string BASE_URL = "http://localhost:4436";
	const string TOKEN = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImRldmVsb3BlcnMiXSwiZW1haWwiOiJhZG1pbkB5b3VyY29tcGFueS5jb20iLCJzdWIiOiI2MzYzODkwMzA0ODgzNjc0MjZlNjRjMTNlZiIsIm5iZiI6MTUxOTE3NTA0OSwiaWF0IjoxNTAzMjc3NDQ5LCJleHAiOjE1MDk0MDgwMDAsImF1ZCI6IkRldlYxIn0._gdyzqOqVHP7sxPZ7VAEmj-sZ-JhIe4fS1yRzftMdNQ";
	private HttpClient client = new HttpClient { BaseAddress = new Uri(BASE_URL) };
	public PatientReader()
	{
		var handler = new HttpClientHandler();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TOKEN);

	}
	public async Task StartAsync(string lhdn)
	{
		var lines = File.ReadLines(lhdn);
		var patients = new List<Patient>();
		var count = 0;
		foreach (var l in lines)
		{
			count++;
			var patient = Parse(l, count);
			if (patient.Dob != DateTime.MinValue)
				patients.Add(patient);
				
			if (patients.Count > BATCH_SIZE)
			{
				await RegisterAll(patients);
				patients.Clear();
			}

		}
		// the last bit
		/**/
		await RegisterAll(patients);
		patients.Clear();
		Console.WriteLine("Done with " + lhdn);
	}

	private async Task RegisterAll(List<Patient> patients)
	{
		var tasks = from p in patients
					select Register(p);
		await Task.WhenAll(tasks);
	}

	private async Task Register(Patient patient)
	{
		// post it to
		var json = patient.ToJsonString();
		var request = new StringContent(json);
		request.Headers.ContentType = new MediaTypeHeaderValue("application/json");
		var url = "/api/patients";

		var response = await client.PostAsync(url, request);
		var content = response.Content as StreamContent;
		if (null == content) throw new Exception("Cannot execute query on es " + request);
		//this.Response.ContentType = "application/json; charset=utf-8";
		var r = await content.ReadAsStringAsync();
		if (response.StatusCode == HttpStatusCode.OK)
			Console.Write(".");
		else
			Console.Write("FAILED :" + r);
	}
	
	private Patient Parse(string l, int count)
	{

		var patient = new Patient
		{
			Status = "New",
			MaritalStatus = "Married",			
			FullName = l.Read(73, 134),
			Mrn = l.Read(58, 72),
			HomeAddress = new CustomHomeAddress
			{
				Street = l.Read(226, 267),
				//Street2 = l.Read(266,307),
				City = l.Read(346, 377),
				State = l.Read(386, 417),
				Postcode = l.Read(376, 382)
			},
			PassportNo = l.Read(162, 171),
			Religion = l.GuessReligion(),
			Race = l.GuessRace(),
			IdentificationNo = l.Read(138, 151),
			Nationality = "Malaysian",
			WebId = Guid.NewGuid().ToString(),
			Occupation = "",
			Gender = l.GuessGender()

		};
		DateTime dob;
		var culture = System.Globalization.CultureInfo.InvariantCulture;
		var style = System.Globalization.DateTimeStyles.None;
		//Console.WriteLine (l.Read(186,197));
		if (DateTime.TryParseExact(l.Read(186, 197), "yyyy-MM-dd", culture, style, out dob))
		{
			patient.Dob = dob;
			patient.Age = Convert.ToInt32((DateTime.Today - dob).TotalDays / 365);
			patient.Income = Convert.ToDecimal((DateTime.Today - dob).TotalDays);
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(patient.IdentificationNo) && DateTime.TryParseExact("19" + patient.IdentificationNo.Substring(0, 6), "yyyyMMdd", culture, style, out dob))
			{
				patient.Dob = dob;
				patient.Age = Convert.ToInt32((DateTime.Today - dob).TotalDays / 365);
				patient.Income = Convert.ToDecimal((DateTime.Today - dob).TotalDays);
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(patient.IdentificationNo))
					Console.WriteLine("[{1}] Cannot parse DOB : {0} - {2}", patient.IdentificationNo.Substring(0, 6), count, patient.IdentificationNo);
			}
		}
		DateTime rd;
		if (DateTime.TryParseExact(l.Read(205, 216), "yyyy-MM-dd", culture, style, out rd))
		{
			patient.RegisteredDate = rd;
		}
		else
		{
			patient.RegisteredDate = new DateTime(2000, 1, 1);
		}
		
		return patient;
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