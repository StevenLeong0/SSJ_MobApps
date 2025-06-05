using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Models;
using BCryptHash = BCrypt.Net.BCrypt;

namespace SeniorLearnApi.Services;

public class UserProfileService
{
    private readonly IMongoCollection<User> _user;
    private readonly MemberBulletinService _memberBulletinService;

    public UserProfileService(IMongoDatabase MongoDb, MemberBulletinService memberBulletinService)
    {
        _user = MongoDb.GetCollection<User>("User");
        _memberBulletinService = memberBulletinService;
    }

    #region User Profile Methods

    public async Task<ProfileResponse> GetUserProfileAsync(string userId)
    {
        if (ObjectId.TryParse(userId, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var user = await _user
                .Find(filter)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var userBulletins = await GetUserBulletinsAsync(userId);

            return MapToProfileResponse(user, userBulletins);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(userId));
        }
    }

    public async Task<ProfileResponse> UpdateUserProfileAsync(string userId, UpdateProfileRequest request)
    {
        if (ObjectId.TryParse(userId, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var update = Builders<User>.Update
                .Set(u => u.FirstName, request.FirstName)
                .Set(u => u.LastName, request.LastName)
                .Set(u => u.Email, request.Email);

            await _user.UpdateOneAsync(filter, update);

            var updatedUser = await _user.Find(filter).FirstOrDefaultAsync();
            if (updatedUser == null)
            {
                return null;
            }

            var userBulletins = await GetUserBulletinsAsync(userId);
            return MapToProfileResponse(updatedUser, userBulletins);
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(userId));
        }
    }

    //UpdateUserProfileMethods
    public async Task PartialUpdateUserPassword(string IdString, string Password)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var passwordHash = await Task.Run(() => BCryptHash.HashPassword(Password));
            var update = Builders<User>.Update
            .Set(u => u.PasswordHash, passwordHash);
            await _user.UpdateOneAsync(filter, update);
        }
        else
        {
            //TASK1User: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }

    public async Task PartialUpdateUsername(string IdString, string Username)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var update = Builders<User>.Update
            .Set(u => u.Username, Username);
            await _user.UpdateOneAsync(filter, update);
        }
        else
        {
            //TASK1User: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }

    #endregion

    #region Helper Methods

    private async Task<List<MemberBulletinListItemResponse>> GetUserBulletinsAsync(string userId)
    {
        return await _memberBulletinService.GetMemberBulletinsByUserIdAsync(userId);
    }

    #endregion

    #region Mapping Methods

    private static ProfileResponse MapToProfileResponse(User user, List<MemberBulletinListItemResponse> userBulletins)
    {
        return new ProfileResponse
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            MembershipDate = user.MemberSince,
            MyBulletins = userBulletins
        };
    }

    #endregion
}