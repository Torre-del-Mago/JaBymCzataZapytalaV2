//Generuj Listę wycieczek (Kryteria użytkownika)
using Models.Trip.DTO;

namespace Models.Gate.Trip.Request
{
    public class GenerateTripsReqeust
    {
        public CriteriaForTrips Criteria { get; set; }
    }
}