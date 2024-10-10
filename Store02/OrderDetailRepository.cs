using Microsoft.Data.SqlClient;
using Store02.Models;

public class OrderDetailRepository
{
    private readonly string _connectionString;

    public OrderDetailRepository(string connectionString)
    {
        _connectionString = connectionString;
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

