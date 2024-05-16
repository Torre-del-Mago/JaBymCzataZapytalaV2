// Wyświetl Wycieczkę (Wycieczka)
using Models.Trip.DTO;

namespace Models.Gate.Trip.Response
{
    public class GenerateTripResponse : EventModel
    {
        public TripDTO TripDTO { get; set; }
    }
}