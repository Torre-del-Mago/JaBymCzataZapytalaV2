using MassTransit;
using Models.TravelAgency;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("rabbitmq", "/", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
});
busControl.Start();
int hotelCount = 207;
int transportsCount = 5760;
int dietsCount = 6;
var rand = new Random(123);

while (true)
{
    var number = rand.Next(1, 5);
    switch (number)
    {
        case 1: ChangeDiscount(rand, busControl);
            break;
        case 2: AddDiet(rand, busControl);
            break;
        case 3: ChangeNumberOfSeats(rand, busControl);
            break;
        case 4: ChangePricePerSeat(rand, busControl);
            break;
    }
    var randTime = rand.Next(0, 10000) - 5000;
    await Task.Delay(5 * 60 * 1000 - randTime);
}

busControl.Stop();

return 0;

async void ChangeDiscount(Random random, IBusControl bus)
{
    var discountChange = (random.NextDouble() - 0.5) / 10.0;
    var hotelToChange = random.Next(1, hotelCount + 1);
    var @event = new ChangeHotelDiscountEvent()
    {
        HotelId = hotelToChange,
        DiscountChange = discountChange
    };
    await bus.Publish(@event);
    Console.Out.WriteLine("Published ChangeHotelDiscountEvent hotelId: " + hotelToChange + " discountChange: " + discountChange );
}

async void AddDiet(Random random, IBusControl bus)
{
    var dietToChange = random.Next(1, dietsCount + 1);
    var hotelToChange = random.Next(1, hotelCount + 1);
    var @event = new AddDietEvent()
    {
        HotelId = hotelToChange,
        DietId = dietToChange
    };
    await bus.Publish(@event);
    Console.Out.WriteLine("Published AddDietEvent hotelId: " + hotelToChange + " dietToChange: " + dietToChange );
}

async void ChangeNumberOfSeats(Random random, IBusControl bus)
{
    var transportToChange = random.Next(1, transportsCount + 1);
    var numberOfSeats = random.Next( 5)-2;
    var @event = new ChangeNumberOfSeatsEvent()
    {
        TransportId = transportToChange,
        NumberOfSeats = numberOfSeats
    };
    await bus.Publish(@event);
    Console.Out.WriteLine("Published ChangeNumberOfSeats transportToChange: " + transportToChange + " numberOfSeats: " + numberOfSeats );
}

async void ChangePricePerSeat(Random random, IBusControl bus)
{
    var transportToChange = random.Next(1, transportsCount + 1);
    var priceChange = (random.NextDouble() - 0.5) * 300.0;
    var @event = new ChangePricePerSeatEvent()
    {
        TransportId = transportToChange,
        PriceChange = priceChange
    };
    await bus.Publish(@event);
    Console.Out.WriteLine("Published ChangePricePerSeat transportToChange: " + transportToChange + " priceChange: " + priceChange );
}