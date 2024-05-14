//Generuj Listę wycieczek (Kryteria użytkownika)
using Models.Trip.DTO;

namespace Models.Trip
{
    public class GenerateTripsEvent : EventModel
    {
        public CriteriaForTrips Criteria { get; set; }
    }
}