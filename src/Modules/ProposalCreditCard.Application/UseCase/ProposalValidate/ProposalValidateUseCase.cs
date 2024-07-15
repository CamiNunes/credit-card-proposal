using System;
using Marraia.Queue.Interfaces;
using ProposalCreditCard.Application.UseCase.ProposalValidate.Interfaces;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using ProposalCreditCard.Application.Utils;

namespace ProposalCreditCard.Application.UseCase.ProposalValidate
{
	public class ProposalValidateUseCase : IProposalValidateUseCase
    {
        const int incomeCommitment = 40;
        const int percentage = 100;

        private readonly IEventBus _eventBus;

        public ProposalValidateUseCase(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task ProposalValidateAsync(ProposalEvent proposalEvent)
        {
            var validateSalary = proposalEvent.Salary * incomeCommitment / percentage;

            if (validateSalary < proposalEvent.CreditValue)
            {
                var simulation = new CustomerEvent()
                {
                    Decision = "RETORNO",
                    CreditValue = proposalEvent.CreditValue,
                    Name = proposalEvent.Name,
                    Salary = proposalEvent.Salary,
                    Simulation = new ProposalSimulationEvent()
                    {
                        CustomerId = proposalEvent.CustomerId,
                        Message = "Não foi possivel aprovar o cartão pois a solicitaçao de crédito é maior que o comprometimento de renda do cliente",
                        Status = "NAOAPROVADO"
                    }
                };

                await _eventBus
                        .PublishAsync(simulation, "messagebus.customersimulation.eventhandler")
                        .ConfigureAwait(false);

                return;
            }

            //TODO: Envar para a fila de criaçao de cartao de credito...

            var creditCardNumber = CreditCardNumberGenerator.GenerateCreditCardNumber();

            var proposalCreditCardEvent = new ProposalCreditCardEvent
            {
                CustomerId = proposalEvent.CustomerId,
                Name = proposalEvent.Name,
                LimitValue = proposalEvent.CreditValue,
                CreditCardNumber = creditCardNumber
            };

            await _eventBus
                    .PublishAsync(proposalCreditCardEvent, "messagebus.proposalCreditCard.eventhandler")
                    .ConfigureAwait(false);

        }
    }
}

