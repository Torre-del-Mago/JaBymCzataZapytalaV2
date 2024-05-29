﻿using MassTransit;
using Models.Hotel;
using Models.Hotel.DTO;
using Models.Transport;
using Models.Transport.DTO;
using Models.Trip;
using Models.Trip.DTO;

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
            var @event = context.Message;
            var criteria = context.Message.Criteria;
            if (criteria == null)
            {
                context.Message.Criteria = new CriteriaForTrips
                {
                    NrOfPeople = 2,
                    Country = "Grecja",
                    BeginDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(7),
                    Departure = "Gdańsk"
                };
            }
            if ((criteria.BeginDate == null) || (criteria.EndDate == null))
            {
                context.Message.Criteria.BeginDate = DateTime.Today;
                context.Message.Criteria.EndDate = DateTime.Today.AddDays(7);
            }
            if ((criteria.NrOfPeople <= 0) || (criteria.NrOfPeople == null))
            {
                context.Message.Criteria.NrOfPeople = 2;
            }
            if (criteria.Country == null)
            {
                context.Message.Criteria.Country = "Grecja";
            }
            if (criteria.Departure == null)
            {
                context.Message.Criteria.Departure = "Gdańsk";
            }
            
            var hotelRequest = new GetHotelDataForTripsEvent() { };
            var transportRequest = new GetTransportDataForTripsEvent() { };
            var hotelResponse = await _hotelClient.GetResponse<GetHotelDataForTripsEventReply>(hotelRequest);
            var transportResponse = await _hotelClient.GetResponse<GetTransportDataForTripsEventReply>(transportRequest);
            
            var hotelsDto = hotelResponse.Message.Hotels.Hotels;
            var transportsDto = transportResponse.Message.Transports.Transports;
            
            var matchingTrips = new List<Tuple<HotelDTO, TransportDTO>>();
            foreach (var hotelDto in hotelsDto)
            {
                foreach (var transportDto in transportsDto)
                {
                    if (hotelDto.Country == transportDto.Destination && hotelDto.City == transportDto.Destination)
                    {
                        matchingTrips.Add(Tuple.Create(hotelDto, transportDto));
                    }
                }
            }
            
            var random = new Random();
            var selectedTrips = matchingTrips.OrderBy(x => random.Next()).Take(10);

            var trips = new List<TripDTO>();

            foreach (var tripTuple in selectedTrips)
            {
                var hotelDto = tripTuple.Item1;
                var transportDto = tripTuple.Item2;
                
                var trip = new TripDTO
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

                trips.Add(trip);
            }

            var tripsDto = new TripsDTO() {Trips = trips};
            await context.Publish(new GenerateTripsEventReply() {
                Id = @event.Id,
                CorrelationId = @event.CorrelationId,
                Trips = tripsDto
            });
        }
    }
}
