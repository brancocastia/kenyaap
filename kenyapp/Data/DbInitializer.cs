using Microsoft.Data.Sqlite;
using System.IO;

namespace kenyapp.Data
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            if (!File.Exists("boliche.db"))
            {
                using var connection = new SqliteConnection("Data Source=boliche.db");
                connection.Open();

                var createProductos = @"
                    CREATE TABLE Productos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Precio REAL NOT NULL,
                        Tipo TEXT NOT NULL,
                        Activo INTEGER NOT NULL
                    );";

                var createVentas = @"
                    CREATE TABLE Ventas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductoId INTEGER NOT NULL,
                        FechaHora TEXT NOT NULL,
                        PrecioUnitario REAL NOT NULL,
                        Cantidad INTEGER NOT NULL,
                        FOREIGN KEY(ProductoId) REFERENCES Productos(Id)
                    );";

                using var cmd1 = new SqliteCommand(createProductos, connection);
                cmd1.ExecuteNonQuery();

                using var cmd2 = new SqliteCommand(createVentas, connection);
                cmd2.ExecuteNonQuery();
            }
        }
    }
}