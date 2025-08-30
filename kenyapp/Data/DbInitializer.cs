using Microsoft.Data.Sqlite;
using System.IO;

namespace kenyapp.Data
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            if (!File.Exists("kenya.db"))
            {
                using var connection = new SqliteConnection("Data Source=kenya.db");
                connection.Open();

                var createBebidas = @"
                    CREATE TABLE Bebidas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Precio REAL NOT NULL,
                        Activo INTEGER NOT NULL
                    );";

                var createVentas = @"
                    CREATE TABLE Ventas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        BebidaId INTEGER NOT NULL,
                        FechaHora TEXT NOT NULL,
                        PrecioUnitario REAL NOT NULL,
                        Cantidad INTEGER NOT NULL
                    );";

                using var cmd1 = new SqliteCommand(createBebidas, connection);
                cmd1.ExecuteNonQuery();

                using var cmd2 = new SqliteCommand(createVentas, connection);
                cmd2.ExecuteNonQuery();
            }
        }
    }
}
