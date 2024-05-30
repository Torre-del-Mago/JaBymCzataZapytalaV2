using Models.Offer.DTO;
using System.Data;
using OfferCommand.Database.Tables;

namespace OfferCommand.Repository.OfferRepository
{
    public interface IOfferRepository
    {
        Offer InsertOffer(OfferDTO dto);
        Offer UpdateStatus(int offerId, string status);

        List<OfferRoom> getOfferRoomsByOfferId(int offerId);
    }
}
