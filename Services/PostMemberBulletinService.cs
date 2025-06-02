/*
what it needs Id
why is it sep from Admin (needs to confirm that Id is admin) needs access to post to official bulletins
*/
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Data.Common;
using MongoDbSettingsabc;
using MongoDB.Bson;
using System.Threading.Tasks;


public class PostMemberBulletinService
{
    private readonly IMongoCollection<MemberBulletin> _memberBulletinList;
    public PostMemberBulletinService(IOptions<MongoDbSettings> dbSettings)
    {
        var client = new MongoClient(dbSettings.Value.ConnectionString);
        var database = client.GetDatabase(dbSettings.Value.DatabaseName);
        _memberBulletinList = database.GetCollection<MemberBulletin>("MemberBulletin");
    }
    public async Task PostMemberBulletin(string Title, MemberType Type, string Content, string Author)
    {
        MemberBulletin newMemberBulletin = new()
        //var newMemberBulletin =new MemberBulletin
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Title = Title,
            Type = Type,
            Content = Content,
            Author = Author
        };
        await _memberBulletinList.InsertOneAsync(newMemberBulletin);
    }
}