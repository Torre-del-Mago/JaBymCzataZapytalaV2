using MassTransit;
using MongoDB.Driver;
using OfferQuery.Consumer;
using OfferQuery.Database.Entity;
using OfferQuery.Repository;
using OfferQuery.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IOfferRepository, OfferRepository>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<PaidOfferSyncConsumer>();
    cfg.AddConsumer<RemoveOfferSyncConsumer>();
    cfg.AddConsumer<ReservedOfferSyncConsumer>();
    cfg.AddConsumer<ReserveOfferSyncConsumer>();
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
    var mongoClient = new MongoClient(ConnectionString);
    var database = mongoClient.GetDatabase("offer_query");

    var offerCollection = database.GetCollection<Offer>("offers");
    var offerRoomCollection = database.GetCollection<OfferRoom>("offer_rooms");

    if (!offerCollection.AsQueryable().Any())
    {
        Offer offer = new Offer() { 
            Id = 0,
            ArrivalTransportId = 0,
            DateFrom = DateOnly.FromDateTime(DateTime.Now),
            DateTo = DateOnly.FromDateTime(DateTime.Now),
            DepartureTransportId = 0,
            HotelId = 0,
            NumberOfAdults = 0,
            NumberOfNewborns = 0,
            NumberOfTeenagers = 0,
            NumberOfToddlers = 0,
            OfferStatus = "STATUS",
            UserLogin = "USER_LOGIN"
        };

        OfferRoom room = new OfferRoom()
        {
            Id = 0,
            NumberOfRooms = 0,
            OfferId = 0,
            RoomType = "ROOM_TYPE"
        };

        offerCollection.InsertOne(offer);
        offerRoomCollection.InsertOne(room);
    }
}
