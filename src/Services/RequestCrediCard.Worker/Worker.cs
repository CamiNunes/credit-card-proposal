using Marraia.Queue;
using Marraia.Queue.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ProposalCreditCard.Application.UseCase.ProposalValidate.Interfaces;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using ProposalCreditCard.Application.UseCase.RequestCreditCard.Interfaces;

namespace RequestCrediCard.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEventBus _eventBus;
    private readonly Consumer _consumer;
    private readonly IServiceScopeFactory _serviceScope;
    private readonly IRequestCreditCardUseCase _requestCreditCardUseCase;
    private readonly IConfiguration _configuration;
    private IDisposable _disposable;

    public Worker(ILogger<Worker> logger,
              IEventBus eventBus,
              Consumer consumer,
              IServiceScopeFactory serviceScope,
              IRequestCreditCardUseCase requestCreditCardUseCase,
              IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
        _requestCreditCardUseCase = requestCreditCardUseCase ?? throw new ArgumentNullException(nameof(requestCreditCardUseCase));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _disposable = _consumer.Start<ProposalCreditCardEvent>(_configuration.GetSection("RabbitMq:QueueName").Value, async (message) =>
        {
            await ProcessCreditCardRequestAsync(message).ConfigureAwait(false);
        });

        return Task.CompletedTask;
    }

    private async Task ProcessCreditCardRequestAsync(ProposalCreditCardEvent message)
    {
        try
        {
            await using (var scope = _serviceScope.CreateAsyncScope())
            {
                var service = scope
                                .ServiceProvider
                                .GetRequiredService<IRequestCreditCardUseCase>();
                await service
                        .ProcessCreditCardRequestAsync(message)
                        .ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar solicitação de cartão de crédito.");
        }
    }
}