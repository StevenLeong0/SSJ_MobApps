using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Models;

namespace SeniorLearnApi.Services;

public class UserSettingService
{
    private readonly IMongoCollection<UserSetting> _userSetting;

    //Constructor Dependency Injection
    public UserSettingService(IMongoDatabase MongoDb)
    {
        _userSetting = MongoDb.GetCollection<UserSetting>("UserSetting");
    }

    #region User Setting Methods

    public async Task<SettingsResponse> GetUserSettingsAsync(string userId)
    {
        if (ObjectId.TryParse(userId, out ObjectId objectId))
        {
            var filter = Builders<UserSetting>.Filter.Eq("_id", objectId);
            var userSetting = await _userSetting
                .Find(filter)
                .FirstOrDefaultAsync();

            if (userSetting == null)
            {
                return null;
            }

            return MapToSettingsResponse(userSetting);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(userId));
        }
    }

    public async Task CreateUserSettingsAsync(string userId)
    {
        UserSetting newUserSettings = new()
        {
            Id = userId,
            FontSize = 32,
            DarkMode = false,
            EnableNotifications = true,
        };

        await _userSetting.InsertOneAsync(newUserSettings);
    }

    public async Task<SettingsResponse> UpdateUserSettingsAsync(string userId, UpdateSettingsRequest request)
    {
        if (ObjectId.TryParse(userId, out ObjectId objectId))
        {
            var filter = Builders<UserSetting>.Filter.Eq("_id", objectId);
            var update = Builders<UserSetting>.Update
                .Set(u => u.FontSize, request.FontSize)
                .Set(u => u.DarkMode, request.DarkMode)
                .Set(u => u.EnableNotifications, request.EnableNotifications);

            await _userSetting.UpdateOneAsync(filter, update);

            var updatedUserSetting = await _userSetting.Find(filter).FirstOrDefaultAsync();
            if (updatedUserSetting == null)
            {
                return null;
            }

            return MapToSettingsResponse(updatedUserSetting);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(userId));
        }
    }

    #endregion

    #region Mapping Methods

    private static SettingsResponse MapToSettingsResponse(UserSetting userSetting)
    {
        return new SettingsResponse
        {
            FontSize = userSetting.FontSize,
            DarkMode = userSetting.DarkMode,
            EnableNotifications = userSetting.EnableNotifications
        };
    }

    #endregion
}