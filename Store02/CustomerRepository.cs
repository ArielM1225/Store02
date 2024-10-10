using Microsoft.Data.SqlClient;
using Store02.Models;

public class CustomerRepository
{

    private readonly string _connectionString;

    // Constructor que recibe la configuración
    public CustomerRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OrderDatabase");
    }

    // Método para crear un nuevo cliente
    public void CreateCustomer(Customer customer)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber, AddressCustomer, City, PostalCode, Country)" +
                " VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @AddressCustomer, @City, @PostalCode, @Country)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                command.Parameters.AddWithValue("@AddressCustomer", customer.AddressCustomer);
                command.Parameters.AddWithValue("@City", customer.City);
                command.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
                command.Parameters.AddWithValue("@Country", customer.Country);

                command.ExecuteNonQuery();  // Ejecuta la consulta
            }
        }
    }
}