namespace KassabokenAPI.Models
{
    public class Transaction
    {
        public string? FromAccount { get; set; }
        public string? ToAccount { get; set; }
        public double Amount { get; set; }
    }
}
