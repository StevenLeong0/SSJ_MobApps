using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.Configuration;
using SeniorLearnApi.Extensions;
using SeniorLearnApi.Interfaces;
using SeniorLearnApi.Middleware;
using SeniorLearnApi.Models;
using SeniorLearnApi.Services;
using SeniorLearnApi.Settings;
using System.Text.Json.Serialization;

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
