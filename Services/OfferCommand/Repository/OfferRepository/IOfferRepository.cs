using Models.Offer.DTO;
using System.Data;
using OfferCommand.Database.Tables;

namespace OfferCommand.Repository.OfferRepository
{
    public interface IOfferRepository
    {
        Offer InsertOffer(OfferDTO dto);
        void UpdateStatus(int offerId, string status);
    }
}
