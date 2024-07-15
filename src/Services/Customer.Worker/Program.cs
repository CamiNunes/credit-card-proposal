using Customer.Worker;
using Marraia.MongoDb.Configurations;
using Marraia.Queue;
using Marraia.Queue.Interfaces;
using ProposalCreditCard.Application.UseCase.RegisterCustomer;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Interfaces;
using ProposalCreditCard.Domain.Repositories.Interfaces;
using ProposalCreditCard.Infrastructure.Repositories;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var configuration = builder.Configuration;

builder.Services.AddSingleton<IEventBus>(provider => new EventBus(configuration.GetSection("RabbitMq:Connection").Value, configuration.GetSection("RabbitMq:ExchangeName").Value, "direct"));
builder.Services.AddSingleton((provider) =>
{
    var consummer = new Consumer((EventBus)provider.GetService<IEventBus>()!);
    consummer.Subscribe<CustomerEvent>(configuration.GetSection("RabbitMq:QueueName").Value, configuration.GetSection("RabbitMq:RoutingKey").Value);
    return consummer;
});

builder.Services.AddMongoDbSingleton();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IRegisterCustomerUseCase, RegisterCustomerUseCase>();

var host = builder.Build();
host.Run();

