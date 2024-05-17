using HotelQuery.Consumer;
using HotelQuery.Repository.Hotel;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// MongoDB context
builder.Services.AddScoped<IHotelRepository, HotelRepository>();

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

//builder.Services.AddMassTransit(cfg =>
//{
//    // adding consumers
//    cfg.AddConsumer<HotelQueryConsumer>();
//    cfg.AddConsumer<HotelListQueryConsumer>();

//    // telling masstransit to use rabbitmq
//    cfg.UsingRabbitMq((context, rabbitCfg) =>
//    {
//        // rabbitmq config
//        rabbitCfg.Host("rabbitmq", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });

//        rabbitCfg.ConfigureEndpoints(context);
//    });
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
