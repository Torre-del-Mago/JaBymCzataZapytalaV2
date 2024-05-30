using Models.Offer.DTO;
using OfferCommand.Database.Tables;

namespace OfferCommand
{
    public class ClassConverter
    {
        public static OfferSyncDTO convert(Offer offer)
        {
            return new OfferSyncDTO()
            {
                ArrivalTransportId = offer.ArrivalTransportId,
                DepartureTransportId = offer.DepartureTransportId,
                NumberOfAdults = offer.NumberOfAdults,
                NumberOfNewborns = offer.NumberOfNewborns,
                NumberOfTeenagers = offer.NumberOfTeenagers,
                NumberOfToddlers = offer.NumberOfToddlers,
                DateFrom = offer.DateFrom,
                DateTo = offer.DateTo,
                HotelId = offer.HotelId,
                OfferStatus = offer.OfferStatus,
                UserLogin = offer.UserLogin,
                Id = offer.Id,
            };
        }

        public static List<OfferRoomSyncDTO> convert(List<OfferRoom> list)
        {
            return list.Select(r => new OfferRoomSyncDTO()
            {
                OfferId = r.OfferId,
                Id = r.Id,
                NumberOfRooms = r.NumberOfRooms,
                RoomType = r.RoomType
            }).ToList();
        }
    }
}
