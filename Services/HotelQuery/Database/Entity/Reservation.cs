﻿using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace HotelQuery.Database.Entity;

public class Reservation
{
    [BsonId]
    public int Id { get; set; }
    
    [BsonElement("hotelId")]
    public int HotelId { get; set; }
    
    [BsonElement("from")]
    public DateOnly  From  { get; set; }
    
    [BsonElement("to")]
    public DateOnly  To { get; set; }

    [BsonElement("rooms")] 
    public List<ReservedRoom> Rooms { get; set; }
    
    [BsonElement("offerId")]
    public int OfferId { get; set; }
    
    [BsonElement("reservedAt")]
    public DateTime ReservedAt { get; set; }
}