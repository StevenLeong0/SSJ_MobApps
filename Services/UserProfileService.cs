using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using BCryptHash = BCrypt.Net.BCrypt;


public class UserProfileService
{
    private readonly IMongoCollection<User> _user;
    private readonly UserSettingService _userSettingService;
    public UserProfileService(IMongoDatabase MongoDb, UserSettingService userSettingService)
    {
        _user = MongoDb.GetCollection<User>("User");
        _userSettingService = userSettingService;

    }
    public async Task<User> GetUserProfile(string IdString)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var UserProfile = await _user
            .Find(filter)
            .FirstOrDefaultAsync();
            return UserProfile;
        }
        else
        {
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
    //Post New Member
    public async Task PostMember(string Username, string Password, string FirstName, string LastName, string Email)
    {
        User newUser = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Username = Username,
            PasswordHash = BCryptHash.HashPassword(Password),
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            MemberSince = DateTime.Now,
            Role = 0,
        };
        await _user.InsertOneAsync(newUser);
        //addUserSettingService method
        await _userSettingService.PostUserSettings(newUser.Id);
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
    public async Task PartialUpdateUserProfile(string IdString, string FirstName, string LastName, string Email)
    {
        if (ObjectId.TryParse(IdString, out ObjectId objectId))
        {
            var filter = Builders<User>.Filter.Eq("_id", objectId);
            var update = Builders<User>.Update
            .Set(u => u.FirstName, FirstName)
            .Set(u => u.LastName, LastName)
            .Set(u => u.Email, Email);
            await _user.UpdateOneAsync(filter, update);
        }
        else
        {
            //TASK1User: need to write a try-catch block to catch this exception
            throw new ArgumentException("Invalid Id", nameof(IdString));
        }
    }
    public async Task<bool> UserLogin(string Username, string Password)
    {
        var filter = Builders<User>.Filter.Eq("Username", Username);
        var UserLogin = await _user
            .Find(filter)
            .FirstOrDefaultAsync();
        var PasswordHash = BCryptHash.HashPassword(Password);
        if (UserLogin != null)
        {
            if (UserLogin.PasswordHash == PasswordHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
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
}