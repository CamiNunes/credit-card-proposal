using Marraia.MongoDb.Core;
using ProposalCreditCard.Domain.ValueObjects;

namespace ProposalCreditCard.Domain;

public class Customer : Entity<Guid>
{
    public Customer()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; } = decimal.Zero;
    public ICollection<CreditCardValueObject> CreditCard { get; set; } = new List<CreditCardValueObject>();
    public ICollection<LogProposalValueObject> HistoryProposal { get; set; } = new List<LogProposalValueObject>();

    public void AddHistoryProposal(string message)
    {
        HistoryProposal.Add(new LogProposalValueObject() { Message = message });
    }

    public void AddCreditCard(string cardNumber, decimal limit)
    {
        CreditCard.Add(new CreditCardValueObject() { Number = cardNumber, LimitCreditValue = limit });
    }
}
