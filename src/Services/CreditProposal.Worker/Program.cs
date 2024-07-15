using CreditProposal.Worker;
using Marraia.MongoDb.Configurations;
using Marraia.Queue;
using Marraia.Queue.Interfaces;
using ProposalCreditCard.Application.UseCase.ProposalValidate;
using ProposalCreditCard.Application.UseCase.ProposalValidate.Interfaces;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using ProposalCreditCard.Domain.Repositories.Interfaces;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var configuration = builder.Configuration;

builder.Services.AddSingleton<IEventBus>(provider => new EventBus(configuration.GetSection("RabbitMq:Connection").Value, configuration.GetSection("RabbitMq:ExchangeName").Value, "direct"));
builder.Services.AddSingleton((provider) =>
{
    var consumer = new Consumer((EventBus)provider.GetService<IEventBus>()!);
    consumer.Subscribe<ProposalEvent>(configuration.GetSection("RabbitMq:QueueName").Value, configuration.GetSection("RabbitMq:RoutingKey").Value);
    return consumer;
});

builder.Services.AddMongoDbSingleton();
builder.Services.AddTransient<IProposalValidateUseCase, ProposalValidateUseCase>();

var host = builder.Build();
host.Run();
