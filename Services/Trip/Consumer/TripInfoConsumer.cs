using MassTransit;
using MassTransit.Clients;
using Models.Hotel;
using Models.Hotel.DTO;
using Models.Transport;
using Models.Transport.DTO;
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
            var @event = context.Message;
            var hotelRequest = new GetHotelDataForTripEvent() { Criteria = new CriteriaForHotel() { 
                BeginDate = context.Message.Criteria.BeginDate,
                EndDate = context.Message.Criteria.EndDate,
                NumberOfPeople= context.Message.Criteria.NrOfPeople,
                HotelId = context.Message.Criteria.HotelId,
            }
            };
            var transportRequest = new GetTransportDataForTripEvent() { Criteria = new CriteriaForTransport() {
                BeginDate= context.Message.Criteria.BeginDate,
                EndDate= context.Message.Criteria.EndDate,
                NumberOfPeople=context.Message.Criteria.NrOfPeople,
                Country= context.Message.Criteria.Country,
                Departure= context.Message.Criteria.Departure,
                }
            };
            HotelDTO hotelDto = new HotelDTO();

            var hotelResponse = await _hotelClient.GetResponse<GetHotelDataForTripEventReply, HotelDataForTripNotFoundEvent>(hotelRequest);

            if (hotelResponse.Is(out Response<HotelDataForTripNotFoundEvent> responseA))
            {
                await context.RespondAsync(new TripNotFoundEvent()
                {
                    CorrelationId = @event.CorrelationId,
                });
            }
            else if (hotelResponse.Is(out Response<GetHotelDataForTripEventReply> responseB))
            {
                hotelDto = responseB.Message.Hotel;
            }

            TransportDTO transportDto = new TransportDTO();

            var transportResponse = await _hotelClient.GetResponse<GetTransportDataForTripEventReply, TransportDataForTripNotFoundEvent>(transportRequest);

            if (transportResponse.Is(out Response<TransportDataForTripNotFoundEvent> responseC))
            {
                await context.RespondAsync(new TripNotFoundEvent()
                {
                    CorrelationId = @event.CorrelationId,
                });
            }
            else if (transportResponse.Is(out Response<GetTransportDataForTripEventReply> responseD))
            {
                transportDto = responseD.Message.Transport;
            }

            var tripDto = new TripDTO
            {
                HotelName = hotelDto.HotelName,
                HotelId = hotelDto.HotelId,
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

            await context.RespondAsync(new GenerateTripEventReply() {
                CorrelationId = @event.CorrelationId,
                TripDTO = tripDto});
        }
    }
}
