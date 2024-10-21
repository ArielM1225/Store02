using Microsoft.Data.SqlClient;
using Store02.Models;

public class OrderPurchaseDetailRepository
{
    private readonly string _connectionString;

    public OrderPurchaseDetailRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDatabase");
    }

    // Método para crear un nuevo detalle de order purchase
    public void CreateOrderPurchaseDetail(OrderPurchaseDetail orderPurchaseDetail)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = @"
                INSERT INTO OrderPurchaseDetail (OrderPID, ProductID, Quantity, Price)
                VALUES (@OrderPID, @ProductID, @Quantity, @Price)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OrderPID", orderPurchaseDetail.OrderPID);
                command.Parameters.AddWithValue("@ProductID", orderPurchaseDetail.ProductID);
                command.Parameters.AddWithValue("@Quantity", orderPurchaseDetail.Quantity);
                command.Parameters.AddWithValue("@Price", orderPurchaseDetail.Price);

                command.ExecuteNonQuery();
            }
        }
    }
}
