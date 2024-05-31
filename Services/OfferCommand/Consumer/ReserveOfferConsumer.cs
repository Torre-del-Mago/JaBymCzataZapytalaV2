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
            Console.Out.WriteLine($"Started creating offer for hotel {context.Message.Offer.HotelId}");
            Offer offer = _offerRepository.InsertOffer(context.Message.Offer);
            Console.Out.WriteLine($"Created offer with id {offer.Id}");
            _eventRepository.InsertCreatedEvent(offer.Id);
            var rooms = _offerRepository.getOfferRoomsByOfferId(offer.Id);
            await _publishEndpoint.Publish(new ReserveOfferSyncEvent()
            {
                OfferSync = ClassConverter.convert(offer),
                RoomSyncs = ClassConverter.convert(rooms)
            });

            var response = await _requestClient.GetResponse<CreatedOfferEventReply>(new CreatedOfferEvent()
            {
                Offer = context.Message.Offer,
                OfferId = offer.Id,
                CorrelationId = context.Message.CorrelationId
            });
            Console.Out.WriteLine($"Got reserving response for offer with id {offer.Id}");

            if (response.Message.Answer == CreatedOfferEventReply.State.NOT_RESERVED)
            {
                Console.Out.WriteLine($"offer with id {offer.Id} was not reserved");
                _eventRepository.InsertNotReservedEvent(offer.Id);
                _offerRepository.UpdateStatus(offer.Id, EventTypes.NotReserved);
                await _publishEndpoint.Publish(new ReservedOfferSyncEvent()
                {
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

            Console.Out.WriteLine($"offer with id {offer.Id} was reserved");
            _eventRepository.InsertReservedEvent(offer.Id);
            _offerRepository.UpdateStatus(offer.Id, EventTypes.Reserved);
            await _publishEndpoint.Publish(new ReservedOfferSyncEvent()
            {
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
