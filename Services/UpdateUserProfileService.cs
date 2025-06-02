using MongoDB.Driver;
using MongoDB.Bson;
using BCryptHash = BCrypt.Net.BCrypt;


public class UpdateUserProfileService
{
    private readonly IMongoCollection<User> _user;
    public UpdateUserProfileService(IMongoDatabase MongoDb)
    {
        _user = MongoDb.GetCollection<User>("User");
    }
    public async Task PartialUpdateUserPassword(string IdString, string Password)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var passwordHash = await Task.Run(() => BCryptHash.HashPassword(Password));
            var update = Builders<User>.Update
            .Set(u => u.PasswordHash, passwordHash);
            await _user.UpdateOneAsync(filter, update);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
    public async Task PartialUpdateUserProfile(string IdString, string FirstName, string LastName, string Email)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var update = Builders<User>.Update
            .Set(u => u.FirstName, FirstName)
            .Set(u => u.LastName, LastName)
            .Set(u => u.Email, Email);
            await _user.UpdateOneAsync(filter, update);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
    public async Task PartialUpdateUsername(string IdString, string Username)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var update = Builders<User>.Update
            .Set(u => u.Username, Username);
            await _user.UpdateOneAsync(filter, update);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }

    }
}