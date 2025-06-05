namespace SeniorLearnApi.Interfaces;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
public interface IBulletinModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}