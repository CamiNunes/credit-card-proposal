using Marraia.MongoDb.Repositories;
using Marraia.MongoDb.Repositories.Interfaces;
using ProposalCreditCard.Domain;
using ProposalCreditCard.Domain.Repositories.Interfaces;

namespace ProposalCreditCard.Infrastructure.Repositories;

public class CustomerRepository : MongoDbRepositoryBase<Customer, Guid>, ICustomerRepository
{

    public CustomerRepository(IMongoContext context)
            : base(context)
    {
    }
}

