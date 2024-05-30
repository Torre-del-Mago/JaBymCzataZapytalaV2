using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Models.Gate.Offer.Request;
using Models.Gate.Offer.Response;
using Models.Offer;
using Models.Trip;

namespace Gate.Controllers
{
    [ApiController]
    [Route("api/offer/")]
    public class OfferController : ControllerBase
    {
        private IRequestClient<ReserveOfferEvent> _requestClient { get; set; }
        public OfferController(IRequestClient<ReserveOfferEvent> reserveClient) { 
            _requestClient = reserveClient;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] ReserveOfferRequest request)
        {
            try
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromSeconds(10));
                var cancellationToken = cts.Token;
                var clientResponse = await _requestClient.GetResponse<ReserveOfferEventReply>(
                    new ReserveOfferEvent() { Offer = request.Offer }, cancellationToken);
                var response = new ReserveOfferResponse();
                if (clientResponse.Message.Answer == ReserveOfferEventReply.State.RESERVED)
                {
                    response.Answer = ReserveOfferResponse.State.RESERVED;
                }
                else
                {
                    response.Answer = ReserveOfferResponse.State.NOT_RESERVED;
                }
                response.OfferId = clientResponse.Message.OfferId;
                response.Error = clientResponse.Message.Error;
                response.Registration = clientResponse.Message.Registration;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost("test-reserve")]
        public async Task<IActionResult> TestReserve([FromBody] ReserveOfferRequest request)
        {
            try
            {
                Console.WriteLine(request.ToString());
                Console.WriteLine(request.Offer.HotelId);
                foreach(var room in request.Offer.Rooms)
                {
                    Console.WriteLine(room.Count);
                    Console.WriteLine(room.NumberOfPeopleForTheRoom);
                    Console.WriteLine(room.TypeOfRoom);
                }
                Console.WriteLine(request.Offer.BeginDate);
                Console.WriteLine(request.Offer.EndDate);
                Console.WriteLine(request.Offer.Flight.ReturnTransportId);
                Console.WriteLine(request.Offer.Flight.DepartureTransportId);
                Console.WriteLine(request.Offer.Flight.Departure);
                Console.WriteLine(request.Offer.Country);
                Console.WriteLine(request.Offer.City);
                Console.WriteLine(request.Offer.NumberOfAdults);
                Console.WriteLine(request.Offer.NumberOfTeenagers);
                Console.WriteLine(request.Offer.NumberOfNewborns);
                Console.WriteLine(request.Offer.HotelId);
                Console.WriteLine(request.Offer.TypeOfMeal);
                Random rnd = new Random();
                bool hasPaymentNotCompleted = rnd.Next(1, 11) == 1;
                var response = new ReserveOfferResponse();
                if (hasPaymentNotCompleted)
                {
                    response.Answer = ReserveOfferResponse.State.RESERVED;
                    response.Error = "";
                }
                else
                {
                    response.Answer = ReserveOfferResponse.State.NOT_RESERVED;
                    response.Error = "Poludniowa Samba";
                }
                response.OfferId = 1;
                response.Registration = 0;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
