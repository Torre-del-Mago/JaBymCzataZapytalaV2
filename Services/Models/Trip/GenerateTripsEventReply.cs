// Wyświetl listę Wycieczek(Lista Wycieczek)
using Models.Trip.DTO;

namespace Models.Trip
{
    public class GenerateTripsEventReply : EventModel
    {
        public TripsDTO Trips { get; set; }
    }
}
