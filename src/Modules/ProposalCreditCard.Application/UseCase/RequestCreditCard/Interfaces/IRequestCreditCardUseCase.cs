using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProposalCreditCard.Application.UseCase.RequestCreditCard.Interfaces
{
    public interface IRequestCreditCardUseCase
    {
        Task ProcessCreditCardRequestAsync(ProposalCreditCardEvent message);
    }
}
