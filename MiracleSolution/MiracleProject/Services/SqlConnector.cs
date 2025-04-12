using System;
using System.Data;
using System.Data.SqlClient;
using MiracleProject.Models;

namespace MiracleProject.Services
{
    public class SqlConnector
    {
        private readonly string _connectionString = @"Data Source=JEBINSTECH\MSSQLSERVER01;Initial Catalog=MiracleDB;Integrated Security=True;TrustServerCertificate=True";

        /// <summary>
        /// Checks if connection to MiracleDB works.
        /// </summary>
        public void TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("✅ Connected to MiracleDB successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Database connection failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Fetches all properties from the Properties table.
        /// </summary>
        public DataTable GetAllProperties()
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Properties";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(table);
                }
            }

            return table;
        }

        /// <summary>
        /// Adds a new property record to the database.
        /// </summary>
        /// <param name="property">Property model to insert</param>
        /// <returns>True if inserted successfully, otherwise false.</returns>
        public bool AddProperty(Property property)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Properties 
                                 (PropertyID, Address, MonthlyRent, Bedrooms, SquareFootage, PropertyType, IsOccupied)
                                 VALUES 
                                 (@PropertyID, @Address, @MonthlyRent, @Bedrooms, @SquareFootage, @PropertyType, @IsOccupied)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PropertyID", property.PropertyID);
                    cmd.Parameters.AddWithValue("@Address", property.Address);
                    cmd.Parameters.AddWithValue("@MonthlyRent", property.MonthlyRent);
                    cmd.Parameters.AddWithValue("@Bedrooms", property.Bedrooms);
                    cmd.Parameters.AddWithValue("@SquareFootage", property.SquareFootage);
                    cmd.Parameters.AddWithValue("@PropertyType", property.PropertyType);
                    cmd.Parameters.AddWithValue("@IsOccupied", property.IsOccupied);

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public void AddTenant(Tenant tenant)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Tenants (TenantID, FullName, Email, Phone, DateOfBirth)
                         VALUES (@TenantID, @FullName, @Email, @Phone, @DateOfBirth)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TenantID", tenant.TenantID);
                    cmd.Parameters.AddWithValue("@FullName", tenant.FullName);
                    cmd.Parameters.AddWithValue("@Email", tenant.Email);
                    cmd.Parameters.AddWithValue("@Phone", tenant.Phone);
                    cmd.Parameters.AddWithValue("@DateOfBirth", tenant.DateOfBirth);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("✅ Tenant added to database successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error adding tenant to DB: {ex.Message}");
                    }
                }
            }
        }

    }
}
