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
    public partial class SelectTipoWindow : Window
    {
        public string SelectedTipo { get; private set; }

        public SelectTipoWindow(string tipoActual = null)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(tipoActual))
            {
                // Buscar el ítem con ese tipo y seleccionarlo
                foreach (ComboBoxItem item in TipoComboBox.Items)
                {
                    if (item.Content.ToString() == tipoActual)
                    {
                        TipoComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }


        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            if (TipoComboBox.SelectedItem is System.Windows.Controls.ComboBoxItem item)
            {
                SelectedTipo = item.Content.ToString();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Seleccione un tipo antes de aceptar.");
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}