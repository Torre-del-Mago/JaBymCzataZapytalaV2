using MassTransit;
using Models.Hotel;
using OfferQuery.Service;

namespace OfferQuery.Consumer
{
    public class ReserveHotelReplyConsumer : IConsumer<ReserveHotelEventReply>
    {
        private readonly IOfferService _service;

        public ReserveHotelReplyConsumer(IOfferService service)
        {
            _service = service;
        }

        public Task Consume(ConsumeContext<ReserveHotelEventReply> context)
        {
            throw new NotImplementedException();
        }
    }
}
