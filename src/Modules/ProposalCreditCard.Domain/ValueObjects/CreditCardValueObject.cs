using System;
namespace ProposalCreditCard.Domain.ValueObjects
{
	public class CreditCardValueObject
	{
		public CreditCardValueObject()
		{
		}

		public string Number { get; set; } = string.Empty;
		public decimal LimitCreditValue { get; set; }
	}
}

