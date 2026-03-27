using System.ComponentModel.DataAnnotations;

namespace FinoraFinance.Models.Vista
{
    public class Login
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo es invalido")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Display(Name = "Recuerdame")]
        public bool RememberMe { get; set; } 
    }
}
