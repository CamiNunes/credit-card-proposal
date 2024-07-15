using Marraia.Queue.Interfaces;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using ProposalCreditCard.Application.UseCase.RequestCreditCard.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProposalCreditCard.Application.UseCase.RequestCreditCard
{
    public class RequestCreditCardUseCase : IRequestCreditCardUseCase
    {
        private readonly IEventBus _eventBus;

        public RequestCreditCardUseCase(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public async Task ProcessCreditCardRequestAsync(ProposalCreditCardEvent message)
        {
            var confirmationEvent = new CustomerEvent
            {
                Decision = "APROVADO",
                CreditValue = message.LimitValue,
                Name = message.Name,
                Simulation = new ProposalSimulationEvent
                {
                    CustomerId = message.CustomerId,
                    Status = "APROVADO",
                    Message = "Cartão de crédito aprovado e emitido com sucesso.",
                    CardNumber = message.CreditCardNumber
                }
            };

            await _eventBus.PublishAsync(confirmationEvent, "messagebus.customersimulation.eventhandler").ConfigureAwait(false);
        }
    }
}
