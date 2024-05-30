using Models.Offer.DTO;
using OfferQuery.Database.Entity;
using System.Data;

namespace OfferCommand.Repository.OfferRepository
{
    public interface IOfferRepository
    {
        public Offer insertOffer(OfferDTO dto);

        public void UpdateStatus(int offerId, string status);
    }
}
