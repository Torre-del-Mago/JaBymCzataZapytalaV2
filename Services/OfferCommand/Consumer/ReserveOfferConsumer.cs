using MassTransit;
using Models.Offer;
using OfferCommand.Database.Tables;
using OfferCommand.Repository;
using OfferCommand.Repository.EventRepository;
using OfferCommand.Repository.OfferRepository;

namespace OfferCommand.Consumer
{
    public class ReserveOfferConsumer : IConsumer<ReserveOfferEvent>
    {
        private IRequestClient<CreatedOfferEvent> _requestClient;
        private IOfferRepository _offerRepository;
        private IEventRepository _eventRepository;
        private IPublishEndpoint _publishEndpoint;
        public ReserveOfferConsumer(IRequestClient<CreatedOfferEvent> requestClient,
            IOfferRepository offerRepository,
            IEventRepository eventRepository,
            IPublishEndpoint publishEndpoint)
        {
            _requestClient = requestClient;
            _offerRepository = offerRepository;
            _eventRepository = eventRepository;
            _publishEndpoint = publishEndpoint; 
        }
        public async Task Consume(ConsumeContext<ReserveOfferEvent> context)
        {
            Offer offer = _offerRepository.InsertOffer(context.Message.Offer);
            _eventRepository.InsertCreatedEvent(offer.Id);
            var rooms = _offerRepository.getOfferRoomsByOfferId(offer.Id);
            await _publishEndpoint.Publish(new ReserveOfferSyncEvent()
            {
                Offer = context.Message.Offer,
                OfferSync = ClassConverter.convert(offer),
                RoomSyncs = ClassConverter.convert(rooms)
            });

            var response = await _requestClient.GetResponse<CreatedOfferEventReply>(new CreatedOfferEvent()
            {
                Offer = context.Message.Offer,
                OfferId = offer.Id,
            });

            if(response.Message.Answer == CreatedOfferEventReply.State.NOT_RESERVED)
            {
                _eventRepository.InsertNotReservedEvent(offer.Id);
                _offerRepository.UpdateStatus(offer.Id, EventTypes.NotReserved);
                await _publishEndpoint.Publish(new ReservedOfferSyncEvent()
                {
                    OfferId = offer.Id,
                    Answer = ReservedOfferSyncEvent.State.NOT_RESERVED,
                    OfferSync = ClassConverter.convert(offer),
                    RoomSyncs = ClassConverter.convert(rooms)
                });
                await context.RespondAsync(new ReserveOfferEventReply()
                {
                    Answer = ReserveOfferEventReply.State.NOT_RESERVED,
                    CorrelationId = context.Message.CorrelationId,
                    Error = response.Message.Error,
                });
                return;
            }

            _eventRepository.InsertReservedEvent(offer.Id);
            _offerRepository.UpdateStatus(offer.Id, EventTypes.Reserved);
            await _publishEndpoint.Publish(new ReservedOfferSyncEvent()
            {
                OfferId = offer.Id,
                Answer = ReservedOfferSyncEvent.State.RESERVED,
                OfferSync = ClassConverter.convert(offer),
                RoomSyncs = ClassConverter.convert(rooms)
            });
            await context.RespondAsync(new ReserveOfferEventReply()
            {
                Answer = ReserveOfferEventReply.State.RESERVED,
                CorrelationId = context.Message.CorrelationId,
            });
        }
    }
}
