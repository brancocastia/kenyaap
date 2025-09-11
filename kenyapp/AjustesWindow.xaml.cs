using System.Windows;
using kenyapp.Models;
using kenyapp.Data;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace kenyapp
{
    public partial class AjustesWindow : Window
    {
        public AjustesWindow()
        {
            InitializeComponent();
            DbInitializer.Initialize(); // crea la DB si no existe
            CargarProductos();
        }

        private void CargarProductos()
        {
            using var connection = new SqliteConnection("Data Source=boliche.db");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Id, Nombre, Tipo, Precio FROM Productos WHERE Activo = 1";

            using var reader = cmd.ExecuteReader();
            var productos = new List<Producto>();

            while (reader.Read())
            {
                productos.Add(new Producto
                {
                    Id = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Tipo = reader.GetString(2),
                    Precio = reader.GetDecimal(3)
                });
            }

            ProductosGrid.ItemsSource = productos;
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            var nombre = Microsoft.VisualBasic.Interaction.InputBox("Nombre del producto:", "Agregar Producto");
            if (string.IsNullOrWhiteSpace(nombre)) return;

            // Abrir ventana de selección de tipo
            var selector = new SelectTipoWindow { Owner = this };
            if (selector.ShowDialog() != true) return; // si cancela, salimos
            string tipo = selector.SelectedTipo;

            var precioStr = Microsoft.VisualBasic.Interaction.InputBox("Precio del producto:", "Agregar Producto");
            if (!decimal.TryParse(precioStr, out decimal precio)) return;

            using var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=boliche.db");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Productos (Nombre, Precio, Tipo, Activo) VALUES (@nombre, @precio, @tipo, 1)";
            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@precio", precio);
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.ExecuteNonQuery();

            CargarProductos();
        }


        private void Modificar_Click(object sender, RoutedEventArgs e)
        {
            if (ProductosGrid.SelectedItem is Producto producto)
            {

                // 1. Nuevo nombre
                var nombre = Microsoft.VisualBasic.Interaction.InputBox("Nuevo nombre:", "Modificar Producto", producto.Nombre);
                if (string.IsNullOrWhiteSpace(nombre)) return;

                // 2. Nuevo tipo usando la ventanita
                var selector = new SelectTipoWindow(producto.Tipo) { Owner = this }; // le pasamos tipo actual
                if (selector.ShowDialog() != true) return;
                string tipo = selector.SelectedTipo;

                // 3. Nuevo precio
                var precioStr = Microsoft.VisualBasic.Interaction.InputBox("Nuevo precio:", "Modificar Producto", producto.Precio.ToString());
                if (!decimal.TryParse(precioStr, out decimal precio)) return;

                using var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=boliche.db");
                connection.Open();

                var cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE Productos SET Nombre = @nombre, Precio = @precio, Tipo = @tipo WHERE Id = @id";
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@id", producto.Id);
                cmd.ExecuteNonQuery();

                CargarProductos();
            }
            else
            {
                MessageBox.Show("Seleccione un producto de la lista para modificar.");
            }
        }


        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (ProductosGrid.SelectedItem is Producto producto)
            {
                if (MessageBox.Show($"¿Eliminar {producto.Nombre}?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using var connection = new SqliteConnection("Data Source=boliche.db");
                    connection.Open();

                    var cmd = connection.CreateCommand();
                    // en vez de borrar, marcamos inactivo
                    cmd.CommandText = "UPDATE Productos SET Activo = 0 WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", producto.Id);
                    cmd.ExecuteNonQuery();

                    CargarProductos();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto de la lista para eliminar.");
            }
        }

        private void Volver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
