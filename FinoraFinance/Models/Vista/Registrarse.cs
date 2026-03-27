using System.ComponentModel.DataAnnotations;

namespace FinoraFinance.Models.Vista
{
    public class Registrarse
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio"), EmailAddress]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El numero de telefono es obligatorio")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "El teléfono debe contener solo 9 dígitos numéricos")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage ="La contraseña es obligatoria"), MinLength(8,ErrorMessage ="La contraseña debe tener 8 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El nombre para el usuario es obligatorio")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        [Display(Name = "Nombre de Usuario")]
        public string FullName { get; set; }
    }
}
