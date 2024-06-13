using HotelQuery.Database.Entity;
using HotelQuery.Repository.Hotel;
using Models.Hotel.DTO;
using MongoDB.Driver;

namespace HotelQuery.Repository.Reservation;

public class ReservationRespository : IReservationRepository
{
    const string ConnectionString = "mongodb://root:example@mongo:27017/";
    private MongoClient Client { get; set; }
    private IMongoDatabase Database { get; set; }

    private IHotelRepository HotelRepository { get; set; }

    public ReservationRespository(IHotelRepository hotelRepository)
    {
        Client = new MongoClient(ConnectionString);
        Database = Client.GetDatabase("hotel_query");
        HotelRepository = hotelRepository;
    }
    
    public async Task AddReservationAsync(IClientSessionHandle session, Database.Entity.Reservation reservation)
    {
        var reservations = Database.GetCollection<Database.Entity.Reservation>("reservations");
        await reservations.InsertOneAsync(session, reservation);
    }
    
    public async Task AddNewReservationStatusAsync(IClientSessionHandle session, ReservationStatus status)
    {
        var statuses = Database.GetCollection<ReservationStatus>("reservation_statuses");
        await statuses.InsertOneAsync(session, status);
    }
    
    public async Task<bool> FindIfReservationsAreCanceledAsync(int OfferId)
    {
        var reservationStatus = Database.GetCollection<ReservationStatus>("reservation_statuses");
        var filter = Builders<ReservationStatus>.Filter.And(
            Builders<ReservationStatus>.Filter.Eq(t => t.OfferId, OfferId),
            Builders<ReservationStatus>.Filter.Eq(t => t.reservationStatus, "CANCELED")
        );
        return await reservationStatus.Find(filter).AnyAsync();
    }
    
    public async Task<bool> FindReservationStatusAsync(int OfferId)
    {
        var reservationStatus = Database.GetCollection<ReservationStatus>("reservation_statuses");
        var filter = Builders<ReservationStatus>.Filter.And(
            Builders<ReservationStatus>.Filter.Eq(t => t.OfferId, OfferId)
        );
        return await reservationStatus.Find(filter).AnyAsync();
    }
    
    public async Task<bool> ReserveAsync(int reservationId, int HotelId, DateOnly BeginDate, DateOnly EndDate, List<RoomDTO> Rooms, int OfferId)
    {
        using (var session = await Client.StartSessionAsync())
        {
            try
            {
                if (FindIfReservationsAreCanceledAsync(OfferId).Result)
                {
                    throw new Exception("Reservations are canceled for this offer.");
                }
                
                var hotel = HotelRepository.GetHotel(HotelId);
                if (hotel == null)
                {
                    throw new Exception("Hotel not found.");
                }
                
                 // Pobierz wszystkie istniejące rezerwacje dla hotelu w podanym przedziale czasowym
                var existingReservations = HotelRepository.GetReservationWithin(BeginDate, EndDate);
                
                // Sprawdź dostępność pokoi
                foreach (var room in Rooms)
                {
                    var roomType = HotelRepository.GetRoomTypes().FirstOrDefault(rt => rt.Id == room.Id);
                    if (roomType == null)
                    {
                        throw new Exception($"Room type {room.TypeOfRoom} not found.");
                    }

                    int reservedRoomCount = existingReservations
                        .SelectMany(r => HotelRepository.GetRoomsForReservation(r.Id))
                        .Where(rr => HotelRepository.GetHotelRoomType(rr.HotelRoomTypesId).RoomTypeId == roomType.Id)
                        .Sum(rr => rr.NumberOfRooms);

                    var availableRooms = hotel.Rooms
                        .Where(r => r.RoomTypeId == roomType.Id)
                        .Sum(r => r.Count);

                    if (reservedRoomCount + room.Count > availableRooms)
                    {
                        throw new Exception($"Not enough available rooms for room type {room.TypeOfRoom}.");
                    }
                }

                // Dodaj rezerwację
                var newReservation = new Database.Entity.Reservation
                {
                    Id = reservationId,
                    HotelId = HotelId,
                    From = BeginDate,
                    To = EndDate,
                    OfferId = OfferId,
                    Rooms = new List<ReservedRoom>(),
                    ReservedAt = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };
                

                // Dodaj zarezerwowane pokoje
                foreach (var room in Rooms)
                {
                    var roomType = HotelRepository.GetRoomTypes().FirstOrDefault(rt => rt.Id == room.Id);
                    var hotelRoomType = hotel.Rooms.First(r => r.RoomTypeId == roomType.Id);

                    var reservedRoom = new ReservedRoom
                    {
                        ReservationId = newReservation.Id,
                        HotelRoomTypesId = hotelRoomType.Id,
                        NumberOfRooms = room.Count
                    };
                    newReservation.Rooms.Add(reservedRoom);
                }
                await AddReservationAsync(session, newReservation);

                // Dodaj nowy status rezerwacji
                var newStatus = new ReservationStatus
                {
                    Id = OfferId,
                    OfferId = OfferId,
                    reservationStatus = "RESERVED"
                };
                await AddNewReservationStatusAsync(session, newStatus);

                return true;
            }
            catch (Exception e)
            {
                Console.Error.Write(e);
                return false;
            }
        }
    }
    
    public async Task CancelReservation(int OfferId)
    {
        using (var session = await Client.StartSessionAsync())
        {
             try
            {
                var newStatus = new ReservationStatus
                {
                    Id = OfferId,
                    OfferId = OfferId,
                    reservationStatus = "CANCELED"
                };
                if (FindReservationStatusAsync(OfferId).Result)
                {
                    var reservations = Database.GetCollection<Database.Entity.Reservation>("reservations");
                    var reservationStatus = Database.GetCollection<ReservationStatus>("reservation_statuses");
                    var filterStatus = Builders<ReservationStatus>.Filter.And(
                        Builders<ReservationStatus>.Filter.Eq(t => t.OfferId, OfferId)
                    );
                    await reservationStatus.FindOneAndReplaceAsync(session, filterStatus, newStatus);
                    var filterReservations = Builders<Database.Entity.Reservation>.Filter.And(
                        Builders<Database.Entity.Reservation>.Filter.Eq(t => t.OfferId, OfferId)
                    );

                    await reservations.DeleteManyAsync(session, filterReservations);
                }
                else
                {
                    await AddNewReservationStatusAsync(session,newStatus);
                }
            }
            catch (Exception e)
            {
                Console.Error.Write(e);
            }
        }
    }

    public List<Database.Entity.Reservation> GetListOfReservationsOfHotel(int hotelId)
    {
        var reservations = Database.GetCollection<Database.Entity.Reservation>("reservations").AsQueryable();
        return (from reservation in reservations
            where reservation.HotelId == hotelId
            select reservation).ToList();
    }
}