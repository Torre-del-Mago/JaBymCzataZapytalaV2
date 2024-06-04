using MassTransit;
using Models.Offer;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class ReserveOfferSyncConsumer : IConsumer<ReserveOfferSyncEvent>
    {
        private readonly IOfferService _service;

        public ReserveOfferSyncConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveOfferSyncEvent> context)
        {
            Console.Out.WriteLine("Offer Gets Event ReserveOfferSyncEvent");
            return _service.SynchroniseContent(context.Message.OfferSync, context.Message.RoomSyncs);
        }
    }
}
