

using System.Data;
using MySql.Data.MySqlClient;

namespace CadeirasDentistas.Data
{
    public class ApplicationDbContext
    {
    private readonly string _connectionString;

    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
    }
    
}