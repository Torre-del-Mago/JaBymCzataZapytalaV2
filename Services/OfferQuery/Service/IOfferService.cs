using Models.Offer.DTO;

namespace OfferQuery.Service
{
    public interface IOfferService
    {
        public Task SynchroniseContent(OfferSyncDTO offerDto, List<OfferRoomSyncDTO> roomsDto);
    }
}
