using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelCommand.Database.Tables;

public class ReservedRoom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public Reservation Reservation { get; set; }

    [Required]
    public int ReservationId { get; set; }

    public HotelRoomType HotelRoomType { get; set; }

    [Required]
    public int HotelRoomTypeId { get; set; }

    [Required]
    public int NumberOfRooms { get; set; }
}