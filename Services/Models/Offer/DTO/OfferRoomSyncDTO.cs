using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Offer.DTO
{
    public class OfferRoomSyncDTO
    {
        public int Id { get; set; }

        public int OfferId { get; set; }

        public string RoomType { get; set; }

        public int NumberOfRooms { get; set; }
    }
}
