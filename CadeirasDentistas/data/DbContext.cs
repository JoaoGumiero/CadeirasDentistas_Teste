

using System.Data;
using Microsoft.Data.SqlClient;

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
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
    }
    
}