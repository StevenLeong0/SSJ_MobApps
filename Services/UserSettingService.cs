using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


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
            //TASK1Settings: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
    public async Task PostUserSettings(string Id, TextSize TextSize, bool DarkMode, bool EnableNotifications)
    {
        UserSetting PostUserSettings = new()
        {
            Id = Id,
            TextSize = TextSize,
            DarkMode = DarkMode,
            EnableNotifications = EnableNotifications,
        };
        await _userSetting.InsertOneAsync(PostUserSettings);
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
            //TASK1Settings: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }

}