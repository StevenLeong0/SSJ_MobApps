using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.Interfaces;

namespace SeniorLearnApi.Services;

//T is generic so you can put MemberBulletin or OfficialBulletin [StevenLeong]
public class ShowBulletinDetailsService<T> : IShowBulletinService<T> where T : IBulletinModel
{
    private readonly IMongoCollection<T> _Bulletin;

    //check with Tyson
    public ShowBulletinDetailsService(IMongoDatabase MongoDb)
    {
        _Bulletin = MongoDb.GetCollection<T>("Bulletin");
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
            //TASK1: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(objectId));
        }
    }

}

