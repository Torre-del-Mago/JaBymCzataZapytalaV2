using Models.TravelAgency.DTO;

namespace Models.TravelAgency;

public class GetLastTravelAgencyChangesEventReply : EventModel
{
    public LastTravelAgencyChangesDTO LastTravelAgencyChangesDto { get; set; }
}