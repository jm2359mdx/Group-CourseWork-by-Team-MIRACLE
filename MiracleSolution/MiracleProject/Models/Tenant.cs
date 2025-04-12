using System;

namespace MiracleProject.Models
{
    public class Tenant
    {
        public string TenantID { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        public void DisplayTenant()
        {
            Console.WriteLine($"ID: {TenantID}, Name: {FullName}, Email: {Email}, Phone: {Phone}, DOB: {DateOfBirth:MM/dd/yyyy}");
        }

        public int GetAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateTime.Now.DayOfYear < DateOfBirth.DayOfYear)
                age--;
            return age;
        }
    }
}
