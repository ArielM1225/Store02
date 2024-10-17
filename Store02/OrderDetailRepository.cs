using Microsoft.Data.SqlClient;
using Store02.Models;

public class OrderDetailRepository
{
    private readonly string _connectionString;

    public OrderDetailRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDatabase");
    }

    // Método para crear un nuevo detalle de orden
    public void CreateOrderDetail(OrderDetail orderDetail)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = @"
                INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price) 
                VALUES (@OrderID, @ProductID, @Quantity, @Price)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@OrderID", orderDetail.OrderID);
                command.Parameters.AddWithValue("@ProductID", orderDetail.ProductID);
                command.Parameters.AddWithValue("@Quantity", orderDetail.Quantity);
                command.Parameters.AddWithValue("@Price", orderDetail.Price);

                command.ExecuteNonQuery();
            }
        }
    }

    //// Método para agregar un detalle a la orden
    //public void Add(OrderDetail detail)
    //{
    //    using (SqlConnection connection = new SqlConnection(_connectionString))
    //    {
    //        connection.Open();
    //        string query = "INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price) " +
    //                       "VALUES (@OrderID, @ProductID, @Quantity, @Price)";

    //        using (SqlCommand cmd = new SqlCommand(query, connection))
    //        {
    //            cmd.Parameters.AddWithValue("@OrderID", detail.OrderID);
    //            cmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
    //            cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
    //            cmd.Parameters.AddWithValue("@Price", detail.Price);
    //            cmd.ExecuteNonQuery();
    //        }
    //    }
    //}

    // Método para agregar los detalles de la orden
    //public void AddOrderDetails(List<OrderDetail> orderDetails, SqlConnection connection, SqlTransaction transaction)
    //{
    //    string query = "INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price) VALUES (@OrderID, @ProductID, @Quantity, @Price)";

    //    foreach (var detail in orderDetails)
    //    {
    //        SqlCommand cmd = new SqlCommand(query, connection, transaction); // Usar la conexión y transacción
    //        cmd.Parameters.AddWithValue("@OrderID", detail.OrderID);
    //        cmd.Parameters.AddWithValue("@ProductID", detail.ProductID);
    //        cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
    //        cmd.Parameters.AddWithValue("@Price", detail.Price);
    //        cmd.ExecuteNonQuery();
    //    }
    //}

}

