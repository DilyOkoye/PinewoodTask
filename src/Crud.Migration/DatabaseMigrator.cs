using System.Reflection;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Crud.Migration;

public class DatabaseMigrator
{
    private readonly IConfiguration _configuration;

    public DatabaseMigrator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void MigrateDatabase()
    {
        var connectionString = _configuration.GetConnectionString("Default");
        var masterConnectionString = _configuration.GetConnectionString("Master");

        // Check if the database exists, create it if not
        EnsureDatabaseExists(connectionString, masterConnectionString);

        // Perform database migrations
        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw new Exception("Database migration failed", result.Error);
        }
    }

    static void EnsureDatabaseExists(string? connectionString,string? masterConnectionString)
    {
        try
        {
            // Try to open a connection to check if the database exists
            using var connection = new SqlConnection(connectionString);
            connection.Open();
        }
        catch (SqlException)
        {
            // Create the database using the master connection string
            using var createConnection = new SqlConnection(masterConnectionString);
            createConnection.Open();

            var command = new SqlCommand($"CREATE DATABASE CustomerDb", createConnection);
            command.ExecuteNonQuery();
        }
    }
}