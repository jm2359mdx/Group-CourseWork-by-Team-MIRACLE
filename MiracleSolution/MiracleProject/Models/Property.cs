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
            Console.WriteLine($"ID: {PropertyID}, Address: {Address}, Rent: {Rent}, Status: {Status}");
        }
    }
}
