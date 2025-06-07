using System;
using System.Threading.Tasks;

namespace SeniorLearnApi.DataSeeding;

public class SeedDataProgram
{

    //can not do this.
    public static async Task Main(string[] args)
    {
        try
        {
            var seeder = new DatabaseSeeder();
            if (args.Length > 0 && args[0].ToLower() == "delete")
            {
                await seeder.DeleteAllDataAsync();
            }
            else
            {
                await seeder.SeedDataAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Environment.Exit(1);
        }
    }
}