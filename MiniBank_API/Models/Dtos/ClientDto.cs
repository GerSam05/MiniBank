using System;
using System.Collections.Generic;

namespace MiniBank_API.Models
{
    public partial class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
    }
}