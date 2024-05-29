using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Transport
{
    public class CancelReservationTransportEvent :EventModel
    {
        public int OfferId { get; set; }

        public int ArrivalTicketId {  get; set; }
        public int ReturnTicketId { get; set; }
    }
}
