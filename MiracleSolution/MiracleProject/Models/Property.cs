using System;

namespace MiracleProject.Models
{
    public class Property
    {
        public string PropertyID { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal MonthlyRent { get; set; }
        public string PropertyType { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int SquareFootage { get; set; }
        public bool IsOccupied { get; set; } = false;

        public void DisplayProperty()
        {
            Console.WriteLine($"ID: {PropertyID}, Address: {Address}, MonthlyRent: {MonthlyRent:C}, Type: {PropertyType}, Bedrooms: {Bedrooms}, Size: {SquareFootage} sqft, Status: {(IsOccupied ? "Occupied" : "Available")}");
        }

        public void UpdateStatus(bool occupied)
        {
            IsOccupied = occupied;
        }

        public bool IsAvailable()
        {
            return !IsOccupied;
        }
    }
}
