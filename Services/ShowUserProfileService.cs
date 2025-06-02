using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSettingsabc;

public class ShowUserProfileService
{
    private readonly IMongoCollection<User> _User;
    public ShowUserProfileService(IOptions<MongoDbSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _User = database.GetCollection<User>("User");   
    }
    public async Task<User> GetUserProfile(string IdString)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var UserProfile = await _User
            .Find(filter)
            .FirstOrDefaultAsync();
            return UserProfile;
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
}