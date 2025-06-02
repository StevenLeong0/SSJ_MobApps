using MongoDB.Bson;
using MongoDB.Driver;
public class memberAuthorUsernameService
{
    //MemberBulletinList -> iterate over to find AuthorObjectId-> find Username-> replace ObjectID with current Username
    private readonly IMongoCollection<MemberBulletin> _memberBulletinList;
    private readonly IMongoCollection<User> _user;
    public memberAuthorUsernameService(IMongoDatabase database)
    {
        _memberBulletinList = database.GetCollection<MemberBulletin>("MemberBulletin");
        _user = database.GetCollection<User>("User");

    }
    public async Task<List<MemberBulletin>> MemberBulletinAuthorService()
    {
        /*
        you have bulletin
        iterate collection
        find author id
        [replace author id with username]
        search objectid _id in user collection
        
        */

        //Check with Tyson
        var memberBulletinList = await _memberBulletinList.Find(_ => true).ToListAsync();
        foreach (var m in memberBulletinList)
        {
            if (ObjectId.TryParse(m.Author, out ObjectId AuthorObjectId))
            {
                var filter = Builders<User>.Filter.Eq("_id", AuthorObjectId);
                var user = await _user.Find(filter).FirstOrDefaultAsync();
                if (user != null)
                {
                    m.Author = user.Username;
                }
                else
                {
                    throw new ArgumentException("Invalid Id: UserCollection", nameof(_user));
                }
            }
            else
            {
                throw new ArgumentException("Invalid Id: AuthorObjectId", nameof(m.Author));
            }
        }
        return memberBulletinList;
    }
}