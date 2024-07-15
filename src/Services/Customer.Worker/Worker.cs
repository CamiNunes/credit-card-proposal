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
        var queueName = _configuration.GetSection("RabbitMq:QueueName").Value;

        _logger.LogInformation("Iniciando Worker para consumir da fila: {QueueName}", queueName);

        _disposable = _consumer.Start<CustomerEvent>(queueName, async (message) =>
        {
            _logger.LogInformation("Mensagem recebida na fila: {QueueName}", queueName);
            await ProcessRegisterAsync(message).ConfigureAwait(false);
        });

        return Task.CompletedTask;
    }

    private async Task ProcessRegisterAsync(CustomerEvent message)
    {
        try
        {
            _logger.LogInformation("Processando mensagem: {@Message}", message);

            await using (var scope = _serviceScope.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRegisterCustomerUseCase>();
                await service.ProcessEventCustomerAsync(message).ConfigureAwait(false);
            }

            _logger.LogInformation("Mensagem processada com sucesso.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar proposta.");
        }
    }
}
