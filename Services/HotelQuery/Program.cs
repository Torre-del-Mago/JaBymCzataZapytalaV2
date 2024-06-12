using HotelQuery.Consumer;
using HotelQuery.Database.Entity;
using HotelQuery.Repository.Hotel;
using HotelQuery.Repository.Reservation;
using HotelQuery.Service.Hotel;
using MassTransit;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MongoDB context
builder.Services.AddScoped<IHotelRepository, HotelRepository>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRespository>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<CancelReservationHotelSyncConsumer>();
    cfg.AddConsumer<GetTopHotelRoomTypeConsumer>();
    cfg.AddConsumer<HotelQueryConsumer>();
    cfg.AddConsumer<HotelListQueryConsumer>();
    cfg.AddConsumer<ReserveHotelSyncConsumer>();
    cfg.AddConsumer<AddWatcherConsumer>();
    cfg.AddConsumer<RemoveWatcherConsumer>();
    cfg.AddConsumer<GetHotelStatisticsConsumer>();
    
    cfg.AddDelayedMessageScheduler();
    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        rabbitCfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });
        rabbitCfg.UseDelayedMessageScheduler();
        rabbitCfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

initDB(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void initDB(IServiceProvider services)
{
    using var scope = services.CreateScope();
    const string ConnectionString = "mongodb://root:example@mongo:27017/";
    //Client = new MongoClient(ConnectionString);
    //Database = Client.GetDatabase("hotel_query");
    //var mongoClient = scope.ServiceProvider.GetRequiredService<IMongoClient>();
    //var database = mongoClient.GetDatabase(ConnectionString);
    var mongoClient = new MongoClient(ConnectionString);
    var database = mongoClient.GetDatabase("hotel_query");

    var random = new Random(123);

    var diets = new[]
    {
        "wegetarianska",
        "azjatycka",
        "srodziemnomorska",
        "klasyczna",
        "keto",
        "weganska"
    };

    var roomTypes = new[]
    {
        "Pokój typu business", "Pokój dla niepalócych", "Pokój typu loft", "Pokój typu studio", "Pokój typu suite", "Pokój typu deluxe",
        "Pokój typu superior", "Pokój z tarasem", "Pokój z balkonem", "Pokój typu penthouse"
    };

    var hotelsCollection = database.GetCollection<Hotel>("hotels");
    var dietsCollection = database.GetCollection<Diet>("diets");
    var roomTypesCollection = database.GetCollection<RoomType>("room_types");

    int IdDiets = 1;
    int IdRoomTypes = 1;


    if (!dietsCollection.AsQueryable().Any())
    {
        foreach (var diet in diets)
        {
            dietsCollection.InsertOne(new Diet { Name = diet, Id = IdDiets });
            IdDiets++;
        }
    }

    if (!roomTypesCollection.AsQueryable().Any())
    {
        foreach (var roomType in roomTypes)
        {
            for (int i = 1; i < 6; i++)
            {
                roomTypesCollection.InsertOne(new RoomType
                {
                    Name = roomType,
                    NumberOfPeople = i,
                    Id = IdRoomTypes
                });
                IdRoomTypes++;
            }
        }
    }

    if (!hotelsCollection.AsQueryable().Any())
    {
        int[] typesId = {
            1,2,3,4,5,6,7,8,9,10,
            11,12,13,14,15,16,17,18,19,20,
            21,22,23,24,25,26,27,28,29,30,
            31,32,33,34,35,36,37,38,39,40,
            41,42,43,44,45,46,47,48,49,50,
            51,52,53,54,55,56,57,58,59,60,
        };
        int IdHotels = 1;
        int IdHotelRoomType = 1;

        string csvPath = @"InitData/hotels.csv";
        using (var reader = new StreamReader(csvPath))
        {
            reader.ReadLine(); //Reading headers
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] elements = line.Split(',');

                float Discount = random.Next(10, 30) / 100.0f;

                Hotel hotel = new Hotel
                {
                    Id= IdHotels,
                    Name = elements[0],
                    Country = elements[elements.Length - 3],
                    City = elements[elements.Length - 2],
                    Discount = Discount,
                    Rooms = new List<HotelRoomType>()
                };

                IdHotels++;

                var selectedNames = diets.OrderBy(_ => random.Next()).Take(random.Next(3, 6)).ToList();
                var dietFilter = Builders<Diet>.Filter.In(rt => rt.Name, selectedNames);
                hotel.Diets = dietsCollection.Find(dietFilter).ToList();

                var selectedIds = typesId.OrderBy(_ => random.Next()).Take(random.Next(3, 6)).ToList();
                var filter = Builders<RoomType>.Filter.In(rt => rt.Id, selectedIds);
                foreach (RoomType roomType in roomTypesCollection.Find(filter).ToList()) {
                    HotelRoomType hotelRoomType = new HotelRoomType
                    {
                        Id= IdHotelRoomType,
                        RoomTypeId = roomType.Id,
                        PricePerNight = random.Next(50, 120),
                        Count = random.Next(1, 5)
                    };

                    IdHotelRoomType++;

                    hotel.Rooms.Add(hotelRoomType);
                }

                hotelsCollection.InsertOne(hotel);
            }
        }
    }
}
