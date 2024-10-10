namespace Store02.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int SupplierID { get; set; }
        public string StatusOrder { get; set; }
        public string OrderType { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
