using Microsoft.Data.SqlClient;
using Store02.Models;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDatabase");
    }

    // Método para agregar datos de un nuevo producto
    public void CreateProduct(Product product)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta SQL para insertar un nuevo producto
            string query = @"
            INSERT INTO Products (NameProduct, DescriptionProduct, Price)
            VALUES (@NameProduct, @DescriptionProduct, @Price)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NameProduct", product.NameProduct);
                command.Parameters.AddWithValue("@DescriptionProduct", product.DescriptionProduct);
                command.Parameters.AddWithValue("@Price", product.Price);

                // Ejecutar la consulta
                command.ExecuteNonQuery();
            }
        }
    }

    // Método para modificar datos de un producto
    public bool UpdatePrice(int productID, decimal newPrice)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta SQL para actualizar el precio del producto
            string query = @"
            UPDATE Products
            SET Price = @NewPrice, UpdatedAt = GETDATE()
            WHERE ProductID = @ProductID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Añadimos los parámetros a la consulta
                command.Parameters.AddWithValue("@NewPrice", newPrice);
                command.Parameters.AddWithValue("@ProductID", productID);

                int affectedRows = command.ExecuteNonQuery(); // Ejecuta la consulta
                return affectedRows > 0; // Retorna true si se actualizó una fila
            }
        }
    }

    // Método para modificar el nombre y la descripción de un producto
    public bool UpdateData(int productID, string newNameProduct, string newDescriptionProduct)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta SQL para actualizar la data del producto
            string query = @"
            UPDATE Products
            SET NameProduct = @NewNameProduct, DescriptionProduct = @NewDescriptionProduct, UpdatedAt = GETDATE()
            WHERE ProductID = @ProductID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Añadimos los parámetros a la consulta
                command.Parameters.AddWithValue("@NewNameProduct", newNameProduct);
                command.Parameters.AddWithValue("@NewDescriptionProduct", newDescriptionProduct);
                command.Parameters.AddWithValue("@ProductID", productID);

                int affectedRows = command.ExecuteNonQuery();
                return affectedRows > 0;
            }
        }
    }
}
