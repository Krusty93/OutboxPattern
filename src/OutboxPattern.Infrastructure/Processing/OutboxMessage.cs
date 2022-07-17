namespace OutboxPattern.Infrastructure.Processing
{
    public class OutboxMessage
    {
        /// <summary>
        /// EF reserved constructor
        /// </summary>
        protected OutboxMessage()
        { }

        private OutboxMessage(
            DateTime occurredOn,
            string type,
            string data)
        {
            Id = Guid.NewGuid();
            OccurredOn = occurredOn;
            Type = type;
            Data = data;
        }

        public Guid Id { get; private set; }

        public DateTime OccurredOn { get; private set; }

        public string Type { get; private set; }

        public string Data { get; private set; }

        public DateTime? ProcessedDate { get; private set; }

        public static OutboxMessage Create(
            DateTime occurredOn,
            string type,
            string data)
        {
            return new OutboxMessage(occurredOn, type, data);
        }
    }
}
