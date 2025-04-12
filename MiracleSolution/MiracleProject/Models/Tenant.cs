using System;

namespace MiracleProject.Models
{
    public class Tenant
    {
        public int TenantID { get; set; }
        public string Name { get; set; }
        public int? PropertyID { get; set; }

        public void DisplayTenant()
        {
            string assigned = PropertyID.HasValue ? $"Assigned Property ID: {PropertyID}" : "Not Assigned";
            Console.WriteLine($"ID: {TenantID}, Name: {Name}, {assigned}");
        }

        public bool IsAssigned()
        {
            return PropertyID.HasValue;
        }
    }
}
