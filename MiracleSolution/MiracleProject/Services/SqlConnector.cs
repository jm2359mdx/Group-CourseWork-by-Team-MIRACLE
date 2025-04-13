using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using MiracleProject.Models;

namespace MiracleProject.Services
{
    public class SqlConnector
    {
        private readonly string _connectionString = @"Data Source=JEBINSTECH\MSSQLSERVER01;Initial Catalog=MiracleDB;Integrated Security=True;TrustServerCertificate=True";

        public void TestConnection()
        {
            using var conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                Console.WriteLine("✅ Connected to MiracleDB successfully!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"❌ Database connection failed: {ex.Message}");
            }
        }

        public DataTable GetAllProperties()
        {
            DataTable table = new DataTable();
            using var conn = new SqlConnection(_connectionString);
            const string query = "SELECT * FROM Properties";
            using var cmd = new SqlCommand(query, conn);
            using var adapter = new SqlDataAdapter(cmd);

            conn.Open();
            adapter.Fill(table);
            return table;
        }

        public bool AddProperty(Property property)
        {
            property.IsOccupied = false;

            using var conn = new SqlConnection(_connectionString);
            const string query = @"
                INSERT INTO Properties 
                (PropertyID, Address, MonthlyRent, Bedrooms, SquareFootage, PropertyType, IsOccupied)
                VALUES 
                (@PropertyID, @Address, @MonthlyRent, @Bedrooms, @SquareFootage, @PropertyType, @IsOccupied)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PropertyID", property.PropertyID);
            cmd.Parameters.AddWithValue("@Address", property.Address);
            cmd.Parameters.AddWithValue("@MonthlyRent", property.MonthlyRent);
            cmd.Parameters.AddWithValue("@Bedrooms", property.Bedrooms);
            cmd.Parameters.AddWithValue("@SquareFootage", property.SquareFootage);
            cmd.Parameters.AddWithValue("@PropertyType", property.PropertyType);
            cmd.Parameters.AddWithValue("@IsOccupied", property.IsOccupied);

            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"[DEBUG] SQL Insert - Rows affected: {rows}");
                return rows > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Primary key violation
                    Console.WriteLine("❌ Property with this ID already exists in database.");
                else
                    Console.WriteLine($"❌ Error adding property: {ex.Message}");
                return false;
            }
        }

        public bool AddTenant(Tenant tenant)
        {
            using var conn = new SqlConnection(_connectionString);
            const string query = @"
                INSERT INTO Tenants (TenantID, FullName, Email, Phone, DateOfBirth)
                VALUES (@TenantID, @FullName, @Email, @Phone, @DateOfBirth)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@TenantID", tenant.TenantID);
            cmd.Parameters.AddWithValue("@FullName", tenant.FullName);
            cmd.Parameters.AddWithValue("@Email", tenant.Email);
            cmd.Parameters.AddWithValue("@Phone", tenant.Phone);
            cmd.Parameters.AddWithValue("@DateOfBirth", tenant.DateOfBirth);

            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Tenant inserted into database.");
                return rows > 0;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Primary key violation
                    Console.WriteLine("❌ Tenant with this ID already exists in database.");
                else
                    Console.WriteLine($"❌ Error adding tenant: {ex.Message}");
                return false;
            }
        }

        public List<Property> LoadPropertiesFromDB()
        {
            var properties = new List<Property>();
            using var conn = new SqlConnection(_connectionString);
            const string query = "SELECT * FROM Properties";
            using var cmd = new SqlCommand(query, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                properties.Add(new Property
                {
                    PropertyID = reader["PropertyID"]?.ToString() ?? "",
                    Address = reader["Address"]?.ToString() ?? "",
                    MonthlyRent = Convert.ToDecimal(reader["MonthlyRent"]),
                    Bedrooms = Convert.ToInt32(reader["Bedrooms"]),
                    SquareFootage = Convert.ToInt32(reader["SquareFootage"]),
                    PropertyType = reader["PropertyType"]?.ToString() ?? "",
                    IsOccupied = Convert.ToBoolean(reader["IsOccupied"])
                });
            }

            return properties;
        }

        public List<Tenant> LoadTenantsFromDB()
        {
            var tenants = new List<Tenant>();
            using var conn = new SqlConnection(_connectionString);
            const string query = "SELECT * FROM Tenants";
            using var cmd = new SqlCommand(query, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tenants.Add(new Tenant
                {
                    TenantID = reader["TenantID"]?.ToString() ?? "",
                    FullName = reader["FullName"]?.ToString() ?? "",
                    Email = reader["Email"]?.ToString() ?? "",
                    Phone = reader["Phone"]?.ToString() ?? "",
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"])
                });
            }

            return tenants;
        }

        public bool AssignTenantToProperty(string propertyId)
        {
            using var conn = new SqlConnection(_connectionString);
            const string query = "UPDATE Properties SET IsOccupied = 1 WHERE PropertyID = @PropertyID";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PropertyID", propertyId);

            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"❌ SQL Error assigning tenant in DB: {ex.Message}");
                return false;
            }
        }

        public bool VacateProperty(string propertyId)
        {
            using var conn = new SqlConnection(_connectionString);
            const string query = "UPDATE Properties SET IsOccupied = 0 WHERE PropertyID = @PropertyID";
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@PropertyID", propertyId);

            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"❌ SQL Error vacating property in DB: {ex.Message}");
                return false;
            }
        }
    }
}
