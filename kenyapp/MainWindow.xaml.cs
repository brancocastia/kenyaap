using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using kenyapp.Views;


namespace kenyapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Botonera_Click(object sender, RoutedEventArgs e)
        {
            var botoneraWindow = new VentaWindow();
            botoneraWindow.Show(); // Se abre de manera no modal
        }

        private void Ajustes_Click(object sender, RoutedEventArgs e)
        {
            var ajustesWindow = new AjustesWindow();
            ajustesWindow.ShowDialog(); // Se abre como ventana modal
        }

        private void Reportes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aquí se abrirá la pantalla de reportes.");
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}