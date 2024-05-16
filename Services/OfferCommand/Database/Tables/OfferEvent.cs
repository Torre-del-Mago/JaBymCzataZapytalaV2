using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using OfferQuery.Database.Entity;

namespace OfferCommand.Database.Tables
{
    public class OfferEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OfferId { get; set; }

        public Offer Offer { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
