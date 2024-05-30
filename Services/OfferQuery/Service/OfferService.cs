using Models.Offer.DTO;
using OfferQuery.Repository;

namespace OfferQuery.Service
{
    public class OfferService : IOfferService
    {

        private IOfferRepository _offerRepository;
        public OfferService(IOfferRepository offerRepository) {
            _offerRepository = offerRepository;
        }

        public Task SynchroniseContent(OfferSyncDTO offerDto, List<OfferRoomSyncDTO> roomsDto)
        {
            try {
                if (_offerRepository.CreateOrUpdateOfferAsync(offerDto).Result)
                {
                    _offerRepository.AddRooms(roomsDto);
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return Task.CompletedTask;
        }
    }
}
