using kenyapp.Models;
using kenyapp.Services;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Windows;

namespace kenyapp.Views
{
    public partial class VentaWindow : Window
    {
        private Pedido pedidoActual = new Pedido { Fecha = DateTime.Now };
        private ImpresoraService _impresoraService;

        public VentaWindow()
        {
            InitializeComponent();
            InicializarImpresora();
            CargarBebidas();
        }

        private void InicializarImpresora()
        {
            try
            {
                string[] posiblesNombres = { "PrinterPOS-80", "Printer80", "Factura" };
                string nombre = BuscarImpresoraDisponible(posiblesNombres);

                _impresoraService = new ImpresoraService(nombre);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Advertencia: No se pudo conectar con la impresora.\n{ex.Message}",
                    "Impresora", MessageBoxButton.OK, MessageBoxImage.Warning);
                _impresoraService = null;
            }
        }

        private string BuscarImpresoraDisponible(string[] posiblesNombres)
        {
            foreach (string nombre in posiblesNombres)
            {
                foreach (string installed in PrinterSettings.InstalledPrinters)
                {
                    if (installed.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                    {
                        return installed;
                    }
                }
            }
            throw new Exception("No se encontró ninguna impresora válida.");
        }

        private void CargarBebidas()
        {
            try
            {
                using var con = new SqliteConnection("Data Source=kenya.db");
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, Nombre, Precio FROM Bebidas WHERE Activo = 1 ORDER BY Nombre";
                var reader = cmd.ExecuteReader();
                var lista = new List<Producto>();
                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Precio = reader.GetDecimal(2)
                    });
                }
                lstBebidas.ItemsSource = lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando bebidas: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lstBebidas_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(lstBebidas.SelectedItem is Producto bebida)) return;

            int cantidad = 1;
            if (!string.IsNullOrEmpty(txtCantidad.Text))
            {
                if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad <= 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            for (int i = 0; i < cantidad; i++)
            {
                pedidoActual.Bebidas.Add(new Producto
                {
                    Id = bebida.Id,
                    Nombre = bebida.Nombre,
                    Precio = bebida.Precio
                });
            }

            ActualizarListaPedido();
            ActualizarSubtotal();
            txtCantidad.Text = "1";
        }

        private void ActualizarListaPedido()
        {
            var bebidasAgrupadas = pedidoActual.Bebidas
                .GroupBy(b => new { b.Id, b.Nombre, b.Precio })
                .Select(g => new
                {
                    Nombre = g.Key.Nombre,
                    Cantidad = g.Count(),
                    PrecioUnitario = g.Key.Precio,
                    PrecioTotal = g.Key.Precio * g.Count()
                })
                .ToList();

            lstPedido.ItemsSource = bebidasAgrupadas;
        }

        private void ActualizarSubtotal()
        {
            decimal subtotal = pedidoActual.Bebidas.Sum(b => b.Precio);
            txtSubtotal.Text = $"Subtotal: ${subtotal:N2}";
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            if (pedidoActual.Bebidas.Count == 0)
            {
                MessageBox.Show("No hay bebidas en el pedido", "Pedido vacío",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int cantidadTickets = 1;
            if (!string.IsNullOrEmpty(txtCantidadTickets.Text))
            {
                if (!int.TryParse(txtCantidadTickets.Text, out cantidadTickets) || cantidadTickets <= 0)
                {
                    cantidadTickets = 1;
                }
            }

            if (_impresoraService == null)
            {
                var result = MessageBox.Show("No hay impresora conectada. ¿Desea intentar reconectar?",
                    "Sin impresora", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    InicializarImpresora();
                }

                if (_impresoraService == null)
                {
                    MessageBox.Show("No se puede imprimir sin impresora conectada", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            for (int i = 0; i < cantidadTickets; i++)
            {
                _impresoraService.ImprimirTicket(pedidoActual);

                if (i < cantidadTickets - 1)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }

            MessageBox.Show($"Se imprimieron {cantidadTickets} ticket(s) correctamente.",
                "Impresión exitosa", MessageBoxButton.OK, MessageBoxImage.Information);

            ReiniciarPedido();
        }


        private void ReiniciarPedido()
        {
            pedidoActual = new Pedido { Fecha = DateTime.Now };
            lstPedido.ItemsSource = null;
            txtSubtotal.Text = "Subtotal: $0.00";
            txtCantidad.Text = "1";
            txtCantidadTickets.Text = "1";
        }


        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            _impresoraService = null;
            Close();
        }
        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            // Cierra la ventana actual (VentaWindow)
            this.Close();

            // Abre la ventana principal (MainWindow)
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }
        private void txtCantidad_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                lstBebidas.Focus();
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1) btnTicket_Click(sender, null);
            else if (e.Key == System.Windows.Input.Key.F2) ReiniciarPedido();
            else if (e.Key == System.Windows.Input.Key.Escape) btnSalir_Click(sender, null);
        }
    }
}
