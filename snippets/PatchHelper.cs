using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ToolsHelper
{
    public static class PatchHelper
    {
        private static string m_connectionString;

        public static string GetConnectionString()
        {

            #region "get connection string"
            Console.Write("Please provide the center code : ");
            var center = Console.ReadLine();
            if (string.IsNullOrEmpty(center))
            {
                Console.WriteLine("Is not a valid center code");
            }

            m_connectionString = string.Format("Data Source=FDB{0}01\\KATMAI,51551;Initial Catalog=forensic_{0};Integrated Security=True;Application Name=forensic_client;Asynchronous Processing=true;", center);

            if (center == "1234")
            {
                m_connectionString = string.Format("Data Source=.\\KATMAI;Initial Catalog=forensic;Integrated Security=True;Application Name=forensic_client;Asynchronous Processing=true;");
            }
            return m_connectionString;
            #endregion


        }

        public static bool CanApplyPatch(string patchNumber)
        {
            // check patch
            using (var conn = new SqlConnection(m_connectionString))
            using (var command = new SqlCommand("SELECT ValueColumn FROM Forensic.Setting WHERE [KeyColumn] = @Key", conn))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Key", patchNumber);
                conn.Open();

                var patches = new List<string>();
                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                    {
                        patches.Add(reader.GetString(0));
                    }
                }
                if (patches.Count > 0)
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("This patch has been applied on :");
                    Console.ForegroundColor = ConsoleColor.Green;
                    patches.ForEach(Console.WriteLine);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("Yes to re-apply or No to exit :");
                    Console.ForegroundColor = color;
                    var yes = Console.ReadLine();
                    if (yes != "Yes")
                        return false;
                }
                return true;
            }
        }

        public static void ApplyPatch(string patchNumber)
        {

            // update patch info
            using (var conn = new SqlConnection(m_connectionString))
            using (var command = new SqlCommand("INSERT INTO Forensic.Setting(KeyColumn, ValueColumn) VALUES(@Key, @Value)", conn))
            {
                var fileVersion = string.Empty;
                var atts = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if (atts.Length == 1)
                {
                    fileVersion = ((AssemblyFileVersionAttribute)atts[0]).Version;
                }

                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@Key", patchNumber);
                command.Parameters.AddWithValue("@Value", string.Format("{0:s},{1}", DateTime.Now, fileVersion));

                conn.Open();
                command.ExecuteNonQuery();

            }
        }
    }
}