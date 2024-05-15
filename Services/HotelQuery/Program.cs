using HotelQuery.Consumer;
using HotelQuery.Database;
using HotelQuery.Database.Entity;
using HotelQuery.Repository;
using MassTransit;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// MongoDB context
builder.Services.AddSingleton(sp =>
{
    var connectionString = "mongodb://localhost:27017";
    var databaseName = "Hotel";
    var client = new MongoClient(connectionString);
    var database = client.GetDatabase(databaseName);
    return new HotelContext(database);
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

builder.Services.AddMassTransit(cfg =>
{
    // adding consumers
    cfg.AddConsumer<HotelQueryConsumer>();
    cfg.AddConsumer<HotelListQueryConsumer>();

    // telling masstransit to use rabbitmq
    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {
        // rabbitmq config
        rabbitCfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        // automatic endpoint configuration (and I think the reason why naming convention is important
        rabbitCfg.ConfigureEndpoints(context);
    });
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
