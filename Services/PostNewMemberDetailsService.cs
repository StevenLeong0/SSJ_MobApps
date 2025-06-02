using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using BCryptHash = BCrypt.Net.BCrypt;

public class PostNewMemberDetailsService
{
    private readonly IMongoCollection<User> _user;
    public PostNewMemberDetailsService(IMongoDatabase MongoDb)
    {
        _user = MongoDb.GetCollection<User>("User");
    }
    public async Task PostMember(string Username, string Password, string FirstName, string LastName, string Email)
    {
        User newUser = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Username = Username,
            PasswordHash = BCryptHash.HashPassword(Password),
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            MemberSince = DateTime.Now,
            Role = 0,
        };
        await _user.InsertOneAsync(newUser);

    }

}