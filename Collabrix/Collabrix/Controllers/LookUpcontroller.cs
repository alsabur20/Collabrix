using Collabrix.Models;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace Collabrix.Controllers
{
    public class LookUpcontroller
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<List<Lookup>> GetLookUps(int lookUpType)
        {
            List<Lookup> lookUps = new List<Lookup>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = $"SELECT * FROM LookUp WHERE LookUpType = {lookUpType}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Lookup lookUp = new Lookup
                                {
                                    LookupId = reader.GetInt32(reader.GetOrdinal("LookUpId")),
                                    Value = reader.GetString(reader.GetOrdinal("Value")),
                                    Category = reader.GetString(reader.GetOrdinal("Category")),
                                    Description = reader.GetString(reader.GetOrdinal("Description"))
                                };
                                lookUps.Add(lookUp);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
            return lookUps;
        }

        public static string getValueById(int id)
        { 
            string value = "";
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = $"SELECT Value FROM LookUp WHERE LookupId = {id}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader =  command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                value = reader.GetString(reader.GetOrdinal("Value"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
            return value;
        }

        public static int getIdByValue(string value)
        { 
            int id = 0;
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = $"SELECT LookupId FROM LookUp WHERE Value = '{value}'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader =  command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader.GetInt32(reader.GetOrdinal("LookupId"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
            return id;
        }
    }
}
