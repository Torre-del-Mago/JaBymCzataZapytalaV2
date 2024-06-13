using HotelCommand.Consumer;
using HotelCommand.Database;
using HotelCommand.Repository.DietRepository;
using HotelCommand.Repository.HotelRepository;
using HotelCommand.Repository.HotelDietRepository;
using HotelCommand.Repository.HotelRoomTypeRepository;
using HotelCommand.Repository.ReservationEventRepository;
using MassTransit;
using HotelCommand.Repository.ReservationRepository;
using HotelCommand.Repository.ReservedRoomRepository;
using HotelCommand.Repository.RoomTypeRepository;
using HotelCommand.Service;
using HotelCommand.Database.Tables;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HotelContext>(
    DbContextOptions => DbContextOptions.UseNpgsql(connectionString)
        .LogTo(Console.Write, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);
// Add services to the container.

builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HotelContext>();
builder.Services.AddScoped<IDietRepository, DietRepository>();
builder.Services.AddScoped<IHotelDietRepository, HotelDietRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelRoomTypeRepository, HotelRoomTypeRepository>();
builder.Services.AddScoped<IReservationEventRepository, ReservationEventRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservedRoomRepository, ReservedRoomRepository>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IEventService, EventService>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<CancelReservationHotelConsumer>();
    cfg.AddConsumer<ReserveHotelConsumer>();
    cfg.AddConsumer<AddDietConsumer>();
    cfg.AddConsumer<ChangeHotelDiscountConsumer>();
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

initDB();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

void initDB()
{
    string[] names = {
                "wegetarianska",
                "azjatycka",
                "srodziemnomorska",
                "klasyczna",
                "keto",
                "weganska"
            };
    int[] typesId = {
        1,2,3,4,5,6,7,8,9,10,
        11,12,13,14,15,16,17,18,19,20,
        21,22,23,24,25,26,27,28,29,30,
        31,32,33,34,35,36,37,38,39,40,
        41,42,43,44,45,46,47,48,49,50,
        51,52,53,54,55,56,57,58,59,60,
    };
    using (var contScope = app.Services.CreateScope())
    using (var context = contScope.ServiceProvider.GetRequiredService<HotelContext>())
    {
        var random = new Random(123);
        context.Database.EnsureCreated();
        if (!context.Diets.Any())
        {
            
            foreach (var name in names)
            {
                context.Diets.Add(new Diet { HotelDiets = new List<HotelDiet>(), Name=name});
            }
            context.SaveChanges();
        }
        if (!context.RoomTypes.Any())
        {
            string[] namesTypes = {"Pokój typu business", "Pokój dla niepalących", "Pokój typu loft", "Pokój typu studio", "Pokój typu suite", "Pokój typu deluxe",
            "Pokój typu superior", "Pokój z tarasem", "Pokój z balkonem", "Pokój typu penthouse"};
            for (int i = 1; i < 6; i++)
            {
                foreach (var name in namesTypes)
                {
                    context.RoomTypes.Add(new RoomType { Name = name, RoomTypes = new List<HotelRoomType>(), NumberOfPeople = i });
                }
            }
            context.SaveChanges();
        }
        if (!context.Hotels.Any()){
            List<Diet> diets = context.Diets.ToList();
            List<RoomType> roomTypes = context.RoomTypes.ToList();
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
                        Name = elements[0],
                        Country = elements[elements.Length - 3],
                        City = elements[elements.Length - 2],
                        Discount = Discount,
                    };

                    var selectedNames = names.OrderBy(_ => random.Next()).Take(random.Next(3, 6)).ToList();

                    List<HotelDiet> HotelDiets = new List<HotelDiet>();
                    List<Diet> diets_shuffeled = diets.Where(d => selectedNames.Contains(d.Name)).ToList();
                    foreach (Diet diet in diets_shuffeled)
                    {
                        HotelDiet hotelDiet = new HotelDiet { Diet = diet, DietId = diet.Id, Hotel = hotel, HotelId = hotel.Id, };
                        context.HotelDiets.Add(hotelDiet);
                        diet.HotelDiets.Add(hotelDiet);
                        HotelDiets.Add(hotelDiet);
                        context.Diets.Update(diet);
                    }

                    var selectedIds = typesId.OrderBy(_ => random.Next()).Take(random.Next(3, 6)).ToList();

                    List<HotelRoomType> HotelRoomTypes = new List<HotelRoomType>();
                    List<RoomType> roomTypes_shuffeled = roomTypes.Where(rt => selectedIds.Contains(rt.Id)).ToList();
                    foreach (RoomType roomType in roomTypes_shuffeled)
                    {
                        HotelRoomType hotelRoomType = new HotelRoomType { Hotel = hotel, HotelId = hotel.Id, RoomTypeId=roomType.Id,
                            RoomType=roomType, ReservedRooms=new List<ReservedRoom>(), PricePerNight=random.Next(50, 120), NumberOfRooms= random.Next(1, 5)
                        };
                        context.HotelRoomTypes.Add(hotelRoomType);
                        roomType.RoomTypes.Add(hotelRoomType);
                        HotelRoomTypes.Add(hotelRoomType);
                        context.RoomTypes.Update(roomType);
                    }

                    hotel.HotelRoomType = HotelRoomTypes;
                    hotel.HotelDiets = HotelDiets;
                    context.Hotels.Add(hotel);
                }
            }
            context.SaveChanges();
        }
        
    }
}
