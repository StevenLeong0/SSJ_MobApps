using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SeniorLearnApi.Enums;
using SeniorLearnApi.Interfaces;

namespace SeniorLearnApi.Models;

public class MemberBulletin : IBulletinModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public required string Title { get; set; }
    // public MemberType Type { get; set; }
    public MemberBulletinCategory Category { get; set; }
    //public MemberBulletinCategory Category 

    public string? Content { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateCreated { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateUpdated { get; set; }
    public required string AuthorId { get; set; }
    //Author To AuthorId
    //AuthorUsername
    public required string AuthorUsername { get; set; }

    public MemberBulletin()
    {
        DateCreated = DateTime.Now;
        DateUpdated = DateCreated;
    }
}