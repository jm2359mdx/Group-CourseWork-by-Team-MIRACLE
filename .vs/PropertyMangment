using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertyManagementSystem
{
    class Program
    {
        private static readonly PropertyManager _propertyManager = new PropertyManager();
        private static bool _shouldExit = false;

        static void Main(string[] args)
        {
            Console.Title = "Property Management System";

            while (!_shouldExit)
            {
                DisplayMainMenu();
                HandleMenuSelection();
            }
        }

        #region Main Menu and Navigation Helpers

        private static void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== PROPERTY MANAGEMENT SYSTEM ===");
            Console.WriteLine("1. Property Operations");
            Console.WriteLine("2. Tenant Operations");
            Console.WriteLine("3. Assignment Operations");
            Console.WriteLine("4. Reports");
            Console.WriteLine("5. Exit");
            Console.Write("\nEnter your choice (1-5): ");
        }

        private static void HandleMenuSelection()
        {
            int choice = GetValidIntInput(1, 5);
            switch (choice)
            {
                case 1:
                    PropertyOperationsMenu();
                    break;
                case 2:
                    TenantOperationsMenu();
                    break;
                case 3:
                    AssignmentOperationsMenu();
                    break;
                case 4:
                    ReportsMenu();
                    break;
                case 5:
                    _shouldExit = true;
                    Console.WriteLine("\nExiting the system. Goodbye!");
                    break;
            }

            if (!_shouldExit)
            {
                WaitForUser();
            }
        }

        private static void WaitForUser()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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
                case 1:
                    AddProperty();
                    break;
                case 2:
                    ViewProperties();
                    break;
                case 3:
                    ViewProperties(false);
                    break;
                case 4:
                    ViewProperties(true);
                    break;
                case 5:
                    return;
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
                _propertyManager.AddProperty(property);
                ShowSuccess($"Property {property.PropertyID} added successfully!");
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

            // Using formatted strings for a neat table header and rows
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
                case 1:
                    AddTenant();
                    break;
                case 2:
                    ViewTenants();
                    break;
                case 3:
                    return;
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
                _propertyManager.AddTenant(tenant);
                ShowSuccess($"Tenant {tenant.FullName} added successfully!");
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

            // Output table header with alignment
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
                case 1:
                    AssignTenantToProperty();
                    break;
                case 2:
                    VacateProperty();
                    break;
                case 3:
                    return;
            }
        }

        private static void AssignTenantToProperty()
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGN TENANT TO PROPERTY ===");

            ViewTenants();
            string tenantId = GetValidInput("\nEnter Tenant ID: ", ValidateRequired);

            ViewProperties(false); // Show only available properties
            string propertyId = GetValidInput("\nEnter Property ID: ", ValidateRequired);

            try
            {
                _propertyManager.AssignTenantToProperty(tenantId, propertyId);
                ShowSuccess($"Tenant {tenantId} assigned to property {propertyId} successfully!");
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

            ViewProperties(true); // Show only occupied properties
            string propertyId = GetValidInput("\nEnter Property ID: ", ValidateRequired);

            try
            {
                _propertyManager.VacateProperty(propertyId);
                ShowSuccess($"Property {propertyId} has been vacated successfully!");
            }
            catch (Exception ex)
            {
                ShowError($"Error vacating property: {ex.Message}");
            }
        }

        #endregion

        #region Reports

        private static void ReportsMenu()
        {
            Console.Clear();
            Console.WriteLine("=== REPORTS ===");
            Console.WriteLine("1. Rental Income Summary");
            Console.WriteLine("2. Occupancy Rate");
            Console.WriteLine("3. Tenant Demographic Report");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("\nEnter your choice (1-4): ");

            int choice = GetValidIntInput(1, 4);
            switch (choice)
            {
                case 1:
                    ShowRentalIncomeSummary();
                    break;
                case 2:
                    ShowOccupancyRate();
                    break;
                case 3:
                    ShowTenantDemographics();
                    break;
                case 4:
                    return;
            }
        }

        private static void ShowRentalIncomeSummary()
        {
            Console.Clear();
            Console.WriteLine("=== RENTAL INCOME SUMMARY ===");

            var properties = _propertyManager.ViewProperties();
            decimal totalPotentialIncome = properties.Sum(p => p.MonthlyRent);
            decimal currentIncome = properties.Where(p => p.IsOccupied).Sum(p => p.MonthlyRent);

            Console.WriteLine($"\nTotal Properties: {properties.Count}");
            Console.WriteLine($"Occupied Properties: {properties.Count(p => p.IsOccupied)}");
            Console.WriteLine($"Vacant Properties: {properties.Count(p => !p.IsOccupied)}");
            Console.WriteLine($"\nTotal Potential Monthly Income: {totalPotentialIncome:C}");
            Console.WriteLine($"Current Monthly Income: {currentIncome:C}");
            Console.WriteLine($"Vacancy Loss: {(totalPotentialIncome - currentIncome):C}");
        }

        private static void ShowOccupancyRate()
        {
            Console.Clear();
            Console.WriteLine("=== OCCUPANCY RATE ===");

            var properties = _propertyManager.ViewProperties();
            if (!properties.Any())
            {
                Console.WriteLine("\nNo properties available.");
                return;
            }

            int occupied = properties.Count(p => p.IsOccupied);
            double rate = (double)occupied / properties.Count * 100;

            Console.WriteLine($"\nOccupancy Rate: {rate:F2}%");
            Console.WriteLine($"Occupied: {occupied}");
            Console.WriteLine($"Vacant: {properties.Count - occupied}");
            Console.WriteLine($"Total: {properties.Count}");
        }

        private static void ShowTenantDemographics()
        {
            Console.Clear();
            Console.WriteLine("=== TENANT DEMOGRAPHICS ===");

            var tenants = _propertyManager.ViewTenants();
            if (!tenants.Any())
            {
                Console.WriteLine("\nNo tenants available.");
                return;
            }

            var now = DateTime.Now;
            var ages = tenants.Select(t => now.Year - t.DateOfBirth.Year -
                             (now.DayOfYear < t.DateOfBirth.DayOfYear ? 1 : 0)).ToList();

            Console.WriteLine($"\nTotal Tenants: {tenants.Count}");
            Console.WriteLine($"Average Age: {ages.Average():F1}");
            Console.WriteLine($"Youngest Tenant: {ages.Min()} years");
            Console.WriteLine($"Oldest Tenant: {ages.Max()} years");
        }

        #endregion

        #region Helper Methods

        // Reads an input and validates it with the given validator delegate.
        private static string GetValidInput(string prompt, Func<string, bool> validator, string errorMessage = "Invalid input. Please try again.")
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = (Console.ReadLine() ?? string.Empty).Trim();
                if (!validator(input))
                {
                    ShowError(errorMessage);
                }
            }
            while (!validator(input));

            return input;
        }

        // Reads and returns an integer that falls between min and max (inclusive).
        private static int GetValidIntInput(int min, int max)
        {
            int choice;
            while (true)
            {
                string input = (Console.ReadLine() ?? string.Empty).Trim();
                if (int.TryParse(input, out choice) && choice >= min && choice <= max)
                    return choice;
                ShowError($"Please enter a valid number between {min} and {max}.");
                Console.Write("Try again: ");
            }
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

        #region Validation Methods

        private static bool ValidateRequired(string input) =>
            !string.IsNullOrWhiteSpace(input);

        private static bool ValidatePositiveDecimal(string input) =>
            decimal.TryParse(input, out decimal value) && value > 0;

        private static bool ValidatePositiveInteger(string input) =>
            int.TryParse(input, out int value) && value > 0;

        private static bool ValidatePropertyType(string input) =>
            ValidateRequired(input) &&
            new[] { "Apartment", "House", "Commercial" }
                .Contains(input, StringComparer.OrdinalIgnoreCase);

        private static bool ValidateEmail(string input) =>
            ValidateRequired(input) &&
            input.Contains("@") &&
            input.Contains(".") &&
            input.Length > 5;

        private static bool ValidatePhone(string input) =>
            ValidateRequired(input) &&
            input.All(c => char.IsDigit(c) || c == '-' || c == '(' || c == ')' || c == ' ' || c == '+');

        private static bool ValidateDateOfBirth(string input) =>
            DateTime.TryParse(input, out DateTime dob) &&
            dob < DateTime.Now.AddYears(-18) &&
            dob > DateTime.Now.AddYears(-120);

        #endregion
    }

    #region Model Classes

    public class Property
    {
        public string PropertyID { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public int Bedrooms { get; set; }
        public int SquareFootage { get; set; }
        public string PropertyType { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
    }

    public class Tenant
    {
        public string TenantID { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
    }

    public class PropertyManager
    {
        private readonly List<Property> _properties = new List<Property>();
        private readonly List<Tenant> _tenants = new List<Tenant>();
        private readonly Dictionary<string, string> _propertyAssignments = new Dictionary<string, string>();

        public void AddProperty(Property property)
        {
            if (_properties.Any(p => p.PropertyID.Equals(property.PropertyID, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Property ID already exists.");

            _properties.Add(property);
        }

        public List<Property> ViewProperties() => _properties;

        public void AddTenant(Tenant tenant)
        {
            if (_tenants.Any(t => t.TenantID.Equals(tenant.TenantID, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Tenant ID already exists.");

            _tenants.Add(tenant);
        }

        public List<Tenant> ViewTenants() => _tenants;

        public void AssignTenantToProperty(string tenantId, string propertyId)
        {
            var tenant = _tenants.FirstOrDefault(t => t.TenantID.Equals(tenantId, StringComparison.OrdinalIgnoreCase));
            var property = _properties.FirstOrDefault(p => p.PropertyID.Equals(propertyId, StringComparison.OrdinalIgnoreCase));

            if (tenant == null)
                throw new Exception("Tenant not found.");
            if (property == null)
                throw new Exception("Property not found.");
            if (property.IsOccupied)
                throw new Exception("Property is already occupied.");

            property.IsOccupied = true;
            _propertyAssignments[propertyId] = tenantId;
        }

        public void VacateProperty(string propertyId)
        {
            var property = _properties.FirstOrDefault(p => p.PropertyID.Equals(propertyId, StringComparison.OrdinalIgnoreCase));

            if (property == null)
                throw new Exception("Property not found.");
            if (!property.IsOccupied)
                throw new Exception("Property is already vacant.");

            property.IsOccupied = false;
            _propertyAssignments.Remove(propertyId);
        }
    }

    #endregion
}
