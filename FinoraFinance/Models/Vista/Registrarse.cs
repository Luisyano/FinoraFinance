using System.ComponentModel.DataAnnotations;

namespace FinoraFinance.Models.Vista
{
    public class Registrarse
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio"), EmailAddress]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage ="La contraseña es obligatoria"), MinLength(8,ErrorMessage ="La contraseña debe tener 8 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El nombre para el usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }
    }
}
