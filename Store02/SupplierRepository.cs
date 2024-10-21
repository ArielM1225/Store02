using Microsoft.Data.SqlClient;
using Store02.Models;

public class SupplierRepository
{

    private readonly string _connectionString;

    public SupplierRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDatabase");
    }

    // Método para agregar suppliers
    public void CreateSupplier(Supplier supplier)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta SQL para insertar un nuevo supplier
            string query = @"
            INSERT INTO Suppliers (SupplierName, ContactInfo)
            VALUES (@SupplierName, @ContactInfo)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
                command.Parameters.AddWithValue("@ContactInfo", supplier.ContactInfo);

                // Ejecutar la consulta
                command.ExecuteNonQuery();
            }
        }
    }

    // Método para modificar datos de un supplier
    public bool UpdateData(int supplierID, string newSupplierName, string newContactInfo)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta SQL para actualizar el nombre y contacto del supplier
            string query = @"
            UPDATE Suppliers
            SET SupplierName = @SupplierName, ContactInfo = @ContactInfo, UpdatedAt = GETDATE()
            WHERE SupplierID = @SupplierID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@SupplierName", newSupplierName);
                command.Parameters.AddWithValue("@ContactInfo", newContactInfo);
                command.Parameters.AddWithValue("@SupplierID", supplierID);

                int affectedrows = command.ExecuteNonQuery();
                return affectedrows > 0;
            }
        }
    }

}
