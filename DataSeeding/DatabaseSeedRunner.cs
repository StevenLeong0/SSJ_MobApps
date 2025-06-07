using System;
using System.Threading.Tasks;
using SeniorLearnApi.DataSeeding;

namespace SeniorLearnApi.DataSeeding;

public class DatabaseSeedRunner
{
    public static async Task RunMongoSeedAsync()
    {
        try
        {
            var seeder = new DatabaseSeeder();
            await seeder.SeedDataAsync();
            Console.WriteLine("MongoDB seeding completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running MongoDB seeding: {ex.Message}");
        }
    }

    public static async Task RunMongoSeedAsync(string connectionString, string databaseName)
    {
        try
        {
            var seeder = new DatabaseSeeder(connectionString, databaseName);
            await seeder.SeedDataAsync();
            Console.WriteLine("MongoDB seeding completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error running MongoDB seeding: {ex.Message}");
        }
    }
   

    public static async Task DeleteMongoDataAsync()
    {
        try
        {
            var seeder = new DatabaseSeeder();
            await seeder.DeleteAllDataAsync();
            Console.WriteLine("MongoDB data deletion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting MongoDB data: {ex.Message}");
        }
    }

    public static async Task DeleteMongoDataAsync(string connectionString, string databaseName)
    {
        try
        {
            var seeder = new DatabaseSeeder(connectionString, databaseName);
            await seeder.DeleteAllDataAsync();
            Console.WriteLine("MongoDB data deletion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting MongoDB data: {ex.Message}");
        }
    }
}
