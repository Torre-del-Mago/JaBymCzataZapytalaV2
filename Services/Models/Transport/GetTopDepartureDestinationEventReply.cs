using Models.Admin.DTO;

namespace Models.Transport
{
    public class GetTopDepartureDestinationEventReply : EventModel
    {
        public TopDepartureDTO TopDepartureDto { get; set; }
        public TopDestinationDTO TopDestinationDto { get; set; }
    }
}