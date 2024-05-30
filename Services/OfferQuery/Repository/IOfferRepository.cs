using Models.Offer.DTO;

namespace OfferQuery.Repository
{
    public interface IOfferRepository
    {
        public Task<bool> CreateOrUpdateOfferAsync(OfferSyncDTO offer);

        public Task AddRooms(List<OfferRoomSyncDTO> rooms);
    }
}
