using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelCommand.Database.Tables
{
    public class HotelDiet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int DietId { get; set; }

        public Diet Diet { get; set; }

        [Required]
        public int HotelId { get; set; }

        public Hotel Hotel { get; set; }
    }
}
