using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSettingsabc;

public class OfficialBulletinService : IBulletinTypeListService<OfficialBulletin>
{
    private readonly IMongoCollection<OfficialBulletin> _officialBulletinList;
    public OfficialBulletinService(IMongoDatabase MongoDb)
    {
        _officialBulletinList = MongoDb.GetCollection<OfficialBulletin>("OfficialBulletin");
    }
    //Not Refactored because Official Bulletins don't need to be sorted by Type
    public Task<List<OfficialBulletin>> GetBulletinListByDate<OfficialType>(OfficialType NoType)
    {
        var result = _officialBulletinList
        .Find(doc => true)
        .SortByDescending(b => b.DateCreated)
        .ToListAsync();
        return result;
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
        public async Task FullUpdateOfficialBulletin(string IdString, string Title, OfficialType Type, string Content)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<OfficialBulletin>.Filter.Eq("_id", objectId);
            var update = Builders<OfficialBulletin>.Update
            .Set(b => b.Title, Title)
            .Set(b => b.Type, Type)
            .Set(b => b.Content, Content);
            await _officialBulletinList.UpdateOneAsync(filter, update);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
}