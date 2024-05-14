using MassTransit;
using Models.Hotel;
using Models.Transport;
using Models.Trip;

namespace Trip.Consumer
{
    public class TripListInfoConsumer : IConsumer<GenerateTripsEvent>
    {
        private IRequestClient<GetHotelDataForTripsEvent> _hotelClient { get; set; }
        IRequestClient<GetTransportDataForTripsEvent> _transportClient { get; set; }

        public TripListInfoConsumer(IRequestClient<GetHotelDataForTripsEvent> hotelClient,
            IRequestClient<GetTransportDataForTripsEvent> transportClient)
        {
            _hotelClient = hotelClient;
            _transportClient = transportClient;
        }

        public async Task Consume(ConsumeContext<GenerateTripsEvent> context)
        {
            var hotelRequest = new GetHotelDataForTripsEvent() { };
            var transportRequest = new GetTransportDataForTripsEvent() { };
            var hotelResponse = await _hotelClient.GetResponse<GetHotelDataForTripsEventReply>(hotelRequest);
            var transportResponse = await _hotelClient.GetResponse<GetTransportDataForTripsEventReply>(transportRequest);
            /*
             Do something
             */
            await context.Publish(new GenerateTripsEventReply() { });
        }
    }
}
