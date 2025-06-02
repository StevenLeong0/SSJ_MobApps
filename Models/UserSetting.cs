using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class UserSetting
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }
    public TextSize TextSize { get; set; }
    public bool DarkMode { get; set; }
    public bool EnableNotifications { get; set; }

}