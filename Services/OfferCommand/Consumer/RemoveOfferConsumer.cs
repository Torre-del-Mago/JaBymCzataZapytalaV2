using MassTransit;
using Models.Offer;
using OfferCommand.Repository;
using OfferCommand.Repository.EventRepository;
using OfferCommand.Repository.OfferRepository;

namespace OfferCommand.Consumer
{
    public class RemoveOfferConsumer : IConsumer<RemoveOfferEvent>
    {
        private IOfferRepository _offerRepository;
        private IEventRepository _eventRepository;
        private IPublishEndpoint _publishEndpoint;
        public RemoveOfferConsumer(
            IOfferRepository offerRepository,
            IEventRepository eventRepository,
            IPublishEndpoint publishEndpoint)
        {
            _offerRepository = offerRepository;
            _eventRepository = eventRepository;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<RemoveOfferEvent> context)
        {
            var offer = _offerRepository.UpdateStatus(context.Message.OfferId, EventTypes.Removed);
            if(offer == null)
            {
                return;
            }
            var rooms = _offerRepository.getOfferRoomsByOfferId(context.Message.OfferId);
            _eventRepository.InsertRemovedEvent(context.Message.OfferId);
            await _publishEndpoint.Publish(new RemoveOfferSyncEvent()
            {
                OfferSync = ClassConverter.convert(offer),
                RoomSyncs = ClassConverter.convert(rooms)
            });
        }
    }
}
