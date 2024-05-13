using MassTransit;
using MassTransit.Clients;

namespace Trip.Consumer
{
    public class TripInfoConsumer : IConsumer<TripInfoEvent>
    {
        private RequestClient<HotelQueryEvent> _hotelClient { get; set; } 
        private RequestClient<TransportQueryEvent> _transportClient { get; set; } 
        public TripInfoConsumer(IRequestClient<HotelQueryEvent> hotelClient,
            IRequestClient<TransportQueryEvent> transportClient
            ) { 
            _hotelClient = hotelClient;
            _transportClient = transportClient;
        }

        public async Task Consume(ConsumeContext<TripInfoEvent> context)
        {

            var hotelRequest = new HotelQueryEvent() { };
            var hotelResponse = _hotelClient.GetResponse<HotelQueryReplyEvent>(hotelRequest);
            var transportRequest = new TransportQueryEvent() { };
            var transportResponse = _hotelClient.GetResponse<TransportQueryReplyEvent>(transportRequest);

            /*
             Do something
             */
            await context.Publish(new TripInfoReplyEvent() { });
        }
    }
}
