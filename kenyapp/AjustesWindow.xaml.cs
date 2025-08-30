using System.Windows;
using kenyapp.Models;
using kenyapp.Data;
using Microsoft.Data.Sqlite;

namespace kenyapp
{
    public partial class AjustesWindow : Window
    {
        public AjustesWindow()
        {
            InitializeComponent();
            DbInitializer.Initialize(); // crea la DB si no existe
            CargarBebidas();
        }

        private void CargarBebidas()
        {
            var lista = new List<Bebida>();

            using (var connection = new SqliteConnection("Data Source=kenya.db"))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Id, Nombre, Precio FROM Bebidas";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Bebida
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Precio = reader.GetDecimal(2)
                        });
                    }
                }
            }

            BebidasGrid.ItemsSource = lista;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            var nombre = Microsoft.VisualBasic.Interaction.InputBox("Nombre de la bebida:", "Agregar Bebida");
            if (string.IsNullOrWhiteSpace(nombre)) return;

            var precioStr = Microsoft.VisualBasic.Interaction.InputBox("Precio de la bebida:", "Agregar Bebida");
            if (!decimal.TryParse(precioStr, out decimal precio)) return;

            using var connection = new SqliteConnection("Data Source=kenya.db");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Bebidas (Nombre, Precio, Activo) VALUES (@nombre, @precio, 1)";
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@precio", precio);
            cmd.ExecuteNonQuery();

            CargarBebidas();
        }

        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (BebidasGrid.SelectedItem is Bebida bebida)
            {
                var nombre = Microsoft.VisualBasic.Interaction.InputBox("Nuevo nombre:", "Modificar Bebida", bebida.Nombre);
                if (string.IsNullOrWhiteSpace(nombre)) return;

                var precioStr = Microsoft.VisualBasic.Interaction.InputBox("Nuevo precio:", "Modificar Bebida", bebida.Precio.ToString());
                if (!decimal.TryParse(precioStr, out decimal precio)) return;

                using var connection = new SqliteConnection("Data Source=kenya.db");
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Bebidas SET Nombre = @nombre, Precio = @precio WHERE Id = @id";
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@id", bebida.Id);
                cmd.ExecuteNonQuery();

                CargarBebidas();
            }
        }


        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (BebidasGrid.SelectedItem is Bebida bebida)
            {
                if (MessageBox.Show($"¿Eliminar {bebida.Nombre}?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using var connection = new SqliteConnection("Data Source=kenya.db");
                    connection.Open();

                    var cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM Bebidas WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", bebida.Id);
                    cmd.ExecuteNonQuery();

                    CargarBebidas();
                }
            }
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
