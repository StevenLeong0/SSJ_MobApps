using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.Configuration;
using SeniorLearnApi.Models;
using System.Security.Cryptography;

namespace SeniorLearnApi.Services;

public class RefreshTokenService
{
    private readonly IMongoCollection<RefreshToken> _refreshToken;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenService(IMongoDatabase MongoDb, JwtSettings jwtSettings)
    {
        _refreshToken = MongoDb.GetCollection<RefreshToken>("RefreshToken");
        _jwtSettings = jwtSettings;
    }

    #region Refresh Token Methods

    public async Task<string> CreateRefreshTokenAsync(string userId)
    {
        var tokenString = GenerateTokenString();
        var refreshToken = CreateRefreshTokenEntity(tokenString, userId);

        await _refreshToken.InsertOneAsync(refreshToken);
        return tokenString;
    }

    public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
    {
        var hashedToken = HashToken(token);
        var filter = Builders<RefreshToken>.Filter.And(
            Builders<RefreshToken>.Filter.Eq("Token", hashedToken),
            Builders<RefreshToken>.Filter.Eq("IsRevoked", false),
            Builders<RefreshToken>.Filter.Gt("ExpiresAt", DateTime.UtcNow)
        );

        return await _refreshToken.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<string?> ReplaceRefreshTokenAsync(string oldToken)
    {
        var hashedOldToken = HashToken(oldToken);
        var filter = Builders<RefreshToken>.Filter.And(
            Builders<RefreshToken>.Filter.Eq("Token", hashedOldToken),
            Builders<RefreshToken>.Filter.Eq("IsRevoked", false),
            Builders<RefreshToken>.Filter.Gt("ExpiresAt", DateTime.UtcNow)
        );

        var oldRefreshToken = await _refreshToken.Find(filter).FirstOrDefaultAsync();
        if (oldRefreshToken == null)
        {
            return null;
        }

        var timeUntilExpiration = oldRefreshToken.ExpiresAt - DateTime.UtcNow;
        var shouldReplaceToken = timeUntilExpiration.TotalDays <= _jwtSettings.RefreshTokenReplacementThresholdDays;

        if (shouldReplaceToken)
        {
            // Replace the token only if it's near expiration
            var newTokenString = GenerateTokenString();
            var hashedNewToken = HashToken(newTokenString);

            // Update old token
            var updateFilter = Builders<RefreshToken>.Filter.Eq("Id", oldRefreshToken.Id);
            var update = Builders<RefreshToken>.Update
                .Set("IsRevoked", true)
                .Set("RevokedAt", DateTime.UtcNow)
                .Set("ReplacedByToken", hashedNewToken);

            await _refreshToken.UpdateOneAsync(updateFilter, update);

            // Create new token
            var newRefreshToken = CreateRefreshTokenEntity(newTokenString, oldRefreshToken.UserId);

            await _refreshToken.InsertOneAsync(newRefreshToken);
            return newTokenString;
        }
        else
        {
            // Return the same token if it's not near expiration
            return oldToken;
        }
    }

    public async Task<bool> RevokeRefreshTokenAsync(string userId, string token)
    {
        var hashedToken = HashToken(token);
        var filter = Builders<RefreshToken>.Filter.And(
            Builders<RefreshToken>.Filter.Eq("Token", hashedToken),
            Builders<RefreshToken>.Filter.Eq("UserId", userId),
            Builders<RefreshToken>.Filter.Eq("IsRevoked", false),
            Builders<RefreshToken>.Filter.Gt("ExpiresAt", DateTime.UtcNow)
        );

        var update = Builders<RefreshToken>.Update
            .Set("IsRevoked", true)
            .Set("RevokedAt", DateTime.UtcNow);

        var result = await _refreshToken.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    #endregion

    #region Helper Methods

    private string GenerateTokenString()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string HashToken(string token)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hashedBytes);
    }

    private RefreshToken CreateRefreshTokenEntity(string token, string userId)
    {
        return new RefreshToken
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Token = HashToken(token),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false
        };
    }

    #endregion
}
