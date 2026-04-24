namespace FinoraFinance.Models
{
    public class TransaccionDTO
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? Nota { get; set; }
        public int CuentaId { get; set; }
        public int EtiquetaId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CuentaNombre { get; set; } = string.Empty;
        public string Moneda { get; set; } = string.Empty;
        public string EtiquetaNombre { get; set; } = string.Empty;
        public int TotalRegistros { get; set; }
    }
}