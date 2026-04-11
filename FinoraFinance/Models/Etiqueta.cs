using System.ComponentModel.DataAnnotations;

namespace FinoraFinance.Models
{
    public class Etiqueta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la etiqueta es obligatorio")]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
        [MaxLength(30, ErrorMessage = "El nombre no puede tener más de 30 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúñÑ\s]+$", ErrorMessage = "Solo letras y espacios")]
        [Display(Name = "Nombre de la etiqueta")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "La descripción no puede tener más de 100 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public string UserId { get; set; } = string.Empty;
        public virtual Usuario User { get; set; } = null!;
    }
}