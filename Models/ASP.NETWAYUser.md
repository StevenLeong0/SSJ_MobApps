using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class User:IdentityUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public override required string Id { get; set; }
    public required string Username { get; set; }
    public override string? PasswordHash { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public override string? Email { get; set; }
    public UserRole Role { get; set; }

}