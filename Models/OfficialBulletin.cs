using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SeniorLearnApi.Enums;
using SeniorLearnApi.Interfaces;

namespace SeniorLearnApi.Models;

public class OfficialBulletin : IBulletinModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public required string Title { get; set; }
    public OfficialType Type { get; set; }
    public string? Content { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateCreated { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateUpdated { get; set; }
    public required string AuthorId { get; set; }
    public required string AuthorName { get; set; }
    public OfficialBulletin()
    {
        DateCreated = DateTime.Now;
        DateUpdated = DateCreated;
    }
}