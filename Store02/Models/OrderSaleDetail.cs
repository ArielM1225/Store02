namespace Store02.Models
{
    public class OrderSaleDetail
    {
        public int OrderDetailID {  get; set; }
        public int OrderSID { get; set; }
        public int ProductoID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
