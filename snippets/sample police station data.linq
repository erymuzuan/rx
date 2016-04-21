<Query Kind="Statements">
  <Reference Relative="..\source\web\web.sph\bin\DevV1.PoliceStation.dll">C:\project\work\sph\source\web\web.sph\bin\DevV1.PoliceStation.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\domain.sph.dll">C:\project\work\sph\source\web\web.sph\bin\domain.sph.dll</Reference>
  <Reference Relative="..\source\web\web.sph\bin\Newtonsoft.Json.dll">C:\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll</Reference>
  <Namespace>Bespoke.DevV1.PoliceStations.Domain</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

var ps = new PoliceStation {
	Id = "123456",
	Name = "Balai Polist Bukit Bunga",
	Address = new Address {
		Street1 = "No 1",
		Street2 = "Jalan Besar",
		Postcode = "17700",
		State = "Kelantan"
	},
	HqAddress = new HqAddress {
	State = "Kuala Lumpur"
	},
	OcsAddress = new HomeAddress {
	 Postcode = "189999"
	}
};

ps.Policemen.Add(new Police { Name = "Ali", Rank = "Const", Age = 25 });
ps.Policemen.Add(new Police { Name = "Abu", Rank = "Const", Age = 25 });


JsonConvert.SerializeObject(ps).Dump();