using MassTransit;
using MassTransit.Clients;
using Models.Hotel;
using Models.Transport;
using Models.Trip;
using Models.Trip.DTO;

namespace Trip.Consumer
{
    public class TripInfoConsumer : IConsumer<GenerateTripEvent>
    {
        private IRequestClient<GetHotelDataForTripEvent> _hotelClient { get; set; } 
        private IRequestClient<GetTransportDataForTripEvent> _transportClient { get; set; } 
        public TripInfoConsumer(IRequestClient<GetHotelDataForTripEvent> hotelClient,
            IRequestClient<GetTransportDataForTripEvent> transportClient) 
        { 
            _hotelClient = hotelClient;
            _transportClient = transportClient;
        }

        public async Task Consume(ConsumeContext<GenerateTripEvent> context)
        {
            var hotelRequest = new GetHotelDataForTripEvent() { };
            var transportRequest = new GetTransportDataForTripEvent() { };
            var hotelResponse = await _hotelClient.GetResponse<GetHotelDataForTripEventReply>(hotelRequest);
            var transportResponse = await _hotelClient.GetResponse<GetTransportDataForTripEventReply>(transportRequest);

            var hotelDto = hotelResponse.Message.Hotel;
            var transportDto = transportResponse.Message.Transport;

            var tripDto = new TripDTO
            {
                HotelName = hotelDto.HotelName,
                Country = hotelDto.Country,
                City = hotelDto.City,
                BeginDate = hotelDto.BeginDate,
                EndDate = hotelDto.EndDate,
                TypesOfMeals = hotelDto.TypesOfMeals,
                Rooms = hotelDto.Rooms,
                Discount = hotelDto.Discount,
                ChosenFlight = transportDto.ChosenFlight,
                PossibleFlights = transportDto.PossibleFlights
            };

            await context.Publish(new GenerateTripEventReply() {TripDTO = tripDto});
        }
    }
}
