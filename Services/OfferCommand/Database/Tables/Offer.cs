using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfferCommand.Database.Tables;

public class Offer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int DepartureTransportId { get; set; }
    
    public int ArrivalTransportId { get; set; }
    
    public DateOnly DateFrom { get; set; }
    
    public DateOnly DateTo { get; set; }
    
    public int HotelId { get; set; }
    
    public int NumberOfAdults { get; set; }
    
    public int NumberOfNewborns { get; set; }
    
    public int NumberOfToddlers { get; set; }
    
    public int NumberOfTeenagers { get; set; }
    
    public string UserLogin { get; set; }
    
    public string OfferStatus { get; set; }
}