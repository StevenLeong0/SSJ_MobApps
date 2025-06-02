using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDbSettingsabc;

//T is generic so you can put MemberBulletin or OfficialBulletin [StevenLeong]
public class ShowBulletinDetailsService<T> : IShowBulletinService<T> where T : IBulletinModel
{
    private readonly IMongoCollection<T> _Bulletin;

    //check with Tyson
    public ShowBulletinDetailsService(IOptions<MongoDbSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _Bulletin = database.GetCollection<T>("Bulletin");
    }
    public async Task<T> GetBulletinDetails(string IdString)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<T>.Filter.Eq("_id", objectId);
            var BulletinDetails = await _Bulletin
            .Find(filter)
            .FirstOrDefaultAsync();
            return BulletinDetails;
        }
        else
        {
            // need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(objectId));
        }
    }
}

