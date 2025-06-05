using MongoDB.Bson;
using MongoDB.Driver;
using SeniorLearnApi.DTOs.Requests;
using SeniorLearnApi.DTOs.Responses;
using SeniorLearnApi.Interfaces;
using SeniorLearnApi.Models;

namespace SeniorLearnApi.Services;

public class OfficialBulletinService : IBulletinTypeListService<OfficialBulletin>
{
    private readonly IMongoCollection<OfficialBulletin> _officialBulletinList;

    public OfficialBulletinService(IMongoDatabase MongoDb)
    {
        _officialBulletinList = MongoDb.GetCollection<OfficialBulletin>("OfficialBulletin");
    }

    #region Official Bulletin Methods

    public async Task<List<OfficialBulletinListItemResponse>> GetAllOfficialBulletinsAsync()
    {
        var result = await _officialBulletinList
            .Find(doc => true)
            .SortByDescending(b => b.DateCreated)
            .ToListAsync();

        return result.Select(MapToOfficialBulletinListItemResponse).ToList();
    }

    public async Task<OfficialBulletinDetailResponse?> GetOfficialBulletinByIdAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return null;
        }

        var filter = Builders<OfficialBulletin>.Filter.Eq("_id", objectId);
        var bulletin = await _officialBulletinList
            .Find(filter)
            .FirstOrDefaultAsync();

        return bulletin != null ? MapToOfficialBulletinDetailResponse(bulletin) : null;
    }

    public async Task<OfficialBulletinDetailResponse> CreateOfficialBulletinAsync(CreateOfficialBulletinRequest request, string adminId, string adminName)
    {
        OfficialBulletin newOfficialBulletin = new()
        {
            Id = ObjectId.GenerateNewId().ToString(),
            Title = request.Title,
            // Type = request.Type,
            Content = request.Content,
            AuthorId = adminId,
            AuthorName = adminName
        };

        await _officialBulletinList.InsertOneAsync(newOfficialBulletin);
        return MapToOfficialBulletinDetailResponse(newOfficialBulletin);
    }

    public async Task<OfficialBulletinDetailResponse?> UpdateOfficialBulletinAsync(string id, UpdateOfficialBulletinRequest request)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return null;
        }

        var filter = Builders<OfficialBulletin>.Filter.Eq("_id", objectId);
        var update = Builders<OfficialBulletin>.Update
            .Set(b => b.Title, request.Title)
            //.Set(b => b.Type, request.Type)
            .Set(b => b.Content, request.Content)
            .Set(b => b.DateUpdated, DateTime.Now);

        var result = await _officialBulletinList.UpdateOneAsync(filter, update);

        if (result.ModifiedCount > 0)
        {
            var updatedBulletin = await _officialBulletinList.Find(filter).FirstOrDefaultAsync();
            return MapToOfficialBulletinDetailResponse(updatedBulletin);
        }

        return null;
    }

    public async Task<bool> DeleteOfficialBulletinAsync(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return false;
        }

        var filter = Builders<OfficialBulletin>.Filter.Eq("_id", objectId);
        var result = await _officialBulletinList.DeleteOneAsync(filter);

        return result.DeletedCount > 0;
    }

    #endregion

    #region Mapping Methods

    private static OfficialBulletinListItemResponse MapToOfficialBulletinListItemResponse(OfficialBulletin bulletin)
    {
        return new OfficialBulletinListItemResponse
        {
            Id = bulletin.Id,
            Title = bulletin.Title,
            CreatedById = bulletin.AuthorId,
            CreatedByUsername = bulletin.AuthorName,
            CreatedAt = bulletin.DateCreated ?? DateTime.MinValue,
            UpdatedAt = bulletin.DateUpdated
        };
    }

    private static OfficialBulletinDetailResponse MapToOfficialBulletinDetailResponse(OfficialBulletin bulletin)
    {
        return new OfficialBulletinDetailResponse
        {
            Id = bulletin.Id,
            Title = bulletin.Title,
            Content = bulletin.Content,
            CreatedById = bulletin.AuthorId,
            CreatedByUsername = bulletin.AuthorName,
            CreatedAt = bulletin.DateCreated ?? DateTime.MinValue,
            UpdatedAt = bulletin.DateUpdated
        };
    }

    #endregion
}