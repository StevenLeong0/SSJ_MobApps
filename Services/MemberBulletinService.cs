using System.Collections.ObjectModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

public class MemberBulletinListService : IBulletinTypeListService<MemberBulletin>
{
    private readonly IMongoCollection<MemberBulletin> _memberBulletinList;
    private readonly IMongoCollection<User> _user;
    public MemberBulletinListService(IMongoDatabase database)
    {
        _memberBulletinList = database.GetCollection<MemberBulletin>("MemberBulletin");
        _user = database.GetCollection<User>("User");
    }
    //Task for async
    public async Task<List<MemberBulletin>> GetBulletinListByDate<MemberBulletinCategory>(MemberBulletinCategory Category)
    {
        var filter = Builders<MemberBulletin>.Filter.Eq("Category", Category);
        var result = await _memberBulletinList
        .Find(filter)
        .SortByDescending(b => b.DateCreated)
        .ToListAsync();

        //Changes Made: Keep Author as Username
        //change author from objectid to username;
        // await ReplaceAuthorIdwithUserService(result);
        return result;
    }
    private async Task ReplaceAuthorIdIdwithUserService(List<MemberBulletin> memberBulletinList)
    {
        foreach (var m in memberBulletinList)
        {
            if (ObjectId.TryParse(m.AuthorId, out ObjectId AuthorObjectId))
            {
                var filter = Builders<User>.Filter.Eq("_id", AuthorObjectId);
                var user = await _user.Find(filter).FirstOrDefaultAsync();
                if (user != null)
                {
                    m.AuthorUsername = user.Username;
                }
                else
                {
                    //TASK1Member: need to write a try-catch block to catch this exception
                    throw new ArgumentException("Invalid Id: UserCollection", nameof(_user));
                }
            }
            else
            {
                //TASK1Member: need to write a try-catch block to catch this exception
                throw new ArgumentException("Invalid Id: AuthorObjectId", nameof(m.AuthorId));
            }
        }
    }
    public async Task PostMemberBulletin(string Title, MemberBulletinCategory Category, string Content, string AuthorId, string AuthorUsername)
    {
        MemberBulletin newMemberBulletin = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Title = Title,
            Category = Category,
            Content = Content,
            AuthorId = AuthorId,
            AuthorUsername = AuthorUsername
        };
        await _memberBulletinList.InsertOneAsync(newMemberBulletin);
    }
    public async Task FullUpdateMemberBulletin(string IdString, string Title, MemberBulletinCategory Category, string Content)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<MemberBulletin>.Filter.Eq("_id", objectId);
            var update = Builders<MemberBulletin>.Update
            .Set(b => b.Title, Title)
            .Set(b => b.Category, Category)
            .Set(b => b.Content, Content)
            .Set(b => b.DateUpdated, DateTime.Now);
            await _memberBulletinList.UpdateOneAsync(filter, update);
        }
        else
        {
            //TASK1Member: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
    public async Task DeleteMemberBulletin(string IdString)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<MemberBulletin>.Filter.Eq("_id", objectId);
            await _memberBulletinList.DeleteOneAsync(filter);
        }
        else
        {
            //TASK1Member: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
}