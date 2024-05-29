using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class Reservation
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("hotelId")]
    public int HotelId { get; set; }
    
    [BsonElement("from")]
    public DateTime  From  { get; set; }
    
    [BsonElement("to")]
    public DateTime  To { get; set; }

    [BsonElement("rooms")] 
    public List<ReservedRoom> Rooms { get; set; }
}