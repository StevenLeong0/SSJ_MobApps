using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


public class MemberBulletin:IBulletinModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public required string Title { get; set; }
    public MemberType Type { get; set; }
    public string? Content { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateCreated { get; set; }
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime? DateUpdated { get; set; }
    public required string Author { get; set; }
    public MemberBulletin()
    {
        DateCreated = DateTime.Now;
        DateUpdated = DateCreated;
    }
}