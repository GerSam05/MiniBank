using System.ComponentModel.DataAnnotations;

namespace MiniBank_API.Models.Dtos
{
    public class AccountCreateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Campo AccountType requerido debe ser mayor que cero (0)")]
        public int AccountType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Campo ClientId requerido debe ser mayor que cero (0)")]
        public int ClientId { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}
