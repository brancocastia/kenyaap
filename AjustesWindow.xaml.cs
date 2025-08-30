using System.Windows;
using kenyapp.Models;
using kenyapp.Data;


namespace kenyapp
{
    public partial class AjustesWindow : Window
    {
        public AjustesWindow()
        {
            InitializeComponent();
            CargarBebidas();
        }

        private void CargarBebidas()
        {
            using (var db = new AppDbContext())
            {
                BebidasGrid.ItemsSource = db.Bebidas.ToList();
            }
        }

        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            var nombre = Microsoft.VisualBasic.Interaction.InputBox("Nombre de la bebida:", "Agregar Bebida");
            if (string.IsNullOrWhiteSpace(nombre)) return;

            var precioStr = Microsoft.VisualBasic.Interaction.InputBox("Precio de la bebida:", "Agregar Bebida");
            if (!decimal.TryParse(precioStr, out decimal precio)) return;

            using (var db = new AppDbContext())
            {
                db.Bebidas.Add(new Bebida { Nombre = nombre, Precio = precio });
                db.SaveChanges();
            }

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

                using (var db = new AppDbContext())
                {
                    var b = db.Bebidas.Find(bebida.Id);
                    if (b != null)
                    {
                        b.Nombre = nombre;
                        b.Precio = precio;
                        db.SaveChanges();
                    }
                }

                CargarBebidas();
            }
        }

        private void Eliminar_Click(object sender, RoutedEventArgs e)
        {
            if (BebidasGrid.SelectedItem is Bebida bebida)
            {
                if (MessageBox.Show($"¿Eliminar {bebida.Nombre}?", "Confirmar", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    using (var db = new AppDbContext())
                    {
                        var b = db.Bebidas.Find(bebida.Id);
                        if (b != null)
                        {
                            db.Bebidas.Remove(b);
                            db.SaveChanges();
                        }
                    }

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
