using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelQuery.Database.Tables;

public class Hotel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    public float Discount { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    
    public List<Diet> Diets { get; set; }
    public List<RoomType> RoomTypes { get; set; }
}