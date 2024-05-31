using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Offer.Request;
using Models.Gate.Offer.Response;
using Models.Gate.Trip.Request;
using Models.Gate.Trip.Response;
using Models.Hotel.DTO;
using Models.Hotel;
using Models.Offer;
using Models.Transport.DTO;
using Models.Trip;
using Models.Trip.DTO;
using System.Globalization;


namespace Gate.Controllers
{
    [ApiController]
    [Route("api/trip/")]
    public class TripController : ControllerBase
    {
        private IRequestClient<GenerateTripEvent> _tripRequestClient { get; set; }
        private IRequestClient<GenerateTripsEvent> _tripListRequestClient { get; set; }
        public TripController(IRequestClient<GenerateTripEvent> tripRequestClient,
            IRequestClient<GenerateTripsEvent> tripListRequestClient) { 
            _tripRequestClient = tripRequestClient;
            _tripListRequestClient = tripListRequestClient;
        }

        [HttpGet("test-get")]
        public async Task<IActionResult> getTestInfo([FromQuery] string destination, [FromQuery] int numberOfPeople,
            [FromQuery] string departure, [FromQuery] DateOnly beginDate, [FromQuery] DateOnly endDate )
        {
            Console.Out.WriteLine(beginDate);
            Console.Out.WriteLine(endDate);
            List<FlightDTO> flights = new List<FlightDTO>
            {
                new FlightDTO
                {
                    Departure = "Warszawa",
                    DepartureTransportId = 1, // Assuming an empty string in TypeScript translates to 0 in C#
                    PricePerSeat = 100,
                    ReturnTransportId = 2 // Assuming an empty string in TypeScript translates to 0 in C#
                },
                new FlightDTO
                {
                    Departure = "Kraków",
                    DepartureTransportId = 3, // Assuming an empty string in TypeScript translates to 0 in C#
                    PricePerSeat = 80,
                    ReturnTransportId = 4 // Assuming an empty string in TypeScript translates to 0 in C#
                }
            };
            List<RoomDTO> rooms = new List<RoomDTO>
            {
                new RoomDTO
                {
                    Count = 2,
                    NumberOfPeopleForTheRoom = 2,
                    PricePerRoom = 200,
                    TypeOfRoom = "Cowabunga"
                },
                new RoomDTO
                {
                    Count = 2,
                    NumberOfPeopleForTheRoom = 1,
                    PricePerRoom = 500,
                    TypeOfRoom = "Funky Monkey"
                },
                new RoomDTO
                {
                    Count = 2,
                    NumberOfPeopleForTheRoom = 1,
                    PricePerRoom = 300,
                    TypeOfRoom = "Chilled dog"
                }
            };
            List<TripDTO> trips = new List<TripDTO>
            {
                new TripDTO
                {
                    BeginDate = new DateOnly(2024, 5, 20),
                    EndDate = new DateOnly(2024, 5, 27),
                    ChosenFlight = flights[0],
                    City = "Ateny",
                    Country = "Grecja",
                    Discount = 0,
                    HotelName = "Hilton",
                    PossibleFlights = flights,
                    Rooms = rooms,
                    TypesOfMeals = new List<string> { "all inclusive" },
                    HotelId = 1
                },
                new TripDTO
                {
                    BeginDate = new DateOnly(2024, 5, 22),
                    EndDate = new DateOnly(2024, 5, 29),
                    ChosenFlight = flights[0],
                    City = "Ateny",
                    Country = "Grecja",
                    Discount = 0,
                    HotelName = "Sheraton",
                    PossibleFlights = flights,
                    Rooms = rooms,
                    TypesOfMeals = new List<string> { "all inclusive" },
                    HotelId = 2
                }
            };
            var seltrips = trips.Where(t =>
                t.Country == destination &&
                t.ChosenFlight.Departure == departure &&
                t.BeginDate >= beginDate &&
                t.EndDate <= endDate
            ).ToList();
            var response = new GenerateTripsResponse()
            {
                Trips = new TripsDTO() {
                    Trips = seltrips }
            };
            return Ok(response);
        }

        [HttpGet("test-single-get")]
        public async Task<IActionResult> getTestInfo([FromQuery] string destination, [FromQuery] int numberOfPeople,
            [FromQuery] string departure, [FromQuery] DateOnly beginDate, [FromQuery] DateOnly endDate, [FromQuery] int hotelId )
        {
            Console.Out.WriteLine(beginDate);
            Console.Out.WriteLine(endDate);
            List<FlightDTO> flights = new List<FlightDTO>
            {
                new FlightDTO
                {
                    Departure = "Warszawa",
                    DepartureTransportId = 1, // Assuming an empty string in TypeScript translates to 0 in C#
                    PricePerSeat = 100,
                    ReturnTransportId = 2 // Assuming an empty string in TypeScript translates to 0 in C#
                },
                new FlightDTO
                {
                    Departure = "Kraków",
                    DepartureTransportId = 3, // Assuming an empty string in TypeScript translates to 0 in C#
                    PricePerSeat = 80,
                    ReturnTransportId = 4 // Assuming an empty string in TypeScript translates to 0 in C#
                }
            };
            List<RoomDTO> rooms = new List<RoomDTO>
            {
                new RoomDTO
                {
                    Count = 2,
                    NumberOfPeopleForTheRoom = 2,
                    PricePerRoom = 200,
                    TypeOfRoom = "Cowabunga"
                },
                new RoomDTO
                {
                    Count = 2,
                    NumberOfPeopleForTheRoom = 1,
                    PricePerRoom = 500,
                    TypeOfRoom = "Funky Monkey"
                },
                new RoomDTO
                {
                    Count = 2,
                    NumberOfPeopleForTheRoom = 1,
                    PricePerRoom = 300,
                    TypeOfRoom = "Chilled dog"
                }
            };
            List<TripDTO> trips = new List<TripDTO>
            {
                new TripDTO
                {
                    BeginDate = new DateOnly(2024, 5, 20),
                    EndDate = new DateOnly(2024, 5, 27),
                    ChosenFlight = flights[0],
                    City = "Ateny",
                    Country = "Grecja",
                    Discount = 0,
                    HotelName = "Hilton",
                    PossibleFlights = flights,
                    Rooms = rooms,
                    TypesOfMeals = new List<string> { "all inclusive" },
                    HotelId = 1
                },
                new TripDTO
                {
                    BeginDate = new DateOnly(2024, 5, 22),
                    EndDate = new DateOnly(2024, 5, 29),
                    ChosenFlight = flights[0],
                    City = "Ateny",
                    Country = "Grecja",
                    Discount = 0,
                    HotelName = "Sheraton",
                    PossibleFlights = flights,
                    Rooms = rooms,
                    TypesOfMeals = new List<string> { "all inclusive" },
                    HotelId = 2
                }
            };
            var trip = trips.Where(t =>
                t.Country == destination &&
                t.ChosenFlight.Departure == departure &&
                t.BeginDate >= beginDate &&
                t.EndDate <= endDate &&
                t.HotelId == hotelId
            ).FirstOrDefault();
            var response = new GenerateTripResponse()
            {
                TripDTO = trip
            };
            return Ok(response);
        }

        [HttpGet("trip-info")]
        public async Task<IActionResult> getTripInfo([FromQuery] string destination, [FromQuery] string country, [FromQuery] int numberOfPeople,
            [FromQuery] string departure, [FromQuery] DateOnly beginDate, [FromQuery] DateOnly endDate, [FromQuery] int hotelId)
        {
            try
            {
                var clientResponse = await _tripRequestClient.GetResponse<GenerateTripEventReply, TripNotFoundEvent>(
                    new GenerateTripEvent()
                    {
                        Criteria = new CriteriaForTrip()
                        {
                            BeginDate = beginDate,
                            EndDate = endDate,
                            Departure = departure,
                            HotelId = hotelId,
                            Country = country,
                            NrOfPeople = numberOfPeople,
                            Destination = destination
                        }
                    });
                
                if (clientResponse.Is(out Response<TripNotFoundEvent> responseA))
                {
                    return BadRequest();
                }
                else if (clientResponse.Is(out Response<GenerateTripEventReply> responseB))
                {
                    var response = new GenerateTripResponse();
                    response.TripDTO = responseB.Message.TripDTO;
                    return Ok(response);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("trip-list-info")]
        public async Task<IActionResult> getTripListInfo([FromQuery] string destination, [FromQuery] int numberOfPeople,
            [FromQuery] string departure, [FromQuery] DateOnly beginDate, [FromQuery] DateOnly endDate)
        {
            try
            {
                var clientResponse = await _tripListRequestClient.GetResponse<GenerateTripsEventReply, TripsNotFoundEvent>(
                    new GenerateTripsEvent()
                    {
                        Criteria = new CriteriaForTrips()
                        {
                            BeginDate = beginDate,
                            EndDate = endDate,
                            Departure = departure,
                            NrOfPeople = numberOfPeople,
                            Country = destination
                        }
                    });
                if (clientResponse.Is(out Response<TripsNotFoundEvent> responseA))
                {
                    return BadRequest();
                }
                else if (clientResponse.Is(out Response<GenerateTripsEventReply> responseB))
                {
                    var response = new GenerateTripsResponse();
                    response.Trips = responseB.Message.Trips;
                    return Ok(response);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
