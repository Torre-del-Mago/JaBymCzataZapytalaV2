using Models.Admin.DTO;
using Models.Gate.TravelAgency;

namespace Models.Gate
{
    public class GetAdminDataResponse
    {
        public TopHotelsDTO TopHotelsDto { get; set; }
        public TopRoomTypesDTO TopRoomTypesDto { get; set; }
        public TopDepartureDTO TopDepartureDto { get; set; }
        public TopDestinationDTO TopDestinationDto { get; set; }
        public LastTravelAgencyChangesResponse LastTravelAgencyChangesResponse { get; set; }
    }
}