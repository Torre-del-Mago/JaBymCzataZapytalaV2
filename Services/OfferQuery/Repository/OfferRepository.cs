using Models.Offer.DTO;
using MongoDB.Driver;
using OfferQuery.Database.Entity;

namespace OfferQuery.Repository
{
    public class OfferRepository : IOfferRepository
    {
        const string ConnectionString = "mongodb://root:example@mongo:27017/";
        private MongoClient Client { get; set; }
        private IMongoDatabase Database { get; set; }

        public OfferRepository()
        {
            Client = new MongoClient(ConnectionString);
            Database = Client.GetDatabase("offer_query");
        }

        public async Task AddRooms(List<OfferRoomSyncDTO> rooms)
        {
            var collection = Database.GetCollection<OfferRoom>("offer_rooms");

            var offerRooms = new List<OfferRoom>();
            foreach (var room in rooms)
            {
                var offerRoom = new OfferRoom
                {
                    Id = room.Id,
                    OfferId = room.OfferId,
                    RoomType = room.RoomType,
                    NumberOfRooms = room.NumberOfRooms
                };
                offerRooms.Add(offerRoom);
            }

            await collection.InsertManyAsync(offerRooms);
        }

        // return true if created new offer
        public async Task<bool> CreateOrUpdateOfferAsync(OfferSyncDTO offer) 
        {
            using (var session = await Client.StartSessionAsync())
            {
                try
                {
                    var collection = Database.GetCollection<Offer>("offers");

                    var filter = Builders<Offer>.Filter.Eq(o => o.Id, offer.Id);
                    var oldOffer = await collection.Find(filter).FirstOrDefaultAsync();

                    Offer newOffer = new Offer()
                    {
                        ArrivalTransportId = offer.ArrivalTransportId,
                        DepartureTransportId = offer.DepartureTransportId,
                        DateFrom = offer.DateFrom,
                        DateTo = offer.DateTo,
                        HotelId = offer.HotelId,
                        NumberOfAdults = offer.NumberOfAdults,
                        NumberOfNewborns = offer.NumberOfNewborns,
                        NumberOfTeenagers = offer.NumberOfTeenagers,
                        NumberOfToddlers = offer.NumberOfToddlers,
                        OfferStatus = offer.OfferStatus,
                        UserLogin = offer.UserLogin,
                        Id = offer.Id,
                    };


                    if (oldOffer == null)
                    {
                        await collection.InsertOneAsync(session, newOffer);

                        return true;
                    }
                    else {
                        string nSt = newOffer.OfferStatus;
                        string oSt = oldOffer.OfferStatus;
                        if (oSt == "CREATED")
                        {
                            await collection.ReplaceOneAsync(session, filter, newOffer);
                        }
                        else if (oSt == "RESERVED" && nSt == "PAID")
                        {
                            await collection.ReplaceOneAsync(session, filter, newOffer);
                        }
                        else if (nSt == "REMOVED")
                        {
                            await collection.ReplaceOneAsync(session, filter, newOffer);
                        }
                        else
                        {
                            throw new Exception("Unknown status of Offer");
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error with synchronise Offer");
                }
            }
            return false;
        }
    }
}
