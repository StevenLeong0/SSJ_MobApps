using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.Configuration;
using SeniorLearnApi.DataSeeding;
using SeniorLearnApi.Extensions;
using SeniorLearnApi.Interfaces;
using SeniorLearnApi.Middleware;
using SeniorLearnApi.Models;
using SeniorLearnApi.Services;
using SeniorLearnApi.Settings;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using ZstdSharp.Unsafe;

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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Custom model validation response
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                .ToList();

            var errorMessage = string.Join("; ", errors);
            var response = SeniorLearnApi.DTOs.Responses.ApiResponse<object>.ErrorResponse(errorMessage);

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(response);
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
//Check if you registered this correctly
builder.Services.AddScoped<IBulletinTypeListService<MemberBulletin>, MemberBulletinService>();
builder.Services.AddScoped<IBulletinTypeListService<OfficialBulletin>, OfficialBulletinService>();
builder.Services.AddScoped<UserContextService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MemberBulletinService>();
builder.Services.AddScoped<OfficialBulletinService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<UserSettingService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//DeleteMongoDbScript();
await RunMongoScript();
app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionMiddleware>();
//app.UseMiddleware<InputValidationMiddleware>();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// --- MongoDB Test ---
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    try
    {
        await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
        Console.WriteLine("MongoDB connection successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"MongoDB connection failed: {ex.Message}");
    }
}
// --------------------

app.Run();

// async Task DeleteMongoDbScript(IMongoDatabase MongoDb)
// {
    //     try
    // {
    //     var process = new Process
    //     {
    //         StartInfo = new ProcessStartInfo
    //         {
    //             FileName = "/bin/bash", 
    //             Arguments = "DataSeeding/DeleteSeedData.sh",
    //             RedirectStandardOutput = true,
    //             RedirectStandardError = true,
    //             UseShellExecute = false,
    //             CreateNoWindow = true,
    //         }
    //     };

    //     process.Start();

    //     string output = process.StandardOutput.ReadToEnd();
    //     string error = process.StandardError.ReadToEnd();
    //     process.WaitForExit();

    //     Console.WriteLine($"Mongo script output:\n{output}");
    //     if (!string.IsNullOrWhiteSpace(error))
    //     {
    //         Console.WriteLine($"Mongo script error:\n{error}");
    //     }
    // }
    // catch (Exception ex)
    // {
    //     Console.WriteLine($"Error running MongoDB script: {ex.Message}");
    // }
// }


async Task RunMongoScript() // Fixed: Made async
{

    await DatabaseSeedRunner.DeleteMongoDataAsync();
    // To seed data
    //await DatabaseSeedRunner.RunMongoSeedAsync();
    // To delete data (commented out as it would delete right after seeding)
    //await DatabaseSeedRunner.DeleteMongoDataAsync();
    // With custom connection string (commented out to avoid duplicate seeding)
    // await DatabaseSeedRunner.RunMongoSeedAsync("mongodb://localhost:27017", "SeniorLearnBulletin");
}

//void RunMongoScript()
//{
//    try
//    {
//        var process = new Process
//        {
//            StartInfo = new ProcessStartInfo
//            {
//                FileName = "DataSeeding/SeedData.sh", // Assumes 'mongo' is in PATH
//                Arguments = "\"mongodb://localhost:27017/SLearnMobApp_db\" DataSeeding/SeedData.sh",
//                RedirectStandardOutput = true,
//                RedirectStandardError = true,
//                UseShellExecute = false,
//                CreateNoWindow = true,
//            }
//        };

//        process.Start();

//        string output = process.StandardOutput.ReadToEnd();
//        string error = process.StandardError.ReadToEnd();
//        process.WaitForExit();

//        Console.WriteLine($"Mongo script output:\n{output}");
//        if (!string.IsNullOrWhiteSpace(error))
//        {
//            Console.WriteLine($"Mongo script error:\n{error}");
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Error running MongoDB script: {ex.Message}");
//    }
//}

