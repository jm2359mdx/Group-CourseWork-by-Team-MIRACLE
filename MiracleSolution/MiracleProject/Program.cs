/*
 * Author: Secretary, Scrum Master
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using MiracleProject.Models;
using MiracleProject.Services;

namespace MiracleProject
{
    class Program
    {
        private static readonly PropertyManager _propertyManager = new();
        private static bool _shouldExit = false;

        static void Main(string[] args)
        {
            Console.Title = "Property Management System";

            var db = new SqlConnector();
            db.TestConnection();

            // Load properties from DB into memory
            var loadedProperties = db.LoadPropertiesFromDB();
            foreach (var prop in loadedProperties)
            {
                _propertyManager.AddProperty(prop);
            }

            // Load tenants from SQL
            var loadedTenants = db.LoadTenantsFromDB();
            foreach (var tenant in loadedTenants)
            {
                _propertyManager.AddTenant(tenant);
            }

            while (!_shouldExit)
            {
                DisplayMainMenu();
                HandleMenuSelection();
            }
        }

        #region Main Menu

        private static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== PROPERTY MANAGEMENT SYSTEM ===");
            Console.WriteLine("1. Property Operations");
            Console.WriteLine("2. Tenant Operations");
            Console.WriteLine("3. Assignment Operations");
            Console.WriteLine("4. Exit");
            Console.Write("\nEnter your choice (1-4): ");
        }

        private static void HandleMenuSelection()
        {
            int choice = GetValidIntInput(1, 5);
            switch (choice)
            {
                case 1: PropertyOperationsMenu(); break;
                case 2: TenantOperationsMenu(); break;
                case 3: AssignmentOperationsMenu(); break;
                case 4:
                    _shouldExit = true;
                    Console.WriteLine("\nExiting the system. Goodbye!");
                    break;
            }

            if (!_shouldExit)
            {
                WaitForUser();
            }
        }

        #endregion

        #region Property Operations

        private static void PropertyOperationsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== PROPERTY OPERATIONS ===");
            Console.WriteLine("1. Add New Property");
            Console.WriteLine("2. View All Properties");
            Console.WriteLine("3. View Available Properties");
            Console.WriteLine("4. View Occupied Properties");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("\nEnter your choice (1-5): ");

            int choice = GetValidIntInput(1, 5);
            switch (choice)
            {
                case 1: AddProperty(); break;
                case 2: ViewProperties(); break;
                case 3: ViewProperties(false); break;
                case 4: ViewProperties(true); break;
                case 5: return;
            }
        }

        private static void AddProperty()
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW PROPERTY ===");

            var property = new Property
            {
                PropertyID = GetValidInput("Property ID: ", ValidateRequired),
                Address = GetValidInput("Address: ", ValidateRequired),
                MonthlyRent = decimal.Parse(GetValidInput("Monthly Rent: ", ValidatePositiveDecimal)),
                Bedrooms = int.Parse(GetValidInput("Number of Bedrooms: ", ValidatePositiveInteger)),
                SquareFootage = int.Parse(GetValidInput("Square Footage: ", ValidatePositiveInteger)),
                PropertyType = GetValidInput("Property Type (Apartment/House/Commercial): ", ValidatePropertyType)
            };

            try
            {
                var db = new SqlConnector();
                bool isInserted = db.AddProperty(property);

                if (isInserted)
                {
                    _propertyManager.AddProperty(property);
                    ShowSuccess($"Property {property.PropertyID} added to database successfully!");
                }
                else
                {
                    ShowError("❌ Failed to add property to the database.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error adding property: {ex.Message}");
            }
        }

        private static void ViewProperties(bool? isOccupied = null)
        {
            Console.Clear();
            string header = isOccupied.HasValue
                ? $"=== {(isOccupied.Value ? "OCCUPIED" : "AVAILABLE")} PROPERTIES ==="
                : "=== ALL PROPERTIES ===";
            Console.WriteLine(header);

            var properties = isOccupied.HasValue
                ? _propertyManager.ViewProperties().Where(p => p.IsOccupied == isOccupied.Value).ToList()
                : _propertyManager.ViewProperties();

            if (!properties.Any())
            {
                Console.WriteLine("\nNo properties found.");
                return;
            }

            Console.WriteLine("\n{0,-10} {1,-20} {2,-12} {3,-10} {4,12} {5,-10}", "ID", "Address", "Type", "Bedrooms", "Rent", "Status");
            Console.WriteLine(new string('-', 80));
            foreach (var property in properties)
            {
                Console.WriteLine("{0,-10} {1,-20} {2,-12} {3,-10} {4,12:C} {5,-10}",
                    property.PropertyID,
                    property.Address.Length > 20 ? property.Address.Substring(0, 20) : property.Address,
                    property.PropertyType,
                    property.Bedrooms,
                    property.MonthlyRent,
                    property.IsOccupied ? "Occupied" : "Vacant");
            }

            Console.WriteLine($"\nTotal: {properties.Count} properties");
        }

        #endregion

        #region Tenant Operations

        private static void TenantOperationsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== TENANT OPERATIONS ===");
            Console.WriteLine("1. Add New Tenant");
            Console.WriteLine("2. View All Tenants");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("\nEnter your choice (1-3): ");

            int choice = GetValidIntInput(1, 3);
            switch (choice)
            {
                case 1: AddTenant(); break;
                case 2: ViewTenants(); break;
                case 3: return;
            }
        }

        private static void AddTenant()
        {
            Console.Clear();
            Console.WriteLine("=== ADD NEW TENANT ===");

            var tenant = new Tenant
            {
                TenantID = GetValidInput("Tenant ID: ", ValidateRequired),
                FullName = GetValidInput("Full Name: ", ValidateRequired),
                Email = GetValidInput("Email: ", ValidateEmail),
                Phone = GetValidInput("Phone: ", ValidatePhone),
                DateOfBirth = DateTime.Parse(GetValidInput("Date of Birth (MM/DD/YYYY): ", ValidateDateOfBirth))
            };

            try
            {
                var db = new SqlConnector();
                bool isInserted = db.AddTenant(tenant);

                if (isInserted)
                {
                    _propertyManager.AddTenant(tenant);
                    ShowSuccess($"Tenant {tenant.FullName} added successfully!");
                }
                else
                {
                    ShowError("❌ Failed to add tenant to the database.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error adding tenant: {ex.Message}");
            }
        }

        private static void ViewTenants()
        {
            Console.Clear();
            Console.WriteLine("=== ALL TENANTS ===");

            var tenants = _propertyManager.ViewTenants();
            if (!tenants.Any())
            {
                Console.WriteLine("\nNo tenants found.");
                return;
            }

            Console.WriteLine("\n{0,-10} {1,-15} {2,-20} {3,-15} {4,-5}", "ID", "Name", "Email", "Phone", "Age");
            Console.WriteLine(new string('-', 70));
            foreach (var tenant in tenants)
            {
                int age = DateTime.Now.Year - tenant.DateOfBirth.Year;
                if (DateTime.Now.DayOfYear < tenant.DateOfBirth.DayOfYear)
                    age--;

                Console.WriteLine("{0,-10} {1,-15} {2,-20} {3,-15} {4,-5}",
                    tenant.TenantID,
                    tenant.FullName.Length > 15 ? tenant.FullName.Substring(0, 15) : tenant.FullName,
                    tenant.Email.Length > 20 ? tenant.Email.Substring(0, 20) : tenant.Email,
                    tenant.Phone,
                    age);
            }

            Console.WriteLine($"\nTotal: {tenants.Count} tenants");
        }

        #endregion

        #region Assignment Operations

        private static void AssignmentOperationsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGNMENT OPERATIONS ===");
            Console.WriteLine("1. Assign Tenant to Property");
            Console.WriteLine("2. Vacate Property");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("\nEnter your choice (1-3): ");

            int choice = GetValidIntInput(1, 3);
            switch (choice)
            {
                case 1: AssignTenantToProperty(); break;
                case 2: VacateProperty(); break;
                case 3: return;
            }
        }

        private static void AssignTenantToProperty()
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGN TENANT TO PROPERTY ===");

            ViewTenants();
            string tenantId = GetValidInput("\nEnter Tenant ID: ", ValidateRequired);

            ViewProperties(false); // Available only
            string propertyId = GetValidInput("\nEnter Property ID: ", ValidateRequired);

            try
            {
                _propertyManager.AssignTenantToProperty(tenantId, propertyId);

                var db = new SqlConnector();
                bool success = db.AssignTenantToProperty(propertyId);

                if (success)
                    ShowSuccess($"Tenant {tenantId} assigned to property {propertyId} successfully!");
                else
                    ShowError("❌ Failed to update property status in the database.");
            }
            catch (Exception ex)
            {
                ShowError($"Error assigning tenant: {ex.Message}");
            }
        }

        private static void VacateProperty()
        {
            Console.Clear();
            Console.WriteLine("=== VACATE PROPERTY ===");

            ViewProperties(true); // Only occupied
            string propertyId = GetValidInput("\nEnter Property ID: ", ValidateRequired);

            try
            {
                _propertyManager.VacateProperty(propertyId);

                var db = new SqlConnector();
                bool success = db.VacateProperty(propertyId);

                if (success)
                    ShowSuccess($"Property {propertyId} has been vacated successfully!");
                else
                    ShowError("❌ Failed to update property status in the database.");
            }
            catch (Exception ex)
            {
                ShowError($"Error vacating property: {ex.Message}");
            }
        }

        #endregion

        #region Helpers

        private static string GetValidInput(string prompt, Func<string, bool> validator, string error = "Invalid input.")
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim() ?? "";
                if (!validator(input))
                    ShowError(error);
            } while (!validator(input));
            return input;
        }

        private static int GetValidIntInput(int min, int max)
        {
            while (true)
            {
                string input = Console.ReadLine()?.Trim() ?? "";
                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                    return choice;
                ShowError($"Please enter a number between {min} and {max}.");
                Console.Write("Try again: ");
            }
        }

        private static void WaitForUser()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}");
            Console.ResetColor();
        }

        #endregion

        #region Validation

        private static bool ValidateRequired(string input) =>
            !string.IsNullOrWhiteSpace(input);

        private static bool ValidatePositiveDecimal(string input) =>
            decimal.TryParse(input, out var value) && value > 0;

        private static bool ValidatePositiveInteger(string input) =>
            int.TryParse(input, out var value) && value > 0;

        private static bool ValidatePropertyType(string input) =>
            new[] { "Apartment", "House", "Commercial" }.Contains(input, StringComparer.OrdinalIgnoreCase);

        private static bool ValidateEmail(string input) =>
            input.Contains("@") && input.Contains(".") && input.Length > 5;

        private static bool ValidatePhone(string input) =>
            input.All(c => char.IsDigit(c) || " ()-+".Contains(c));

        private static bool ValidateDateOfBirth(string input) =>
            DateTime.TryParse(input, out var dob) && dob < DateTime.Now.AddYears(-18);

        #endregion
    }
}
