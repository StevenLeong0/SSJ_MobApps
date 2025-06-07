using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SeniorLearnApi.Models;

public class UserSetting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public int FontSize { get; set; }
    public bool DarkMode { get; set; }
    public bool EnableNotifications { get; set; }
}