using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Offer.Request;
using Models.Gate.Offer.Response;
using Models.Gate.Trip.Request;
using Models.Gate.Trip.Response;
using Models.Hotel.DTO;
using Models.Offer;
using Models.Transport.DTO;
using Models.Trip;
using Models.Trip.DTO;


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
            [FromQuery] string departure, [FromQuery] DateTime beginDate, [FromQuery] DateTime endDate )
        {
            Console.Out.WriteLine(beginDate);
            Console.Out.WriteLine(endDate);
            List<FlightDTO> flights = new List<FlightDTO>
            {
                new FlightDTO
                {
                    Departure = "Warszawa",
                    DepartureTransportId = 0, // Assuming an empty string in TypeScript translates to 0 in C#
                    PricePerSeat = 100,
                    ReturnTransportId = 0 // Assuming an empty string in TypeScript translates to 0 in C#
                },
                new FlightDTO
                {
                    Departure = "Kraków",
                    DepartureTransportId = 0, // Assuming an empty string in TypeScript translates to 0 in C#
                    PricePerSeat = 80,
                    ReturnTransportId = 0 // Assuming an empty string in TypeScript translates to 0 in C#
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
                    BeginDate = new DateTime(2024, 5, 20),
                    EndDate = new DateTime(2024, 5, 27),
                    ChosenFlight = flights[0],
                    City = "Ateny",
                    Country = "Grecja",
                    Discount = 0,
                    HotelName = "Hilton",
                    PossibleFlights = flights,
                    Rooms = rooms,
                    TypesOfMeals = new List<string> { "all inclusive" }
                },
                new TripDTO
                {
                    BeginDate = new DateTime(2024, 5, 22),
                    EndDate = new DateTime(2024, 5, 29),
                    ChosenFlight = flights[0],
                    City = "Ateny",
                    Country = "Grecja",
                    Discount = 0,
                    HotelName = "Sheraton",
                    PossibleFlights = flights,
                    Rooms = rooms,
                    TypesOfMeals = new List<string> { "all inclusive" }
                }
            };
            return Ok(trips.Where(t =>
                t.Country == destination &&
                t.ChosenFlight.Departure == departure &&
                t.BeginDate >= beginDate &&
                t.EndDate <= endDate
            ).ToList());
        }

        [HttpGet("trip-info")]
        public async Task<IActionResult> getTripInfo([FromQuery] GenerateTripRequest request)
        {
            try
            {
                var clientResponse = await _tripRequestClient.GetResponse<GenerateTripEventReply>(
                    new GenerateTripEvent() { Criteria = request.Criteria });
                var response = new GenerateTripResponse();
                response.TripDTO = clientResponse.Message.TripDTO;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("trip-list-info")]
        public async Task<IActionResult> getTripListInfo([FromQuery] GenerateTripsReqeust request)
        {
            try
            {
                var clientResponse = await _tripRequestClient.GetResponse<GenerateTripsEventReply>(
                    new GenerateTripsEvent() { Criteria = request.Criteria });
                var response = new GenerateTripsResponse();
                response.Trips = clientResponse.Message.Trips;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
