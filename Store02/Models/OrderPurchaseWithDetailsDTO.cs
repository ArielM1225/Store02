using Store02.Models;

namespace Store02.Models
{
    public class OrderPurchaseWithDetailsDTO
    {
        public int SupplierID { get; set; }
        public string StatusOrder { get; set; }
        public List<OrderDetailDTO> OrderPurchaseDetail { get; set; }
    }
}
