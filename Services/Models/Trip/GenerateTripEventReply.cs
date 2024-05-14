// Wyświetl Wycieczkę (Wycieczka)
using Models.Trip.DTO;

namespace Models.Trip
{
    public class GenerateTripEventReply : EventModel
    {
        public TripDTO TripDTO { get; set; }
    }
}