using Newtonsoft.Json;
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
        public int ClientId { get; set; }
        public decimal Balance { get; set; }
        public DateTime RegDate { get; set; }

        [JsonIgnore]
        public virtual AccountType AccountTypeNavigation { get; set; } = null!;

        [JsonIgnore]
        public virtual Client Client { get; set; }

        [JsonIgnore]
        public virtual ICollection<BankTransaction> BankTransactions { get; set; }
    }
}
