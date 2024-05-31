using MongoDB.Bson.Serialization.Attributes;

namespace Login.Database.Entity;

public class User
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("login")]
    public string Login { get; set; }
}