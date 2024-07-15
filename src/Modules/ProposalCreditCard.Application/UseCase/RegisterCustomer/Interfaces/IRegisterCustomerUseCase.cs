using ProposalCreditCard.Application.UseCase.RegisterCustomer.Event;

namespace ProposalCreditCard.Application.UseCase.RegisterCustomer.Interfaces
{
	public interface IRegisterCustomerUseCase
	{
		Task ProcessEventCustomerAsync(CustomerEvent eventCustomer);
	}
}

