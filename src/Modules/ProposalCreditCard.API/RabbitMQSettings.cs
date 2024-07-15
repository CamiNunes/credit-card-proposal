namespace ProposalCreditCard.API
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string ExchangeName { get; set; } = string.Empty;
        public string RoutingKey { get; set; } = string.Empty;
        public string Connection { get; set; } = string.Empty;
    }
}
