using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Store02.Models;
using Store02;

public class OrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDatabase");
    }

    // Método para crear una nueva orden
    public int CreateOrder(Order order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Verificar si es una compra o una venta
            if (order.OrderType == "Purchase")
            {
                if (order.SupplierID == null)
                {
                    throw new InvalidOperationException("SupplierID es requerido para compras.");
                }
            }
            else if (order.OrderType == "Sale")
            {
                if (order.CustomerID == null)
                {
                    throw new InvalidOperationException("CustomerID es requerido para ventas.");
                }
            }

            string query = @"
            INSERT INTO Orders (CustomerID, SupplierID, OrderDate, TotalAmount, StatusOrder, OrderType)
            VALUES (@CustomerID, @SupplierID, @OrderDate, @TotalAmount, @StatusOrder, @OrderType);
            SELECT SCOPE_IDENTITY();";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@CustomerID", (object)order.CustomerID ?? DBNull.Value);
                command.Parameters.AddWithValue("@SupplierID", (object)order.SupplierID ?? DBNull.Value);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                command.Parameters.AddWithValue("@StatusOrder", order.StatusOrder);
                command.Parameters.AddWithValue("@OrderType", order.OrderType);

                // Ejecutar el comando y devolver el ID de la nueva orden
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }


    // Método para actualizar el TotalAmount de una orden
    public void UpdateTotalAmount(int orderId, decimal totalAmount)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = @"
                UPDATE Orders 
                SET TotalAmount = @TotalAmount 
                WHERE OrderID = @OrderID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                command.Parameters.AddWithValue("@OrderID", orderId);

                command.ExecuteNonQuery();
            }
        }
    }

    // Método para agregar una nueva orden
    //public int AddOrderWithDetails(Order order, List<OrderDetail> orderDetails)
    //{
    //    using (SqlConnection connection = new SqlConnection(_connectionString))
    //    {
    //        connection.Open();
    //        using (SqlTransaction transaction = connection.BeginTransaction())
    //        {
    //            try
    //            {
    //                // Insertar la orden
    //                string queryOrder = "INSERT INTO Orders (CustomerID, SupplierID, OrderDate, TotalAmount, StatusOrder, OrderType) " +
    //                                    "OUTPUT INSERTED.OrderID " +
    //                                    "VALUES (@CustomerID, @SupplierID, @OrderDate, @TotalAmount, @StatusOrder, @OrderType)";

    //                SqlCommand cmdOrder = new SqlCommand(queryOrder, connection, transaction);
    //                cmdOrder.Parameters.AddWithValue("@CustomerID", (object)order.CustomerID ?? DBNull.Value);
    //                cmdOrder.Parameters.AddWithValue("@SupplierID", (object)order.SupplierID ?? DBNull.Value);
    //                cmdOrder.Parameters.AddWithValue("@OrderDate", order.OrderDate);
    //                cmdOrder.Parameters.AddWithValue("@TotalAmount", 0); // Inicialmente en 0
    //                cmdOrder.Parameters.AddWithValue("@StatusOrder", order.StatusOrder);
    //                cmdOrder.Parameters.AddWithValue("@OrderType", order.OrderType);

    //                // Ejecuta la consulta y retorna el nuevo OrderID
    //                int newOrderID = (int)cmdOrder.ExecuteScalar();

    //                //// Insertar los detalles de la orden
    //                //_orderDetailRepository.AddOrderDetails(orderDetails, connection, transaction);

    //                // Confirmar la transacción
    //                transaction.Commit();
    //                return newOrderID;
    //            }
    //            catch
    //            {
    //                transaction.Rollback();
    //                throw;
    //            }
    //        }
    //    }
    //}


    // Método para actualizar el TotalAmount
    //public void UpdateTotalAmount(int orderId, decimal totalAmount)
    //{
    //    using (SqlConnection connection = new SqlConnection(_connectionString))
    //    {
    //        connection.Open();
    //        string query = "UPDATE Orders SET TotalAmount = @TotalAmount WHERE OrderID = @OrderID";

    //        using (SqlCommand cmd = new SqlCommand(query, connection))
    //        {
    //            cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
    //            cmd.Parameters.AddWithValue("@OrderID", orderId);
    //            cmd.ExecuteNonQuery();
    //        }
    //    }
    //}
}

