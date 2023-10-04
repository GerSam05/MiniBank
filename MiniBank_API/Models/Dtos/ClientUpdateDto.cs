using System.ComponentModel.DataAnnotations;

namespace MiniBank_API.Models.Dtos
{
    public class ClientUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Campo Id requerido debe ser mayor que cero (0)")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Name es requerido")]
        [MaxLength(30, ErrorMessage = "El Campo Name no debe exceder los 30 Caracteres")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "El Campo Nombre sólo admite letras")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo PhoneNumber es requerido")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[- ]?([0-9]{3})[- ]?([0-9]{2})[- ]?([0-9]{2})$", ErrorMessage = "Formato de telefono inválido")]
        public string PhoneNumber { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }
    }
}
