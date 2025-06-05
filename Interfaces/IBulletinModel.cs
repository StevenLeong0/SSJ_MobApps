using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SeniorLearnApi.Interfaces;

public interface IBulletinModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}