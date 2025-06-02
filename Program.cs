using MongoDbSettingsabc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
