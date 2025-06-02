using MongoDB.Driver;
using MongoDB.Bson;
using System.Data.Common;

public class UpdateMemberBulletinService
{
    private readonly IMongoCollection<MemberBulletin> _memberBulletinList;
    public UpdateMemberBulletinService(IMongoDatabase MongoDb)
    {
        _memberBulletinList = MongoDb.GetCollection<MemberBulletin>("MemberBulletin");
    }
    public async Task FullUpdateMemberBulletin(string IdString, string Title, MemberType Type, string Content)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<MemberBulletin>.Filter.Eq("_id", objectId);
            var update = Builders<MemberBulletin>.Update
            .Set(b => b.Title, Title)
            .Set(b => b.Type, Type)
            .Set(b => b.Content, Content);
            await _memberBulletinList.UpdateOneAsync(filter, update);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
}