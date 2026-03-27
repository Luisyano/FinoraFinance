using Microsoft.AspNetCore.Identity;

namespace FinoraFinance.Models
{
    public class Usuario : IdentityUser
    {
        public string? Telefono { get; set; }
        public string? FullName { get; set; }
    }
}
