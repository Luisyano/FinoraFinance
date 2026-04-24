using FinoraFinance.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinoraFinance.Repositories
{
    public class EtiquetaRepository
    {
        private readonly string _connectionString;

        public EtiquetaRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<(List<Etiqueta> items, int TotalRegistros)> FiltrarPaginadoAsync(
            string search,
            int page,
            int pageSize,
            string userId)
        {
            var lista = new List<Etiqueta>();
            int totalRegistros = 0;

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_FiltrarEtiquetasPaginadas", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@search", (object?)search ?? DBNull.Value);
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

                        var etiqueta = new Etiqueta
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"] as string ?? string.Empty,
                            Descripcion = reader["Descripcion"] as string,
                            UserId = reader["UserId"] as string ?? string.Empty
                        };

                        lista.Add(etiqueta);
                    }
                }
            }

            return (lista, totalRegistros);
        }
    }
}