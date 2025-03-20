namespace FinanceModule.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Type { get; set; } // Income/Expense
        public string Name { get; set; }
    }
}