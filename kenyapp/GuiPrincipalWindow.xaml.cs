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
            var mainwindow = new MainWindow();
            Application.Current.MainWindow = mainwindow; // ahora la nueva es la principal
            mainwindow.Show();
            this.Close();
        }


        private void btbCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Para poder arrastrar la ventana con la barra superior
        private void BarraSuperior_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


    }
}
