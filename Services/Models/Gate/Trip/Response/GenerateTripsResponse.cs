using Models.Trip.DTO;

namespace Models.Gate.Trip.Response
{
    public class GenerateTripsResponse : EventModel
    {
        public TripsDTO Trips { get; set; }
    }
}
