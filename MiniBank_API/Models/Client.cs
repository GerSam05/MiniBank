using System;
using System.Collections.Generic;

namespace MiniBank_API.Models
{
    public partial class Client
    {
        public Client()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public DateTime RegDate { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
