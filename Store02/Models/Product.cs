namespace Store02.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string NameProduct { get; set; }
        public string DescriptionProduct { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
    }
}
