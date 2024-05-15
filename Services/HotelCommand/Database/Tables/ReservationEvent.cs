using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelCommand.Database.Tables
{
    public class ReservationEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; }

    }
}
