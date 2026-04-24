using FinoraFinance.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinoraFinance.Repositories
{
    public class TransaccionRepository
    {
        private readonly string _connectionString;

        public TransaccionRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<(List<TransaccionDTO> items, int TotalRegistros)> FiltrarPaginadoAsync(
            string search,
            string orden,
            int page,
            int pageSize,
            string userId)
        {
            var lista = new List<TransaccionDTO>();
            int totalRegistros = 0;

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_FiltrarTransaccionesPaginadas", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", (object?)search ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@orden", orden);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);
                cmd.Parameters.AddWithValue("@userId", userId);

                await con.OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (totalRegistros == 0 && !reader.IsDBNull(reader.GetOrdinal("TotalRegistros")))
                        {
                            totalRegistros = Convert.ToInt32(reader["TotalRegistros"]);
                        }

                        var transaccion = new TransaccionDTO
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Monto = Convert.ToDecimal(reader["Monto"]),
                            Fecha = Convert.ToDateTime(reader["Fecha"]),
                            Tipo = reader["Tipo"] as string ?? string.Empty,
                            Nota = reader["Nota"] as string,
                            CuentaId = Convert.ToInt32(reader["CuentaId"]),
                            EtiquetaId = Convert.ToInt32(reader["EtiquetaId"]),
                            UserId = reader["UserId"] as string ?? string.Empty,
                            CuentaNombre = reader["CuentaNombre"] as string ?? string.Empty,
                            Moneda = reader["Moneda"] as string ?? string.Empty,
                            EtiquetaNombre = reader["EtiquetaNombre"] as string ?? string.Empty
                        };

                        lista.Add(transaccion);
                    }
                }
            }

            return (lista, totalRegistros);
        }
    }
}