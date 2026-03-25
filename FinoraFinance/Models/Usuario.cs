using Microsoft.AspNetCore.Identity;

namespace FinoraFinance.Models
{
    public class Usuario : IdentityUser
    {
        public string? Username { get; set; } 

    }
}
