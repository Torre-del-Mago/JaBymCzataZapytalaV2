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

var builder = WebApplication.CreateBuilder(args);

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
    using (var contScope = app.Services.CreateScope())
    using (var context = contScope.ServiceProvider.GetRequiredService<HotelContext>())
    {
        var random = new Random(123);
        context.Database.EnsureCreated();
        if (!context.Diets.Any())
        {
            string[] names = {
                "wegetarianska",
                "azjatycka",
                "srodziemnomorska",
                "klasyczna",
                "keto",
                "weganska"
            };
            foreach (var name in names)
            {
                context.Diets.Add(new Diet { HotelDiets = new List<HotelDiet>(), Name=name});
            }
            context.SaveChanges();
        }
        if (!context.RoomTypes.Any())
        {
            string[] names = {"Pok�j typu business", "Pok�j dla niepal�cych", "Pok�j typu loft", "Pok�j typu studio", "Pok�j typu suite", "Pok�j typu deluxe",
            "Pok�j typu superior", "Pok�j z tarasem", "Pok�j z balkonem", "Pok�j typu penthouse"};
            for (int i = 1; i < 6; i++)
            {
                foreach (var name in names)
                {
                    context.RoomTypes.Add(new RoomType { Name = name, RoomTypes = new List<HotelRoomType>(), NumberOfPeople = i });
                }
            }
            context.SaveChanges();
        }
        if (!context.Hotels.Any()){
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

                    List<HotelDiet> HotelDiets = new List<HotelDiet>();
                    List<Diet> diets = context.Diets.OrderBy(e => random.Next()).Take(random.Next(3, 6)).ToList();
                    foreach (Diet diet in diets)
                    {
                        HotelDiet hotelDiet = new HotelDiet { Diet = diet, DietId = diet.Id, Hotel = hotel, HotelId = hotel.Id, };
                        context.HotelDiets.Add(hotelDiet);
                        diet.HotelDiets.Add(hotelDiet);
                        HotelDiets.Add(hotelDiet);
                        context.Diets.Update(diet);
                    }

                    List<HotelRoomType> HotelRoomTypes = new List<HotelRoomType>();
                    List<RoomType> roomTypes = context.RoomTypes.OrderBy(e => random.Next()).Take(random.Next(3, 6)).ToList();
                    foreach (RoomType roomType in roomTypes)
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
