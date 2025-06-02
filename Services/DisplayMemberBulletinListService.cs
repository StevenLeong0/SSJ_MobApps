using System.Collections.ObjectModel;
using System.Data.Common;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSettingsabc;


public class DisplayMemberBulletinListService : IDisplayBulletinListService<MemberBulletin>
{
    private readonly IMongoCollection<MemberBulletin> _memberBulletinList;
    // public DisplayMemberBulletinListService(IOptions<MongoDbSettings> dbSettings)
    // {
    //     var client = new MongoClient(dbSettings.Value.ConnectionString);
    //     var database = client.GetDatabase(dbSettings.Value.DatabaseName);
    //     _memberBulletinList = database.GetCollection<MemberBulletin>("MemberBulletin");   
    // }
    public DisplayMemberBulletinListService(IMongoDatabase database)
    {
        _memberBulletinList = database.GetCollection<MemberBulletin>("MemberBulletin");
    }
    //Task for async
    public Task<List<MemberBulletin>> GetBulletinListByDate<MemberType>(MemberType Type)
    {
        var filter = Builders<MemberBulletin>.Filter.Eq("Type", Type);
        var result = _memberBulletinList
        .Find(filter)
        .SortByDescending(b => b.DateCreated)
        .ToListAsync();
        //change author from objectid to username; return that object;
        return result;
    }
}