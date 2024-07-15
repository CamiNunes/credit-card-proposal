using System;
using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;

namespace ProposalCreditCard.Application.UseCase.ProposalValidate.Interfaces
{
	public interface IProposalValidateUseCase
	{
		Task ProposalValidateAsync(ProposalEvent proposalEvent);
	}
}

