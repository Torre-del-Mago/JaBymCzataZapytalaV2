using HotelQuery.Database.Entity;
using Models.Admin.DTO;
using Models.TravelAgency;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace HotelQuery.Repository.Hotel;

public class HotelRepository : IHotelRepository
{
    const string ConnectionString = "mongodb://root:example@mongo:27017/";
    private MongoClient Client { get; set; }
    private IMongoDatabase Database { get; set; }
    
    public HotelRepository()
    {
        Client = new MongoClient(ConnectionString);
        Database = Client.GetDatabase("hotel_query");
    }
    
    public List<Database.Entity.Hotel> GetHotels()
    {
        var connectionCollection = Database.GetCollection<Database.Entity.Hotel>("hotels").AsQueryable();
        var result = connectionCollection.ToList();
        return result;
    }

    public Database.Entity.Hotel GetHotel(int hotelId)
    {
        var hotelCollection = Database.GetCollection<Database.Entity.Hotel>("hotels");
        var hotel = hotelCollection.Find(h => h.Id == hotelId).FirstOrDefault();
        return hotel;
    }

    public List<RoomType> GetRoomTypes()
    {
        var roomTypeCollection = Database.GetCollection<RoomType>("room_types").AsQueryable();
        var result = roomTypeCollection.ToList();
        return result;
    }

    public RoomType GetRoomType(int roomTypeId)
    {
        var roomTypeCollection = Database.GetCollection<RoomType>("room_types");
        var roomType = roomTypeCollection.Find(rt => rt.Id == roomTypeId).FirstOrDefault();
        return roomType;
    }

    public List<Diet> GetDiets()
    {
        var dietCollection = Database.GetCollection<Diet>("diets").AsQueryable();
        var result = dietCollection.ToList();
        return result;
    }

    public Diet GetDiet(int dietId)
    {
        var dietCollection = Database.GetCollection<Diet>("diets");
        var diet = dietCollection.Find(d => d.Id == dietId).FirstOrDefault();
        return diet;
    }

    public List<Database.Entity.Reservation> GetReservations()
    {
        var reservationCollection = Database.GetCollection<Database.Entity.Reservation>("reservations").AsQueryable();
        var result = reservationCollection.ToList();
        return result;
    }

    public Database.Entity.Reservation GetReservation(int reservationId)
    {
        var reservationCollection = Database.GetCollection<Database.Entity.Reservation>("reservations");
        var reservation = reservationCollection.Find(r => r.Id == reservationId).FirstOrDefault();
        return reservation;
    }

    public List<Database.Entity.Reservation> GetReservationWithin(DateOnly from, DateOnly to)
    {
        var reservationCollection = Database.GetCollection<Database.Entity.Reservation>("reservations");
        var reservations = reservationCollection.Find(r => r.From >= from && r.To <= to).ToList();
        return reservations;
    }

    public List<ReservedRoom> GetReservedRooms()
    {
        var reservedRoomCollection = Database.GetCollection<ReservedRoom>("reserved_rooms").AsQueryable();
        var result = reservedRoomCollection.ToList();
        return result;
    }

    public ReservedRoom GetReservedRoom(int reservedRoomId)
    {
        var reservedRoomCollection = Database.GetCollection<ReservedRoom>("reserved_rooms");
        var reservedRoom = reservedRoomCollection.Find(rr => rr.Id == reservedRoomId).FirstOrDefault();
        return reservedRoom;
    }

    public List<ReservedRoom> GetRoomsForReservation(int reservationId)
    {
        var reservedRoomCollection = Database.GetCollection<ReservedRoom>("reserved_rooms");
        var reservedRooms = reservedRoomCollection.Find(rr => rr.ReservationId == reservationId).ToList();
        return reservedRooms;
    }

    public List<HotelRoomType> GetHotelRoomTypes()
    {
        var hotelRoomTypeCollection = Database.GetCollection<HotelRoomType>("hotel_room_types").AsQueryable();
        var result = hotelRoomTypeCollection.ToList();
        return result;
    }

    public HotelRoomType GetHotelRoomType(int hotelRoomTypeId)
    {
        var hotelRoomTypeCollection = Database.GetCollection<HotelRoomType>("hotel_room_types");
        var hotelRoomType = hotelRoomTypeCollection.Find(rr => rr.Id == hotelRoomTypeId).FirstOrDefault();
        return hotelRoomType;
    }

    public List<EntryDTO> GetTopHotels(int numberOfElements)
    {
        var reservetionsCollection = Database.GetCollection<Database.Entity.Reservation>("reservations").AsQueryable();
        var hotelCollection = Database.GetCollection<Database.Entity.Hotel>("hotels").AsQueryable();
        var statusesCollection = Database.GetCollection<ReservationStatus>("reservation_statuses");
        var topHotels = from reservation in reservetionsCollection
            join hotel in hotelCollection on reservation.HotelId equals hotel.Id 
            join status in statusesCollection on reservation.OfferId equals status.OfferId
            where status.reservationStatus == "RESERVED"
            group hotel by hotel.Name into grp
            orderby grp.Count() descending
            select new { key = grp.Key, cnt = grp.Count() };
        return topHotels.Take(numberOfElements)
            .Select(res => new EntryDTO()
            {
                Name = res.key,
                NumberOfElements = res.cnt
            })
            .ToList();
    }

    public List<EntryDTO> GetTopRoomTypes(int numberOfElements)
    {
        var reservetionsCollection = Database.GetCollection<Database.Entity.Reservation>("reservations").AsQueryable();
        var reservedHotelsId = reservetionsCollection.Select(r => r.HotelId).ToList();
        var hotelCollection = Database.GetCollection<Database.Entity.Hotel>("hotels").AsQueryable().Where(h => reservedHotelsId.Contains(h.Id)).ToList();
        var roomTypeCollection = Database.GetCollection<Database.Entity.RoomType>("room_types").AsQueryable().ToList();
        var roomTypeMap = roomTypeCollection.ToDictionary(room => room.Id, room => room.Name);
        var hotelRoomTypeRoomTypeMap = hotelCollection.SelectMany(h => h.Rooms)
            .ToDictionary(room => room.Id, room => roomTypeMap[room.RoomTypeId]);
        return reservetionsCollection.SelectMany(r => r.Rooms).ToList().Select(room => hotelRoomTypeRoomTypeMap[room.HotelRoomTypesId])
            .GroupBy(roomTypeId => roomTypeId).Select(group => new EntryDTO {Name = group.Key, NumberOfElements = group.Count() })
            .OrderByDescending(entry => entry.NumberOfElements).Take(numberOfElements).ToList();
    }
    
    public void AddWatcher(int hotelId)
    {
        var hotelWatcher = GetHotelWatcherIfExists(hotelId);
        if (hotelWatcher != null)
        {
            hotelWatcher.Count++;
            Database.GetCollection<HotelWatchers>("hotel_watchers")
                .FindOneAndReplace(hw => hw.HotelId == hotelId, hotelWatcher);
        }
        else
        {
            Database.GetCollection<HotelWatchers>("hotel_watchers").InsertOne(new HotelWatchers()
            {
                Id = hotelId,
                HotelId = hotelId,
                Count = 1
            });
        }
    }

    public void RemoveWatcher(int hotelId)
    {
        var hotelWatcher = GetHotelWatcherIfExists(hotelId);
        if (hotelWatcher != null)
        {
            hotelWatcher.Count--;
            if (hotelWatcher.Count > 0)
            {
                Database.GetCollection<HotelWatchers>("hotel_watchers")
                    .FindOneAndReplace(hw => hw.HotelId == hotelId, hotelWatcher);
            }
            else
            {
                Database.GetCollection<HotelWatchers>("hotel_watchers")
                    .DeleteOne(hw => hw.HotelId == hotelId);
            }
        }
    }

    public int NumberOfCurrentWatchers(int hotelId)
    {
        var hotelWatcher = GetHotelWatcherIfExists(hotelId);
        if (hotelWatcher != null)
        {
            return hotelWatcher.Count;
        }
        else
        {
            return 0;
        }
    }

    public void AddDietToHotel(int hotelId, int dietId)
    {
        var hotel = GetHotel(hotelId);
        var diet = GetDiet(dietId);
        hotel.Diets.Add(diet);
        Database.GetCollection<Database.Entity.Hotel>("hotels").FindOneAndReplace(h => h.Id == hotelId, hotel);
    }

    public void RegisterTransportAgencyChange(string eventName, int idChanged, double change)
    {
        var transportAgencyCollection = Database.GetCollection<TransportAgencyChange>("transport_agency_change");
        var nextId = transportAgencyCollection.AsQueryable().Count() + 1;
        TransportAgencyChange transportAgencyChange = new TransportAgencyChange()
        {
            Id = nextId,
            EventName = eventName,
            IdChanged = idChanged,
            Change = change
        };
        transportAgencyCollection.InsertOne(transportAgencyChange);
    }

    public List<TravelAgencyEntryDTO> getLastTravelAgencyChanges(int numberOfChanges)
    {
        var transportAgencyCollection =
            Database.GetCollection<TransportAgencyChange>("transport_agency_change").AsQueryable();
        var lastChanges = transportAgencyCollection.OrderByDescending(t => t.Id)
            .Take(numberOfChanges).Select(t =>
            new TravelAgencyEntryDTO()
            {
                EventName = t.EventName,
                IdChanged = t.IdChanged,
                Change = t.Change
            }).ToList();
        return lastChanges;
    }

    private HotelWatchers? GetHotelWatcherIfExists(int hotelId)
    {
        var hotelWatchers = GetAllHotelWatchers();
        return hotelWatchers.Find(hw => hw.HotelId == hotelId);
    }

    private List<HotelWatchers> GetAllHotelWatchers()
    {
        var hotelWatchersCollection = Database.GetCollection<HotelWatchers>("hotel_watchers").AsQueryable();
        return hotelWatchersCollection.ToList();
    }
}