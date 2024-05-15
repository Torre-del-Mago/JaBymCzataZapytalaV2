using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelCommand.Database.Tables
{
    public class HotelRoomType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }

        [Required]
        public int RoomTypeId { get; set; }

        public RoomType RoomType { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }

        [Required]
        public int PricePerNight { get; set; }

        public List<ReservedRoom> ReservedRooms { get; set; }

    }
}
