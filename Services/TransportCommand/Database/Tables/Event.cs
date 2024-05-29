using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportCommand.Database.Tables
{
    [Table("events")]
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TicketId { get; set; }
        public int TransportId {  get; set; }

        public DateTime Timestamp { get; set; }

        public string EventType { get; set; }
    }
}
