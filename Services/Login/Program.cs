using Login.Consumer;
using Login.Database.Entity;
using Login.Service.LoginService;
using MassTransit;
using Models.Login;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();

    cfg.AddConsumer<LoginConsumer>();

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

//app.UseAuthorization();

//app.MapControllers();

app.Run();

void initDB(IServiceProvider services)
{
    using var scope = services.CreateScope();
    const string ConnectionString = "mongodb://root:example@mongo:27017/";
    var mongoClient = new MongoClient(ConnectionString);
    var database = mongoClient.GetDatabase("login");

    var userCollection = database.GetCollection<User>("users");


    if (!userCollection.AsQueryable().Any())
    {
        User user1 = new User() { 
            Id = 1,
            Login="Agatka"
        };
        User user2 = new User()
        {
            Id = 2,
            Login = "Mareczek"
        };
        User user3 = new User()
        {
            Id = 3,
            Login = "Kubu�"
        };
        User user4 = new User()
        {
            Id = 4,
            Login = "Krzysiu"
        };

        userCollection.InsertOne(user1);
        userCollection.InsertOne(user2);
        userCollection.InsertOne(user3);
        userCollection.InsertOne(user4);
    }
}
