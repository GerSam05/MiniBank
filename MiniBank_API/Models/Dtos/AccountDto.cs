namespace MiniBank_API.Models.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int AccountType { get; set; }
        public int ClientId { get; set; }
        public decimal Balance { get; set; }
    }
}
