namespace Store02.Models
{
    public class OrderSale
    {
        public int OrderSID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string StatusOrder { get; set; }
    }
}
