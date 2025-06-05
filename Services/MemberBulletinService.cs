using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Enums;
using SeniorLearnApi.Interfaces;
using SeniorLearnApi.Models;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace SeniorLearnApi.Services;

public class MemberBulletinService : IBulletinTypeListService<MemberBulletin>
{
    private readonly IMongoCollection<MemberBulletin> _memberBulletinList;

    public MemberBulletinService(IMongoDatabase database)
    {
        _memberBulletinList = database.GetCollection<MemberBulletin>("MemberBulletin");
    }

    #region Member Bulletin Methods

    public async Task<List<MemberBulletinListItemResponse>> GetMemberBulletinsAsync(
        MemberBulletinCategory? category = null,
        string? userId = null)
    {
        if (category.HasValue && !string.IsNullOrEmpty(userId))
        {
            return await GetMemberBulletinsByCategoryAndUserAsync(category.Value, userId);
        }
        else if (category.HasValue)
        {
            return await GetMemberBulletinsByCategoryAsync(category.Value);
        }
        else if (!string.IsNullOrEmpty(userId))
        {
            return await GetMemberBulletinsByUserIdAsync(userId);
        }
        else
        {
            return await GetAllMemberBulletinsAsync();
        }
    }

    public async Task<List<MemberBulletinListItemResponse>> GetAllMemberBulletinsAsync()
    {
        var result = await _memberBulletinList
            .Find(doc => true)
            .SortByDescending(b => b.DateCreated)
            .ToListAsync();

        return result.Select(MapToMemberBulletinListItemResponse).ToList();
    }

    public async Task<List<MemberBulletinListItemResponse>> GetMemberBulletinsByCategoryAsync(MemberBulletinCategory category)
    {
        var filter = Builders<MemberBulletin>.Filter.Eq("Category", category);
        var result = await _memberBulletinList
            .Find(filter)
            .SortByDescending(b => b.DateCreated)
            .ToListAsync();

        return result.Select(MapToMemberBulletinListItemResponse).ToList();
    }

    public async Task<List<MemberBulletinListItemResponse>> GetMemberBulletinsByUserIdAsync(string userId)
    {
        var filter = Builders<MemberBulletin>.Filter.Eq("AuthorId", userId);
        var result = await _memberBulletinList
            .Find(filter)
            .SortByDescending(b => b.DateCreated)
            .ToListAsync();

        return result.Select(MapToMemberBulletinListItemResponse).ToList();
    }

    public async Task<List<MemberBulletinListItemResponse>> GetMemberBulletinsByCategoryAndUserAsync(
        MemberBulletinCategory category,
        string userId)
    {
        var filter = Builders<MemberBulletin>.Filter.And(
            Builders<MemberBulletin>.Filter.Eq("Category", category),
            Builders<MemberBulletin>.Filter.Eq("AuthorId", userId)
        );

        var result = await _memberBulletinList
            .Find(filter)
            .SortByDescending(b => b.DateCreated)
            .ToListAsync();

        return result.Select(MapToMemberBulletinListItemResponse).ToList();
    }

    public async Task<MemberBulletinDetailResponse?> GetMemberBulletinByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return null;
        }

        var filter = Builders<MemberBulletin>.Filter.Eq("_id", objectId);
        var bulletin = await _memberBulletinList
            .Find(filter)
            .FirstOrDefaultAsync();

        return bulletin != null ? MapToMemberBulletinDetailResponse(bulletin) : null;
    }

    public async Task<MemberBulletinDetailResponse> CreateMemberBulletinAsync(CreateMemberBulletinRequest request, string memberId, string memberName)
    {
        MemberBulletin newMemberBulletin = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Title = request.Title,
            Category = request.Category,
            Content = request.Content,
            AuthorId = memberId,
            AuthorUsername = memberName
        };

        await _memberBulletinList.InsertOneAsync(newMemberBulletin);
        return MapToMemberBulletinDetailResponse(newMemberBulletin);
    }

    public async Task<MemberBulletinDetailResponse?> UpdateMemberBulletinAsync(string id, UpdateMemberBulletinRequest request, string memberId)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return null;
        }

        var filter = Builders<MemberBulletin>.Filter.And(
            Builders<MemberBulletin>.Filter.Eq("_id", objectId),
            Builders<MemberBulletin>.Filter.Eq("AuthorId", memberId)
        );

        var update = Builders<MemberBulletin>.Update
            .Set(b => b.Title, request.Title)
            .Set(b => b.Category, request.Category)
            .Set(b => b.Content, request.Content)
            .Set(b => b.DateUpdated, DateTime.Now);

        var result = await _memberBulletinList.UpdateOneAsync(filter, update);

        if (result.ModifiedCount > 0)
        {
            var updatedBulletin = await _memberBulletinList.Find(filter).FirstOrDefaultAsync();
            return MapToMemberBulletinDetailResponse(updatedBulletin);
        }

        return null;
    }

    public async Task<bool> DeleteMemberBulletinAsync(string id, string memberId)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return false;
        }

        var filter = Builders<MemberBulletin>.Filter.And(
            Builders<MemberBulletin>.Filter.Eq("_id", objectId),
            Builders<MemberBulletin>.Filter.Eq("AuthorId", memberId)
        );

        var result = await _memberBulletinList.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    #endregion

    #region Mapping Methods

    private static MemberBulletinListItemResponse MapToMemberBulletinListItemResponse(MemberBulletin bulletin)
    {
        return new MemberBulletinListItemResponse
        {
            Id = bulletin.Id,
            Title = bulletin.Title,
            CreatedById = bulletin.AuthorId,
            CreatedByUsername = bulletin.AuthorUsername,
            Category = bulletin.Category,
            CreatedAt = bulletin.DateCreated ?? DateTime.MinValue,
            UpdatedAt = bulletin.DateUpdated
        };
    }

    private static MemberBulletinDetailResponse MapToMemberBulletinDetailResponse(MemberBulletin bulletin)
    {
        return new MemberBulletinDetailResponse
        {
            Id = bulletin.Id,
            Title = bulletin.Title,
            Content = bulletin.Content,
            CreatedById = bulletin.AuthorId,
            CreatedByUsername = bulletin.AuthorUsername,
            Category = bulletin.Category,
            CreatedAt = bulletin.DateCreated ?? DateTime.MinValue,
            UpdatedAt = bulletin.DateUpdated
        };
    }

    #endregion
}