using Models.Trip.DTO;

namespace Models.Gate.Trip.Request
{
    public class GenerateTripRequest : EventModel
    {
        public CriteriaForTrip Criteria { get; set; }
    }
}