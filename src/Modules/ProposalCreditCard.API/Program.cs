using Marraia.Queue;
using Marraia.Queue.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Registro do IEventBus com suas dependências
builder.Services.AddSingleton<IEventBus>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetSection("RabbitMq:Connection").Value;
    var exchangeName = configuration.GetSection("RabbitMq:ExchangeName").Value;
    var exchangeType = configuration.GetSection("RabbitMq:ExchangeType").Value;

    return new EventBus(connectionString, exchangeName, exchangeType);
});

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
