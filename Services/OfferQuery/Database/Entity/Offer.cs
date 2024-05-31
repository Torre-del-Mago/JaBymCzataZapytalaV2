using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfferQuery.Database.Entity;

public class Offer
{
    [BsonId]
    public int Id { get; set; }

    [BsonElement("departureTransportId")]
    public int DepartureTransportId { get; set; }

    [BsonElement("arrivalTransportId")]
    public int ArrivalTransportId { get; set; }

    [BsonElement("dateFrom")]
    public DateOnly DateFrom { get; set; }

    [BsonElement("dateTo")]
    public DateOnly DateTo { get; set; }

    [BsonElement("hotelId")]
    public int HotelId { get; set; }

    [BsonElement("numberOfAdults")]
    public int NumberOfAdults { get; set; }

    [BsonElement("numberOfNewborns")]
    public int NumberOfNewborns { get; set; }

    [BsonElement("numberOfToddlers")]
    public int NumberOfToddlers { get; set; }

    [BsonElement("numberOfTeenagers")]
    public int NumberOfTeenagers { get; set; }

    [BsonElement("userLogin")]
    public string UserLogin { get; set; }

    [BsonElement("offerStatus")]
    public string OfferStatus { get; set; }
}