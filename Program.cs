using SeniorLearnApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics;
using SeniorLearnApi.Interfaces;


var builder = WebApplication.CreateBuilder(args);
// Bind Settings
builder.Services.Configure<MongoDbSettings>(
builder.Configuration.GetSection("MongoDbSettings"));


// dependency injection uses types to call methods rather than names
builder.Services.AddSingleton<MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

//Why? You can use interfaces to access the properties of classes that implement them
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<MongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});
// Add services to the container.
//Check if you registered this correctly
builder.Services.AddScoped<IBulletinTypeListService<MemberBulletin>, MemberBulletinListService>();
builder.Services.AddScoped<IBulletinTypeListService<OfficialBulletin>, OfficialBulletinService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<UserSettingService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
DeleteMongoDbScript();
RunMongoScript();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void DeleteMongoDbScript()
{
        try
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash", 
                Arguments = "DataSeeding/DeleteSeedData.sh",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine($"Mongo script output:\n{output}");
        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine($"Mongo script error:\n{error}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error running MongoDB script: {ex.Message}");
    }
}



void RunMongoScript()
{
    try
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "DataSeeding/SeedData.sh", // Assumes 'mongo' is in PATH
                Arguments = "\"mongodb://localhost:27017/SLearnMobApp_db\" DataSeeding/SeedData.sh",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        Console.WriteLine($"Mongo script output:\n{output}");
        if (!string.IsNullOrWhiteSpace(error))
        {
            Console.WriteLine($"Mongo script error:\n{error}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error running MongoDB script: {ex.Message}");
    }
}

