using MongoDB.Bson.Serialization.Attributes;

namespace Login.Database.Entity;

public class User
{
    [BsonElement("login")]
    public string Login { get; set; }
}