using Models.Offer.DTO;
using OfferCommand.Database;
using OfferQuery.Database.Entity;

namespace OfferCommand.Repository.OfferRepository
{
    public class OfferRepository : IOfferRepository
    {
        private OfferContext _context;
        public OfferRepository(OfferContext context) {
            _context = context;
        }
        public Offer insertOffer(OfferDTO dto)
        {
            Offer offer = new Offer()
            {
                HotelId = dto.HotelId,
                DateFrom = dto.BeginDate,
                DateTo = dto.EndDate,
                NumberOfAdults = dto.NumberOfAdults,
                NumberOfNewborns = dto.NumberOfNewborns,
                NumberOfTeenagers = dto.NumberOfTeenagers,
                NumberOfToddlers = dto.NumberOfToddlers,
                DepartureTransportId = dto.Flight.DepartureTransportId,
                ArrivalTransportId = dto.Flight.ReturnTransportId,
                OfferStatus = EventTypes.Created
            };
            _context.Offers.Add(offer);
            List<OfferRoom> rooms = new List<OfferRoom>();
            foreach (var room in dto.Rooms)
            {
                rooms.Add(new OfferRoom()
                {
                    RoomType = room.TypeOfRoom,
                    NumberOfRooms = room.Count,
                    OfferId = offer.Id
                });
            }
            _context.Rooms.AddRange(rooms);

            return offer;
        }

        public void UpdateStatus(int offerId, string status)
        {
            Offer offer = _context.Offers.FirstOrDefault(o => o.Id == offerId);
            if (offer == null)
            {
                return;
            }
            offer.OfferStatus = status;
            _context.SaveChanges();
        }
    }
}
