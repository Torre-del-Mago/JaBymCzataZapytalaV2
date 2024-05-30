using MassTransit;
using Models.Offer;
using OfferCommand.Repository;
using OfferCommand.Repository.EventRepository;
using OfferCommand.Repository.OfferRepository;

namespace OfferCommand.Consumer
{
    public class PaidOfferConsumer : IConsumer<PaidOfferEvent>
    {
        private IOfferRepository _offerRepository;
        private IEventRepository _eventRepository;
        private IPublishEndpoint _publishEndpoint;
        public PaidOfferConsumer(
            IOfferRepository offerRepository,
            IEventRepository eventRepository,
            IPublishEndpoint publishEndpoint)
        {
            _offerRepository = offerRepository;
            _eventRepository = eventRepository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<PaidOfferEvent> context)
        {
            var offer = _offerRepository.UpdateStatus(context.Message.OfferId, EventTypes.Paid);
            if(offer == null)
            {
                return;
            }
            var rooms = _offerRepository.getOfferRoomsByOfferId(context.Message.OfferId);
            _eventRepository.InsertPaidEvent(context.Message.OfferId);
            await _publishEndpoint.Publish(new PaidOfferSyncEvent()
            {
                OfferSync = ClassConverter.convert(offer),
                RoomSyncs = ClassConverter.convert(rooms)
            });
        }
    }
}
