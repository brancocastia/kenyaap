using kenyapp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace kenyapp.Views
{
    public partial class VentaWindow : Window
    {
        private Pedido pedidoActual = new Pedido { Fecha = DateTime.Now };

        public VentaWindow()
        {
            InitializeComponent();
            CargarBebidas();
        }

        private void CargarBebidas()
        {
            try
            {
                using var con = new SqliteConnection("Data Source=kenya.db");
                con.Open();

                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Id, Nombre, Precio FROM Bebidas";

                var reader = cmd.ExecuteReader();
                var lista = new List<Bebida>();

                while (reader.Read())
                {
                    lista.Add(new Bebida
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
                MessageBox.Show("Error cargando bebidas: " + ex.Message);
            }
        }

        private void lstBebidas_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (!(lstBebidas.SelectedItem is Bebida bebida)) return;

                int cantidad = int.TryParse(txtCantidad.Text, out int n) ? n : 1;

                // Agregar bebida al pedido actual
                for (int i = 0; i < cantidad; i++)
                {
                    pedidoActual.Bebidas.Add(bebida);
                }

                // Mostrar en lista de pedidos (agrupado)
                lstPedido.Items.Add(new
                {
                    Nombre = bebida.Nombre,
                    Cantidad = cantidad,
                    PrecioTotal = bebida.Precio * cantidad
                });

                ActualizarSubtotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la bebida: " + ex.Message);
            }
        }

        private void ActualizarSubtotal()
        {
            try
            {
                decimal subtotal = pedidoActual.Bebidas.Sum(b => b.Precio);
                txtSubtotal.Text = $"Subtotal: ${subtotal}";
            }
            catch
            {
                txtSubtotal.Text = "Subtotal: $0";
            }
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            if (pedidoActual.Bebidas.Count == 0) return;

            // Cantidad de tickets a imprimir
            int cantidadTickets = 1; // siempre 1 por ahora
            int.TryParse(txtCantidadTickets.Text, out cantidadTickets);
            if (cantidadTickets <= 0) cantidadTickets = 1;

            var impresora = new ImpresoraService();

            // Imprimir tickets
            for (int i = 0; i < cantidadTickets; i++)
            {
                impresora.ImprimirTicket(pedidoActual);
            }

            MessageBox.Show("Tickets enviados a impresora.", "Ticket", MessageBoxButton.OK, MessageBoxImage.Information);

            // Reiniciar pedido para próximo cliente
            pedidoActual = new Pedido { Fecha = DateTime.Now };
            lstPedido.Items.Clear();
            txtSubtotal.Text = "Subtotal: $0";
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
