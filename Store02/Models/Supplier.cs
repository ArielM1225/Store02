namespace Store02.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactInfo { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
