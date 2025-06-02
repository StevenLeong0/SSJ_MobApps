using MongoDB.Driver;
using MongoDB.Bson;

public class PostOfficialBulletinService
{
    private readonly IMongoCollection<OfficialBulletin> _officialBulletinList;
    public PostOfficialBulletinService(IMongoDatabase database)
    {
        _officialBulletinList = database.GetCollection<OfficialBulletin>("OfficialBulletin");
    }
    public async Task PostOfficialBulletin(string Title, OfficialType Type, string Content)
    {
        OfficialBulletin newOfficialBulletin = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Title = Title,
            Type = Type,
            Content = Content,
            Author = "Admin"
        };
        await _officialBulletinList.InsertOneAsync(newOfficialBulletin);
    }
}