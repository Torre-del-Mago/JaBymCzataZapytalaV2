using Models.Hotel.DTO;
using Models.Transport.DTO;

namespace Models.Trip.DTO
{
    public class TripDTO
    {
        public string HotelName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<string> TypesOfMeals { get; set; }

        public List<RoomDTO> Rooms { get; set; }

        public FlightDTO ChosenFlight { get; set; }

        public List<FlightDTO> PossibleFlights { get; set; }

        public float Discount { get; set; }
    }
}
