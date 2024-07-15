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
                  Consumer consumer)
    {
        _logger = logger;
        _configuration = configuration;
        _proposalValidateUseCase = proposalValidateUseCase;
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _disposable = _consumer.Start<ProposalEvent>(_configuration.GetSection("RabbitMq:QueueName").Value, async (message) =>
        {
            await ProcessProposalAsync(message).ConfigureAwait(false);
        });
        return Task.CompletedTask;
    }

    private async Task ProcessProposalAsync(ProposalEvent message)
{
    try
    {
        if (_serviceScope != null)
        {
            await using (var scope = _serviceScope.CreateAsyncScope())
            {
                var service = scope
                                .ServiceProvider
                                .GetRequiredService<IProposalValidateUseCase>();
                await service
                        .ProposalValidateAsync(message)
                        .ConfigureAwait(false);
            }
        }
        else
        {
            // Handle the case where _serviceScope is unexpectedly null.
            // This could be logging an error or taking appropriate action.
            _logger.LogError("Service scope factory (_serviceScope) is unexpectedly null.");
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao processar proposta.");
    }
}

}

