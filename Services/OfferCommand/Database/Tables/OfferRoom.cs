using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfferQuery.Database.Entity;

public class OfferRoom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    
    public string OfferId { get; set; }
    
    public string RoomType { get; set; }
    
    public int NumberOfRooms { get; set; }
}