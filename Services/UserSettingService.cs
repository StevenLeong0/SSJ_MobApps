using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbSettingsabc;

public class UserSettingService
{
    private readonly IMongoCollection<UserSetting> _userSetting;

    //Constructor Dependency Injection
    public UserSettingService(IMongoDatabase MongoDb)
    {
        _userSetting = MongoDb.GetCollection<UserSetting>("UserSetting");
    }
    public async Task<UserSetting> GetUserSetting(string IdString)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<UserSetting>.Filter.Eq("_id", objectId);
            var UserSetting = await _userSetting
            .Find(filter)
            .FirstOrDefaultAsync();
            return UserSetting;
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
        public async Task UpdateUserSetting(string IdString, TextSize TextSize, bool DarkMode, bool EnableNotifications)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<UserSetting>.Filter.Eq("_id", objectId);
            var update = Builders<UserSetting>.Update
            .Set(u => u.TextSize, TextSize)
            .Set(u => u.DarkMode, DarkMode)
            .Set(u => u.EnableNotifications, EnableNotifications);
            await _userSetting.UpdateOneAsync(filter, update);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }

}