namespace Store02.Models
{
    public class OrderPurchaseDetailDTO
    {
        public int OrderPID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
