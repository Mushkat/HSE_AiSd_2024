namespace FinanceModule.Domain.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }
}