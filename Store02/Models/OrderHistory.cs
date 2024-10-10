namespace Store02.Models
{
    public class OrderHistory
    {
        public int OrderHistoryID { get; set; }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int SupplierID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string StatusOrder {  get; set; }
        public string OrderType { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime HistoryDate { get; set; } = DateTime.Now;
    }
}
