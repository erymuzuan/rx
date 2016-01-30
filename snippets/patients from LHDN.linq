<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="..\bin\output\DevV1.Patient.dll">E:\project\work\rx\bin\output\DevV1.Patient.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\domain.sph.dll">e:\project\work\rx\source\web\web.sph\bin\domain.sph.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\Newtonsoft.Json.dll">e:\project\work\rx\source\web\web.sph\bin\Newtonsoft.Json.dll</Reference>
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
	var files = Directory.GetFiles(@"E:\temp\lhdn\14","*.txt",SearchOption.AllDirectories);
	foreach (var txt in files)
	{
		DoWork(txt).Wait();
	}
	Console.WriteLine ("Done");
		
}

private async static Task DoWork(string lhdn)
{
	var lines = File.ReadLines(lhdn);
	var patients = new List<Patient>();
	var count = 0;
	foreach (var l in lines)
	{
		count++;
		var patient = new Patient
			{
				FullName = l.Read(73,134),
				Mrn = l.Read(58,72),
				HomeAddress = new CustomHomeAddress{
					Street = l.Read(226,267),
					//Street2 = l.Read(266,307),
					City = l.Read(346,377),
					State = l.Read(386,417),
					Postcode = l.Read(376,382)
				},
				PassportNo = l.Read(162,171),
				Religion = l.GuessReligion(),
				Race = l.GuessRace(),
				IdentificationNo = l.Read(138,151),
				Nationality = "Malaysian",
				WebId = Guid.NewGuid().ToString(),
				Occupation = "",
				Gender = l.GuessGender()
				
			};
		DateTime dob;
		var culture = System.Globalization.CultureInfo.InvariantCulture;
		var style = System.Globalization.DateTimeStyles.None;
		//Console.WriteLine (l.Read(186,197));
		if(DateTime.TryParseExact(l.Read(186,197),"yyyy-MM-dd", culture,style, out dob))
		{
			patient.Dob = dob;
			patient.Age =Convert.ToInt32( (DateTime.Today - dob).TotalDays/ 365);
			patient.Income = Convert.ToDecimal( (DateTime.Today - dob).TotalDays);
		}else
		{
			if(!string.IsNullOrWhiteSpace(patient.IdentificationNo) && DateTime.TryParseExact("19" + patient.IdentificationNo.Substring(0,6),"yyyyMMdd", culture,style, out dob))
			{
				patient.Dob = dob;
				patient.Age =Convert.ToInt32( (DateTime.Today - dob).TotalDays/ 365);
				patient.Income = Convert.ToDecimal( (DateTime.Today - dob).TotalDays);
			}else
			{
				if(!string.IsNullOrWhiteSpace(patient.IdentificationNo))
					Console.WriteLine ("[{1}] Cannot parse DOB : {0} - {2}",patient.IdentificationNo.Substring(0,6), count,patient.IdentificationNo);
			}
		}
		DateTime rd;
		if(DateTime.TryParseExact(l.Read(205,216), "yyyy-MM-dd", culture,style,out rd))
		{
			patient.RegisteredDate = rd;
		}else
		{
			patient.RegisteredDate = new DateTime(2000,1,1);
		}
		if(patient.Dob != DateTime.MinValue)
			patients.Add(patient);
		// do it btach of 50
		if(patients.Count > 50 )
		{
			/**/
			await RegisterAll(patients);
			patients.Clear();
		}
		
	}
	// the last bit
	/**/
	await RegisterAll(patients);
	patients.Clear();
	Console.WriteLine ("Done with " + lhdn);
}

private static async Task RegisterAll(List<Patient> patients)
{
	var tasks = from p in patients
				select Register(p);
	await Task.WhenAll(tasks);
}
private static async Task Register(Patient patient)
{
		// post it to
		var json = patient.ToJsonString();
		var request = new StringContent(json);
		var url = "/api/patients/register";
		
		var handler = new HttpClientHandler();
			
		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "6DWmE2B7xIlZBaTSjlDaWsiV8SEYMjUUOqxci/hBD+L4BlTK9l5ULh2bze2+iNFwcgunZG1szruQjDVJ9IJYM3y9l/Ps9rA1rYFRgUTDuUdRUcouWyrXuNRoDaQPTCuq17gx0iaBJrsZ1DHAAprwT8xsfiKY12syzxbo+Py1yzebmui0vpz8Y3e6oEMKR24Rx4Fy/bAtGNmIi1cdaitSrgK8BRFlRrkLXDRabc3s8G+6Ul7nOGLIpdxPUyCPfSDveUqf/ssbcxdjAgo2p66NYwwkMcg28/ZNbiwZ2QZfZWqSyvtR73Q0NjISOaVcXtexIaRBgl9h292k/Nxl0agUjOw6G6exehH7OVfeTwLE/nrp4Hi9PaaGAIJUblBEbDffIrQJVkth/M/Y3R4O1vDoaFIdk73dAEMSiVf6B9LIWHKc22iORjbhVzH/mYPrb2b6b0kXIETkOf9NkSgY/BEl/e5oL2AOkPvj3SJLtgHVBHEPdUlbU3vGg383BpWNSs5EKRK5+uNkUKesSmzzz7ot2yikXLHJvjPvbsfmosAG/M7SFkaH65csa0TpZXIJ58qs3ScemyMi4h0G/K5OPAGZ0Q6tayP4hxEAjTuxnjrNXUiOl4sKC9FbMatD3bsuNWVpu4psRmuMhDYx4iDctTT7rCM6bbTLDPJLVHyQMzko86J5LfDuOu1kfPIX2ICH/BD8c5ITsLQj+S5uCWmekSAUDu+ni/MjjUIgC3n62tKov19kQT5wMnMnhcapNhm/l7SDv/kOi6Bs5TWudBjm+lS/WiE7nD94+toFSoNU9YS4MFy9N6rPNHIzthOxsARqDdvWbxBjq9joLXGuxbT2J4FUIvBY1GhrVRjjjhWbVsW3pAMPhqAoM/kCpkvNM9DK7S1Sc6YZcHuIzMvl+VyU6rGDKRiGW0Ai3vV8sJXF3Tc+Gi0jLNr7G/t8g5HIYDmZbP15pAyx0OBVp55YX1e58WpCwjoQCAbEk2XoT0wBczLaP6kwI+uyL5ONYC5iFlYh5WHr");
			client.BaseAddress = new Uri("http://localhost:4436");
	
			var response = await client.PostAsync(url, request);
			var content = response.Content as StreamContent;
			if (null == content) throw new Exception("Cannot execute query on es " + request);
			//this.Response.ContentType = "application/json; charset=utf-8";
			var r = await content.ReadAsStringAsync();
			if(response.StatusCode == HttpStatusCode.OK)
				Console.Write (".");
			else
				Console.Write("FAILED :" + r);
	
		}

}

// Define other methods and classes here
public static class Strings
{
	public static string Read(this string line, int startIndex, int endIndex)
	{
		if(line.Length < endIndex - 1)return string.Empty;
		return line.Substring(startIndex, endIndex -1 - startIndex).TrimEnd();
	}
	
	public static string GuessReligion(this string line)
	{
		var name = line.Read(73,134);
		if(name.Contains(" BIN "))return "Islam";
		if(name.Contains(" BINTI "))return "Islam";
		if(name.Contains(" B. "))return "Islam";
		if(name.Contains(" BT. "))return "Islam";
		if(name.Contains(" B "))return "Islam";
		if(name.Contains(" BT "))return "Islam";
		if(name.Contains(" A/L "))return "Hinduism";
		if(name.Contains(" A/P "))return "Hinduism";
	
		
		return "Others";
	}
	
	
	public static string GuessGender(this string line)
	{
		var name = line.Read(73,134);
		if(name.Contains(" BIN "))return "Male";
		if(name.Contains(" BINTI "))return "Female";
		if(name.Contains(" B. "))return "Male";
		if(name.Contains(" BT. "))return "Female";
		if(name.Contains(" B "))return "Male";
		if(name.Contains(" BT "))return "Female";
		if(name.Contains(" A/L "))return "Male";
		if(name.Contains(" A/P "))return "Female";
		
		var id = line.Read(138,151).ToCharArray().LastOrDefault();
		int lastid;
		if(int.TryParse(string.Format("{0}", id), out lastid))
		{
			return lastid % 2 == 0 ? "Female" : "Male";
		}
		
		return "Others";
	}
	
	
	
	public static string GuessRace(this string line)
	{
		var name = line.Read(73,134);
		if(name.Contains(" BIN "))return "Malay";
		if(name.Contains(" BINTI "))return "Malay";
		if(name.Contains(" B. "))return "Malay";
		if(name.Contains(" BT. "))return "Malay";
		if(name.Contains(" B "))return "Malay";
		if(name.Contains(" BT "))return "Malay";
		if(name.Contains(" A/L "))return "Indian";
		if(name.Contains(" A/P "))return "Indian";
		int count = name.Split(' ').Length;
		if(count == 3) return "Chinese";
		
		return "Others";
	}
}