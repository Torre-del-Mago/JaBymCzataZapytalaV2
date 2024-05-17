using MassTransit;
using Models.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(cfg =>
{
    //cfg.AddRequestClient<CheckLoginEvent>(new Uri("exchange:name"));



    cfg.UsingRabbitMq((context, rabbitCfg) =>
    {

        rabbitCfg.Host("rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        //rabbitCfg.ReceiveEndpoint("input-queue", e =>
        //{
        //    e.Bind("exchange-name");
        //    e.Bind<MessageType>();
        //});


        rabbitCfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

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
