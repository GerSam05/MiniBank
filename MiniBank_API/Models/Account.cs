using System;
using System.Collections.Generic;

namespace MiniBank_API.Models
{
    public partial class Account
    {
        public Account()
        {
            BankTransactions = new HashSet<BankTransaction>();
        }

        public int Id { get; set; }
        public int AccountType { get; set; }
        public int? ClientId { get; set; }
        public decimal Balance { get; set; }
        public DateTime RegDate { get; set; }

        public virtual AccountType AccountTypeNavigation { get; set; } = null!;
        public virtual Client? Client { get; set; }
        public virtual ICollection<BankTransaction> BankTransactions { get; set; }
    }
}
