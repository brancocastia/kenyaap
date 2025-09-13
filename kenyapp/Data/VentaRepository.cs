using kenyapp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace kenyapp.Data
{
    public class VentaRepository
    {
        // 🔹 Connection string definido una sola vez
        private readonly string _connectionString = "Data Source=boliche.db";

        public void RegistrarVenta(Venta venta)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Ventas (ProductoId, FechaHora, PrecioUnitario, Cantidad)
                VALUES ($productoId, $fechaHora, $precio, $cantidad)";
            cmd.Parameters.AddWithValue("$productoId", venta.BebidaId);
            cmd.Parameters.AddWithValue("$fechaHora", venta.FechaHora.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("$precio", venta.PrecioUnitario);
            cmd.Parameters.AddWithValue("$cantidad", venta.Cantidad);

            cmd.ExecuteNonQuery();
        }

        public List<(string Nombre, int Cantidad, decimal Total)> ObtenerResumenPorNoche(DateTime fecha)
        {
            var lista = new List<(string, int, decimal)>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT p.Nombre, SUM(v.Cantidad), SUM(v.Cantidad * v.PrecioUnitario)
                FROM Ventas v
                INNER JOIN Productos p ON v.ProductoId = p.Id
                WHERE DATE(v.FechaHora) = DATE($fecha)
                GROUP BY p.Nombre";
            cmd.Parameters.AddWithValue("$fecha", fecha.Date.ToString("yyyy-MM-dd"));

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var nombre = reader.IsDBNull(0) ? "" : reader.GetString(0);
                var cantidad = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                var total = reader.IsDBNull(2) ? 0 : Convert.ToDecimal(reader.GetValue(2));

                lista.Add((nombre, cantidad, total));
            }

            return lista;
        }

        public decimal ObtenerTotalNoche(DateTime fecha)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT SUM(Cantidad * PrecioUnitario)
                FROM Ventas
                WHERE DATE(FechaHora) = DATE($fecha)";
            cmd.Parameters.AddWithValue("$fecha", fecha.Date.ToString("yyyy-MM-dd"));

            var result = cmd.ExecuteScalar();
            if (result == null || result == DBNull.Value) return 0m;
            return Convert.ToDecimal(result);
        }
    }
}
