namespace Store02.Models
{
    public class OrderPurchase
    {
        public int OrderPID { get; set; }
        public int SupplierID { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string StatusOrder { get; set; }

    }
}
