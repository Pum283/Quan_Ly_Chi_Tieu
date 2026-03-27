namespace Domain.Entity
{
    public class TransactionTagMapping
    {
        public int TransactionId { get; set; }
        public int TagId { get; set; }

        public virtual Transaction? Transaction { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
