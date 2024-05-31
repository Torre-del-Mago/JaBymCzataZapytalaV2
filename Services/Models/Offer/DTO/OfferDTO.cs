using Models.Hotel.DTO;
using Models.Transport.DTO;

namespace Models.Offer.DTO
{
    public class OfferDTO
    {
        public int HotelId { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public DateOnly BeginDate { get; set; }

        public DateOnly EndDate { get; set; }

        public FlightDTO Flight { get; set; }

        public string TypeOfMeal { get; set; }

        public List<RoomDTO> Rooms { get; set; }

        public int NumberOfAdults { get; set; }
        
        public int NumberOfNewborns { get; set; }
        
        public int NumberOfToddlers { get; set; }

        public int NumberOfTeenagers { get; set; }

    }
}
