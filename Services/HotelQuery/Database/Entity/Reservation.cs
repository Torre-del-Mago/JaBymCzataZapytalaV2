using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class Reservation
{
    [Key]
    public int Id { get; set; }
    
    [BsonElement("hotelId")]
    public Guid HotelId { get; set; }
    
    [BsonElement("from")]
    public DateOnly  From  { get; set; }
    
    [BsonElement("to")]
    public DateOnly  To { get; set; }

    [BsonElement("rooms")] 
    public List<ReservedRoom> Rooms { get; set; }
}