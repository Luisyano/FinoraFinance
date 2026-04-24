using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FinoraFinance.Models
{
    public class Usuario : IdentityUser
    {
        public string? Telefono { get; set; }
        public string? FullName { get; set; }

        [Display(Name = "Foto de perfil")]
        public string? FotoPerfil { get; set; }  
    }
}
