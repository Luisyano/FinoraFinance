using System.ComponentModel.DataAnnotations;

namespace FinoraFinance.Models.Vista
{
    public class EditarPerfil
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Nombre completo")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe tener 9 dígitos")]
        public string Telefono { get; set; } = string.Empty;

        [Display(Name = "Foto de perfil")]
        public string? FotoActual { get; set; }
    }
}