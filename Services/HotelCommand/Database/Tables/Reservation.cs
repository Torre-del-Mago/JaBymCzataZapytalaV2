using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelCommand.Database.Tables;

public class Reservation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateOnly  From  { get; set; }

    [Required]
    public DateOnly  To { get; set; }
    
    public List<ReservedRoom> Rooms { get; set; }

    [Required]
    public int HotelId { get; set; }

    public Hotel Hotel { get; set; }
    
    public int OfferId { get; set; }

}