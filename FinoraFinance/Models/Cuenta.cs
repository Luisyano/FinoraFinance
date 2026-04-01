using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinoraFinance.Models
{
    public class Cuenta
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la cuenta es obligatorio")]
        [Display(Name = "Nombre de la cuenta")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La moneda es obligatoria")]
        [Display(Name = "Moneda")]
        public string Moneda { get; set; }  

        [Required(ErrorMessage = "El saldo inicial es obligatorio")]
        [Display(Name = "Saldo inicial")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "El saldo debe ser mayor o igual a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoInicial { get; set; }

        public string UserId { get; set; }
        public virtual Usuario User { get; set; }
    }
}