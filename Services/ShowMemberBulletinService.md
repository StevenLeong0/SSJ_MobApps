using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class ShowMemberBulletinService
{
    private readonly IMongoCollection<MemberBulletin> _MemberBulletin;
    public ShowMemberBulletinService(IOptions<MongoDbSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _MemberBulletin = database.GetCollection<MemberBulletin>("MemberBulletin");   
    }
    public async Task<MemberBulletin> GetBulletinDetails<ObjectId>(ObjectId Id) {
        var filter = Builders<MemberBulletin>.Filter.Eq("Id", Id);
        var BulletinDetails = await _MemberBulletin.Find(filter).FirstOrDefaultAsync();
        return BulletinDetails;
    }
}

