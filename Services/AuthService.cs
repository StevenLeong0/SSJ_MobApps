using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.Configuration;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Enums;
using SeniorLearnApi.Models;
using BCryptHash = BCrypt.Net.BCrypt;

namespace SeniorLearnApi.Services;

public class AuthService
{
    private readonly IMongoCollection<User> _user;
    private readonly JwtService _jwtService;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly UserSettingService _userSettingService;
    private readonly JwtSettings _jwtSettings;

    // Mock data storage
    private static readonly List<User> _users = new();

    public AuthService(IMongoDatabase MongoDb, JwtSettings jwtSettings, JwtService jwtService, UserSettingService userSettingService, RefreshTokenService refreshTokenService)
    {
        _user = MongoDb.GetCollection<User>("User");
        _jwtSettings = jwtSettings;
        _jwtService = jwtService;
        _userSettingService = userSettingService;
        _refreshTokenService = refreshTokenService;
    }


    public async Task<User?> RegisterAsync(RegisterRequest request)
    {
        // Check if username already exists
        var usernameFilter = Builders<User>.Filter.Eq("Username", request.Username);
        var existingUserByUsername = await _user.Find(usernameFilter).FirstOrDefaultAsync();
        if (existingUserByUsername != null)
        {
            return null;
        }

        // Check if email already exists
        var emailFilter = Builders<User>.Filter.Eq("Email", request.Email);
        var existingUserByEmail = await _user.Find(emailFilter).FirstOrDefaultAsync();
        if (existingUserByEmail != null)
        {
            return null;
        }

        User newUser = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Username = request.Username,
            PasswordHash = BCryptHash.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            MemberSince = DateTime.UtcNow,
            Role = UserRole.Member,
        };

        await _user.InsertOneAsync(newUser);

        await _userSettingService.CreateUserSettingsAsync(newUser.Id);

        return newUser;
    }

    public async Task<User?> SignInAsync(SignInRequest request)
    {
        var filter = Builders<User>.Filter.Eq("Username", request.Username);
        var user = await _user.Find(filter).FirstOrDefaultAsync();

        if (user == null || !BCryptHash.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }

    public async Task<AuthResponse> CreateAuthResponseAsync(User user)
    {
        var accessToken = _jwtService.GenerateAccessToken(user);
        var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user.Id);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _jwtSettings.AccessTokenExpirationMinutes * 60
        };
    }

    public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var refreshTokenEntity = await _refreshTokenService.ValidateRefreshTokenAsync(request.RefreshToken);
        if (refreshTokenEntity == null || !refreshTokenEntity.IsActive)
        {
            return null;
        }

        var userFilter = Builders<User>.Filter.Eq("Id", refreshTokenEntity.UserId);
        var user = await _user.Find(userFilter).FirstOrDefaultAsync();
        if (user == null)
        {
            return null;
        }

        // Generate new access token
        var accessToken = _jwtService.GenerateAccessToken(user);

        // Replace refresh token only if near expiration
        var newRefreshToken = await _refreshTokenService.ReplaceRefreshTokenAsync(request.RefreshToken);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = _jwtSettings.AccessTokenExpirationMinutes * 60
        };
    }

    public async Task<bool> SignOutAsync(string userId, string refreshToken)
    {
        return await _refreshTokenService.RevokeRefreshTokenAsync(userId, refreshToken);
    }
}
