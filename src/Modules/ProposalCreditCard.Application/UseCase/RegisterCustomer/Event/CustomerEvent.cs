namespace ProposalCreditCard.Application.UseCase.RegisterCustomer.Event
{
	public class CustomerEvent
	{
		public string Name { get; set; } = string.Empty;
		public decimal Salary { get; set; } = decimal.Zero;
    public decimal CreditValue { get; set; } = decimal.Zero;
		public string Decision { get; set; }  = string.Empty;
		public ProposalSimulationEvent Simulation { get; set; } = new ProposalSimulationEvent();
	}

	public class ProposalSimulationEvent
	{
		public Guid CustomerId { get; set; }
		public string Status { get; set; }  = string.Empty;
		public string Message { get; set; }  = string.Empty;
		public string CardNumber { get; set; }  = string.Empty;
	}

    public class ProposalEvent
    {
		public Guid CustomerId { get; set; }
		public string Name { get; set; }  = string.Empty;
    public decimal Salary { get; set; } = decimal.Zero;
		public decimal CreditValue { get; set; } = decimal.Zero;
	}

    public class ProposalCreditCardEvent
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }  = string.Empty;
        public decimal LimitValue { get; set; } = decimal.Zero;
        public string CreditCardNumber { get; set; }  = string.Empty;
    }
}

