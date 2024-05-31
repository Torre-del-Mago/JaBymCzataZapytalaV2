using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Offer.DTO
{
    public class OfferSyncDTO
    {
        public int Id { get; set; }

        public int DepartureTransportId { get; set; }

        public int ArrivalTransportId { get; set; }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }

        public int HotelId { get; set; }

        public int NumberOfAdults { get; set; }

        public int NumberOfNewborns { get; set; }

        public int NumberOfToddlers { get; set; }

        public int NumberOfTeenagers { get; set; }

        public string UserLogin { get; set; }

        public string OfferStatus { get; set; }
    }
}
