using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Offer
{
    public class CreatedOfferEventReply: EventModel
    {
        public enum State
        {
            RESERVED,
            NOT_RESERVED
        };

        public State Answer { get; set; }

        public string Error { get; set; }
    }
}
