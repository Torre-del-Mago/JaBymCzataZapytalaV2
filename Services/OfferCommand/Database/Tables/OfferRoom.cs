using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfferCommand.Database.Tables;

public class OfferRoom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int OfferId { get; set; }

    [Required]
    public string RoomType { get; set; }

    [Required]
    public int NumberOfRooms { get; set; }
}