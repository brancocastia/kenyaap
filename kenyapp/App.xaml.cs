using kenyapp.Data;
using System.Configuration;
using System.Data;
using System.Windows;

namespace kenyapp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Crear DB si no existe
            DbInitializer.Initialize();
        }
    }

}
