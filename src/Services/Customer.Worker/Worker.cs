using Marraia.Queue;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Interfaces;

namespace Customer.Worker;

public class Worker : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Worker> _logger;
    private readonly Consumer _consumer;
    private readonly IServiceScopeFactory _serviceScope;
    private IDisposable _disposable;

    public Worker(ILogger<Worker> logger,
                  IConfiguration configuration,
                  IServiceScopeFactory serviceScope,
                  Consumer consumer)
    {
        _configuration = configuration;
        _consumer = consumer;
        _serviceScope = serviceScope;

        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _disposable = _consumer.Start<CustomerEvent>(_configuration.GetSection("RabbitMq:QueueName").Value, async (message) =>
        {
            _logger.LogInformation("Mensagem recebida: {Message}", message);

            await ProcessRegisterAsync(message).ConfigureAwait(false);
        });

        return Task.CompletedTask;
    }

    private async Task ProcessRegisterAsync(CustomerEvent message)
    {
        try
        {
             _logger.LogInformation("Mensagem: ", message);

            await using (var scope = _serviceScope.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRegisterCustomerUseCase>();

                 _logger.LogInformation("Service: ", service);

                await service.ProcessEventCustomerAsync(message).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar proposta.");
        }
    }
}
