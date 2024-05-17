using MassTransit;
using TransportQuery.Consumer;
using TransportQuery.Repository.TransportRepository;

var builder = WebApplication.CreateBuilder(args);

// MongoDB context
builder.Services.AddScoped<ITransportRepository, TransportRepository>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//builder.Services.AddMassTransit(cfg =>
//{
//    // adding consumers
//    cfg.AddConsumer<TransportQueryConsumer>();
//    cfg.AddConsumer<TransportListQueryConsumer>();

//    // telling masstransit to use rabbitmq
//    cfg.UsingRabbitMq((context, rabbitCfg) =>
//    {
//        // rabbitmq config
//        rabbitCfg.Host("rabbitmq", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });
//        // automatic endpoint configuration (and I think the reason why naming convention is important
//        rabbitCfg.ConfigureEndpoints(context);
//    });
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
