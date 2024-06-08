using Models.Admin.DTO;

namespace Models.Gate
{
    public class GetAdminDataResponse
    {
        public TopHotelsDTO TopHotelsDto { get; set; }
        public TopRoomTypesDTO TopRoomTypesDto { get; set; }
        public TopDepartureDTO TopDepartureDto { get; set; }
        public TopDestinationDTO TopDestinationDto { get; set; }
    }
}