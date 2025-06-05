using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SeniorLearnApi.Enums;

namespace SeniorLearnApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required DateTime MemberSince { get; set; }
    public UserRole Role { get; set; }
}