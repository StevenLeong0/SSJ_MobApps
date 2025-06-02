using MongoDB.Driver;
using MongoDB.Bson;


public class UpdateUserSettingService
{
    private readonly IMongoCollection<UserSetting> _userSetting;
    public UpdateUserSettingService(IMongoDatabase MongoDb)
    {
        _userSetting = MongoDb.GetCollection<UserSetting>("UserSetting");
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