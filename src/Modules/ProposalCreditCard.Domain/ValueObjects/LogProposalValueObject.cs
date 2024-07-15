namespace ProposalCreditCard.Domain.ValueObjects
{
	public class LogProposalValueObject
	{
		public LogProposalValueObject()
		{
			Date = DateTime.Now;
		}

		public string Message { get; set; } = string.Empty;
		public DateTime Date { get; set; }
	}
}

