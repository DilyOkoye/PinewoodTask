using Crud.Migration;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var migrator = new DatabaseMigrator(configuration);
        migrator.MigrateDatabase();

    }
}