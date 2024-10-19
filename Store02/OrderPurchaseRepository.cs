using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Store02.Models;
using Store02;

public class OrderPurchaseRepository
 {
    private readonly string _connectionString;

    public OrderPurchaseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDatabase");
    }

    // Método para crear una orden de Ingreso
    public int CreatOrderIn(Order order)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta SQL para insertar una nueva orden de compra
            string query = @"
            INSERT INTO OrderPurchase (SupplierID, StatusOrder)
            VALUES (@SupplierID, @StatusOrder)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SupplierID", (object)order.CustomerID ?? DBNull.Value);
                command.Parameters.AddWithValue("@StatusOrder", order.StatusOrder);

                // Ejecutar el comando y devolver el ID de la nuva orden
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }

    // Método para actualizar el TotalAmount de una Orden de Ingreso
    public void UpdateTotalAmount(int orderId, decimal totalAmount)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = @"
            UPDATE OrderPurchase
            SET TotalAmount = @TotalAmount
            WHERE OrderPID = @OrderPID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                command.Parameters.AddWithValue("@OrderPID", orderId);

                command.ExecuteNonQuery();
            }
        }
    }
 }
