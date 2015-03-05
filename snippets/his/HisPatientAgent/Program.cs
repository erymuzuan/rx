using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using Bespoke.DevV1_patient.Domain;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            using (var notifer = new SqlNotifier())
            {
                notifer.NewMessageArrived += notifer_NewMessage;
                notifer.RegisterDependency().Wait();
                Console.ReadLine();
            }
        }

        async static void notifer_NewMessage(object sender, SqlNotificationEventArgs e)
        {
            Console.WriteLine("**");
            var changes = new List<Tuple<int, int, string>>();
            var patients = new List<HisPatient>();

            const string connectionString =
                @"Data Source=(localdb)\ProjectsV12;Initial Catalog=His;Integrated Security=True";
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT  [Id], [RowId], [Table] FROM dbo.TransactionQueue", conn))
            {
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        changes.Add(new Tuple<int, int, string>(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));
                    }
                }
                foreach (var item in changes)
                {
                    using (var selectCmd = new SqlCommand("SELECT * FROM dbo." + item.Item3 + " WHERE Id = @Id", conn))
                    {
                        selectCmd.Parameters.AddWithValue("@Id", item.Item2);
                        using (var reader = await selectCmd.ExecuteReaderAsync(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                var pt = new HisPatient
                                {
                                    QueueId = item.Item1,
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Mrn = reader.GetString(2),
                                    Gender = reader.GetString(3),
                                    Income = reader.GetDecimal(4),
                                    Dob = reader.GetDateTime(5)
                                };
                                patients.Add(pt);
                            }
                        }
                    }
                }
            }


            foreach (var pt in patients)
            {
                var sphPatient = pt.Transform();
                using (var handler = new HttpClientHandler())
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(ConfigurationManager.AppSettings["BaseUrl"]) })
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Bearer",
                        ConfigurationManager.AppSettings["token"]);

                    var pjson = JsonConvert.SerializeObject(sphPatient);
                    var content = new StringContent(pjson);
                    var response = await client.PostAsync("Patient/Register", content);
                    Console.WriteLine(response.StatusCode);
                }

                //
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM dbo.TransactionQueue WHERE [Id] = @QueueId", conn))
                {
                    cmd.Parameters.AddWithValue("@QueueId", pt.QueueId);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }

            }
            Console.WriteLine("**");
        }
    }


    public class HisPatient
    {
        public int QueueId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mrn { get; set; }
        public string Gender { get; set; }
        public decimal Income { get; set; }
        public DateTime Dob { get; set; }

        public Patient Transform()
        {
            var pt = new Patient
            {
                Mrn = this.Mrn,
                FullName = this.Name,
                Gender = this.Gender,
                Religion = "UNKNOWN",
                Race = "NOT-SPECIFIED",
                Status = "New",
                MaritalStatus = "N/A",
                Id = this.Id.ToString()
            };

            return pt;
        }
    }


}
