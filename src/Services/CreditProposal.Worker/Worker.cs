using Marraia.Queue;
using Microsoft.Extensions.DependencyInjection;
using ProposalCreditCard.Application.UseCase.ProposalValidate.Interfaces;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Interfaces;

namespace CreditProposal.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IProposalValidateUseCase _proposalValidateUseCase;
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory? _serviceScope;
    private IDisposable? _disposable;
    private Consumer _consumer;

    public Worker(ILogger<Worker> logger,
                  IConfiguration configuration,
                  IProposalValidateUseCase proposalValidateUseCase,
                  Consumer consumer,
                  IServiceScopeFactory? serviceScope)
    {
        _logger = logger;
        _configuration = configuration;
        _proposalValidateUseCase = proposalValidateUseCase;
        _consumer = consumer;
        _serviceScope = serviceScope;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueName = _configuration.GetSection("RabbitMq:QueueName").Value;
        _logger.LogInformation("Iniciando Worker para consumir da fila: {QueueName}", queueName);

        _disposable = _consumer.Start<ProposalEvent>(queueName, async (message) =>
        {
            _logger.LogInformation("Mensagem recebida na fila: {QueueName}", queueName);
            await ProcessProposalAsync(message).ConfigureAwait(false);
        });

        return Task.CompletedTask;
    }

    private async Task ProcessProposalAsync(ProposalEvent message)
    {
        try
        {
            _logger.LogInformation("Processando mensagem: {@Message}", message);

            if (_serviceScope != null)
            {
                await using (var scope = _serviceScope.CreateAsyncScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IProposalValidateUseCase>();
                    await service.ProposalValidateAsync(message).ConfigureAwait(false);
                }

                _logger.LogInformation("Mensagem processada com sucesso.");
            }
            else
            {
                // Handle the case where _serviceScope is unexpectedly null.
                _logger.LogError("Service scope factory (_serviceScope) is inesperadamente null.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar proposta.");
        }
    }
}
