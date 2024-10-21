using Microsoft.Data.SqlClient;
using Store02.Models;
using System.Data;
using System.Runtime.CompilerServices;

public class CustomerRepository
{
    // Probando
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

            // Verificar si el email ya existe
            string checkEmailQuery = "SELECT COUNT(*) FROM Customers WHERE Email = @Email";
            using (SqlCommand checkEmailCommand = new SqlCommand(checkEmailQuery, connection))
            {
                checkEmailCommand.Parameters.AddWithValue("@Email", customer.Email);
                int emailExists = (int)checkEmailCommand.ExecuteScalar();

                if (emailExists > 0)
                {
                    throw new InvalidOperationException("El email ya está registrado.");
                }
            }

            string query = "INSERT INTO Customers (FirstName, LastName, Email, PhoneNumber, AddressCustomer, City, PostalCode, Country)" +
                " VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @AddressCustomer, @City, @PostalCode, @Country)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = customer.FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = customer.LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = customer.Email;
                command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = customer.PhoneNumber;
                command.Parameters.Add("@AddressCustomer", SqlDbType.NVarChar).Value = customer.AddressCustomer;
                command.Parameters.Add("@City", SqlDbType.NVarChar).Value = customer.City;
                command.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = customer.PostalCode;
                command.Parameters.Add("@Country", SqlDbType.NVarChar).Value = customer.Country;

                try
                {
                    command.ExecuteNonQuery();  // Ejecuta la consulta
                }
                catch (SqlException ex)
                {
                    // Manejo de errores específicos de SQL
                    throw new Exception("Error al crear el cliente: " + ex.Message);
                }
            }
        }
    }

    public bool UpdateContact(int customerID, string newEmail, string newPhoneNumber)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Consulta SQL para actualizar los datos de contacto del customer
            string query = @"
            UPDATE Customers
            SET Email = @Email, PhoneNumber = @PhoneNumber, UpdatedAt = GETDATE()
            WHERE CustomerID = @CustomerID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", newEmail);
                command.Parameters.AddWithValue("@PhoneNumber", newPhoneNumber);
                command.Parameters.AddWithValue("@CustomerID", customerID);

                int affectedrows = command.ExecuteNonQuery();
                return affectedrows > 0;
            }
        }
    }

}