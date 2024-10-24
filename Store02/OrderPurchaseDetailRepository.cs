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

    // Método para agregar detalle a order purchase
    //public bool AddOrderPurchaseDetail(OrderPurchaseDetail orderPurchaseDetail)
    //{
    //    using (SqlConnection connection = new SqlConnection(_connectionString))
    //    {
    //        connection.Open();

    //        // Consulta para verificar el estado del pedido (OrderPurchase)
    //        string checkStatusQuery = @"
    //    SELECT StatusOrder 
    //    FROM OrderPurchase 
    //    WHERE OrderPID = @OrderPID";

    //        using (SqlCommand checkStatusCommand = new SqlCommand(checkStatusQuery, connection))
    //        {
    //            checkStatusCommand.Parameters.AddWithValue("@OrderPID", orderPurchaseDetail.OrderPID);
    //            string status = (string)checkStatusCommand.ExecuteScalar();

    //            // Verificar si el estado es "Pending"
    //            if (status != "Pending")
    //            {
    //                return false; // No permite agregar si el estado no es Pending
    //            }
    //        }

    //        // Si el estado es "Pending", insertar el detalle en la tabla OrderPurchaseDetail
    //        string insertQuery = @"
    //    INSERT INTO OrderPurchaseDetail (OrderPID, ProductID, Quantity, Price)
    //    VALUES (@OrderPID, @ProductID, @Quantity, @Price)";

    //        using (SqlCommand command = new SqlCommand(insertQuery, connection))
    //        {
    //            command.Parameters.AddWithValue("@OrderPID", orderPurchaseDetail.OrderPID);
    //            command.Parameters.AddWithValue("@ProductID", orderPurchaseDetail.ProductID);
    //            command.Parameters.AddWithValue("@Quantity", orderPurchaseDetail.Quantity);
    //            command.Parameters.AddWithValue("@Price", orderPurchaseDetail.Price);

    //            int affectedRows = command.ExecuteNonQuery();
    //            return affectedRows > 0;
    //        }
    //    }
    //}

    // Método para agregar detalle a order purchase y actualizar el TotalAmount
    // Método para agregar detalle a order purchase y actualizar el TotalAmount
    public bool AddOrderPurchaseDetail(OrderPurchaseDetail orderPurchaseDetail)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta para verificar el estado del pedido (OrderPurchase)
            string checkStatusQuery = @"
        SELECT StatusOrder 
        FROM OrderPurchase 
        WHERE OrderPID = @OrderPID";

            using (SqlCommand checkStatusCommand = new SqlCommand(checkStatusQuery, connection))
            {
                checkStatusCommand.Parameters.AddWithValue("@OrderPID", orderPurchaseDetail.OrderPID);
                string status = (string)checkStatusCommand.ExecuteScalar();

                // Verificar si el estado es "Pending"
                if (status != "Pending")
                {
                    return false; // No permite agregar si el estado no es Pending
                }
            }

            // Si el estado es "Pending", insertar el detalle en la tabla OrderPurchaseDetail
            string insertDetailQuery = @"
        INSERT INTO OrderPurchaseDetail (OrderPID, ProductID, Quantity, Price)
        VALUES (@OrderPID, @ProductID, @Quantity, @Price)";

            using (SqlCommand command = new SqlCommand(insertDetailQuery, connection))
            {
                command.Parameters.AddWithValue("@OrderPID", orderPurchaseDetail.OrderPID);
                command.Parameters.AddWithValue("@ProductID", orderPurchaseDetail.ProductID);
                command.Parameters.AddWithValue("@Quantity", orderPurchaseDetail.Quantity);
                command.Parameters.AddWithValue("@Price", orderPurchaseDetail.Price);

                int affectedRows = command.ExecuteNonQuery();

                // Si no se insertó el detalle, retornar false
                if (affectedRows <= 0)
                {
                    return false;
                }
            }

            // Ahora actualizamos el TotalAmount en la tabla OrderPurchase
            string updateTotalAmountQuery = @"
        UPDATE OrderPurchase
        SET TotalAmount = (SELECT SUM(Quantity * Price) 
                           FROM OrderPurchaseDetail 
                           WHERE OrderPID = @OrderPID)
        WHERE OrderPID = @OrderPID";

            using (SqlCommand updateTotalCommand = new SqlCommand(updateTotalAmountQuery, connection))
            {
                updateTotalCommand.Parameters.AddWithValue("@OrderPID", orderPurchaseDetail.OrderPID);
                int affectedRows = updateTotalCommand.ExecuteNonQuery();

                // Verificar si se actualizó correctamente el TotalAmount
                return affectedRows > 0;
            }
        }
    }



}
