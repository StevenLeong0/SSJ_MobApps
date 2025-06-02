using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSettingsabc;

public class ShowUserSettingService
{
    private readonly IMongoCollection<UserSetting> _UserSetting;

    //Constructor Dependency Injection
    public ShowUserSettingService(IMongoDatabase MongoDb)
    {
        _UserSetting = MongoDb.GetCollection<UserSetting>("UserSetting");
    }
    public async Task<UserSetting> GetUserSetting(string IdString)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<UserSetting>.Filter.Eq("_id", objectId);
            var UserSetting = await _UserSetting
            .Find(filter)
            .FirstOrDefaultAsync();
            return UserSetting;
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
}








    //Should it query the database?
    /* I think it should return the current settings from the cookies
    to the user by default and if cookies is null then access default settings then database


    fetching current userSettings should be done on frontend React Native working with asyncstorage
    https://medium.com/@Shehryar_sp22-bcs-010/exploring-api-calls-and-asyncstorage-in-react-native-8b8076bed0de
    
    fetch data from API (asyncStorage)
    display data onto form
    return to user
    */
