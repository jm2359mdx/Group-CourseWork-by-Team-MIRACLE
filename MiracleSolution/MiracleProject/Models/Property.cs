using System;

namespace MiracleProject.Models
{
    public class Property
    {
        public int PropertyID { get; set; }
        public string Address { get; set; }
        public decimal Rent { get; set; }
        public string Status { get; set; } = "Available";

        public void DisplayProperty()
        {
            Console.WriteLine($"ID: {PropertyID}, Address: {Address}, Rent: {Rent:C}, Status: {Status}");
        }

        public void UpdateStatus(string newStatus)
        {
            if (!string.IsNullOrWhiteSpace(newStatus))
            {
                Status = newStatus;
            }
        }

        public bool IsAvailable()
        {
            return Status.ToLower() == "available";
        }
    }
}
