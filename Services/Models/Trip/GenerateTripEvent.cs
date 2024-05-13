// GenerujWycieczkę(KryteriaWygenerowania)
using Models.Trip.DTO;

namespace Models.Trip
{
    public class GenerateTripEvent : EventModel
    {
        public CriteriaForTrip Criteria { get; set; }
    }
}