// dotnet add package System.Data.SqlClient

using Microsoft.Data.SqlClient;
using Ado.Model.Base;

namespace Ado.Infrastructure;

public class CustomerRepository : IRepository<Customer>
{
    private readonly string? _connectionString;
    public CustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public void Add(Customer entity)
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = @"INSERT INTO [Sales].[Customer] ([FirstName], [LastName], [Phone], [Email], [Street], [City], [State], [ZipCode])
                    OUTPUT INSERTED.[Id]
                    VALUES (@FirstName, @LastName, @Phone, @Email, @Street, @City, @State, @ZipCode);";
        var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@FirstName", entity.FirstName);
        command.Parameters.AddWithValue("@LastName", entity.LastName);
        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(entity.Phone) ? (object)DBNull.Value : entity.Phone);
        command.Parameters.AddWithValue("@Email", entity.Email);
        command.Parameters.AddWithValue("@Street", string.IsNullOrEmpty(entity.Street) ? (object)DBNull.Value : entity.Street);
        command.Parameters.AddWithValue("@City", string.IsNullOrEmpty(entity.City) ? (object)DBNull.Value : entity.City);
        command.Parameters.AddWithValue("@State", string.IsNullOrEmpty(entity.State) ? (object)DBNull.Value : entity.State);
        command.Parameters.AddWithValue("@ZipCode", string.IsNullOrEmpty(entity.ZipCode) ? (object)DBNull.Value : entity.ZipCode);

        connection.Open();
        
        // call ExecuteNonQuery() to insert the customer if you don't care about getting the ID back of the newly inserted customer
        //command.ExecuteNonQuery();

        // Otherwise, get the ID of the newly inserted customer
        var id = (int)command.ExecuteScalar();
        entity.Id = id;

        connection.Close();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = "DELETE FROM [Sales].[Customer] WHERE [Id] = @Id;";
        var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);    

        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();    
    }

    public IEnumerable<Customer> GetAll()
    {   
        var customers = new List<Customer>();
        var sql = "SELECT * FROM [Sales].[Customer];";

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        
        var command = new SqlCommand(sql, connection);
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var customer = new Customer
            {
                Id = (int)reader["Id"],
                FirstName = reader["FirstName"] as string,
                LastName = reader["LastName"] as string,
                Phone = reader["Phone"] as string,
                Email = reader["Email"] as string,
                Street = reader["Street"] as string,
                City = reader["City"] as string,
                State = reader["State"] as string,
                ZipCode = reader["ZipCode"] as string
            };

            customers.Add(customer);
        }

        connection.Close();
        return customers;
    }

    public Customer GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);

        var command = new SqlCommand("SELECT * FROM [Sales].[Customer] WHERE [Id] = @Id;", connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();

        var reader = command.ExecuteReader();
        reader.Read();

        var customer = new Customer 
        {
            Id = (int)reader["Id"],
            FirstName = reader["FirstName"] as string,
            LastName = reader["LastName"] as string,
            Phone = reader["Phone"] as string,
            Email = reader["Email"] as string,
            Street = reader["Street"] as string,
            City = reader["City"] as string,
            State = reader["State"] as string,
            ZipCode = reader["ZipCode"] as string
        };
        
        return customer;
    }

    public void Update(Customer entity)
    {
        var sql = @"UPDATE [Sales].[Customer]
                    SET [FirstName] = @FirstName,
                        [LastName] = @LastName,
                        [Phone] = @Phone,
                        [Email] = @Email,
                        [Street] = @Street,
                        [City] = @City,
                        [State] = @State,
                        [ZipCode] = @ZipCode
                    WHERE [Id] = @Id;";

        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", entity.Id);
        command.Parameters.AddWithValue("@FirstName", entity.FirstName);
        command.Parameters.AddWithValue("@LastName", entity.LastName);
        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(entity.Phone) ? (object)DBNull.Value : entity.Phone);
        command.Parameters.AddWithValue("@Email", entity.Email);
        command.Parameters.AddWithValue("@Street", string.IsNullOrEmpty(entity.Street) ? (object)DBNull.Value : entity.Street);
        command.Parameters.AddWithValue("@City", string.IsNullOrEmpty(entity.City) ? (object)DBNull.Value : entity.City);
        command.Parameters.AddWithValue("@State", string.IsNullOrEmpty(entity.State) ? (object)DBNull.Value : entity.State);
        command.Parameters.AddWithValue("@ZipCode", string.IsNullOrEmpty(entity.ZipCode) ? (object)DBNull.Value : entity.ZipCode);

        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }
}
