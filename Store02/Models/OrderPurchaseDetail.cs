namespace Store02.Models
{
    public class OrderPurchaseDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderPID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
