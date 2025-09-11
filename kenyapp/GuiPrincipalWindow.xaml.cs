using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kenyapp
{
    public partial class GuiPrincipalWindow : Window
    {
        public GuiPrincipalWindow()
        {
            InitializeComponent();
        }

        private void btbIngresar_Click(object sender, RoutedEventArgs e)
        {
            // Acá abrís tu login o lo que necesites:
            // Ejemplo: abrir otra ventana
            // var login = new LoginWindow();
            // login.Owner = this;
            // login.ShowDialog();

            MessageBox.Show("Ingresar pulsado");
        }

        private void btbCerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }
        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Para poder arrastrar la ventana con la barra superior
        private void BarraSuperior_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        public void MostrarNombreUsuario(string nombre)
        {
            lblUsuario.Text = $"¡Hola, {nombre}!";
            UsuarioPanel.Visibility = Visibility.Visible;
        }
    }
}
