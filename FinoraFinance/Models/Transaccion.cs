using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinoraFinance.Models
{
    public class Transaccion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [Display(Name = "Monto")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El tipo es obligatorio")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "La nota no puede tener más de 200 caracteres")]
        [Display(Name = "Nota (opcional)")]
        public string? Nota { get; set; }


        [Required(ErrorMessage = "Debes seleccionar una cuenta")]
        public int CuentaId { get; set; }
        public virtual Cuenta? Cuenta { get; set; }  

        [Required(ErrorMessage = "Debes seleccionar una etiqueta")]
        public int EtiquetaId { get; set; }
        public virtual Etiqueta? Etiqueta { get; set; }  

        public string UserId { get; set; } = string.Empty;
        public virtual Usuario? User { get; set; }  
    }
}